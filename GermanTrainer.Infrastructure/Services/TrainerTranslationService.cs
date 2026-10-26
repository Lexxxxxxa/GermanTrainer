using GermanTrainer.Data;
using GermanTrainer.Models;
using Microsoft.EntityFrameworkCore;

namespace GermanTrainer.Services;

public class TrainerTranslationService : ITranslationTrainingService
{
    private readonly LearningHistoryService _history;
    private readonly ITrainerDbContextFactory _factory;
    private readonly Dictionary<TrainingMode, string?> _previous = new();
    private List<TranslationWord> _words = new();

    public TrainerTranslationService(ITrainerDbContextFactory factory, LearningHistoryService history)
    {
        _factory = factory; _history = history;
        ReloadWords();
    }

    public TranslationWord? GetNextWord(bool reverse)
    {
        var mode = reverse ? TrainingMode.RussianToGerman : TrainingMode.GermanToRussian;
        var word = AdaptiveSelection.Pick(_words, w => w.Key, _history.Read(mode),
            _previous.GetValueOrDefault(mode), DateTime.UtcNow, Random.Shared);
        if (word != null)
        {
            _previous[mode] = word.Key;
            _history.RecordShown(mode, word.Key);
        }
        return word;
    }

    public IReadOnlyList<string> GetOptions(TranslationWord word, bool reverse) =>
        TranslationChoices.Build(word, _words, reverse, Random.Shared);

    public void RecordAnswer(TranslationWord word, bool reverse, bool correct)
    {
        var mode = reverse ? TrainingMode.RussianToGerman : TrainingMode.GermanToRussian;
        // Old aggregate counters remain available in Settings; mode-specific history is separate.
        using var db = _factory.Create();
        var parts = word.Key.Split(':');
        var id = int.Parse(parts[1]);
        switch (parts[0])
        {
            case "Noun":
                var n = db.GermanNouns.Find(id);
                if (n != null) { n.TimesShown++; if (correct) n.CorrectAnswers++; else n.WrongAnswers++; n.LastShownAt = DateTime.Now; }
                break;
            case "Verb":
                var v = db.GermanVerbs.Find(id);
                if (v != null) { v.TimesShown++; if (correct) v.CorrectAnswers++; else v.WrongAnswers++; v.LastShownAt = DateTime.Now; }
                break;
            case "Adjective":
                var a = db.GermanAdjectives.Find(id);
                if (a != null) { a.TimesShown++; if (correct) a.CorrectAnswers++; else a.WrongAnswers++; a.LastShownAt = DateTime.Now; }
                break;
        }
        LearningHistoryService.RecordAnswer(db, mode, word.Key, correct);
        db.SaveChanges();
    }

    public void ReloadWords()
    {
        using var db = _factory.Create();
        var lessons = AppSettings.EnabledLessons.ToList();
        _words = new();
        if (AppSettings.IsWordTypeEnabled(WordType.Noun))
            _words.AddRange(db.GermanNouns.AsNoTracking()
                .Where(w => !w.IsHidden && (lessons.Count == 0 || lessons.Contains(w.Lesson))).ToList()
                .Select(w => new TranslationWord($"Noun:{w.Id}",
                    string.IsNullOrWhiteSpace(w.Article) ? w.Word : $"{w.Article} {w.Word}", w.Translation)));
        if (AppSettings.IsWordTypeEnabled(WordType.Verb))
            _words.AddRange(db.GermanVerbs.AsNoTracking()
                .Where(w => !w.IsHidden && (lessons.Count == 0 || lessons.Contains(w.Lesson))).ToList()
                .Select(w => new TranslationWord($"Verb:{w.Id}", w.Word, w.Translation)));
        if (AppSettings.IsWordTypeEnabled(WordType.Adjective))
            _words.AddRange(db.GermanAdjectives.AsNoTracking()
                .Where(w => !w.IsHidden && (lessons.Count == 0 || lessons.Contains(w.Lesson))).ToList()
                .Select(w => new TranslationWord($"Adjective:{w.Id}", w.Word, w.Translation)));
        _words = _words.Where(w => !string.IsNullOrWhiteSpace(w.German) &&
            !string.IsNullOrWhiteSpace(w.Russian)).ToList();
    }
}
