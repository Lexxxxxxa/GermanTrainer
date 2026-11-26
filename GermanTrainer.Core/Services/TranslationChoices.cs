using GermanTrainer.Core.Models;

namespace GermanTrainer.Core.Services;

public static class TranslationChoices
{
    public static string Prompt(TranslationWord word, bool reverse) => reverse ? word.Russian : word.German;
    public static string Answer(TranslationWord word, bool reverse) => reverse ? word.German : word.Russian;

    public static IReadOnlyList<string> Build(TranslationWord current,
        IReadOnlyList<TranslationWord> vocabulary, bool reverse, Random random)
    {
        var correct = Answer(current, reverse);

        var forbidden = vocabulary.Where(w =>
            Overlap(w.German, current.German) || Overlap(w.Russian, current.Russian))
            .Select(w => Answer(w, reverse)).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var distractors = vocabulary.Select(w => Answer(w, reverse))
            .Where(a => !string.IsNullOrWhiteSpace(a) && !forbidden.Contains(a))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(_ => random.Next()).Take(3).ToList();
        distractors.Add(correct);

        return distractors.OrderBy(_ => random.Next()).ToList();
    }

    private static bool Overlap(string left, string right)
    {
        static HashSet<string> Parts(string value) => value
            .Split([',', ';', '/'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => s.ToLowerInvariant()).ToHashSet();
        return Parts(left).Overlaps(Parts(right));
    }
}
