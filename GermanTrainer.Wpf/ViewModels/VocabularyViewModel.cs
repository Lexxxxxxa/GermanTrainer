using System.Collections.ObjectModel;
using System.Windows.Input;
using GermanTrainer.Core.Models;
using GermanTrainer.Core.Services;
using GermanTrainer.Infrastructure.Common;
using GermanTrainer.Wpf.Services;
using GermanTrainer.Wpf.ViewModels.Base;

namespace GermanTrainer.Wpf.ViewModels;

/// <summary>
/// One screen for dictionary lookup and management of existing words
/// </summary>
public class VocabularyViewModel : ViewModelBase
{
    private readonly IVocabularyService _service;
    private readonly IDialogService _dialogs;
    private readonly Action _changed;
    private readonly VocabularyScope _scope;
    private List<WordDraft> _all = new();
    private WordDraft? _selected;
    private WordDraft _draft = new();
    private ConstructionDraft? _selectedConstruction;
    private string _search = "", _levelFilter = "Все уровни", _lessonFilter = "Все модули",
        _typeFilter = "Все типы", _articleFilter = "Все артикли", _visibilityFilter = "Все слова",
        _constructionFilter = "Любые конструкции", _lessonText = "1";
    private string _status = "Выберите слово для просмотра или редактирования.";
    private bool _busy, _onlySelectedModules;

    public VocabularyViewModel(IVocabularyService service, IDialogService dialogs, Action changed,
        VocabularyScope scope = VocabularyScope.All)
    {
        _service = service; _dialogs = dialogs; _changed = changed; _scope = scope;
        _onlySelectedModules = scope != VocabularyScope.All;
        SaveCommand = new AsyncRelayCommand(Save, HandleError, () => IsIdle && HasSelection);
        DeleteCommand = new AsyncRelayCommand(Delete, HandleError, () => IsIdle && HasSelection);
        RefreshCommand = new AsyncRelayCommand(Reload, HandleError, () => IsIdle);
        ResetFiltersCommand = new RelayCommand(ResetFilters, () => IsIdle);
        AddConstructionCommand = new RelayCommand(() =>
            Constructions.Add(new() { VerbDisplay = Draft.Word, Preposition = "auf" }), () => IsIdle && HasSelection && IsVerb);
        RemoveConstructionCommand = new RelayCommand(() =>
        { if (SelectedConstruction != null) Constructions.Remove(SelectedConstruction); }, () => IsIdle && SelectedConstruction != null);
    }
    public string Title => _scope switch
    {
        VocabularyScope.Articles => "Словарь · артикли",
        VocabularyScope.Translation => "Словарь · переводы",
        VocabularyScope.VerbPrepositions => "Словарь · глаголы с предлогами",
        _ => "Словарь и управление словами"
    };
    public string ContextHint => _scope == VocabularyScope.VerbPrepositions
        ? "Здесь только глаголы с конструкциями. Скрытые глаголы можно снова включить."
        : "Поиск, редактирование и скрытие существующих слов. Новые слова добавляются в C#-сидах.";
    public ObservableCollection<WordDraft> Words { get; } = new();
    public ObservableCollection<ConstructionDraft> Constructions { get; } = new();
    public IReadOnlyList<string> LevelOptions => new[] { "Все уровни" }
        .Concat(_all.Select(w => CourseLesson.NormalizeLevel(w.Level)).Distinct().Order()).ToList();
    public IReadOnlyList<string> LessonOptions => new[] { "Все модули" }
        .Concat(_all.Select(w => w.Lesson).Distinct().Order().Select(n => n.ToString())).ToList();
    public IReadOnlyList<string> TypeOptions => new[] { "Все типы" }.Concat(Enum.GetValues<WordType>().Select(t => t.ToDisplayName())).ToList();
    public IReadOnlyList<string> ArticleOptions { get; } = ["Все артикли", "der", "die", "das", "der/die"];
    public IReadOnlyList<string> VisibilityOptions { get; } = ["Все слова", "Доступные", "Скрытые"];
    public IReadOnlyList<string> ConstructionOptions { get; } = ["Любые конструкции", "С предлогами", "Без предлогов"];
    public IReadOnlyList<GrammaticalCase> Cases { get; } = Enum.GetValues<GrammaticalCase>();
    public IReadOnlyList<string> Prepositions => VocabularyValidation.Prepositions;
    public string TypeLabel => Draft.Type.ToDisplayName();
    public bool IsVerb => Draft.Type == WordType.Verb;
    public bool IsNoun => Draft.Type == WordType.Noun;
    public bool HasSelection => Selected != null && Draft.Id != 0;
    public string LessonText { get => _lessonText; set => SetProperty(ref _lessonText, value); }
    public bool IsBusy
    {
        get => _busy;
        private set { SetProperty(ref _busy, value); OnPropertyChanged(nameof(IsIdle)); CommandManager.InvalidateRequerySuggested(); }
    }
    public bool IsIdle => !IsBusy;
    public string Status { get => _status; private set => SetProperty(ref _status, value); }
    public string Search { get => _search; set { if (SetProperty(ref _search, value)) Filter(); } }
    public string LevelFilter { get => _levelFilter; set { if (SetProperty(ref _levelFilter, value)) Filter(); } }
    public string LessonFilter { get => _lessonFilter; set { if (SetProperty(ref _lessonFilter, value)) Filter(); } }
    public string TypeFilter { get => _typeFilter; set { if (SetProperty(ref _typeFilter, value)) Filter(); } }
    public string ArticleFilter { get => _articleFilter; set { if (SetProperty(ref _articleFilter, value)) Filter(); } }
    public string VisibilityFilter { get => _visibilityFilter; set { if (SetProperty(ref _visibilityFilter, value)) Filter(); } }
    public string ConstructionFilter { get => _constructionFilter; set { if (SetProperty(ref _constructionFilter, value)) Filter(); } }
    public bool OnlySelectedModules { get => _onlySelectedModules; set { if (SetProperty(ref _onlySelectedModules, value)) Filter(); } }
    public WordDraft Draft
    {
        get => _draft;
        private set
        {
            SetProperty(ref _draft, value);
            LessonText = value.Lesson.ToString();
            OnPropertyChanged(nameof(TypeLabel)); OnPropertyChanged(nameof(IsVerb)); OnPropertyChanged(nameof(IsNoun));
            OnPropertyChanged(nameof(HasSelection));
        }
    }
    public WordDraft? Selected
    {
        get => _selected;
        set
        {
            if (!SetProperty(ref _selected, value)) return;
            Constructions.Clear(); SelectedConstruction = null;
            Draft = value == null ? new() : new()
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
            if (value != null)
                foreach (var c in value.Constructions) Constructions.Add(new()
                { Id = c.Id, VerbDisplay = c.VerbDisplay, Meaning = c.Meaning, Preposition = c.Preposition, Case = c.Case });
            CommandManager.InvalidateRequerySuggested();
        }
    }
    public ConstructionDraft? SelectedConstruction { get => _selectedConstruction; set => SetProperty(ref _selectedConstruction, value); }
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand ResetFiltersCommand { get; }
    public ICommand AddConstructionCommand { get; }
    public ICommand RemoveConstructionCommand { get; }

