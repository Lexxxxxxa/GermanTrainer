using GermanTrainer.Models;

namespace GermanTrainer.Services;

/// <summary>
/// Pure, deterministic-given-Random logic shared by all training modes
/// </summary>
public static class AdaptiveSelection
{
    public static double Weight(TrainingProgress progress, DateTime now)
    {
        var answers = progress.CorrectAnswers + progress.WrongAnswers;
        var errorRate = (progress.WrongAnswers + 1.0) / (answers + 2.0);
        var ageDays = progress.LastShownUtc.HasValue
            ? Math.Max(0, (now - progress.LastShownUtc.Value).TotalDays) : 7;
        var recency = Math.Clamp(progress.RecentErrorScore, 0, 1);
        return (1 + 4 * errorRate + 4 * recency) * (1 + Math.Min(ageDays, 7) / 7);
    }

    public static T? Pick<T>(IReadOnlyList<T> items, Func<T, string> key,
        IReadOnlyDictionary<string, TrainingProgress> history, string? previousKey,
        DateTime now, Random random) where T : class
    {
        if (items.Count == 0) return null;
        var candidates = items.Count > 1
            ? items.Where(i => key(i) != previousKey).ToList() : items.ToList();
        if (candidates.Count == 0) candidates = items.ToList();
        var unseen = candidates.Where(i => !history.TryGetValue(key(i), out var p) || p.TimesShown == 0).ToList();
        var seen = candidates.Except(unseen).ToList();
        if (unseen.Count > 0 && (seen.Count == 0 || random.NextDouble() < 0.3))
            return unseen[random.Next(unseen.Count)];
        var weights = seen.Select(i => Weight(history[key(i)], now)).ToArray();
        var target = random.NextDouble() * weights.Sum();
        for (var i = 0; i < seen.Count; i++)
        {
            target -= weights[i];
            if (target <= 0) return seen[i];
        }
        return seen[^1];
    }
}
