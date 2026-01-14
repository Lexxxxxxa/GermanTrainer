namespace GermanTrainer.Models
{
    /// <summary>
    /// Прилагательные без артиклей - для тренировки переводов
    /// </summary>
    public class GermanAdjective
    {
        public int Id { get; set; }
        public int Lesson { get; set; }
        public string Word { get; set; } = string.Empty;
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