namespace GermanTrainer.Models
{
    /// <summary>
    /// Adjectives without articles - for translation practice
    /// </summary>
    public class GermanAdjective
    {
        public int Id { get; set; }
        public int Lesson { get; set; }
        public string Word { get; set; } = string.Empty;
        public string Translation { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public int DifficultyCoefficient { get; set; } = 5;
        public int TimesShown { get; set; } = 0;
        public int CorrectAnswers { get; set; } = 0;
        public int WrongAnswers { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastShownAt { get; set; }
        public bool IsHidden { get; set; } = false;

        /// <summary>
        /// Percentage of correct answers
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