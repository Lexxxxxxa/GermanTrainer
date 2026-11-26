using System.Windows.Input;
using GermanTrainer.Core.Services;
using GermanTrainer.Wpf.Common;
using GermanTrainer.Wpf.Services;
using GermanTrainer.Wpf.ViewModels.Base;

namespace GermanTrainer.Wpf.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IWordsService _wordsService;
    private readonly IDialogService _dialogService;
    private readonly MenuViewModel _menu;
    private readonly IArticlesTrainingService _articlesService;
    private readonly ITranslationTrainingService _translationService;
    private readonly IPrepositionsTrainingService _prepositionsService;
    private readonly IVocabularyService _vocabularyService;

    private TrainerArticlesViewModel? _articles;
    private TrainerTranslationViewModel? _translation;
    private TrainerPrepositionsViewModel? _prepositions;
    private bool _articlesDirty;
    private bool _translationDirty;
    private bool _prepositionsDirty;

    private ViewModelBase _currentPage;
    private ViewModelBase? _vocabularyReturnPage;

    public MainViewModel(IWordsService wordsService, IDialogService dialogService,
        IArticlesTrainingService articlesService, ITranslationTrainingService translationService,
        IPrepositionsTrainingService prepositionsService, IVocabularyService vocabularyService)
    {
        _wordsService = wordsService;
        _dialogService = dialogService;
        _articlesService = articlesService;
        _translationService = translationService;
        _prepositionsService = prepositionsService;
        _vocabularyService = vocabularyService;

        _menu = new MenuViewModel(ShowArticles, ShowTranslation, ShowSettings, ShowPrepositions);
        _currentPage = _menu;

        BackToMenuCommand = new RelayCommand(ShowMenu);
        BackToTrainerCommand = new RelayCommand(ReturnToTrainer,
            () => CanReturnToTrainer && CurrentPage is VocabularyViewModel { IsIdle: true });
        ToggleThemeCommand = new RelayCommand(ThemeManager.ToggleTheme);
        OpenVocabularyCommand = new RelayCommand(ShowVocabulary, () => CurrentPage is not VocabularyViewModel);

        ThemeManager.OnThemeChanged += () =>
        {
            OnPropertyChanged(nameof(ThemeIcon));
            OnPropertyChanged(nameof(ThemeToolTip));
        };
    }

    public ViewModelBase CurrentPage
    {
        get => _currentPage;
        private set
        {
            if (SetProperty(ref _currentPage, value))
            {
                OnPropertyChanged(nameof(IsMenuPage));
                OnPropertyChanged(nameof(IsTrainerPage));
                OnPropertyChanged(nameof(CanReturnToTrainer));
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }

    public bool IsMenuPage => CurrentPage is MenuViewModel;

    public bool IsTrainerPage => CurrentPage is ITrainerViewModel;
    public bool CanReturnToTrainer => CurrentPage is VocabularyViewModel && _vocabularyReturnPage is ITrainerViewModel;

    public string ThemeIcon => ThemeManager.GetThemeIcon();

    public string ThemeToolTip => ThemeManager.GetThemeTooltip();

    public ICommand BackToMenuCommand { get; }
    public ICommand BackToTrainerCommand { get; }
    public ICommand ToggleThemeCommand { get; }
    public ICommand OpenVocabularyCommand { get; }

    private async void ShowVocabulary()
    {
        _vocabularyReturnPage = CurrentPage is ITrainerViewModel ? CurrentPage : null;
        var scope = CurrentPage switch
        {
            TrainerArticlesViewModel => VocabularyScope.Articles,
            TrainerTranslationViewModel => VocabularyScope.Translation,
            TrainerPrepositionsViewModel => VocabularyScope.VerbPrepositions,
            _ => VocabularyScope.All
        };
        var page = new VocabularyViewModel(_vocabularyService, _dialogService, RefreshTrainers, scope);
        CurrentPage = page;
        try { await page.Reload(); }
        catch (Exception ex) { _dialogService.ShowWarning($"Не удалось загрузить словарь:\n{ex.Message}", "Ошибка"); }
    }

    private void ShowMenu() { _vocabularyReturnPage = null; CurrentPage = _menu; }

    private void ReturnToTrainer()
    {
        switch (_vocabularyReturnPage)
        {
            case TrainerArticlesViewModel: ShowArticles(); break;
            case TrainerTranslationViewModel: ShowTranslation(); break;
            case TrainerPrepositionsViewModel: ShowPrepositions(); break;
        }
        _vocabularyReturnPage = null;
    }

    private void ShowArticles()
    {
        if (_articles == null)
            _articles = new TrainerArticlesViewModel(_articlesService, _dialogService);
        else if (_articlesDirty) _articles.Refresh();
        _articlesDirty = false;
        CurrentPage = _articles;
    }

    private void ShowTranslation()
    {
        if (_translation == null)
        {
            _translationService.ReloadWords();
            _translation = new TrainerTranslationViewModel(_translationService, _dialogService);
        }
        else if (_translationDirty) _translation.Refresh();
        _translationDirty = false;

        CurrentPage = _translation;
    }

    private void ShowSettings()
    {
        CurrentPage = new SettingsViewModel(_wordsService, _dialogService,
            onSettingsChanged: RefreshTrainers,
            onWordsChanged: RefreshTrainers);
    }

    private void ShowPrepositions()
    {
        if (_prepositions == null)
            _prepositions = new TrainerPrepositionsViewModel(_prepositionsService, _dialogService);
        else if (_prepositionsDirty) _prepositions.Refresh();
        _prepositionsDirty = false;
        CurrentPage = _prepositions;
    }

    private void RefreshTrainers()
    {
        _articlesDirty = _translationDirty = _prepositionsDirty = true;
    }
}
