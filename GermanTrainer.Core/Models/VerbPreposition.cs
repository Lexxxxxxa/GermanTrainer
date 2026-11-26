namespace GermanTrainer.Core.Models;

public enum GrammaticalCase { Akkusativ, Dativ }

public class VerbPreposition
{
    public int Id { get; set; }
    public int GermanVerbId { get; set; }
    public GermanVerb Verb { get; set; } = null!;
    public string VerbDisplay { get; set; } = "";
    public string Meaning { get; set; } = "";
    public string Preposition { get; set; } = "";
    public GrammaticalCase Case { get; set; }
}
