using Microsoft.EntityFrameworkCore;
using GermanTrainer.Core.Models;
using GermanTrainer.Infrastructure.Data;
using GermanTrainer.Core.Services;

namespace GermanTrainer.Infrastructure.Services;

public class WordsService(ITrainerDbContextFactory factory) : IWordsService
{
    public void EnsureDatabaseCreated()
    {
        using var context = factory.Create();
        context.Database.EnsureCreated();
        DatabaseUpgrade.Apply(context);
    }

    public List<WordEntry> GetWords(WordType type, IReadOnlyCollection<int>? lessons = null)
    {
        using var context = factory.Create();
        var lessonList = lessons?.ToList();
        switch (type)
        {
            case WordType.Noun:
                var nouns = context.GermanNouns.AsNoTracking().AsQueryable();
                if (lessonList != null) nouns = nouns.Where(w => lessonList.Contains(w.Lesson));
                return nouns.OrderBy(w => w.Level).ThenBy(w => w.Lesson).ThenBy(w => w.Word)
                    .Select(w => new WordEntry(w.Id, WordType.Noun, w.Lesson, w.Article, w.Word,
                        w.Translation, w.IsHidden, w.CorrectAnswers, w.WrongAnswers, w.Level)).ToList();
            case WordType.Verb:
                var verbs = context.GermanVerbs.AsNoTracking().AsQueryable();
                if (lessonList != null) verbs = verbs.Where(w => lessonList.Contains(w.Lesson));
                return verbs.OrderBy(w => w.Level).ThenBy(w => w.Lesson).ThenBy(w => w.Word)
                    .Select(w => new WordEntry(w.Id, WordType.Verb, w.Lesson, null, w.Word,
                        w.Translation, w.IsHidden, w.CorrectAnswers, w.WrongAnswers, w.Level)).ToList();
            case WordType.Adjective:
                var adjectives = context.GermanAdjectives.AsNoTracking().AsQueryable();
                if (lessonList != null) adjectives = adjectives.Where(w => lessonList.Contains(w.Lesson));
                return adjectives.OrderBy(w => w.Level).ThenBy(w => w.Lesson).ThenBy(w => w.Word)
                    .Select(w => new WordEntry(w.Id, WordType.Adjective, w.Lesson, null, w.Word,
                        w.Translation, w.IsHidden, w.CorrectAnswers, w.WrongAnswers, w.Level)).ToList();
            default: return [];
        }
    }
}
