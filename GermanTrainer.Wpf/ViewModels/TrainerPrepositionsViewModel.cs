using System.Collections.ObjectModel;
using System.Windows.Input;
using GermanTrainer.Models;
using GermanTrainer.Services;
using GermanTrainer.ViewModels.Base;

namespace GermanTrainer.ViewModels;

public class TrainerPrepositionsViewModel : ViewModelBase, ITrainerViewModel
{
    private readonly IPrepositionsTrainingService _service;
    private readonly IWordsService _words;
    private readonly IDialogService _dialogs;
    private readonly SessionScore _score = new();
    private VerbPreposition? _current;
    private string? _selectedPreposition;
    private GrammaticalCase? _selectedCase;
    private bool _answered;
    private string? _feedback;

    public TrainerPrepositionsViewModel(IPrepositionsTrainingService service, IWordsService words, IDialogService dialogs)
    {
        _service = service; _words = words; _dialogs = dialogs;
        PrepositionOptions = new(VocabularyValidation.Prepositions
            .Select(p => new AnswerOptionViewModel(p)));
        CaseOptions = new(Enum.GetValues<GrammaticalCase>().Select(c => new AnswerOptionViewModel(c.ToString())));
        SelectPrepositionCommand = new RelayCommand<AnswerOptionViewModel>(SelectPreposition, o => !IsAnswered && _current != null && o != null);
        SelectCaseCommand = new RelayCommand<AnswerOptionViewModel>(SelectCase, o => !IsAnswered && _current != null && o != null);
        CheckOrNextCommand = new RelayCommand(CheckOrNext, () => IsAnswered ||
            (_current != null && _selectedPreposition != null && _selectedCase.HasValue));
        ResetStatisticsCommand = new RelayCommand(ResetStatistics);
        ManageWordsCommand = new RelayCommand(OpenWordsManagement);
        Refresh();
    }

    public ObservableCollection<AnswerOptionViewModel> PrepositionOptions { get; }
    public ObservableCollection<AnswerOptionViewModel> CaseOptions { get; }
    public string CurrentVerbDisplay => _current?.VerbDisplay ?? "Нет доступных конструкций";
    public string Meaning => _current?.Meaning ?? "Выберите другие уроки или снимите скрытие с глаголов.";
    public int CorrectCount => _score.Correct;
    public int WrongCount => _score.Wrong;
    public string SuccessPercentage => _score.Percentage;
    public string? Feedback { get => _feedback; private set => SetProperty(ref _feedback, value); }
    public bool IsAnswered
    {
        get => _answered;
        private set
        {
            if (SetProperty(ref _answered, value)) OnPropertyChanged(nameof(ActionButtonText));
        }
    }
    public string ActionButtonText => IsAnswered ? "→ Дальше" : "✓ Проверить";
    public ICommand SelectPrepositionCommand { get; }
    public ICommand SelectCaseCommand { get; }
    public ICommand CheckOrNextCommand { get; }
    public ICommand ResetStatisticsCommand { get; }
    public ICommand ManageWordsCommand { get; }

    private void SelectPreposition(AnswerOptionViewModel? option)
    {
        if (IsAnswered || option == null) return;
        _selectedPreposition = option.Text;
        foreach (var p in PrepositionOptions) p.IsSelected = ReferenceEquals(p, option);
        CommandManager.InvalidateRequerySuggested();
    }
    private void SelectCase(AnswerOptionViewModel? option)
    {
        if (IsAnswered || option == null) return;
        _selectedCase = Enum.Parse<GrammaticalCase>(option.Text);
        foreach (var c in CaseOptions) c.IsSelected = ReferenceEquals(c, option);
        CommandManager.InvalidateRequerySuggested();
    }
    private void CheckOrNext()
    {
        if (IsAnswered) { Refresh(); return; }
        if (_current == null || _selectedPreposition == null || !_selectedCase.HasValue) return;
        var result = PrepositionAnswer.Check(_current, _selectedPreposition, _selectedCase.Value);
        var correct = result.Preposition && result.Case;
        _service.RecordAnswer(_current, correct);
        _score.Record(correct);
        NotifyScore();
        IsAnswered = true;
        var error = correct ? "✓ Правильно!" :
            !result.Preposition && !result.Case ? "✗ Ошибка в предлоге и падеже." :
            !result.Preposition ? "✗ Ошибка в предлоге; падеж выбран верно." :
            "✗ Предлог выбран верно; ошибка в падеже.";
        Feedback = $"{error}\n{_current.VerbDisplay} + {_current.Preposition} + {_current.Case}";
        CommandManager.InvalidateRequerySuggested();
    }
    public void Refresh()
    {
        _current = _service.GetNext();
        _selectedCase = null; _selectedPreposition = null;
        foreach (var o in PrepositionOptions.Concat(CaseOptions)) o.IsSelected = false;
        IsAnswered = false; Feedback = null;
        OnPropertyChanged(nameof(CurrentVerbDisplay));
        OnPropertyChanged(nameof(Meaning));
        CommandManager.InvalidateRequerySuggested();
    }
    public void OpenWordsManagement() => _dialogs.ShowWordsManagement(
        new WordsManagementViewModel(WordsManagementMode.VerbPrepositions, _words, Refresh));
    private void ResetStatistics()
    {
        if (!_dialogs.Confirm("Сбросить счётчики текущей сессии глаголов с предлогами?\nИстория обучения сохранится.", "Сброс статистики")) return;
        _score.Reset(); NotifyScore();
    }
    private void NotifyScore()
    {
        OnPropertyChanged(nameof(CorrectCount));
        OnPropertyChanged(nameof(WrongCount));
        OnPropertyChanged(nameof(SuccessPercentage));
    }
}
