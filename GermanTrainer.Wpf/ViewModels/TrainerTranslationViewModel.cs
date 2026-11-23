using System.Collections.ObjectModel;
using System.Windows.Input;
using GermanTrainer.Models;
using GermanTrainer.Services;
using GermanTrainer.ViewModels.Base;

namespace GermanTrainer.ViewModels;

public class TrainerTranslationViewModel : ViewModelBase, ITrainerViewModel
{
    private readonly ITranslationTrainingService _service;
    private readonly IWordsService _wordsService;
    private readonly IDialogService _dialogs;
    private readonly SessionScore _forwardScore = new();
    private readonly SessionScore _reverseScore = new();
    private bool _reverse;
    private TranslationWord? _word;
    private AnswerOptionViewModel? _selected;
    private bool _answered;
    private string? _feedback;

    public TrainerTranslationViewModel(ITranslationTrainingService service, IWordsService wordsService, IDialogService dialogService)
    {
        _service = service; _wordsService = wordsService; _dialogs = dialogService;
        SelectAnswerCommand = new RelayCommand<AnswerOptionViewModel>(SelectAnswer, o => !IsAnswered && o != null && _word != null);
        CheckOrNextCommand = new RelayCommand(CheckOrNext, () => IsAnswered || SelectedAnswer != null);
        ManageWordsCommand = new RelayCommand(OpenWordsManagement);
        SwitchDirectionCommand = new RelayCommand(SwitchDirection);
        ResetStatisticsCommand = new RelayCommand(ResetStatistics);
        LoadNextQuestion();
    }

    private SessionScore Score => _reverse ? _reverseScore : _forwardScore;
    public int CorrectCount => Score.Correct;
    public int WrongCount => Score.Wrong;
    public string SuccessPercentage => Score.Percentage;
    public string DirectionText => _reverse ? "Русский → Немецкий" : "Немецкий → Русский";
    public string SwitchDirectionText => _reverse ? "Переключить на DE → RU" : "Переключить на RU → DE";
    public string CurrentWordDisplay => _word == null ? "Нет доступных слов" : TranslationChoices.Prompt(_word, _reverse);
    public ObservableCollection<AnswerOptionViewModel> AnswerOptions { get; } = new();
    public AnswerOptionViewModel? SelectedAnswer
    {
        get => _selected;
        private set
        {
            if (SetProperty(ref _selected, value))
                foreach (var option in AnswerOptions) option.IsSelected = ReferenceEquals(option, value);
        }
    }
    public string? Feedback { get => _feedback; private set => SetProperty(ref _feedback, value); }
    public bool IsAnswered
    {
        get => _answered;
        private set
        {
            if (SetProperty(ref _answered, value))
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
    public ICommand SelectAnswerCommand { get; }
    public ICommand CheckOrNextCommand { get; }
    public ICommand ManageWordsCommand { get; }
    public ICommand ResetStatisticsCommand { get; }
    public ICommand SwitchDirectionCommand { get; }

    public void SelectAnswer(AnswerOptionViewModel? answer)
    {
        if (!IsAnswered && answer != null) SelectedAnswer = answer;
    }
    public void CheckAnswer()
    {
        if (IsAnswered || _word == null || SelectedAnswer == null) return;
        var correctAnswer = TranslationChoices.Answer(_word, _reverse);
        var correct = SelectedAnswer.Text == correctAnswer;

        _service.RecordAnswer(_word, _reverse, correct);
        Score.Record(correct);
        NotifyScore();
        IsAnswered = true;
        Feedback = correct ? $"✓ Правильно! {correctAnswer}" : $"✗ Правильный ответ: {correctAnswer}";
        CommandManager.InvalidateRequerySuggested();
    }
    public void NextQuestion() => LoadNextQuestion();
    public void Refresh() { _service.ReloadWords(); LoadNextQuestion(); }
    public void OpenWordsManagement() => _dialogs.ShowWordsManagement(
        new WordsManagementViewModel(WordsManagementMode.Translation, _wordsService, Refresh));
    private void CheckOrNext() { if (IsAnswered) NextQuestion(); else CheckAnswer(); }
    private void SwitchDirection()
    {
        _reverse = !_reverse;
        OnPropertyChanged(nameof(DirectionText));
        OnPropertyChanged(nameof(SwitchDirectionText));
        NotifyScore();
        LoadNextQuestion();
    }
    private void ResetStatistics()
    {
        if (!_dialogs.Confirm($"Сбросить счётчики текущей сессии: {DirectionText}?\nИстория обучения сохранится.", "Сброс статистики")) return;
        Score.Reset();
        NotifyScore();
    }
    private void NotifyScore()
    {
        OnPropertyChanged(nameof(CorrectCount));
        OnPropertyChanged(nameof(WrongCount));
        OnPropertyChanged(nameof(SuccessPercentage));
    }
    private void LoadNextQuestion()
    {
        _word = _service.GetNextWord(_reverse);
        OnPropertyChanged(nameof(CurrentWordDisplay));
        SelectedAnswer = null;
        AnswerOptions.Clear();
        if (_word != null)
            foreach (var option in _service.GetOptions(_word, _reverse)) AnswerOptions.Add(new(option));
        IsAnswered = false;
        Feedback = _word == null ? "Нет доступных слов. Проверьте уроки, типы слов и скрытие." :
            AnswerOptions.Count == 1 ? "В выбранном наборе нет других однозначных вариантов ответа." : null;
        CommandManager.InvalidateRequerySuggested();
    }
}
