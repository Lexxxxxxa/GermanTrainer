using GermanTrainer.Models;
using GermanTrainer.ViewModels.Base;

namespace GermanTrainer.ViewModels
{
    /// <summary>
    /// A word on control lists
    /// </summary>
    public class WordItemViewModel : ViewModelBase
    {
        private bool _isHidden;

        public WordItemViewModel(WordEntry entry)
        {
            Id = entry.Id;
            Type = entry.Type;
            Lesson = entry.Lesson;
            Article = entry.Article;
            Word = entry.Word;
            DisplayWord = entry.DisplayWord;
            Translation = entry.Translation;
            CorrectAnswers = entry.CorrectAnswers;
            WrongAnswers = entry.WrongAnswers;
            _isHidden = entry.IsHidden;
        }

        public int Id { get; }
        public WordType Type { get; }
        public int Lesson { get; }
        public string? Article { get; }
        public bool HasArticle => Type == WordType.Noun;
        public string Word { get; }
        public string DisplayWord { get; }
        public string Translation { get; }
        public int CorrectAnswers { get; }
        public int WrongAnswers { get; }

        public string StatsText => $"{CorrectAnswers}/{WrongAnswers}";

        public bool IsHidden
        {
            get => _isHidden;
            set
            {
                if (SetProperty(ref _isHidden, value))
                    OnPropertyChanged(nameof(ToggleHiddenText));
            }
        }

        public string ToggleHiddenText => IsHidden ? "👁️‍🗨️ Показать" : "👁️ Скрыть";
    }
}
