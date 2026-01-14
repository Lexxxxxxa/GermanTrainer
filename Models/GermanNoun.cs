namespace GermanTrainer.Models
{
    /// <summary>
    /// Существительные с артиклями - для тренировки артиклей
    /// </summary>
    public class GermanNoun
    {
        public int Id { get; set; }
        public int Lesson { get; set; }
        public string Word { get; set; } = string.Empty;
        public string Article { get; set; } = string.Empty;
        public string? PluralForm { get; set; }
        public string Translation { get; set; } = string.Empty;
        public int DifficultyCoefficient { get; set; } = 5;
        public int TimesShown { get; set; } = 0;
        public int CorrectAnswers { get; set; } = 0;
        public int WrongAnswers { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastShownAt { get; set; }
        public bool IsHidden { get; set; } = false; 

        /// <summary>
        /// Процент правильных ответов
        /// </summary>
        public double SuccessRate
        {
            get
            {
                if (TimesShown == 0) return 0;
                return Math.Round((double)CorrectAnswers / TimesShown * 100, 1);
            }
        }
    }
}