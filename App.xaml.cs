using System.Windows;
using System.Windows.Threading;
using GermanTrainer.Services;
using GermanTrainer.ViewModels;

namespace GermanTrainer
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DispatcherUnhandledException += OnDispatcherUnhandledException;
            ThemeManager.Initialize();

            var wordsService = new WordsService();
            try
            {
                wordsService.EnsureDatabaseCreated();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось подключиться к базе данных.\n\n" +
                    "Проверьте, что установлен SQL Server LocalDB и строка подключения в appsettings.json верна.\n\n" +
                    $"Подробности: {ex.Message}",
                    "Ошибка базы данных", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown(1);
                return;
            }

            var dialogService = new DialogService();
            var mainWindow = new MainWindow
            {
                DataContext = new MainViewModel(wordsService, dialogService)
            };

            MainWindow = mainWindow;
            mainWindow.Show();
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.Exception);
            MessageBox.Show($"Произошла ошибка:\n\n{e.Exception.Message}", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
