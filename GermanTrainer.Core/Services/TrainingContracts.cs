using GermanTrainer.Core.Models;

namespace GermanTrainer.Core.Services;

public interface IWordsService
{
    void EnsureDatabaseCreated();
    List<WordEntry> GetWords(WordType type, IReadOnlyCollection<int>? lessons = null);
}

public interface IArticlesTrainingService
{
    GermanNoun? GetNextNoun();
    void RecordAnswer(GermanNoun noun, bool isCorrect);
}

public interface ITranslationTrainingService
{
    TranslationWord? GetNextWord(bool reverse);
    IReadOnlyList<string> GetOptions(TranslationWord word, bool reverse);
    void RecordAnswer(TranslationWord word, bool reverse, bool correct);
    void ReloadWords();
}

public interface IPrepositionsTrainingService
{
    VerbPreposition? GetNext();
    void RecordAnswer(VerbPreposition item, bool correct);
}
