using GermanTrainer.Models;
using GermanTrainer.Services;
using GermanTrainer.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GermanTrainer.ViewModels
{
    ///<summary>
    ///A group of words of the same type within a lesson
    ///</summary>
    public class WordTypeGroupViewModel : ViewModelBase
    {
        public WordTypeGroupViewModel(WordType type, bool isFirst, IReadOnlyList<WordItemViewModel> words)
        {
            Type = type;
            IsFirst = isFirst;
            Words = new ObservableCollection<WordItemViewModel>(words);
            Words.CollectionChanged += (_, _) => OnPropertyChanged(nameof(Header));
        }

        public WordType Type { get; }
        public bool IsFirst { get; }
        public string Header => $"{Type.ToIcon()} {Type.ToDisplayName()} ({Words.Count})";
        public ObservableCollection<WordItemViewModel> Words { get; }
    }

    ///<summary>
    ///A lesson on the settings page covering all types of words
    ///</summary>
    public class LessonGroupViewModel
    {
        public LessonGroupViewModel(int lesson, IReadOnlyList<WordTypeGroupViewModel> typeGroups)
        {
            Lesson = lesson;
            TypeGroups = typeGroups;
        }

        public int Lesson { get; }
        public string Title => $"Урок {Lesson}";
        public IReadOnlyList<WordTypeGroupViewModel> TypeGroups { get; }
    }

    ///<summary>
    ///Settings page: selecting lessons and word types + managing words
    ///</summary>
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IWordsService _wordsService;
        private readonly IDialogService _dialogService;
        private readonly Action _onSettingsChanged;
        private readonly Action _onWordsChanged;

        public SettingsViewModel(IWordsService wordsService, IDialogService dialogService,
            Action onSettingsChanged, Action onWordsChanged)
        {
            _wordsService = wordsService;
            _dialogService = dialogService;
            _onSettingsChanged = onSettingsChanged;
            _onWordsChanged = onWordsChanged;

            WordTypes = new ObservableCollection<CheckableItemViewModel<WordType>>(
                Enum.GetValues<WordType>().Select(t => new CheckableItemViewModel<WordType>(
                    t, GetWordTypeLabel(t), AppSettings.IsWordTypeEnabled(t))));

            SaveCommand = new RelayCommand(Save);
            DeleteWordCommand = new RelayCommand<WordItemViewModel>(DeleteWord);

            LoadLessons();
            LoadWordGroups();
        }

        public ObservableCollection<CheckableItemViewModel<WordType>> WordTypes { get; }

        public ObservableCollection<CheckableItemViewModel<int>> Lessons { get; } = new();

        public ObservableCollection<LessonGroupViewModel> LessonGroups { get; } = new();

        public ICommand SaveCommand { get; }
        public ICommand DeleteWordCommand { get; }

        ///<summary>
        ///List of lessons with tick marks and the number of visible words
        ///</summary>
        private void LoadLessons()
        {
            var previousChecks = Lessons.ToDictionary(l => l.Value, l => l.IsChecked);

            var allLessons = _wordsService.GetAllLessons();
            var counts = _wordsService.GetVisibleWordCountByLesson();

            Lessons.Clear();
            foreach (var lesson in allLessons)
            {
                var isChecked = previousChecks.TryGetValue(lesson, out var wasChecked)
                    ? wasChecked
                    : AppSettings.IsLessonEnabled(lesson);

                var count = counts.TryGetValue(lesson, out var c) ? c : 0;
                Lessons.Add(new CheckableItemViewModel<int>(lesson, $"📄 Урок {lesson} ({count} слов)", isChecked));
            }

        }

        ///<summary>
        ///Words grouped by lessons and types
        ///</summary>
        private void LoadWordGroups()
        {
            var allLessons = Lessons.Select(l => l.Value).ToList();
            var wordsByType = Enum.GetValues<WordType>()
                .ToDictionary(t => t, t => _wordsService.GetWords(t));

            LessonGroups.Clear();
            foreach (var lesson in allLessons)
            {
                var typeGroups = new List<WordTypeGroupViewModel>();
                foreach (var type in Enum.GetValues<WordType>())
                {
                    var words = wordsByType[type]
                        .Where(w => w.Lesson == lesson)
                        .Select(w => new WordItemViewModel(w))
                        .ToList();

                    if (words.Count > 0)
                        typeGroups.Add(new WordTypeGroupViewModel(type, typeGroups.Count == 0, words));
                }

                LessonGroups.Add(new LessonGroupViewModel(lesson, typeGroups));
            }
        }

        private void Save()
        {
            var previousTypes = AppSettings.EnabledWordTypes.ToList();
            var previousLessons = AppSettings.EnabledLessons.ToList();
            AppSettings.EnabledWordTypes.Clear();
            AppSettings.EnabledWordTypes.AddRange(WordTypes.Where(t => t.IsChecked).Select(t => t.Value.ToSettingsKey()));

            AppSettings.EnabledLessons.Clear();
            AppSettings.EnabledLessons.AddRange(Lessons.Where(l => l.IsChecked).Select(l => l.Value));

            try { AppSettings.SaveSettings(); }
            catch
            {
                AppSettings.EnabledWordTypes = previousTypes;
                AppSettings.EnabledLessons = previousLessons;
                throw;
            }
            _onSettingsChanged();

            var enabledTypes = AppSettings.EnabledWordTypes.Count == 0
                ? "Все типы"
                : string.Join(", ", WordTypes.Where(t => t.IsChecked).Select(t => t.Value.ToDisplayName()));

            var enabledLessons = AppSettings.EnabledLessons.Count == 0
                ? "Все"
                : string.Join(", ", AppSettings.EnabledLessons);

            _dialogService.ShowInfo($"✓ Сохранено!\n\nТипы слов: {enabledTypes}\nУроки: {enabledLessons}", "Готово");
        }

        private void DeleteWord(WordItemViewModel? item)
        {
            if (item == null) return;

            if (!_dialogService.Confirm("Вы уверены, что хотите удалить это слово?", "Подтверждение удаления"))
                return;

            _wordsService.DeleteWord(item.Type, item.Id);

            foreach (var group in LessonGroups.SelectMany(l => l.TypeGroups))
            {
                if (group.Words.Remove(item))
                    break;
            }

            LoadLessons();
            _onWordsChanged();

            _dialogService.ShowInfo("Слово удалено!", "Успешно");
        }

        private static string GetWordTypeLabel(WordType type) => type switch
        {
            WordType.Noun => "📘 Существительные (Nouns)",
            WordType.Adjective => "💚 Прилагательные (Adjectives)",
            WordType.Verb => "🔴 Глаголы (Verbs)",
            _ => type.ToString()
        };
    }
}
