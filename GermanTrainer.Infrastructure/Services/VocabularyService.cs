using Microsoft.EntityFrameworkCore;
using GermanTrainer.Core.Models;
using GermanTrainer.Infrastructure.Data;
using GermanTrainer.Core.Services;

namespace GermanTrainer.Infrastructure.Services;

public class VocabularyService(ITrainerDbContextFactory factory) : IVocabularyService
{
    public IReadOnlyList<WordDraft> GetAll()
    {
        using var db = factory.Create();
        var list = new List<WordDraft>();
        list.AddRange(db.GermanNouns.AsNoTracking().ToList().Select(n => new WordDraft
        {
            Id = n.Id,
            Type = WordType.Noun,
            Word = n.Word,
            Translation = n.Translation,
            Article = n.Article,
            PluralForm = n.PluralForm,
            Lesson = n.Lesson,
            Level = n.Level,
            IsHidden = n.IsHidden,
            CorrectAnswers = n.CorrectAnswers,
            WrongAnswers = n.WrongAnswers
        }));
        list.AddRange(db.GermanAdjectives.AsNoTracking().ToList().Select(a => new WordDraft
        {
            Id = a.Id,
            Type = WordType.Adjective,
            Word = a.Word,
            Translation = a.Translation,
            Lesson = a.Lesson,
            Level = a.Level,
            IsHidden = a.IsHidden,
            CorrectAnswers = a.CorrectAnswers,
            WrongAnswers = a.WrongAnswers
        }));
        list.AddRange(db.GermanVerbs.AsNoTracking().Include(v => v.PrepositionConstructions).ToList().Select(v => new WordDraft
        {
            Id = v.Id,
            Type = WordType.Verb,
            Word = v.Word,
            Translation = v.Translation,
            Lesson = v.Lesson,
            Level = v.Level,
            IsHidden = v.IsHidden,
            CorrectAnswers = v.CorrectAnswers,
            WrongAnswers = v.WrongAnswers,
            Constructions = v.PrepositionConstructions.Select(c => new ConstructionDraft
            { Id = c.Id, VerbDisplay = c.VerbDisplay, Preposition = c.Preposition, Case = c.Case, Meaning = c.Meaning }).ToList()
        }));
        return list.OrderBy(w => w.Level).ThenBy(w => w.Lesson).ThenBy(w => w.Word).ToList();
    }

    public void Save(WordDraft draft)
    {
        EnsureValid(draft);
        draft.Level = draft.Level.Trim().ToUpperInvariant();
        using var db = factory.Create();
        using var tx = db.Database.BeginTransaction();
        var originalId = draft.Id;
        try { Save(db, draft); tx.Commit(); }
        catch { draft.Id = originalId; throw; }
    }

    public void Delete(WordType type, int id)
    {
        using var db = factory.Create();
        object? entity = type switch
        {
            WordType.Noun => db.GermanNouns.Find(id),
            WordType.Verb => db.GermanVerbs.Find(id),
            WordType.Adjective => db.GermanAdjectives.Find(id),
            _ => null
        };
        if (entity == null) return;
        db.Remove(entity); db.SaveChanges();
    }

    private static void EnsureValid(WordDraft draft)
    {
        var errors = VocabularyValidation.Validate(draft);
        if (errors.Count > 0) throw new ArgumentException(string.Join(Environment.NewLine, errors));
    }

    private static void Save(TrainerDbContext db, WordDraft d)
    {
        var candidates = d.Type switch
        {
            WordType.Noun => db.GermanNouns.Where(w => w.Lesson == d.Lesson && w.Level == d.Level).Select(w => new { w.Id, w.Word }).ToList(),
            WordType.Verb => db.GermanVerbs.Where(w => w.Lesson == d.Lesson && w.Level == d.Level).Select(w => new { w.Id, w.Word }).ToList(),
            _ => db.GermanAdjectives.Where(w => w.Lesson == d.Lesson && w.Level == d.Level).Select(w => new { w.Id, w.Word }).ToList()
        };
        if (candidates.Any(w => w.Id != d.Id && w.Word.Trim().Equals(d.Word.Trim(), StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException("Такое слово уже есть в выбранном уроке и категории.");
        switch (d.Type)
        {
            case WordType.Noun:
                var n = d.Id == 0 ? new GermanNoun() : db.GermanNouns.Find(d.Id) ?? throw new ArgumentException("Слово удалено.");
                if (d.Id == 0) db.Add(n);
                n.Word = d.Word.Trim(); n.Translation = d.Translation.Trim(); n.Article = d.Article;
                n.PluralForm = d.PluralForm; n.Lesson = d.Lesson; n.Level = d.Level; n.IsHidden = d.IsHidden;
                db.SaveChanges(); d.Id = n.Id;
                break;
            case WordType.Adjective:
                var a = d.Id == 0 ? new GermanAdjective() : db.GermanAdjectives.Find(d.Id) ?? throw new ArgumentException("Слово удалено.");
                if (d.Id == 0) db.Add(a);
                a.Word = d.Word.Trim(); a.Translation = d.Translation.Trim(); a.Lesson = d.Lesson; a.Level = d.Level; a.IsHidden = d.IsHidden;
                db.SaveChanges(); d.Id = a.Id;
                break;
            case WordType.Verb:
                var v = d.Id == 0 ? new GermanVerb() : db.GermanVerbs.Include(x => x.PrepositionConstructions)
                    .SingleOrDefault(x => x.Id == d.Id) ?? throw new ArgumentException("Глагол удалён.");
                if (d.Id == 0) db.Add(v);
                v.Word = d.Word.Trim(); v.Translation = d.Translation.Trim(); v.Lesson = d.Lesson; v.Level = d.Level; v.IsHidden = d.IsHidden;
                db.SaveChanges(); d.Id = v.Id;
                var old = v.PrepositionConstructions.ToList();
                foreach (var c in old.Where(c => !d.Constructions.Any(x => x.Id == c.Id))) db.Remove(c);
                db.SaveChanges();
                foreach (var c in d.Constructions)
                {
                    var entity = old.FirstOrDefault(x => x.Id == c.Id) ?? new VerbPreposition { GermanVerbId = v.Id };
                    if (entity.Id == 0) db.Add(entity);
                    entity.Preposition = c.Preposition; entity.Case = c.Case;
                    entity.VerbDisplay = c.VerbDisplay.Trim(); entity.Meaning = c.Meaning.Trim();
                }
                db.SaveChanges();
                break;
        }
    }
}
