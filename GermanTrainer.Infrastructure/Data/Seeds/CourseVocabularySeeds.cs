using GermanTrainer.Core.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GermanTrainer.Infrastructure.Data.Configuration;

/// <summary>
/// The C# batches support both HasData and additive upgrades of existing databases
/// </summary>
public static class CourseVocabularySeeds
{
    public static IReadOnlyList<WordDraft> Read()
    {
        var nouns = GermanNounConfiguration.GetGermanNounA1Seeds().Concat(GermanNounConfiguration.GetGermanNounA2Seeds()).Concat(GermanNounConfiguration.GetGermanNounB1Seeds());
        var verbs = GermanVerbConfiguration.GetGermanVerbA1Seeds().Concat(GermanVerbConfiguration.GetGermanVerbA2Seeds()).Concat(GermanVerbConfiguration.GetGermanVerbB1Seeds());
        var adjectives = GermanAdjectiveConfiguration.GetGermanAdjectiveA1Seeds().Concat(GermanAdjectiveConfiguration.GetGermanAdjectiveA2Seeds()).Concat(GermanAdjectiveConfiguration.GetGermanAdjectiveB1Seeds());
        return nouns.Select(w => new WordDraft { Type = WordType.Noun, Level = w.Level, Lesson = w.Lesson,
                Word = w.Word, Translation = w.Translation, Article = w.Article, PluralForm = w.PluralForm })
            .Concat(verbs.Select(w => new WordDraft { Type = WordType.Verb, Level = w.Level, Lesson = w.Lesson,
                Word = w.Word, Translation = w.Translation }))
            .Concat(adjectives.Select(w => new WordDraft { Type = WordType.Adjective, Level = w.Level, Lesson = w.Lesson,
                Word = w.Word, Translation = w.Translation })).ToList();
    }

    public static void Apply(TrainerDbContext db)
    {
        var model = db.GetService<IDesignTimeModel>().Model;
        var originals = db.GermanNouns.ToList().Cast<object>()
            .Concat(db.GermanVerbs.ToList()).Concat(db.GermanAdjectives.ToList())
            .ToDictionary(e => (e.GetType(), (int)db.Entry(e).Property("Id").CurrentValue!));
        foreach (var type in new[] { typeof(GermanNoun), typeof(GermanVerb), typeof(GermanAdjective) })
        foreach (var seed in model.FindEntityType(type)!.GetSeedData().Where(s => (string?)s["Level"] == "A1"))
        {
            if (!originals.TryGetValue((type, (int)seed["Id"]!), out var entity)) continue;
            var entry = db.Entry(entity);
            if ((string?)entry.Property("Level").CurrentValue == "" &&
                (string?)entry.Property("Word").CurrentValue == (string?)seed["Word"] &&
                (int?)entry.Property("Lesson").CurrentValue == (int?)seed["Lesson"])
                entry.Property("Level").CurrentValue = "A1";
        }
        db.SaveChanges();
        var existing = db.GermanNouns.Select(w => new WordDraft
                { Level = w.Level, Lesson = w.Lesson, Type = WordType.Noun, Word = w.Word }).ToList()
            .Concat(db.GermanVerbs.Select(w => new WordDraft
                { Level = w.Level, Lesson = w.Lesson, Type = WordType.Verb, Word = w.Word }).ToList())
            .Concat(db.GermanAdjectives.Select(w => new WordDraft
                { Level = w.Level, Lesson = w.Lesson, Type = WordType.Adjective, Word = w.Word }).ToList())
            .Select(Key).ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var w in Read())
        {
            if (!existing.Add(Key(w))) continue;
            object entity = w.Type switch
            {
                WordType.Noun => new GermanNoun { Level = w.Level, Lesson = w.Lesson, Word = w.Word,
                    Translation = w.Translation, Article = w.Article, PluralForm = w.PluralForm, CreatedAt = new(2025, 1, 1) },
                WordType.Verb => new GermanVerb { Level = w.Level, Lesson = w.Lesson, Word = w.Word,
                    Translation = w.Translation, CreatedAt = new(2025, 1, 1) },
                _ => new GermanAdjective { Level = w.Level, Lesson = w.Lesson, Word = w.Word,
                    Translation = w.Translation, CreatedAt = new(2025, 1, 1) }
            };
            db.Add(entity);
        }
        db.SaveChanges();
    }
    private static string Key(WordDraft w) => $"{w.Level}|{w.Lesson}|{w.Type}|{w.Word.Trim()}";
}
