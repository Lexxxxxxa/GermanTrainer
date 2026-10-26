using GermanTrainer.Data;
using GermanTrainer.Models;
using Microsoft.EntityFrameworkCore;

namespace GermanTrainer.Services;

public class LearningHistoryService(ITrainerDbContextFactory factory)
{
    public Dictionary<string, TrainingProgress> Read(TrainingMode mode)
    {
        using var db = factory.Create();
        return db.TrainingProgress.AsNoTracking().Where(p => p.Mode == mode)
            .ToDictionary(p => p.ItemKey);
    }

    // Record a real display, even if the user skips the question.
    public void RecordShown(TrainingMode mode, string key)
    {
        using var db = factory.Create();
        var progress = GetOrCreate(db, mode, key);
        progress.RecordShown(DateTime.UtcNow);
        db.SaveChanges();
    }

    public void RecordAnswer(TrainingMode mode, string key, bool correct)
    {
        using var db = factory.Create();
        RecordAnswer(db, mode, key, correct);
        db.SaveChanges();
    }

    // Allows legacy counters and new history to be saved atomically in the same DbContext.
    public static void RecordAnswer(TrainerDbContext db, TrainingMode mode, string key, bool correct)
    {
        var progress = GetOrCreate(db, mode, key);
        // Each correct answer reduces the effect of the most recent mistakes by 30%.
        progress.RecordAnswer(correct);
    }

    private static TrainingProgress GetOrCreate(TrainerDbContext db, TrainingMode mode, string key)
    {
        var item = db.TrainingProgress.Find(mode, key);
        if (item != null) return item;
        item = new TrainingProgress { Mode = mode, ItemKey = key };
        db.TrainingProgress.Add(item);
        return item;
    }
}
