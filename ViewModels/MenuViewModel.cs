using System.Windows.Input;
using GermanTrainer.Services;
using GermanTrainer.ViewModels.Base;

namespace GermanTrainer.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        public MenuViewModel(Action openArticles, Action openTranslation, Action openSettings, IDialogService dialogService)
        {
            OpenArticlesCommand = new RelayCommand(openArticles);
            OpenTranslationCommand = new RelayCommand(openTranslation);
            OpenSettingsCommand = new RelayCommand(openSettings);
            OpenPluralCommand = new RelayCommand(() =>
                dialogService.ShowInfo("Режим множественного числа в разработке!", "Скоро"));
        }

        public ICommand OpenArticlesCommand { get; }
        public ICommand OpenTranslationCommand { get; }
        public ICommand OpenPluralCommand { get; }
        public ICommand OpenSettingsCommand { get; }
    }
}
