using GermanTrainer.Core.Models;

namespace GermanTrainer.Core.Services;

public enum VocabularyScope { All, Articles, Translation, VerbPrepositions }

/// <summary>
/// Composable filters independent of WPF and the database
/// </summary>
public sealed class VocabularyQuery
{
    public string Search { get; init; } = "";
    public string? Level { get; init; }
    public int? Lesson { get; init; }
    public WordType? Type { get; init; }
    public string? Article { get; init; }
    public bool? IsHidden { get; init; }
    public bool? HasConstructions { get; init; }
    public VocabularyScope Scope { get; init; }

    public bool Matches(WordDraft w)
    {
        if (Scope == VocabularyScope.VerbPrepositions && (w.Type != WordType.Verb || w.Constructions.Count == 0)) return false;
        if (Scope == VocabularyScope.Articles && (w.Type != WordType.Noun || w.Article is not ("der" or "die" or "das"))) return false;
        if (Level != null && !CourseLesson.NormalizeLevel(w.Level).Equals(Level, StringComparison.OrdinalIgnoreCase)) return false;
        if (Lesson.HasValue && w.Lesson != Lesson || Type.HasValue && w.Type != Type ||
            Article != null && (w.Type != WordType.Noun || w.Article != Article) ||
            IsHidden.HasValue && w.IsHidden != IsHidden ||
            HasConstructions.HasValue && (w.Type != WordType.Verb || (w.Constructions.Count > 0) != HasConstructions)) return false;
        var text = $"{w.Word} {w.Translation} {w.Article} {w.Level} {w.Lesson} {w.Type.ToDisplayName()} " +
            string.Join(" ", w.Constructions.Select(c => $"{c.VerbDisplay} {c.Meaning} {c.Preposition} {c.Case}"));
        return Search.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .All(term => text.Contains(term, StringComparison.OrdinalIgnoreCase));
    }
}
