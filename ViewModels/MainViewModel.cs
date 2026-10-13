using System.Windows.Input;
using GermanTrainer.Services;
using GermanTrainer.ViewModels.Base;

namespace GermanTrainer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly WordsService _wordsService;
        private readonly IDialogService _dialogService;
        private readonly MenuViewModel _menu;

        private TrainerArticlesViewModel? _articles;
        private TrainerTranslationViewModel? _translation;

        private ViewModelBase _currentPage;

        public MainViewModel(WordsService wordsService, IDialogService dialogService)
        {
            _wordsService = wordsService;
            _dialogService = dialogService;

            _menu = new MenuViewModel(ShowArticles, ShowTranslation, ShowSettings, dialogService);
            _currentPage = _menu;

            BackToMenuCommand = new RelayCommand(ShowMenu);
            ManageWordsCommand = new RelayCommand(
                () => (CurrentPage as ITrainerViewModel)?.OpenWordsManagement(),
                () => IsTrainerPage);
            ToggleThemeCommand = new RelayCommand(ThemeManager.ToggleTheme);

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
                }
            }
        }

        public bool IsMenuPage => CurrentPage is MenuViewModel;

        public bool IsTrainerPage => CurrentPage is ITrainerViewModel;

        public string ThemeIcon => ThemeManager.GetThemeIcon();

        public string ThemeToolTip => ThemeManager.GetThemeTooltip();

        public ICommand BackToMenuCommand { get; }
        public ICommand ManageWordsCommand { get; }
        public ICommand ToggleThemeCommand { get; }

        private void ShowMenu() => CurrentPage = _menu;

        private void ShowArticles()
        {
            _articles ??= new TrainerArticlesViewModel(new TrainerArticlesService(), _wordsService, _dialogService);
            CurrentPage = _articles;
        }

        private void ShowTranslation()
        {
            _translation ??= new TrainerTranslationViewModel(
                new TrainerTranslationService(),
                _wordsService,
                _dialogService);

            CurrentPage = _translation;
        }

        private void ShowSettings()
        {
            CurrentPage = new SettingsViewModel(_wordsService, _dialogService,
                onSettingsChanged: RefreshTrainers,
                onWordsChanged: RefreshTrainers);
        }

        private void RefreshTrainers()
        {
            _articles?.Refresh();
            _translation?.Refresh();
        }
    }
}
