using GermanTrainer.Data;
using GermanTrainer.Models;
using Microsoft.EntityFrameworkCore;

namespace GermanTrainer.Services
{
    public class WordsService(ITrainerDbContextFactory factory) : IWordsService
    {
        public void EnsureDatabaseCreated()
        {
            using var context = factory.Create();
            context.Database.EnsureCreated();
            DatabaseUpgrade.Apply(context);
        }

        public List<int> GetAllLessons()
        {
            using var context = factory.Create();
            return context.GermanNouns.Select(w => w.Lesson)
                .Union(context.GermanAdjectives.Select(w => w.Lesson))
                .Union(context.GermanVerbs.Select(w => w.Lesson))
                .Distinct()
                .OrderBy(l => l)
                .ToList();
        }

        public Dictionary<int, int> GetVisibleWordCountByLesson()
        {
            using var context = factory.Create();

            var lessons = context.GermanNouns.Where(w => !w.IsHidden).Select(w => w.Lesson).ToList();
            lessons.AddRange(context.GermanAdjectives.Where(w => !w.IsHidden).Select(w => w.Lesson));
            lessons.AddRange(context.GermanVerbs.Where(w => !w.IsHidden).Select(w => w.Lesson));

            return lessons.GroupBy(l => l).ToDictionary(g => g.Key, g => g.Count());
        }

        public List<WordEntry> GetWords(WordType type, IReadOnlyCollection<int>? lessons = null)
        {
            using var context = factory.Create();
            var lessonList = lessons?.ToList();

            switch (type)
            {
                case WordType.Noun:
                    {
                        IQueryable<GermanNoun> query = context.GermanNouns.AsNoTracking();
                        if (lessonList != null) query = query.Where(w => lessonList.Contains(w.Lesson));

                        return query.OrderBy(w => w.Lesson).ThenBy(w => w.Word)
                            .Select(w => new WordEntry(w.Id, WordType.Noun, w.Lesson, w.Article, w.Word,
                                w.Translation, w.IsHidden, w.CorrectAnswers, w.WrongAnswers))
                            .ToList();
                    }
                case WordType.Adjective:
                    {
                        IQueryable<GermanAdjective> query = context.GermanAdjectives.AsNoTracking();
                        if (lessonList != null) query = query.Where(w => lessonList.Contains(w.Lesson));

                        return query.OrderBy(w => w.Lesson).ThenBy(w => w.Word)
                            .Select(w => new WordEntry(w.Id, WordType.Adjective, w.Lesson, null, w.Word,
                                w.Translation, w.IsHidden, w.CorrectAnswers, w.WrongAnswers))
                            .ToList();
                    }
                case WordType.Verb:
                    {
                        IQueryable<GermanVerb> query = context.GermanVerbs.AsNoTracking();
                        if (lessonList != null) query = query.Where(w => lessonList.Contains(w.Lesson));

                        return query.OrderBy(w => w.Lesson).ThenBy(w => w.Word)
                            .Select(w => new WordEntry(w.Id, WordType.Verb, w.Lesson, null, w.Word,
                                w.Translation, w.IsHidden, w.CorrectAnswers, w.WrongAnswers))
                            .ToList();
                    }
                default:
                    return new List<WordEntry>();
            }
        }

        public bool? ToggleHidden(WordType type, int id)
        {
            using var context = factory.Create();

            bool? result = null;
            switch (type)
            {
                case WordType.Noun:
                    var noun = context.GermanNouns.FirstOrDefault(w => w.Id == id);
                    if (noun != null) { noun.IsHidden = !noun.IsHidden; result = noun.IsHidden; }
                    break;
                case WordType.Adjective:
                    var adj = context.GermanAdjectives.FirstOrDefault(w => w.Id == id);
                    if (adj != null) { adj.IsHidden = !adj.IsHidden; result = adj.IsHidden; }
                    break;
                case WordType.Verb:
                    var verb = context.GermanVerbs.FirstOrDefault(w => w.Id == id);
                    if (verb != null) { verb.IsHidden = !verb.IsHidden; result = verb.IsHidden; }
                    break;
            }

            if (result != null)
                context.SaveChanges();

            return result;
        }

        public bool DeleteWord(WordType type, int id)
        {
            using var context = factory.Create();

            object? entity = type switch
            {
                WordType.Noun => context.GermanNouns.FirstOrDefault(w => w.Id == id),
                WordType.Adjective => context.GermanAdjectives.FirstOrDefault(w => w.Id == id),
                WordType.Verb => context.GermanVerbs.FirstOrDefault(w => w.Id == id),
                _ => null
            };

            if (entity == null)
                return false;

            context.Remove(entity);
            context.SaveChanges();
            return true;
        }
    }
}
