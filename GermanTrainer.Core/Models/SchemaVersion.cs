namespace GermanTrainer.Models;

/// <summary>
/// Internal database upgrade marker, not a product/release version
/// </summary>
public class SchemaVersion
{
    public int Id { get; set; }
    public int Version { get; set; }
    public DateTime AppliedUtc { get; set; }
}
