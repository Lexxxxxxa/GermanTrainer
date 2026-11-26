using System.Collections.ObjectModel;
using System.Windows.Input;
using GermanTrainer.Core.Models;
using GermanTrainer.Core.Services;
using GermanTrainer.Wpf.Services;
using GermanTrainer.Wpf.ViewModels.Base;

namespace GermanTrainer.Wpf.ViewModels;

public class TrainerArticlesViewModel : ViewModelBase, ITrainerViewModel
{
    private readonly IArticlesTrainingService _service;
    private readonly IDialogService _dialogService;

    private GermanNoun? _currentNoun;
    private string? _selectedArticle;
    private string? _feedback;
    private bool _isAnswered;
    private int _correctCount;
    private int _wrongCount;

    public TrainerArticlesViewModel(IArticlesTrainingService service, IDialogService dialogService)
    {
        _service = service;
        _dialogService = dialogService;

        ArticleOptions = new ObservableCollection<ArticleOptionViewModel>
        {
            new("der"),
            new("die"),
            new("das")
        };

        SelectArticleCommand = new RelayCommand<string>(SelectArticle, _ => !IsAnswered && CurrentNoun != null);
        CheckOrNextCommand = new RelayCommand(CheckOrNext, () => IsAnswered || SelectedArticle != null);
        ResetStatisticsCommand = new RelayCommand(() =>
        {
            if (_dialogService.Confirm("Сбросить счётчики текущей сессии артиклей?\nИстория обучения сохранится.", "Сброс статистики"))
            {
                CorrectCount = 0;
                WrongCount = 0;
            }
        });

        LoadNextQuestion();
    }

    #region Properties

    public ObservableCollection<ArticleOptionViewModel> ArticleOptions { get; }

    public GermanNoun? CurrentNoun
    {
        get => _currentNoun;
        private set
        {
            if (SetProperty(ref _currentNoun, value))
                OnPropertyChanged(nameof(CurrentNounDisplay));
        }
    }

    public string CurrentNounDisplay => CurrentNoun?.Word ?? "Загрузка...";

    public string? SelectedArticle
    {
        get => _selectedArticle;
        private set
        {
            if (SetProperty(ref _selectedArticle, value))
            {
                foreach (var option in ArticleOptions)
                    option.IsSelected = option.Article == value;
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

    #region Commands
    public ICommand SelectArticleCommand { get; }
    public ICommand CheckOrNextCommand { get; }
    public ICommand ResetStatisticsCommand { get; }

    #endregion

    public void SelectArticle(string? article)
    {
        if (!IsAnswered && article != null)
            SelectedArticle = article;
    }

    public void CheckAnswer()
    {
        if (IsAnswered || CurrentNoun == null || SelectedArticle == null)
            return;

        bool isCorrect = SelectedArticle == CurrentNoun.Article;
        _service.RecordAnswer(CurrentNoun, isCorrect);
        IsAnswered = true;
        var answerText = $"{CurrentNoun.Article} {CurrentNoun.Word}";
        var translation = string.IsNullOrEmpty(CurrentNoun.Translation) ? "" : $" ({CurrentNoun.Translation})";

        if (isCorrect)
        {
            CorrectCount++;
            Feedback = $"✓ Правильно! {answerText}{translation}";
        }
        else
        {
            WrongCount++;
            Feedback = $"✗ Неправильно. Правильный ответ: {answerText}{translation}";
        }

        CommandManager.InvalidateRequerySuggested();
    }

    public void NextQuestion() => LoadNextQuestion();

    public void ResetQuiz()
    {
        CorrectCount = 0;
        WrongCount = 0;
        LoadNextQuestion();
    }

    public void Refresh() => LoadNextQuestion();

    private void CheckOrNext()
    {
        if (IsAnswered)
            NextQuestion();
        else
            CheckAnswer();
    }

    private void LoadNextQuestion()
    {
        CurrentNoun = _service.GetNextNoun();
        SelectedArticle = null;
        IsAnswered = false;
        Feedback = CurrentNoun == null
            ? "Нет доступных слов для тренировки! Добавьте новые слова или снимите скрытие с существующих."
            : null;

        CommandManager.InvalidateRequerySuggested();
    }
}