    public async Task Reload()
    {
        var id = Selected?.Id; var type = Selected?.Type;
        IsBusy = true;
        try
        {
            var loaded = (await Task.Run(_service.GetAll)).ToList();
            Selected = null; _all = loaded;
            OnPropertyChanged(nameof(LevelOptions)); OnPropertyChanged(nameof(LessonOptions));
            Filter();
            Selected = Words.FirstOrDefault(w => w.Id == id && w.Type == type);
        }
        finally { IsBusy = false; }
    }
    private void Filter()
    {
        var query = new VocabularyQuery
        {
            Search = Search,
            Scope = _scope,
            Level = LevelFilter == "Все уровни" ? null : LevelFilter,
            Lesson = int.TryParse(LessonFilter, out var lesson) ? lesson : null,
            Type = Enum.GetValues<WordType>().Where(t => t.ToDisplayName() == TypeFilter).Select(t => (WordType?)t).FirstOrDefault(),
            Article = ArticleFilter == "Все артикли" ? null : ArticleFilter,
            IsHidden = VisibilityFilter == "Скрытые" ? true : VisibilityFilter == "Доступные" ? false : null,
            HasConstructions = ConstructionFilter == "С предлогами" ? true : ConstructionFilter == "Без предлогов" ? false : null
        };
        var result = _all.Where(w => query.Matches(w) &&
            (!OnlySelectedModules || AppSettings.IsModuleEnabled(w.Level, w.Lesson)) &&
            (_scope != VocabularyScope.Translation || AppSettings.IsWordTypeEnabled(w.Type))).ToList();
        Words.Clear();
        foreach (var word in result) Words.Add(word);
        if (Selected != null && !result.Contains(Selected)) Selected = null;
        Status = $"Найдено: {Words.Count} из {_all.Count}. Новые слова добавляются через сиды.";
    }
    private void ResetFilters()
    {
        Search = ""; LevelFilter = "Все уровни"; LessonFilter = "Все модули"; TypeFilter = "Все типы";
        ArticleFilter = "Все артикли"; VisibilityFilter = "Все слова"; ConstructionFilter = "Любые конструкции";
        OnlySelectedModules = _scope != VocabularyScope.All;
    }
    private async Task Save()
    {
        if (!HasSelection) return;
        if (!int.TryParse(LessonText, out var lesson) || lesson is < 1 or > 999)
        { Status = "Модуль: введите целое число от 1 до 999."; return; }
        Draft.Lesson = lesson;
        Draft.Level = Draft.Level.Trim().ToUpperInvariant();
        Draft.Constructions = IsVerb ? Constructions.ToList() : new();
        var errors = VocabularyValidation.Validate(Draft);
        if (errors.Count > 0) { Status = string.Join("\n", errors); return; }
        IsBusy = true;
        try
        {
            var edited = Draft;
            await Task.Run(() => _service.Save(edited));
            _changed(); await Reload();
            Status = "Сохранено. При возвращении в тренировку изменения будут учтены.";
        }
        finally { IsBusy = false; }
    }
    private async Task Delete()
    {
        if (!HasSelection || !_dialogs.Confirm($"Удалить «{Draft.Word}»?\nКонструкции глагола также будут удалены.", "Удаление слова")) return;
        IsBusy = true;
        try
        {
            await Task.Run(() => _service.Delete(Draft.Type, Draft.Id));
            _changed(); Selected = null;
            await Reload(); Status = "Слово удалено.";
        }
        finally { IsBusy = false; }
    }
    private void HandleError(Exception ex)
    {
        System.Diagnostics.Debug.WriteLine(ex);
        Status = $"Не удалось выполнить операцию: {ex.Message}";
        IsBusy = false;
    }
}
