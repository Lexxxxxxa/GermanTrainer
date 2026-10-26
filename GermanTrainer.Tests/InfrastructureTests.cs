using GermanTrainer.Data;
using GermanTrainer.Models;
using GermanTrainer.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GermanTrainer.Tests;

public sealed class InfrastructureTests : IDisposable
{
    private readonly SqliteConnection _connection = new("Data Source=:memory:");
    private readonly TrainerDbContextFactory _factory;
    private readonly VocabularyService _vocabulary;
    public InfrastructureTests()
    {
        _connection.Open();
        _factory = new(new DbContextOptionsBuilder<TrainerDbContext>().UseSqlite(_connection).Options);
        _vocabulary = new(_factory);
        new WordsService(_factory).EnsureDatabaseCreated();
    }
    public void Dispose() => _connection.Dispose();
    private static WordDraft NewVerb(string word = "test-verb") => new()
    {
        Type = WordType.Verb, Word = word, Translation = "тест", Lesson = 999,
        Constructions =
        [
            new() { VerbDisplay = word, Meaning = "значение один", Preposition = "auf", Case = GrammaticalCase.Akkusativ },
            new() { VerbDisplay = word, Meaning = "значение два", Preposition = "mit", Case = GrammaticalCase.Dativ }
        ]
    };
    [Fact] public void FreshDatabaseHasVocabularyAndStarterConstructions()
    {
        using var db = _factory.Create();
        Assert.True(_vocabulary.GetAll().Count > 800);
        Assert.Equal(20, db.VerbPrepositions.Count());
    }
    [Fact] public void UpgradeDoesNotResurrectDeletedConstructions()
    {
        using (var db = _factory.Create()) { db.Remove(db.VerbPrepositions.First()); db.SaveChanges(); }
        new WordsService(_factory).EnsureDatabaseCreated();
        using var check = _factory.Create();
        Assert.Equal(19, check.VerbPrepositions.Count());
    }
    [Fact] public void OldSchemaUpgradePreservesVocabularyAndStatistics()
    {
        int id;
        using (var db = _factory.Create())
        {
            var noun = db.GermanNouns.First(); id = noun.Id;
            noun.CorrectAnswers = 17; db.SaveChanges();
            db.Database.ExecuteSqlRaw("DROP TABLE SchemaVersions; DROP TABLE VerbPrepositions; DROP TABLE TrainingProgress;");
        }
        new WordsService(_factory).EnsureDatabaseCreated();
        using var check = _factory.Create();
        Assert.Equal(17, check.GermanNouns.Find(id)!.CorrectAnswers);
        Assert.Equal(20, check.VerbPrepositions.Count());
    }
    [Fact] public void CreateUpdateAndDeleteMultipleConstructions()
    {
        var draft = NewVerb(); _vocabulary.Save(draft);
        var saved = _vocabulary.GetAll().Single(w => w.Type == WordType.Verb && w.Id == draft.Id);
        Assert.Equal(2, saved.Constructions.Count);
        saved.Constructions.RemoveAt(0);
        saved.Constructions[0].Meaning = "обновлено";
        _vocabulary.Save(saved);
        using (var db = _factory.Create())
        {
            Assert.Single(db.VerbPrepositions.Where(c => c.GermanVerbId == draft.Id));
            Assert.Equal("обновлено", db.VerbPrepositions.Single(c => c.GermanVerbId == draft.Id).Meaning);
        }
        _vocabulary.Delete(WordType.Verb, draft.Id);
        using var check = _factory.Create();
        Assert.Empty(check.VerbPrepositions.Where(c => c.GermanVerbId == draft.Id));
    }
    [Fact] public void EditingVocabularyKeepsLearningCounters()
    {
        var draft = NewVerb(); _vocabulary.Save(draft);
        var service = new TrainerTranslationService(_factory, new(_factory));
        service.RecordAnswer(new($"Verb:{draft.Id}", draft.Word, draft.Translation), false, false);
        draft.Translation = "обновлённый перевод"; _vocabulary.Save(draft);
        using var db = _factory.Create();
        Assert.Equal(1, db.GermanVerbs.Find(draft.Id)!.WrongAnswers);
        Assert.Equal(1, db.TrainingProgress.Single().WrongAnswers);
    }
    [Fact] public void DuplicateSaveIsRejectedWithoutChangingId()
    {
        _vocabulary.Save(NewVerb());
        var duplicate = NewVerb();
        Assert.Throws<ArgumentException>(() => _vocabulary.Save(duplicate));
        Assert.Equal(0, duplicate.Id);
    }
    [Fact] public void TranslationDirectionsKeepSeparateHistory()
    {
        var draft = NewVerb(); _vocabulary.Save(draft);
        var svc = new TrainerTranslationService(_factory, new(_factory));
        var word = new TranslationWord($"Verb:{draft.Id}", draft.Word, draft.Translation);
        svc.RecordAnswer(word, false, true);
        svc.RecordAnswer(word, true, false);
        using var db = _factory.Create();
        Assert.Equal(1, db.TrainingProgress.Single(p => p.Mode == TrainingMode.GermanToRussian).CorrectAnswers);
        Assert.Equal(1, db.TrainingProgress.Single(p => p.Mode == TrainingMode.RussianToGerman).WrongAnswers);
    }
    [Theory]
    [InlineData(WordType.Noun)]
    [InlineData(WordType.Verb)]
    [InlineData(WordType.Adjective)]
    public void EditorCanCreateEditHideAndDeleteEachWordType(WordType type)
    {
        var draft = new WordDraft { Type = type, Word = "editor-test", Translation = "тест", Lesson = 999 };
        _vocabulary.Save(draft);
        draft.Translation = "новое значение"; draft.IsHidden = true;
        _vocabulary.Save(draft);
        var saved = _vocabulary.GetAll().Single(w => w.Type == type && w.Id == draft.Id);
        Assert.Equal("новое значение", saved.Translation);
        Assert.True(saved.IsHidden);
        _vocabulary.Delete(type, draft.Id);
        Assert.DoesNotContain(_vocabulary.GetAll(), w => w.Type == type && w.Id == draft.Id);
    }
    [Fact] public void InvalidConstructionDoesNotCreateVerb()
    {
        var draft = NewVerb(); draft.Constructions[0].Meaning = "";
        Assert.Throws<ArgumentException>(() => _vocabulary.Save(draft));
        Assert.DoesNotContain(_vocabulary.GetAll(), w => w.Word == draft.Word);
    }
}
