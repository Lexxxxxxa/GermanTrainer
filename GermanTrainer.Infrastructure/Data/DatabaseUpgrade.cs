using GermanTrainer.Models;
using Microsoft.EntityFrameworkCore;

namespace GermanTrainer.Data;

/// <summary>
/// Additive bridge for installations originally created with EnsureCreated (no migration baseline).
/// Does not alter/drop the old three tables. Runs transactionally, under an application lock.
/// </summary>
public static class DatabaseUpgrade
{
    public static void Apply(TrainerDbContext db)
    {
        using var transaction = db.Database.BeginTransaction();

        if (db.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            db.Database.ExecuteSqlRaw("""
                CREATE TABLE IF NOT EXISTS TrainingProgress (
                    Mode INTEGER NOT NULL, ItemKey TEXT NOT NULL,
                    TimesShown INTEGER NOT NULL, CorrectAnswers INTEGER NOT NULL, WrongAnswers INTEGER NOT NULL,
                    RecentErrorScore REAL NOT NULL, LastShownUtc TEXT NULL, PRIMARY KEY (Mode, ItemKey));
                CREATE TABLE IF NOT EXISTS VerbPrepositions (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, GermanVerbId INTEGER NOT NULL,
                    VerbDisplay TEXT NOT NULL, Meaning TEXT NOT NULL, Preposition TEXT NOT NULL, "Case" INTEGER NOT NULL,
                    FOREIGN KEY (GermanVerbId) REFERENCES GermanVerbs(Id) ON DELETE CASCADE);
                CREATE UNIQUE INDEX IF NOT EXISTS IX_VerbPrepositions_GermanVerbId_Preposition_Case_Meaning
                    ON VerbPrepositions(GermanVerbId, Preposition, "Case", Meaning);
                CREATE TABLE IF NOT EXISTS SchemaVersions (
                    Id INTEGER NOT NULL PRIMARY KEY, Version INTEGER NOT NULL, AppliedUtc TEXT NOT NULL);
                """);
        }
        else
        {
            db.Database.ExecuteSqlRaw("""
            DECLARE @lockResult int;
            EXEC @lockResult = sys.sp_getapplock
                @Resource = N'GermanTrainer.SchemaUpgrade', @LockMode = N'Exclusive',
                @LockOwner = N'Transaction', @LockTimeout = 15000;
            IF @lockResult < 0 THROW 51000, 'Could not acquire GermanTrainer upgrade lock.', 1;
            IF OBJECT_ID(N'dbo.TrainingProgress', N'U') IS NULL
            CREATE TABLE dbo.TrainingProgress (
                Mode int NOT NULL, ItemKey nvarchar(80) NOT NULL,
                TimesShown int NOT NULL, CorrectAnswers int NOT NULL, WrongAnswers int NOT NULL,
                RecentErrorScore float NOT NULL, LastShownUtc datetime2 NULL,
                CONSTRAINT PK_TrainingProgress PRIMARY KEY (Mode, ItemKey));
            IF OBJECT_ID(N'dbo.VerbPrepositions', N'U') IS NULL
            BEGIN
                CREATE TABLE dbo.VerbPrepositions (
                    Id int IDENTITY NOT NULL CONSTRAINT PK_VerbPrepositions PRIMARY KEY,
                    GermanVerbId int NOT NULL, VerbDisplay nvarchar(100) NOT NULL,
                    Meaning nvarchar(250) NOT NULL, Preposition nvarchar(20) NOT NULL, [Case] int NOT NULL,
                    CONSTRAINT FK_VerbPrepositions_GermanVerbs_GermanVerbId FOREIGN KEY (GermanVerbId)
                        REFERENCES dbo.GermanVerbs(Id) ON DELETE CASCADE);
                CREATE UNIQUE INDEX IX_VerbPrepositions_GermanVerbId_Preposition_Case_Meaning
                    ON dbo.VerbPrepositions(GermanVerbId, Preposition, [Case], Meaning);
            END
            IF OBJECT_ID(N'dbo.SchemaVersions', N'U') IS NULL
                CREATE TABLE dbo.SchemaVersions (
                    Id int NOT NULL CONSTRAINT PK_SchemaVersions PRIMARY KEY,
                    Version int NOT NULL, AppliedUtc datetime2 NOT NULL);
            """);
        }
        var version = db.SchemaVersions.Find(1);
        if (version == null)
        {
            VerbPrepositionSeeds.AddMissing(db);
            db.SchemaVersions.Add(new SchemaVersion { Id = 1, Version = 3, AppliedUtc = DateTime.UtcNow });
        }
        else if (version.Version < 3) { version.Version = 3; version.AppliedUtc = DateTime.UtcNow; }
        db.SaveChanges();
        transaction.Commit();
    }
}
