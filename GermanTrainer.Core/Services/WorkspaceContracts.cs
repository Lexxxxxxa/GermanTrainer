using GermanTrainer.Models;

namespace GermanTrainer.Services;

public interface IVocabularyService
{
    IReadOnlyList<WordDraft> GetAll();
    void Save(WordDraft draft);
    void Delete(WordType type, int id);
}

public static class VocabularyValidation
{
    public static readonly string[] Prepositions = ["an", "auf", "aus", "bei", "für", "gegen", "in", "mit", "nach", "über", "um", "unter", "von", "vor", "zu", "zwischen"];
    public static IReadOnlyList<string> Validate(WordDraft draft)
    {
        var errors = new List<string>();
        if (!Enum.IsDefined(draft.Type)) errors.Add("Неизвестный тип слова.");
        if (string.IsNullOrWhiteSpace(draft.Word) || draft.Word.Length > 50) errors.Add("Слово: от 1 до 50 символов.");
        if (string.IsNullOrWhiteSpace(draft.Translation) || draft.Translation.Length > 200) errors.Add("Перевод: от 1 до 200 символов.");
        if (draft.Lesson is < 1 or > 999) errors.Add("Номер урока: от 1 до 999.");
        if (draft.Level != "" && !new[] { "A1", "A2", "B1", "B2", "C1", "C2" }.Contains(draft.Level))
            errors.Add("Уровень: A1–C2 или пустое значение.");
        if (draft.Type == WordType.Noun && draft.Article is not ("der" or "die" or "das" or "der/die" or "ein" or ""))
            errors.Add("Артикль: der, die, das, der/die или пустое значение.");
        if (draft.PluralForm?.Length > 50) errors.Add("Форма множественного числа: максимум 50 символов.");
        if (draft.Constructions.Count > 100) errors.Add("Максимум 100 конструкций на один глагол.");
        foreach (var c in draft.Constructions)
        {
            if (draft.Type != WordType.Verb) errors.Add("Конструкции можно добавлять только глаголам.");
            if (!Prepositions.Contains(c.Preposition)) errors.Add("Выберите предлог из списка.");
            if (!Enum.IsDefined(c.Case)) errors.Add("Выберите Akkusativ или Dativ.");
            if (string.IsNullOrWhiteSpace(c.VerbDisplay) || c.VerbDisplay.Length > 100) errors.Add("Укажите форму глагола: максимум 100 символов.");
            if (string.IsNullOrWhiteSpace(c.Meaning) || c.Meaning.Length > 250) errors.Add("Укажите подсказку для конструкции: максимум 250 символов.");
        }
        if (draft.Constructions.GroupBy(c => (c.Preposition, c.Case, c.Meaning.Trim())).Any(g => g.Count() > 1))
            errors.Add("Есть повторяющиеся конструкции.");
        return errors.Distinct().ToList();
    }
}
