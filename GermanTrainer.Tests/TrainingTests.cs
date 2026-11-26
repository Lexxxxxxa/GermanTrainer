using GermanTrainer.Core.Models;
using GermanTrainer.Core.Services;

namespace GermanTrainer.Tests;

public class TrainingTests
{
    private static readonly DateTime Now = new(2026, 10, 13, 12, 0, 0, DateTimeKind.Utc);
    private static readonly TranslationWord A = new("A", "warten", "ждать");
    private static readonly TranslationWord B = new("B", "arbeiten", "работать");
    private static TrainingProgress Seen(int wrong = 0, DateTime? last = null, double recent = 0) => new()
    {
        TimesShown = 10,
        CorrectAnswers = 10 - wrong,
        WrongAnswers = wrong,
        RecentErrorScore = recent,
        LastShownUtc = last ?? Now
    };

    [Fact]
    public void EmptyPoolReturnsNull() =>
        Assert.Null(AdaptiveSelection.Pick(Array.Empty<TranslationWord>(), w => w.Key,
            new Dictionary<string, TrainingProgress>(), null, Now, new Random(1)));

    [Fact]
    public void SingleItemCanRepeat() =>
        Assert.Same(A, AdaptiveSelection.Pick([A], w => w.Key,
            new Dictionary<string, TrainingProgress>(), "A", Now, new Random(1)));

    [Fact]
    public void ImmediateRepeatIsExcluded()
    {
        for (var i = 0; i < 100; i++)
            Assert.Same(B, AdaptiveSelection.Pick([A, B], w => w.Key,
                new Dictionary<string, TrainingProgress>(), "A", Now, new Random(i)));
    }

    [Fact]
    public void AllNewWordsAreSelectable()
    {
        var rng = new Random(10);
        var keys = Enumerable.Range(0, 100).Select(_ => AdaptiveSelection.Pick(
            new[] { A, B }, w => w.Key, new Dictionary<string, TrainingProgress>(), null, Now, rng)!.Key).ToHashSet();
        Assert.Equal(2, keys.Count);
    }

    [Fact]
    public void AllSeenPoolWorks() =>
        Assert.NotNull(AdaptiveSelection.Pick([A, B], w => w.Key,
            new Dictionary<string, TrainingProgress> { ["A"] = Seen(), ["B"] = Seen() }, null, Now, new Random(1)));

    [Fact]
    public void MoreErrorsIncreaseWeight() =>
        Assert.True(AdaptiveSelection.Weight(Seen(8), Now) > AdaptiveSelection.Weight(Seen(0), Now));

    [Fact]
    public void RecentErrorIncreasesWeight() =>
        Assert.True(AdaptiveSelection.Weight(Seen(recent: 1), Now) > AdaptiveSelection.Weight(Seen(), Now));

    [Fact]
    public void OlderDisplayIncreasesWeight() =>
        Assert.True(AdaptiveSelection.Weight(Seen(last: Now.AddDays(-7)), Now) > AdaptiveSelection.Weight(Seen(), Now));

    [Fact]
    public void AgeMultiplierIsCapped() =>
        Assert.Equal(AdaptiveSelection.Weight(Seen(last: Now.AddDays(-7)), Now),
            AdaptiveSelection.Weight(Seen(last: Now.AddDays(-100)), Now));

    [Fact]
    public void FutureTimestampDoesNotGiveNegativeWeight() =>
        Assert.Equal(AdaptiveSelection.Weight(Seen(), Now),
            AdaptiveSelection.Weight(Seen(last: Now.AddDays(1)), Now));

    [Fact]
    public void NewShareIsApproximatelyThirtyPercent()
    {
        var history = new Dictionary<string, TrainingProgress> { ["B"] = Seen() };
        var rng = new Random(42);
        var count = 0;
        for (var i = 0; i < 20000; i++)
            if (AdaptiveSelection.Pick([A, B], w => w.Key, history, null, Now, rng) == A) count++;
        Assert.InRange(count / 20000.0, 0.28, 0.32);
    }

    [Fact]
    public void ErrorWordIsActuallyPickedMoreFrequently()
    {
        var history = new Dictionary<string, TrainingProgress> { ["A"] = Seen(9, recent: 1), ["B"] = Seen() };
        var rng = new Random(42);
        var count = 0;
        for (var i = 0; i < 10000; i++)
            if (AdaptiveSelection.Pick([A, B], w => w.Key, history, null, Now, rng) == A) count++;
        Assert.True(count > 7000);
    }

