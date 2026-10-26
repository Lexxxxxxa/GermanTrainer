using System.Windows;
using GermanTrainer.ViewModels;
using GermanTrainer.Views;

namespace GermanTrainer.Services
{
    public interface IDialogService
    {
        void ShowInfo(string message, string title);
        void ShowWarning(string message, string title);
        bool Confirm(string message, string title);
        void ShowWordsManagement(WordsManagementViewModel viewModel);
    }

    public class DialogService : IDialogService
    {
        public void ShowInfo(string message, string title) =>
            Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

        public void ShowWarning(string message, string title) =>
            Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);

        public bool Confirm(string message, string title) =>
            Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;

        public void ShowWordsManagement(WordsManagementViewModel viewModel)
        {
            var window = new WordsManagementWindow
            {
                DataContext = viewModel,
                Owner = GetOwner()
            };

            viewModel.CloseRequested += window.Close;
            try
            {
                window.ShowDialog();
            }
            finally
            {
                viewModel.CloseRequested -= window.Close;
                viewModel.NotifyClosed();
            }
        }

        private static MessageBoxResult Show(string message, string title, MessageBoxButton buttons, MessageBoxImage image)
        {
            var owner = GetOwner();
            return owner != null
                ? MessageBox.Show(owner, message, title, buttons, image)
                : MessageBox.Show(message, title, buttons, image);
        }

        private static Window? GetOwner() =>
            Application.Current?.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
            ?? Application.Current?.MainWindow;
    }
}
