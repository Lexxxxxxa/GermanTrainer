using GermanTrainer.Models;

namespace GermanTrainer.Services;

public static class PrepositionAnswer
{
    public static (bool Preposition, bool Case) Check(VerbPreposition item, string preposition, GrammaticalCase grammaticalCase)
        => (string.Equals(item.Preposition, preposition, StringComparison.Ordinal), item.Case == grammaticalCase);
}
