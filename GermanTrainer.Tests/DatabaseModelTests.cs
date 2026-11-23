using GermanTrainer.Data;
using GermanTrainer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GermanTrainer.Tests;

/// <summary>
/// SQL generation/model tests only: no live SQL Server is contacted
/// </summary>
public class DatabaseModelTests
{
    private static TrainerDbContext Context() => new(new DbContextOptionsBuilder<TrainerDbContext>()
        .UseSqlServer("Server=localhost;Database=ModelOnly;Integrated Security=True;TrustServerCertificate=True").Options);

    [Fact]
    public void ModelGeneratesNewTablesWithoutConnection()
    {
        using var db = Context();
        var script = db.Database.GenerateCreateScript();
        Assert.Contains("CREATE TABLE [TrainingProgress]", script);
        Assert.Contains("CREATE TABLE [VerbPrepositions]", script);
        Assert.Contains("REFERENCES [GermanVerbs] ([Id]) ON DELETE CASCADE", script);
        Assert.Contains("[ItemKey] nvarchar(80)", script);
        Assert.Contains("[Meaning] nvarchar(250)", script);
    }

    [Fact]
    public void ProgressHasCompositeKeyIncludingMode()
    {
        using var db = Context();
        var key = db.Model.FindEntityType(typeof(TrainingProgress))!.FindPrimaryKey()!;
        Assert.Equal(new[] { "Mode", "ItemKey" }, key.Properties.Select(p => p.Name));
    }

    [Fact]
    public void VerbRelationshipIsOneToManyAndCascade()
    {
        using var db = Context();
        var fk = db.Model.FindEntityType(typeof(VerbPreposition))!.GetForeignKeys().Single();
        Assert.False(fk.IsUnique);
        Assert.Equal(DeleteBehavior.Cascade, fk.DeleteBehavior);
    }

    [Fact]
    public void StarterConstructionsReferenceExistingSeeds()
    {
        using var db = Context();
        var verbs = db.GetService<IDesignTimeModel>().Model.FindEntityType(typeof(GermanVerb))!.GetSeedData()
            .Select(v => (string)v["Word"]!).ToHashSet();
        Assert.Equal(20, VerbPrepositionSeeds.Entries.Length);
        foreach (var entry in VerbPrepositionSeeds.Entries) Assert.Contains(entry.Word, verbs);
        Assert.Equal(20, VerbPrepositionSeeds.Entries.Distinct().Count());
    }

    [Fact]
    public void SeedCreationDatesAreStable()
    {
        using var db = Context();
        var model = db.GetService<IDesignTimeModel>().Model;
        foreach (var type in new[] { typeof(GermanNoun), typeof(GermanVerb), typeof(GermanAdjective) })
            foreach (var seed in model.FindEntityType(type)!.GetSeedData())
                Assert.Equal(new DateTime(2025, 1, 1), seed["CreatedAt"]);
    }

    [Theory]
    [InlineData(TrainingMode.Articles)]
    [InlineData(TrainingMode.GermanToRussian)]
    [InlineData(TrainingMode.RussianToGerman)]
    [InlineData(TrainingMode.VerbPrepositions)]
    public void HistoryIsQueriedByMode(TrainingMode mode)
    {
        using var db = Context();
        var sql = db.TrainingProgress.Where(p => p.Mode == mode).ToQueryString();
        Assert.Contains("[Mode] =", sql);
    }
}
