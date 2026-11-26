using Microsoft.EntityFrameworkCore;
using GermanTrainer.Core.Models;
using GermanTrainer.Core.Services;
using GermanTrainer.Infrastructure.Common;
using GermanTrainer.Infrastructure.Data;

namespace GermanTrainer.Infrastructure.Services;

public class TrainerPrepositionsService(ITrainerDbContextFactory factory, LearningHistoryService history) : IPrepositionsTrainingService
{
    private readonly LearningHistoryService _history = history;
    private string? _previousKey;

    public VerbPreposition? GetNext()
    {
        using var db = factory.Create();

        var items = db.VerbPrepositions.AsNoTracking().Include(p => p.Verb)
            .Where(p => !p.Verb.IsHidden).ToList()
            .Where(p => AppSettings.IsModuleEnabled(p.Verb.Level, p.Verb.Lesson)).ToList();
        var next = AdaptiveSelection.Pick(items, p => $"Construction:{p.Id}",
            _history.Read(TrainingMode.VerbPrepositions), _previousKey, DateTime.UtcNow, Random.Shared);
        if (next != null)
        {
            _previousKey = $"Construction:{next.Id}";
            _history.RecordShown(TrainingMode.VerbPrepositions, _previousKey);
        }
        return next;
    }

    public void RecordAnswer(VerbPreposition item, bool correct)
    {
        using var db = factory.Create();
        LearningHistoryService.RecordAnswer(db, TrainingMode.VerbPrepositions, $"Construction:{item.Id}", correct);
        db.SaveChanges();
    }
}
