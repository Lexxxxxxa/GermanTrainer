using GermanTrainer.Models;

namespace GermanTrainer.Services;

public interface IWordsService
{
    void EnsureDatabaseCreated();
    List<int> GetAllLessons();
    Dictionary<int, int> GetVisibleWordCountByLesson();
    List<WordEntry> GetWords(WordType type, IReadOnlyCollection<int>? lessons = null);
    bool? ToggleHidden(WordType type, int id);
    bool DeleteWord(WordType type, int id);
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
