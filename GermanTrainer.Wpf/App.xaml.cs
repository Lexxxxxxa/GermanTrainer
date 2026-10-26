using System.Windows;
using System.Windows.Threading;
using GermanTrainer.Data;
using GermanTrainer.Services;
using GermanTrainer.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GermanTrainer;

public partial class App : Application
{
    private ServiceProvider? _services;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        DispatcherUnhandledException += OnUnhandled;
        ThemeManager.Initialize();
        try
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json").Build();
            var connection = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("В appsettings.json нет DefaultConnection.");
            var options = new DbContextOptionsBuilder<TrainerDbContext>().UseSqlServer(connection).Options;
            var services = new ServiceCollection();
            services.AddSingleton<ITrainerDbContextFactory>(new TrainerDbContextFactory(options));
            services.AddSingleton<LearningHistoryService>();
            services.AddSingleton<IWordsService, WordsService>();
            services.AddSingleton<IArticlesTrainingService, TrainerArticlesService>();
            services.AddSingleton<ITranslationTrainingService, TrainerTranslationService>();
            services.AddSingleton<IPrepositionsTrainingService, TrainerPrepositionsService>();
            services.AddSingleton<IVocabularyService, VocabularyService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<MainViewModel>();
            _services = services.BuildServiceProvider(new ServiceProviderOptions
            { ValidateOnBuild = true, ValidateScopes = true });
            _services.GetRequiredService<IWordsService>().EnsureDatabaseCreated();
            MainWindow = new MainWindow { DataContext = _services.GetRequiredService<MainViewModel>() };
            MainWindow.Show();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            MessageBox.Show("Не удалось запустить приложение.\nПроверьте SQL Server LocalDB и appsettings.json.\n\n"
                + ex.Message, "Ошибка запуска", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown(1);
        }
    }

    private void OnUnhandled(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine(e.Exception);
        MessageBox.Show($"Операция не выполнена:\n\n{e.Exception.Message}", "Ошибка",
            MessageBoxButton.OK, MessageBoxImage.Warning);
        e.Handled = true;
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _services?.Dispose();
        base.OnExit(e);
    }
}
