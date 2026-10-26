using System.Windows.Input;
using GermanTrainer.Models;
using GermanTrainer.Services;
using GermanTrainer.ViewModels.Base;

namespace GermanTrainer.ViewModels
{
    public enum WordsManagementMode
    {
        Articles,
        Translation,
        VerbPrepositions
    }

    public class WordLessonGroupViewModel
    {
        public WordLessonGroupViewModel(int lesson, IReadOnlyList<WordItemViewModel> words)
        {
            Lesson = lesson;
            Words = words;
        }

        public int Lesson { get; }
        public string Header => $"📖 Урок {Lesson}";
        public IReadOnlyList<WordItemViewModel> Words { get; }
    }

    public class WordSectionViewModel
    {
        public WordSectionViewModel(string? title, bool isFirst, IReadOnlyList<WordLessonGroupViewModel> lessons)
        {
            Title = title;
            IsFirst = isFirst;
            Lessons = lessons;
        }

        public string? Title { get; }
        public bool IsFirst { get; }
        public IReadOnlyList<WordLessonGroupViewModel> Lessons { get; }
    }

    public class WordsManagementViewModel : ViewModelBase
    {
        private readonly IWordsService _wordsService;
        private readonly Action _onClosed;
        private bool _closedNotified;

        public WordsManagementViewModel(WordsManagementMode mode, IWordsService wordsService, Action onClosed)
        {
            _wordsService = wordsService;
            _onClosed = onClosed;

            ToggleHiddenCommand = new RelayCommand<WordItemViewModel>(ToggleHidden);
            DoneCommand = new RelayCommand(() => CloseRequested?.Invoke());

            Sections = BuildSections(mode);
            EmptyMessage = Sections.Count > 0
                ? null
                : mode == WordsManagementMode.Articles
                    ? "Нет существительных в выбранных уроках"
                    : "Нет слов в выбранных уроках и типах";
        }

        public event Action? CloseRequested;

        public IReadOnlyList<WordSectionViewModel> Sections { get; }

        public string? EmptyMessage { get; }

        public ICommand ToggleHiddenCommand { get; }
        public ICommand DoneCommand { get; }

        public void NotifyClosed()
        {
            if (_closedNotified) return;
            _closedNotified = true;
            _onClosed();
        }

        private void ToggleHidden(WordItemViewModel? item)
        {
            if (item == null) return;

            var newState = _wordsService.ToggleHidden(item.Type, item.Id);
            if (newState.HasValue)
                item.IsHidden = newState.Value;
        }

        private List<WordSectionViewModel> BuildSections(WordsManagementMode mode)
        {
            var lessons = AppSettings.EnabledLessons.Count > 0
                ? AppSettings.EnabledLessons.ToList()
                : null;

            var types = mode switch
            {
                WordsManagementMode.Articles => new[] { WordType.Noun },
                WordsManagementMode.VerbPrepositions => new[] { WordType.Verb },
                _ => Enum.GetValues<WordType>().Where(AppSettings.IsWordTypeEnabled).ToArray()
            };

            var sections = new List<WordSectionViewModel>();
            foreach (var type in types)
            {
                var words = _wordsService.GetWords(type, lessons);
                if (words.Count == 0) continue;

                var lessonGroups = words
                    .GroupBy(w => w.Lesson)
                    .OrderBy(g => g.Key)
                    .Select(g => new WordLessonGroupViewModel(g.Key, g.Select(w => new WordItemViewModel(w)).ToList()))
                    .ToList();

                var title = mode == WordsManagementMode.Articles ? null : $"{type.ToIcon()} {type.ToDisplayName()}";
                sections.Add(new WordSectionViewModel(title, sections.Count == 0, lessonGroups));
            }

            return sections;
        }
    }
}
