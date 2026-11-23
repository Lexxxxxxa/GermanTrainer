using System.Windows;
using GermanTrainer.Models;
using GermanTrainer.Services;
using GermanTrainer.ViewModels;
using GermanTrainer.ViewModels.Base;
using GermanTrainer.Views;

namespace GermanTrainer.Wpf.Tests;

public sealed class ViewModelTests
{
    [Fact]
    public void NavigationOpensTranslationAndKeepsSession()
    {
        var fake = new FakeServices();
        var main = fake.Main();
        var menu = Assert.IsType<MenuViewModel>(main.CurrentPage);
        menu.OpenTranslationCommand.Execute(null);
        var page = Assert.IsType<TrainerTranslationViewModel>(main.CurrentPage);
        main.BackToMenuCommand.Execute(null);
        menu.OpenTranslationCommand.Execute(null);
        Assert.Same(page, main.CurrentPage);
        Assert.Equal(1, fake.ReloadCount);
    }
    [Fact]
    public void TranslationSwitchSeparatesScoresAndResetLeavesHistory()
    {
        var fake = new FakeServices();
        var page = new TrainerTranslationViewModel(fake, fake, fake);
        page.SelectAnswer(page.AnswerOptions[0]); page.CheckAnswer(); page.CheckAnswer();
        Assert.Equal(1, fake.Recorded);
        Assert.Equal(1, page.CorrectCount);
        page.SwitchDirectionCommand.Execute(null);
        Assert.Equal(0, page.CorrectCount);
        page.SelectAnswer(page.AnswerOptions[1]); page.CheckAnswer();
        Assert.Equal(1, page.WrongCount);
        page.ResetStatisticsCommand.Execute(null);
        Assert.Equal(0, page.WrongCount);
        Assert.Equal(2, fake.Recorded);
        page.SwitchDirectionCommand.Execute(null);
        Assert.Equal(1, page.CorrectCount);
    }
    [Fact]
    public void ArticleResetDoesNotRecordOrAdvanceQuestion()
    {
        var fake = new FakeServices();
        var page = new TrainerArticlesViewModel(fake, fake, fake);
        page.SelectArticle("der"); page.CheckAnswer(); page.CheckAnswer();
        page.ResetStatisticsCommand.Execute(null);
        Assert.Equal(0, page.CorrectCount);
        Assert.Equal(1, fake.Recorded);
        Assert.True(page.IsAnswered);
    }
    [Fact]
    public void PrepositionsRequireBothSelectionsAndResetIsSessionOnly()
    {
        var fake = new FakeServices();
        var page = new TrainerPrepositionsViewModel(fake, fake, fake);
        Assert.False(page.CheckOrNextCommand.CanExecute(null));
        page.SelectPrepositionCommand.Execute(page.PrepositionOptions.Single(p => p.Text == "auf"));
        Assert.False(page.CheckOrNextCommand.CanExecute(null));
        page.SelectCaseCommand.Execute(page.CaseOptions.Single(c => c.Text == "Akkusativ"));
        Assert.True(page.CheckOrNextCommand.CanExecute(null));
        page.CheckOrNextCommand.Execute(null);
        Assert.Equal(1, page.CorrectCount);
        page.ResetStatisticsCommand.Execute(null);
        Assert.Equal(0, page.CorrectCount);
        Assert.Equal(1, fake.Recorded);
    }
    [Fact]
    public void CanceledResetKeepsScore()
    {
        var fake = new FakeServices { Confirmed = false };
        var page = new TrainerArticlesViewModel(fake, fake, fake);
        page.SelectArticle("der"); page.CheckAnswer();
        page.ResetStatisticsCommand.Execute(null);
        Assert.Equal(1, page.CorrectCount);
    }
    [Fact]
    public void FailedSaveDoesNotCountAnswer()
    {
        var fake = new FakeServices { FailRecord = true };
        var page = new TrainerTranslationViewModel(fake, fake, fake);
        page.SelectAnswer(page.AnswerOptions[0]);
        Assert.Throws<InvalidOperationException>(page.CheckAnswer);
        Assert.False(page.IsAnswered);
        Assert.Equal(0, page.CorrectCount);
    }
    [Fact]
    public void DisabledCommandDoesNotExecute()
    {
        var count = 0;
        new RelayCommand(() => count++, () => false).Execute(null);
        new RelayCommand<string>(_ => count++, _ => false).Execute("anything");
        Assert.Equal(0, count);
    }

