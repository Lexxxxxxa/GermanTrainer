namespace GermanTrainer.Core.Models;

///
/// <summary>Shared representation for translation choice generation, independent of EF and WPF
/// </summary>
public record TranslationWord(string Key, string German, string Russian);
