namespace GermanTrainer.Models;

/// <summary>
/// In-memory only; reset deliberately leaves persistent history untouched
/// </summary>
public class SessionScore
{
    public int Correct { get; private set; }
    public int Wrong { get; private set; }
    public string Percentage => Correct + Wrong == 0 ? "0%" :
        $"{Math.Round(100.0 * Correct / (Correct + Wrong), 1)}%";
    public void Record(bool correct) { if (correct) Correct++; else Wrong++; }
    public void Reset() { Correct = 0; Wrong = 0; }
}
