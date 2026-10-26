using System.Collections.ObjectModel;
using System.Windows.Input;
using GermanTrainer.Models;
using GermanTrainer.Services;
using GermanTrainer.ViewModels.Base;

namespace GermanTrainer.ViewModels;

public class VocabularyViewModel : ViewModelBase
{
    private readonly IVocabularyService _service;
    private readonly IDialogService _dialogs;
    private readonly Action _changed;
    private List<WordDraft> _all = new();
    private WordDraft? _selected;
    private WordDraft _draft = new();
    private string _search = "";
    private string _status = "Выберите слово или создайте новое.";
    private bool _busy;
    private ConstructionDraft? _selectedConstruction;
    private string _lessonText = "1";
    public VocabularyViewModel(IVocabularyService service, IDialogService dialogs, Action changed)
    {
        _service = service; _dialogs = dialogs; _changed = changed;
        NewCommand = new RelayCommand(() => { Selected = null; Draft = new(); Constructions.Clear(); Status = "Новое слово."; }, () => IsIdle);
        SaveCommand = new AsyncRelayCommand(Save, HandleError, () => IsIdle);
        DeleteCommand = new AsyncRelayCommand(Delete, HandleError, () => IsIdle && Draft.Id != 0);
        RefreshCommand = new AsyncRelayCommand(Reload, HandleError, () => IsIdle);
        AddConstructionCommand = new RelayCommand(() =>
            Constructions.Add(new() { VerbDisplay = Draft.Word, Meaning = "", Preposition = "auf" }), () => IsIdle && IsVerb);
        RemoveConstructionCommand = new RelayCommand(() =>
        { if (SelectedConstruction != null) Constructions.Remove(SelectedConstruction); }, () => IsIdle && SelectedConstruction != null);
    }
    public ObservableCollection<WordDraft> Words { get; } = new();
    public ObservableCollection<ConstructionDraft> Constructions { get; } = new();
    public IReadOnlyList<WordType> Types { get; } = Enum.GetValues<WordType>();
    public IReadOnlyList<GrammaticalCase> Cases { get; } = Enum.GetValues<GrammaticalCase>();
    public IReadOnlyList<string> Prepositions => VocabularyValidation.Prepositions;
    public WordType SelectedType
    {
        get => Draft.Type;
        set { Draft.Type = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsVerb)); OnPropertyChanged(nameof(IsNoun)); }
    }
    public bool IsVerb => Draft.Type == WordType.Verb;
    public bool IsNoun => Draft.Type == WordType.Noun;
    public bool CanChangeType => Draft.Id == 0;
    public string LessonText { get => _lessonText; set => SetProperty(ref _lessonText, value); }
    public bool IsBusy { get => _busy; private set { SetProperty(ref _busy, value); OnPropertyChanged(nameof(IsIdle)); } }
    public bool IsIdle => !IsBusy;
    public string Status { get => _status; private set => SetProperty(ref _status, value); }
    public string Search { get => _search; set { if (SetProperty(ref _search, value)) Filter(); } }
    public WordDraft Draft
    {
        get => _draft;
        private set
        {
            SetProperty(ref _draft, value);
            LessonText = value.Lesson.ToString();
            OnPropertyChanged(nameof(SelectedType)); OnPropertyChanged(nameof(IsVerb));
            OnPropertyChanged(nameof(IsNoun)); OnPropertyChanged(nameof(CanChangeType));
        }
    }
    public WordDraft? Selected
    {
        get => _selected;
        set
        {
            if (!SetProperty(ref _selected, value) || value == null) return;
            Draft = new WordDraft
            {
                Id = value.Id,
                Type = value.Type,
                Word = value.Word,
                Translation = value.Translation,
                Article = value.Article,
                PluralForm = value.PluralForm,
                Lesson = value.Lesson,
                Level = value.Level,
                IsHidden = value.IsHidden
            };
            Constructions.Clear();
            foreach (var c in value.Constructions) Constructions.Add(new()
            { Id = c.Id, VerbDisplay = c.VerbDisplay, Meaning = c.Meaning, Preposition = c.Preposition, Case = c.Case });
            Status = $"Редактирование: {value.Word}";
        }
    }
    public ConstructionDraft? SelectedConstruction { get => _selectedConstruction; set => SetProperty(ref _selectedConstruction, value); }
    public ICommand NewCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand AddConstructionCommand { get; }
    public ICommand RemoveConstructionCommand { get; }

    public async Task Reload()
    {
        IsBusy = true;
        try { _all = (await Task.Run(_service.GetAll)).ToList(); Filter(); Status = $"В словаре: {_all.Count} записей."; }
        finally { IsBusy = false; }
    }
    private void Filter()
    {
        Words.Clear();
        foreach (var w in _all.Where(w => string.IsNullOrWhiteSpace(Search) ||
            w.Word.Contains(Search, StringComparison.OrdinalIgnoreCase) || w.Translation.Contains(Search, StringComparison.OrdinalIgnoreCase)))
            Words.Add(w);
    }
    private async Task Save()
    {
        if (!int.TryParse(LessonText, out var lesson) || lesson is < 1 or > 999)
        { Status = "Урок: введите целое число от 1 до 999."; return; }
        Draft.Lesson = lesson;
        Draft.Constructions = IsVerb ? Constructions.ToList() : new();
        var errors = VocabularyValidation.Validate(Draft);
        if (errors.Count > 0) { Status = string.Join("\n", errors); return; }
        IsBusy = true;
        try
        {
            await Task.Run(() => _service.Save(Draft));
            _changed(); await Reload();
            Selected = _all.FirstOrDefault(w => w.Id == Draft.Id && w.Type == Draft.Type);
            Status = "Сохранено. Тренажёры обновятся при открытии.";
        }
        finally { IsBusy = false; }
    }
    private async Task Delete()
    {
        var details = IsVerb ? "\nКонструкции этого глагола также будут удалены." : "";
        if (!_dialogs.Confirm($"Удалить «{Draft.Word}»?{details}", "Удаление слова")) return;
        IsBusy = true;
        try
        {
            await Task.Run(() => _service.Delete(Draft.Type, Draft.Id));
            _changed(); Selected = null; Draft = new(); Constructions.Clear();
            await Reload(); Status = "Слово удалено.";
        }
        finally { IsBusy = false; }
    }
    private void HandleError(Exception ex)
    {
        System.Diagnostics.Debug.WriteLine(ex);
        Status = ex is ArgumentException ? ex.Message : $"Не удалось выполнить операцию: {ex.Message}";
        IsBusy = false;
    }
}
