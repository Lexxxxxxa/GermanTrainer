namespace GermanTrainer.Core.Models;

public class WordDraft
{
    public int Id { get; set; }
    public WordType Type { get; set; } = WordType.Noun;
    public int Lesson { get; set; } = 1;
    public string Word { get; set; } = "";
    public string Translation { get; set; } = "";
    public string Article { get; set; } = "der";
    public string? PluralForm { get; set; }
    public string Level { get; set; } = "";
    public bool IsHidden { get; set; }
    public int CorrectAnswers { get; set; }
    public int WrongAnswers { get; set; }
    public string TypeLabel => Type.ToDisplayName();
    public List<ConstructionDraft> Constructions { get; set; } = new();
}

public class ConstructionDraft
{
    public int Id { get; set; }
    public string VerbDisplay { get; set; } = "";
    public string Meaning { get; set; } = "";
    public string Preposition { get; set; } = "auf";
    public GrammaticalCase Case { get; set; } = GrammaticalCase.Akkusativ;
}
