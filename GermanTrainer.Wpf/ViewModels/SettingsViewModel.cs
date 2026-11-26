using System.Collections.ObjectModel;
using System.Windows.Input;
using GermanTrainer.Core.Models;
using GermanTrainer.Core.Services;
using GermanTrainer.Infrastructure.Common;
using GermanTrainer.Wpf.Services;
using GermanTrainer.Wpf.ViewModels.Base;

namespace GermanTrainer.Wpf.ViewModels;

public sealed class LevelSettingsViewModel
{
    public LevelSettingsViewModel(string level, IEnumerable<WordEntry> words)
    {
        Level = CourseLesson.NormalizeLevel(level);
        var all = words.ToList();
        Modules = new(all.GroupBy(w => w.Lesson).OrderBy(g => g.Key).Select(g =>
            new CheckableItemViewModel<CourseLesson>(new(level, g.Key),
                $"Модуль {g.Key} · {g.Count(w => !w.IsHidden)} слов",
                AppSettings.IsModuleEnabled(level, g.Key))));
        Header = $"{Level} · {Modules.Count} модулей · {all.Count} записей";
        SelectAllCommand = new RelayCommand(() => SetAll(true));
        ClearCommand = new RelayCommand(() => SetAll(false));
    }
    public string Level { get; }
    public string Header { get; }
    public ObservableCollection<CheckableItemViewModel<CourseLesson>> Modules { get; }
    public ICommand SelectAllCommand { get; }
    public ICommand ClearCommand { get; }
    public void SetAll(bool selected) { foreach (var module in Modules) module.IsChecked = selected; }
}

/// <summary>
/// Draft selection is applied only after a successful atomic settings save
/// </summary>
public class SettingsViewModel : ViewModelBase
{
    private readonly IDialogService _dialogs;
    private readonly Action _changed;
    private string _status = "Изменения применяются после сохранения.";
    public SettingsViewModel(IWordsService words, IDialogService dialogs,
        Action onSettingsChanged, Action onWordsChanged)
    {
        _dialogs = dialogs;
        _changed = onSettingsChanged;
        WordTypes = new(Enum.GetValues<WordType>().Select(t =>
            new CheckableItemViewModel<WordType>(t, t.ToDisplayName(), AppSettings.IsWordTypeEnabled(t))));
        var entries = Enum.GetValues<WordType>().SelectMany(t => words.GetWords(t)).ToList();
        Levels = new(entries.GroupBy(w => CourseLesson.NormalizeLevel(w.Level)).OrderBy(g => g.Key)
            .Select(g => new LevelSettingsViewModel(g.Key, g)));
        foreach (var item in Levels.SelectMany(l => l.Modules))
            item.PropertyChanged += (_, _) => OnPropertyChanged(nameof(SelectionSummary));
        SaveCommand = new RelayCommand(Save);
        SelectAllCommand = new RelayCommand(() => { foreach (var level in Levels) level.SetAll(true); });
        ClearCommand = new RelayCommand(() => { foreach (var level in Levels) level.SetAll(false); });
    }
    public ObservableCollection<CheckableItemViewModel<WordType>> WordTypes { get; }
    public ObservableCollection<LevelSettingsViewModel> Levels { get; }
    public string SelectionSummary => $"Выбрано модулей: {Levels.Sum(l => l.Modules.Count(m => m.IsChecked))} из {Levels.Sum(l => l.Modules.Count)}";
    public string Status { get => _status; private set => SetProperty(ref _status, value); }
    public ICommand SaveCommand { get; }
    public ICommand SelectAllCommand { get; }
    public ICommand ClearCommand { get; }

    private void Save()
    {
        var types = WordTypes.Where(t => t.IsChecked).Select(t => t.Value.ToSettingsKey()).ToList();
        if (types.Count == 0)
        {
            _dialogs.ShowWarning("Выберите хотя бы один тип слова для переводов.", "Настройки");
            return;
        }
        var previousModules = AppSettings.EnabledModules?.ToList();
        var previousTypes = AppSettings.EnabledWordTypes.ToList();
        AppSettings.EnabledModules = Levels.SelectMany(l => l.Modules).Where(m => m.IsChecked)
            .Select(m => m.Value.Key).ToList();
        AppSettings.EnabledWordTypes = types;
        try { AppSettings.SaveSettings(); }
        catch (Exception ex)
        {
            AppSettings.EnabledModules = previousModules;
            AppSettings.EnabledWordTypes = previousTypes;
            Status = "Не удалось сохранить настройки. Прежние настройки не изменены.";
            _dialogs.ShowWarning($"{Status}\n{ex.Message}", "Ошибка сохранения");
            return;
        }
        _changed();
        Status = AppSettings.EnabledModules.Count == 0
            ? "Сохранено. Модули не выбраны: тренажёры не будут выдавать слова."
            : "Сохранено. Выбор будет применён при открытии тренажёра.";
    }
}
