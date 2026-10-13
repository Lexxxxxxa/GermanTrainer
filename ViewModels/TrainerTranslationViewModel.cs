using System.Collections.ObjectModel;
using System.Windows.Input;
using GermanTrainer.Models;
using GermanTrainer.Services;
using GermanTrainer.ViewModels.Base;

namespace GermanTrainer.ViewModels
{
    public class TrainerTranslationViewModel : ViewModelBase, ITrainerViewModel
    {
        private const int WrongAnswersCount = 3;

        private readonly TrainerTranslationService _service;
        private readonly WordsService _wordsService;
        private readonly IDialogService _dialogService;

        private object? _currentWord;
        private AnswerOptionViewModel? _selectedAnswer;
        private string? _correctAnswer;
        private string? _feedback;
        private bool _isAnswered;
        private int _correctCount;
        private int _wrongCount;

        public TrainerTranslationViewModel(TrainerTranslationService service, WordsService wordsService, IDialogService dialogService)
        {
            _service = service;
            _wordsService = wordsService;
            _dialogService = dialogService;

            SelectAnswerCommand = new RelayCommand<AnswerOptionViewModel>(SelectAnswer, _ => !IsAnswered);
            CheckOrNextCommand = new RelayCommand(CheckOrNext, () => IsAnswered || SelectedAnswer != null);
            ManageWordsCommand = new RelayCommand(OpenWordsManagement);

            LoadNextQuestion();
        }

        #region Свойства

        public ObservableCollection<AnswerOptionViewModel> AnswerOptions { get; } = new();

        public object? CurrentWord
        {
            get => _currentWord;
            private set
            {
                if (SetProperty(ref _currentWord, value))
                    OnPropertyChanged(nameof(CurrentWordDisplay));
            }
        }

        public string CurrentWordDisplay => CurrentWord switch
        {
            GermanNoun n => $"{n.Article} {n.Word}",
            GermanAdjective a => a.Word,
            GermanVerb v => v.Word,
            _ => "Загрузка..."
        };

        public AnswerOptionViewModel? SelectedAnswer
        {
            get => _selectedAnswer;
            private set
            {
                if (SetProperty(ref _selectedAnswer, value))
                {
                    foreach (var option in AnswerOptions)
                        option.IsSelected = ReferenceEquals(option, value);
                }
            }
        }

        public string? Feedback
        {
            get => _feedback;
            private set => SetProperty(ref _feedback, value);
        }

        public bool IsAnswered
        {
            get => _isAnswered;
            private set
            {
                if (SetProperty(ref _isAnswered, value))
                {
                    OnPropertyChanged(nameof(CanAnswer));
                    OnPropertyChanged(nameof(ActionButtonText));
                    OnPropertyChanged(nameof(ActionButtonToolTip));
                }
            }
        }

        public bool CanAnswer => !IsAnswered;

        public string ActionButtonText => IsAnswered ? "→ Дальше" : "✓ Проверить";

        public string ActionButtonToolTip => IsAnswered ? "Показать следующее слово" : "Проверить ответ";

        public int CorrectCount
        {
            get => _correctCount;
            private set
            {
                if (SetProperty(ref _correctCount, value))
                    OnPropertyChanged(nameof(SuccessPercentage));
            }
        }

        public int WrongCount
        {
            get => _wrongCount;
            private set
            {
                if (SetProperty(ref _wrongCount, value))
                    OnPropertyChanged(nameof(SuccessPercentage));
            }
        }

        public string SuccessPercentage
        {
            get
            {
                var total = CorrectCount + WrongCount;
                if (total == 0) return "0%";
                return $"{Math.Round((double)CorrectCount / total * 100, 1)}%";
            }
        }

        #endregion

        #region Команды

        public ICommand SelectAnswerCommand { get; }
        public ICommand CheckOrNextCommand { get; }
        public ICommand ManageWordsCommand { get; }

        #endregion

        public void SelectAnswer(AnswerOptionViewModel? answer)
        {
            if (!IsAnswered && answer != null)
                SelectedAnswer = answer;
        }

        public void CheckAnswer()
        {
            if (CurrentWord == null || SelectedAnswer == null)
                return;

            IsAnswered = true;
            bool isCorrect = SelectedAnswer.Text == _correctAnswer;

            if (isCorrect)
            {
                CorrectCount++;
                Feedback = $"✓ Правильно! {_correctAnswer}";
            }
            else
            {
                WrongCount++;
                Feedback = $"✗ Неправильно. Правильный ответ: {_correctAnswer}";
            }

            _service.RecordAnswer(CurrentWord, isCorrect);
        }

        public void NextQuestion() => LoadNextQuestion();

        public void ResetQuiz()
        {
            CorrectCount = 0;
            WrongCount = 0;
            LoadNextQuestion();
        }

        public void Refresh()
        {
            _service.ReloadWords();
            LoadNextQuestion();
        }

        public void OpenWordsManagement()
        {
            var viewModel = new WordsManagementViewModel(WordsManagementMode.Translation, _wordsService, Refresh);
            _dialogService.ShowWordsManagement(viewModel);
        }

        private void CheckOrNext()
        {
            if (IsAnswered)
                NextQuestion();
            else
                CheckAnswer();
        }

        private void LoadNextQuestion()
        {
            CurrentWord = _service.GetNextWord();
            _correctAnswer = GetTranslation(CurrentWord);

            SelectedAnswer = null;
            AnswerOptions.Clear();
            foreach (var option in GenerateAnswerOptions(_correctAnswer))
                AnswerOptions.Add(new AnswerOptionViewModel(option));

            IsAnswered = false;
            Feedback = CurrentWord == null
                ? "Нет доступных слов для тренировки! Выберите уроки и типы слов в настройках."
                : null;

            CommandManager.InvalidateRequerySuggested();
        }

        private static string? GetTranslation(object? word) => word switch
        {
            GermanNoun n => n.Translation,
            GermanAdjective a => a.Translation,
            GermanVerb v => v.Translation,
            _ => null
        };

        private List<string> GenerateAnswerOptions(string? correctAnswer)
        {
            if (string.IsNullOrEmpty(correctAnswer))
                return new List<string>();

            var options = new List<string> { correctAnswer };
            options.AddRange(_service.GenerateWrongAnswers(correctAnswer, WrongAnswersCount));

            var random = new Random();
            return options.OrderBy(_ => random.Next()).ToList();
        }
    }
}