    [Theory]
    [InlineData(false, "warten", "ждать")]
    [InlineData(true, "ждать", "warten")]
    public void DirectionChangesPromptAndAnswer(bool reverse, string prompt, string answer)
    {
        Assert.Equal(prompt, TranslationChoices.Prompt(A, reverse));
        Assert.Equal(answer, TranslationChoices.Answer(A, reverse));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void FourDistinctRealOptionsIncludeCorrect(bool reverse)
    {
        TranslationWord[] words = [A, B, new("C", "essen", "есть"), new("D", "gehen", "идти")];
        var choices = TranslationChoices.Build(A, words, reverse, new Random(1));
        Assert.Equal(4, choices.Count);
        Assert.Equal(4, choices.Distinct().Count());
        Assert.Contains(TranslationChoices.Answer(A, reverse), choices);
    }

    [Fact]
    public void SynonymWithSameRussianIsNotDistractor()
    {
        var choices = TranslationChoices.Build(new("S", "sprechen", "говорить"),
            [new("S", "sprechen", "говорить"), new("R", "reden", "говорить"), B], true, new Random(1));
        Assert.DoesNotContain("reden", choices);
        Assert.Contains("sprechen", choices);
    }

    [Fact]
    public void OverlappingRussianMeaningsAreExcluded()
    {
        var choices = TranslationChoices.Build(new("N", "Name", "имя"),
            [new("N", "Name", "имя"), new("P", "Vorname", "имя, личное имя"), B], true, new Random(1));
        Assert.DoesNotContain("Vorname", choices);
    }

    [Fact]
    public void SameGermanDifferentMeaningIsNotDistractor()
    {
        var current = new TranslationWord("1", "die Bank", "банк");
        var choices = TranslationChoices.Build(current, [current, new("2", "die Bank", "скамейка"), B], false, new Random(1));
        Assert.DoesNotContain("скамейка", choices);
    }

    [Fact]
    public void TinyVocabularyDoesNotInventFakeOptions() =>
        Assert.Equal(new[] { "ждать" }, TranslationChoices.Build(A, [A], false, new Random(1)));

    [Fact]
    public void SessionResetLeavesHistoryUntouched()
    {
        var history = Seen(3);
        var score = new SessionScore();
        score.Record(true); score.Record(false); score.Reset();
        Assert.Equal(0, score.Correct); Assert.Equal(0, score.Wrong); Assert.Equal("0%", score.Percentage);
        Assert.Equal(3, history.WrongAnswers); Assert.Equal(10, history.TimesShown);
    }

    [Fact]
    public void ReverseSessionResetDoesNotResetForward()
    {
        var forward = new SessionScore(); var reverse = new SessionScore();
        forward.Record(true); reverse.Record(false); reverse.Reset();
        Assert.Equal(1, forward.Correct); Assert.Equal(0, reverse.Wrong);
    }

    [Fact]
    public void SessionPercentageUsesAnswers()
    {
        var score = new SessionScore();
        score.Record(true); score.Record(false);
        Assert.Equal("50%", score.Percentage);
    }

    [Theory]
    [InlineData("auf", GrammaticalCase.Akkusativ, true, true)]
    [InlineData("mit", GrammaticalCase.Akkusativ, false, true)]
    [InlineData("auf", GrammaticalCase.Dativ, true, false)]
    [InlineData("mit", GrammaticalCase.Dativ, false, false)]
    public void PrepositionAndCaseAreCheckedSeparately(string prep, GrammaticalCase grammaticalCase, bool p, bool c)
    {
        var item = new VerbPreposition { Preposition = "auf", Case = GrammaticalCase.Akkusativ };
        Assert.Equal((p, c), PrepositionAnswer.Check(item, prep, grammaticalCase));
    }

    [Fact]
    public void VerbCanHaveMultipleOrNoConstructions()
    {
        var verb = new GermanVerb();
        Assert.Empty(verb.PrepositionConstructions);
        verb.PrepositionConstructions.Add(new() { Preposition = "mit", Case = GrammaticalCase.Dativ });
        verb.PrepositionConstructions.Add(new() { Preposition = "über", Case = GrammaticalCase.Akkusativ });
        Assert.Equal(2, verb.PrepositionConstructions.Count);
    }

    [Fact]
    public void SkippedDisplayIsRecordedWithoutAnswer()
    {
        var p = new TrainingProgress();
        p.RecordShown(Now);
        Assert.Equal(1, p.TimesShown);
        Assert.Equal(Now, p.LastShownUtc);
        Assert.Equal(0, p.CorrectAnswers + p.WrongAnswers);
    }

    [Fact]
    public void CorrectAnswersGraduallyReduceRecentMistakePriority()
    {
        var p = new TrainingProgress();
        p.RecordAnswer(false);
        var initial = p.RecentErrorScore;
        p.RecordAnswer(true);
        Assert.True(p.RecentErrorScore < initial);
        Assert.True(p.RecentErrorScore > 0);
        Assert.Equal(1, p.CorrectAnswers);
        Assert.Equal(1, p.WrongAnswers);
    }

    [Fact]
    public void MistakeScoreStaysInRange()
    {
        var p = new TrainingProgress();
        for (var i = 0; i < 1000; i++) p.RecordAnswer(false);
        Assert.InRange(p.RecentErrorScore, 0, 1);
    }
}
