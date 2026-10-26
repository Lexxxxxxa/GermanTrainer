using System.Windows.Input;
using GermanTrainer.Services;
using GermanTrainer.ViewModels.Base;

namespace GermanTrainer.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        public MenuViewModel(Action openArticles, Action openTranslation, Action openSettings, Action openPrepositions)
        {
            OpenArticlesCommand = new RelayCommand(openArticles);
            OpenTranslationCommand = new RelayCommand(openTranslation);
            OpenSettingsCommand = new RelayCommand(openSettings);
            OpenPrepositionsCommand = new RelayCommand(openPrepositions);
        }

        public ICommand OpenArticlesCommand { get; }
        public ICommand OpenTranslationCommand { get; }
        public ICommand OpenPrepositionsCommand { get; }
        public ICommand OpenSettingsCommand { get; }
    }
}
