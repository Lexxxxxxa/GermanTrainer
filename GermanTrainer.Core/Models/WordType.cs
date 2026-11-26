namespace GermanTrainer.Core.Models;

public enum WordType
{
    Noun,
    Adjective,
    Verb
}

public static class WordTypeExtensions
{
    public static string ToSettingsKey(this WordType type) => type.ToString();

    public static string ToDisplayName(this WordType type) => type switch
    {
        WordType.Noun => "Существительные",
        WordType.Adjective => "Прилагательные",
        WordType.Verb => "Глаголы",
        _ => type.ToString()
    };

    public static string ToIcon(this WordType type) => type switch
    {
        WordType.Noun => "📘",
        WordType.Adjective => "💚",
        WordType.Verb => "🔴",
        _ => ""
    };
}
