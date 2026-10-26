namespace GermanTrainer.Models;

public enum TrainingMode { Articles, GermanToRussian, RussianToGerman, VerbPrepositions }

///
/// <summary>Persistent learning history. Session reset never changes these records
/// </summary>
public class TrainingProgress
{
    public TrainingMode Mode { get; set; }
    public string ItemKey { get; set; } = "";
    public int TimesShown { get; set; }
    public int CorrectAnswers { get; set; }
    public int WrongAnswers { get; set; }
    public double RecentErrorScore { get; set; }
    public DateTime? LastShownUtc { get; set; }

    public void RecordShown(DateTime now)
    {
        TimesShown++;
        LastShownUtc = now;
    }

    public void RecordAnswer(bool correct)
    {
        if (correct) CorrectAnswers++; else WrongAnswers++;
        RecentErrorScore = 0.7 * RecentErrorScore + (correct ? 0 : 0.3);
    }
}
