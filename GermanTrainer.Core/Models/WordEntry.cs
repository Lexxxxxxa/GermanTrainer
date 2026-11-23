namespace GermanTrainer.Models
{
    public record WordEntry(
        int Id,
        WordType Type,
        int Lesson,
        string? Article,
        string Word,
        string Translation,
        bool IsHidden,
        int CorrectAnswers,
        int WrongAnswers)
    {

        public string DisplayWord => string.IsNullOrEmpty(Article) ? Word : $"{Article} {Word}";
    }
}
