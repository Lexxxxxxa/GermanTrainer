using GermanTrainer.Infrastructure.Data;
using GermanTrainer.Core.Models;
using GermanTrainer.Infrastructure.Common;
using GermanTrainer.Core.Services;

namespace GermanTrainer.Infrastructure.Services;

public class TrainerArticlesService(ITrainerDbContextFactory factory, LearningHistoryService history) : IArticlesTrainingService
{
    private readonly LearningHistoryService _history = history;
    private string? _previousKey;

    public List<GermanNoun> GetAllNouns()
    {
        using var context = factory.Create();
        return context.GermanNouns.OrderBy(w => w.Lesson).ThenBy(w => w.Word).ToList();
    }

    public List<GermanNoun> GetVisibleNouns()
    {
        using var context = factory.Create();

        var query = context.GermanNouns.Where(w => !w.IsHidden &&
            (w.Article == "der" || w.Article == "die" || w.Article == "das"));
        return query.ToList().Where(w => AppSettings.IsModuleEnabled(w.Level, w.Lesson)).ToList();
    }

    public GermanNoun? GetNounById(int id)
    {
        using var context = factory.Create();
        return context.GermanNouns.FirstOrDefault(w => w.Id == id);
    }

    public GermanNoun? GetNextNoun()
    {
        var words = GetVisibleNouns();
        var next = AdaptiveSelection.Pick(words, w => $"Noun:{w.Id}",
            _history.Read(TrainingMode.Articles), _previousKey, DateTime.UtcNow, Random.Shared);
        if (next != null)
        {
            _previousKey = $"Noun:{next.Id}";
            _history.RecordShown(TrainingMode.Articles, _previousKey);
        }
        return next;
    }

    public void RecordAnswer(GermanNoun noun, bool isCorrect)
    {
        using var context = factory.Create();
        var dbNoun = context.GermanNouns.FirstOrDefault(w => w.Id == noun.Id);
        if (dbNoun == null) return;

        dbNoun.TimesShown++;
        if (isCorrect)
            dbNoun.CorrectAnswers++;
        else
            dbNoun.WrongAnswers++;

        dbNoun.LastShownAt = DateTime.Now;
        LearningHistoryService.RecordAnswer(context, TrainingMode.Articles, $"Noun:{noun.Id}", isCorrect);
        context.SaveChanges();
    }

    public void AddNoun(string word, string article, string? translation = null, string? pluralForm = null, int lesson = 1)
    {
        if (!IsValidArticle(article))
            throw new ArgumentException("Артикль должен быть: der, die или das");

        using var context = factory.Create();
        context.GermanNouns.Add(new GermanNoun
        {
            Word = word.Trim(),
            Article = article.ToLowerInvariant(),
            Translation = translation ?? string.Empty,
            PluralForm = pluralForm,
            Lesson = lesson,
            DifficultyCoefficient = 5,
            CreatedAt = DateTime.Now,
            IsHidden = false
        });
        context.SaveChanges();
    }

    public void RemoveNoun(int nounId)
    {
        using var context = factory.Create();
        var noun = context.GermanNouns.FirstOrDefault(w => w.Id == nounId);
        if (noun == null) return;

        context.GermanNouns.Remove(noun);
        context.SaveChanges();
    }

    public void ToggleNounVisibility(int nounId)
    {
        using var context = factory.Create();
        var noun = context.GermanNouns.FirstOrDefault(w => w.Id == nounId);
        if (noun == null) return;

        noun.IsHidden = !noun.IsHidden;
        context.SaveChanges();
    }

    public void ToggleNounVisibility(int nounId, bool isHidden)
    {
        using var context = factory.Create();
        var noun = context.GermanNouns.FirstOrDefault(w => w.Id == nounId);
        if (noun == null) return;

        noun.IsHidden = isHidden;
        context.SaveChanges();
    }

    public (int totalNouns, int correctTotal, int wrongTotal, double averageSuccessRate) GetNounStatistics()
    {
        using var context = factory.Create();
        var nouns = context.GermanNouns.ToList();
        if (nouns.Count == 0)
            return (0, 0, 0, 0);

        return (nouns.Count,
                nouns.Sum(w => w.CorrectAnswers),
                nouns.Sum(w => w.WrongAnswers),
                nouns.Average(w => w.SuccessRate));
    }

    public void ResetNounStatistics(int nounId)
    {
        using var context = factory.Create();
        var noun = context.GermanNouns.FirstOrDefault(w => w.Id == nounId);
        if (noun == null) return;

        noun.TimesShown = 0;
        noun.CorrectAnswers = 0;
        noun.WrongAnswers = 0;
        noun.DifficultyCoefficient = 5;
        noun.LastShownAt = null;
        context.SaveChanges();
    }

    private static bool IsValidArticle(string? article) =>
        article?.ToLowerInvariant() is "der" or "die" or "das";
}
