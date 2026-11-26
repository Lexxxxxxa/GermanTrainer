using GermanTrainer.Core.Models;
using GermanTrainer.Core.Services;

namespace GermanTrainer.Tests;

public class VocabularyQueryTests
{
    private static WordDraft Verb() => new()
    {
        Id = 1, Type = WordType.Verb, Level = "A2", Lesson = 8, Word = "sich freuen", Translation = "радоваться",
        Constructions = [new() { VerbDisplay = "sich freuen", Preposition = "auf", Case = GrammaticalCase.Akkusativ, Meaning = "предстоящее событие" }]
    };

    [Theory]
    [InlineData("freuen")]
    [InlineData("радоваться")]
    [InlineData("A2")]
    [InlineData("auf")]
    [InlineData("Akkusativ")]
    [InlineData("предстоящее")]
    [InlineData("freuen A2 auf")]
    public void SearchIncludesMainFieldsAndConstructions(string search) =>
        Assert.True(new VocabularyQuery { Search = search }.Matches(Verb()));

    [Fact] public void StructuredFiltersAreCombined()
    {
        Assert.True(new VocabularyQuery { Level = "A2", Lesson = 8, Type = WordType.Verb, IsHidden = false, HasConstructions = true }.Matches(Verb()));
        Assert.False(new VocabularyQuery { Level = "B1" }.Matches(Verb()));
        Assert.False(new VocabularyQuery { Lesson = 1 }.Matches(Verb()));
        Assert.False(new VocabularyQuery { Type = WordType.Noun }.Matches(Verb()));
        Assert.False(new VocabularyQuery { IsHidden = true }.Matches(Verb()));
        Assert.False(new VocabularyQuery { HasConstructions = false }.Matches(Verb()));
        Assert.False(new VocabularyQuery { Search = "freuen Dativ" }.Matches(Verb()));
    }

    [Fact] public void PrepositionsScopeNeverIncludesOtherWords()
    {
        var query = new VocabularyQuery { Scope = VocabularyScope.VerbPrepositions };
        Assert.True(query.Matches(Verb()));
        var noConstruction = Verb(); noConstruction.Constructions.Clear();
        Assert.False(query.Matches(noConstruction));
        Assert.False(query.Matches(new() { Type = WordType.Noun, Word = "Haus" }));
        Assert.False(query.Matches(new() { Type = WordType.Adjective, Word = "gut" }));
        var hidden = Verb(); hidden.IsHidden = true;
        Assert.True(query.Matches(hidden));
    }

    [Fact] public void ArticlesScopeAndArticleFilterRequireSuitableNouns()
    {
        var noun = new WordDraft { Type = WordType.Noun, Article = "das", Word = "Haus" };
        Assert.True(new VocabularyQuery { Scope = VocabularyScope.Articles, Article = "das" }.Matches(noun));
        Assert.False(new VocabularyQuery { Article = "der" }.Matches(noun));
        noun.Article = "";
        Assert.False(new VocabularyQuery { Scope = VocabularyScope.Articles }.Matches(noun));
        Assert.False(new VocabularyQuery { Article = "der" }.Matches(Verb()));
    }
}
