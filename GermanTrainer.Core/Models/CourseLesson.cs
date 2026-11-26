namespace GermanTrainer.Core.Models;

/// <summary>
/// A module number is unique only within its course level
/// </summary>
public sealed record CourseLesson(string Level, int Lesson)
{
    public string Key => $"{NormalizeLevel(Level)}:{Lesson}";
    public string Label => $"{NormalizeLevel(Level)} · Модуль {Lesson}";
    public static string NormalizeLevel(string? level) =>
        string.IsNullOrWhiteSpace(level) || level.Trim().Equals("Без уровня", StringComparison.OrdinalIgnoreCase)
            ? "Без уровня" : level.Trim().ToUpperInvariant();
}