    [Fact]
    public void XamlResourcesAndAllPagesLoadOnStaThread()
    {
        Exception? failure = null;
        var thread = new Thread(() =>
        {
            try
            {
                var app = new App();
                app.InitializeComponent();

                var fake = new FakeServices();
                var main = fake.Main();
                var window = new MainWindow { DataContext = main };

                object[] pages =
                [
                    new MenuView { DataContext = main.CurrentPage },
                new ArticlesTrainerView { DataContext = new TrainerArticlesViewModel(fake, fake, fake) },
                new TranslationTrainerView { DataContext = new TrainerTranslationViewModel(fake, fake, fake) },
                new PrepositionsTrainerView { DataContext = new TrainerPrepositionsViewModel(fake, fake, fake) },
                new SettingsView { DataContext = new SettingsViewModel(fake, fake, () => { }, () => { }) },
                new VocabularyView { DataContext = new VocabularyViewModel(fake, fake, () => { }) }
                ];

                var asmName = typeof(App).Assembly.GetName().Name;

                foreach (var file in new[] { "DarkTheme.xaml", "LightTheme.xaml" })
                {
                    var theme = new ResourceDictionary
                    {
                        Source = new Uri($"/{asmName};component/Themes/{file}", UriKind.Relative)
                    };
                    app.Resources.MergedDictionaries[0] = theme;

                    foreach (var element in pages.Cast<FrameworkElement>().Append(window))
                    {
                        element.Measure(new Size(1360, 880));
                        element.Arrange(new Rect(0, 0, 1360, 880));
                        element.UpdateLayout();

                        Assert.NotNull(element.FindResource("BackgroundBrush"));
                        Assert.NotNull(element.FindResource("FlatButton"));
                    }
                }

                app.Shutdown();
            }
            catch (Exception ex)
            {
                failure = ex;
            }
        });

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();

        Assert.Null(failure);
    }
}

    internal sealed class FakeServices : IWordsService, IDialogService, IArticlesTrainingService,
    ITranslationTrainingService, IPrepositionsTrainingService, IVocabularyService
{
    public bool Confirmed = true;
    public bool FailRecord;
    public int Recorded, ReloadCount;
    public MainViewModel Main() => new(this, this, this, this, this, this);
    public void EnsureDatabaseCreated() { }
    public List<int> GetAllLessons() => [1];
    public Dictionary<int, int> GetVisibleWordCountByLesson() => new() { [1] = 3 };
    public List<WordEntry> GetWords(WordType type, IReadOnlyCollection<int>? lessons = null) => [];
    public bool? ToggleHidden(WordType type, int id) => true;
    public bool DeleteWord(WordType type, int id) => true;
    public GermanNoun GetNextNoun() => new() { Id = 1, Word = "Test", Article = "der", Translation = "тест" };
    public TranslationWord GetNextWord(bool reverse) => new("Noun:1", "der Test", "тест");
    public IReadOnlyList<string> GetOptions(TranslationWord word, bool reverse) => [TranslationChoices.Answer(word, reverse), "wrong"];
    public void ReloadWords() => ReloadCount++;
    private void Record() { if (FailRecord) throw new InvalidOperationException("Test failure"); Recorded++; }
    public void RecordAnswer(GermanNoun noun, bool isCorrect) => Record();
    public void RecordAnswer(TranslationWord word, bool reverse, bool correct) => Record();
    public VerbPreposition GetNext() => new() { Id = 1, VerbDisplay = "warten", Meaning = "ждать", Preposition = "auf", Case = GrammaticalCase.Akkusativ };
    public void RecordAnswer(VerbPreposition item, bool correct) => Record();
    public void ShowInfo(string message, string title) { }
    public void ShowWarning(string message, string title) { }
    public bool Confirm(string message, string title) => Confirmed;
    public void ShowWordsManagement(WordsManagementViewModel viewModel) { }
    public IReadOnlyList<WordDraft> GetAll() => [];
    public void Save(WordDraft draft) => draft.Id = 1;
    public void Delete(WordType type, int id) { }
}
