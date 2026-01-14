using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using GermanTrainer.Data;
using GermanTrainer.Models;
using GermanTrainer.ViewModels;
using GermanTrainer.Services;
using GermanTrainer;

namespace GermanTrainer
{
    public partial class MainWindow : Window
    {
        private TrainerArticlesViewModel? _articlesViewModel;
        private TrainerTranslationViewModel? _translationViewModel;
        private Window? _wordsManagementModal;
        private string _currentTrainerMode = "";

        public MainWindow()
        {
            InitializeComponent();
            InitializeDatabase();
            ApplyTheme();
            ThemeManager.OnThemeChanged += ApplyTheme;
            ShowMenuPage();
        }

        private void InitializeDatabase()
        {
            using var context = new TrainerDbContext();
            context.Database.EnsureCreated();
        }

        private void ApplyTheme()
        {
            RootGrid.Background = ThemeManager.GetBackground();
            ApplyThemeToMenuPage();
            ApplyThemeToArticlePage();
            ApplyThemeToSettingsPage();
            ThemeButton.Content = ThemeManager.GetThemeIcon();
            ThemeButton.ToolTip = ThemeManager.GetThemeTooltip();
        }

        private void ApplyThemeToMenuPage()
        {
            if (MenuPage == null) return;
            MenuHeaderBorder.Background = ThemeManager.GetSurface();
            MenuTitle.Foreground = ThemeManager.GetText();
            MenuSubtitle.Foreground = ThemeManager.GetTextSecondary();

            foreach (var border in FindVisualChildren<Border>(MenuPage))
            {
                if (border.Tag?.ToString() == "Card")
                {
                    border.Background = ThemeManager.GetSurface();
                }
            }

            foreach (var textBlock in FindVisualChildren<TextBlock>(MenuPage))
            {
                if (textBlock.Tag?.ToString() == "CardTitle")
                {
                    textBlock.Foreground = ThemeManager.GetText();
                }
                else if (textBlock.Tag?.ToString() == "CardSubtitle")
                {
                    textBlock.Foreground = ThemeManager.GetTextSecondary();
                }
            }
        }

        private void ApplyThemeToArticlePage()
        {
            if (ArticlePage == null) return;
            ArticleHeader.Background = ThemeManager.GetSurface();
            ArticleQuestionText.Foreground = ThemeManager.GetText();
            ArticleFeedbackText.Foreground = ThemeManager.GetText();
            ArticleCorrectBadge.Background = ThemeManager.GetCorrectBg();
            ArticleWrongBadge.Background = ThemeManager.GetWrongBg();
            ArticlePercentBadge.Background = ThemeManager.GetPercentBg();
        }

        private void ApplyThemeToSettingsPage()
        {
            if (SettingsPage == null) return;
            SettingsHeader.Background = ThemeManager.GetSurface();
            SettingsTitle.Foreground = ThemeManager.GetText();
        }

        private void ToggleTheme_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.ToggleTheme();
        }

        // ===== ИСПРАВЛЕННЫЕ МЕТОДЫ НАВИГАЦИИ =====

        private void ShowMenuPage()
        {
            MenuPage.Visibility = Visibility.Visible;
            ArticlePage.Visibility = Visibility.Collapsed;
            SettingsPage.Visibility = Visibility.Collapsed;
            BackButton.Visibility = Visibility.Collapsed;
            ManageWordsButtonArticles.Visibility = Visibility.Collapsed;
            ManageWordsButtonTranslation.Visibility = Visibility.Collapsed;
            CheckAnswerButtonArticles.Visibility = Visibility.Collapsed;
            CheckAnswerButtonTranslation.Visibility = Visibility.Collapsed;
        }

        private void ShowArticlePage()
        {
            MenuPage.Visibility = Visibility.Collapsed;
            ArticlePage.Visibility = Visibility.Visible;
            SettingsPage.Visibility = Visibility.Collapsed;
            BackButton.Visibility = Visibility.Visible;
            ManageWordsButtonArticles.Visibility = Visibility.Visible;
            ManageWordsButtonTranslation.Visibility = Visibility.Collapsed;

            _currentTrainerMode = "articles";

            if (_articlesViewModel == null)
            {
                _articlesViewModel = new TrainerArticlesViewModel();
                ArticlePage.DataContext = _articlesViewModel;
            }

            UpdateCheckButtonState();
        }

        private void ShowTranslationPage()
        {
            MenuPage.Visibility = Visibility.Collapsed;
            ArticlePage.Visibility = Visibility.Collapsed;
            SettingsPage.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Visible;
            ManageWordsButtonArticles.Visibility = Visibility.Collapsed;
            ManageWordsButtonTranslation.Visibility = Visibility.Visible;
            SettingsTitle.Text = "💬 Режим переводов";

            _currentTrainerMode = "translation";

            if (_translationViewModel == null)
            {
                _translationViewModel = new TrainerTranslationViewModel();
            }

            LoadTranslationContent();
            UpdateCheckButtonState();
        }

        private void ShowSettingsPage()
        {
            MenuPage.Visibility = Visibility.Collapsed;
            ArticlePage.Visibility = Visibility.Collapsed;
            SettingsPage.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Visible;
            ManageWordsButtonArticles.Visibility = Visibility.Collapsed;
            ManageWordsButtonTranslation.Visibility = Visibility.Collapsed;
            SettingsTitle.Text = "⚙️ Настройки и управление словами";

            CheckAnswerButtonArticles.Visibility = Visibility.Collapsed;
            CheckAnswerButtonTranslation.Visibility = Visibility.Collapsed;
            LoadSettingsContent();
        }

        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            ShowMenuPage();
        }

        private void ArticleMode_Click(object sender, MouseButtonEventArgs e)
        {
            ShowArticlePage();
        }

        private void TranslationMode_Click(object sender, MouseButtonEventArgs e)
        {
            ShowTranslationPage();
        }

        private void PluralMode_Click(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Режим множественного числа в разработке!", "Скоро", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Settings_Click(object sender, MouseButtonEventArgs e)
        {
            ShowSettingsPage();
        }

        private void ShowWordsManagementModal()
        {
            if (_articlesViewModel == null && _translationViewModel == null)
            {
                MessageBox.Show("Пожалуйста, выберите режим тренировки сначала.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _wordsManagementModal = new Window
            {
                Title = "🛠️ Управление словами",
                Width = 600,
                Height = 600,
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = ResizeMode.CanResize,
                Background = ThemeManager.GetBackground(),
                Foreground = ThemeManager.GetText()
            };

            var content = CreateWordsManagementContent();
            _wordsManagementModal.Content = content;

            _wordsManagementModal.ShowDialog();
        }

        private Border CreateWordsManagementContent()
        {
            var mainBorder = new Border
            {
                Background = ThemeManager.GetBackground(),
                Padding = new Thickness(20)
            };

            var mainStack = new StackPanel();

            var title = new TextBlock
            {
                Text = "📋 Управление словами",
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Foreground = ThemeManager.GetText(),
                Margin = new Thickness(0, 0, 0, 10)
            };
            mainStack.Children.Add(title);

            var scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Height = 450
            };

            var wordsPanel = new StackPanel();

            using var context = new TrainerDbContext();

            // ✅ ПОЛУЧАЕМ АКТИВНЫЕ УРОКИ И ТИПЫ СЛОВ
            var enabledLessons = AppSettings.EnabledLessons.Count == 0
                ? context.GermanNouns.Select(w => w.Lesson)
                    .Union(context.GermanAdjectives.Select(w => w.Lesson))
                    .Union(context.GermanVerbs.Select(w => w.Lesson))
                    .Distinct()
                    .ToList()
                : AppSettings.EnabledLessons.ToList();

            var enabledWordTypes = AppSettings.EnabledWordTypes.Count == 0
                ? new List<string> { "Noun", "Adjective", "Verb" }
                : AppSettings.EnabledWordTypes.ToList();

            if (_currentTrainerMode == "articles")
            {
                // ✅ ТОЛЬКО СУЩЕСТВИТЕЛЬНЫЕ, И ТОЛЬКО ИЗ ВЫБРАННЫХ УРОКОВ
                var allNouns = context.GermanNouns
                    .Where(w => enabledLessons.Contains(w.Lesson))
                    .OrderBy(w => w.Lesson)
                    .ThenBy(w => w.Word)
                    .ToList();

                if (allNouns.Count == 0)
                {
                    var emptyText = new TextBlock
                    {
                        Text = "Нет существительных в выбранных уроках",
                        Foreground = ThemeManager.GetTextSecondary(),
                        FontSize = 12
                    };
                    wordsPanel.Children.Add(emptyText);
                }
                else
                {
                    int currentLesson = -1;
                    foreach (var noun in allNouns)
                    {
                        // ✅ ЗАГОЛОВОК УРОКА
                        if (currentLesson != noun.Lesson)
                        {
                            currentLesson = noun.Lesson;
                            var lessonHeader = new TextBlock
                            {
                                Text = $"📖 Урок {noun.Lesson}",
                                FontSize = 13,
                                FontWeight = FontWeights.SemiBold,
                                Foreground = ThemeManager.GetText(),
                                Margin = new Thickness(0, 12, 0, 6)
                            };
                            wordsPanel.Children.Add(lessonHeader);
                        }

                        var wordCard = CreateWordCard(noun.Id, $"{noun.Article} {noun.Word}",
                            noun.Translation ?? "", noun.IsHidden, "noun");
                        wordsPanel.Children.Add(wordCard);
                    }
                }
            }
            else if (_currentTrainerMode == "translation")
            {
                // ✅ ТОЛЬКО ВЫБРАННЫЕ ТИПЫ СЛОВ И ТОЛЬКО ИЗ ВЫБРАННЫХ УРОКОВ
                var allNouns = enabledWordTypes.Contains("Noun")
                    ? context.GermanNouns.Where(w => enabledLessons.Contains(w.Lesson)).OrderBy(w => w.Lesson).ThenBy(w => w.Word).ToList()
                    : new List<GermanNoun>();

                var allAdj = enabledWordTypes.Contains("Adjective")
                    ? context.GermanAdjectives.Where(w => enabledLessons.Contains(w.Lesson)).OrderBy(w => w.Lesson).ThenBy(w => w.Word).ToList()
                    : new List<GermanAdjective>();

                var allVerbs = enabledWordTypes.Contains("Verb")
                    ? context.GermanVerbs.Where(w => enabledLessons.Contains(w.Lesson)).OrderBy(w => w.Lesson).ThenBy(w => w.Word).ToList()
                    : new List<GermanVerb>();

                // ✅ ТОЛЬКО СУЩЕСТВИТЕЛЬНЫЕ
                if (allNouns.Count > 0)
                {
                    var nounHeader = new TextBlock
                    {
                        Text = "📘 Существительные",
                        FontSize = 14,
                        FontWeight = FontWeights.SemiBold,
                        Foreground = ThemeManager.GetText(),
                        Margin = new Thickness(0, 0, 0, 8)
                    };
                    wordsPanel.Children.Add(nounHeader);

                    int currentLesson = -1;
                    foreach (var noun in allNouns)
                    {
                        if (currentLesson != noun.Lesson)
                        {
                            currentLesson = noun.Lesson;
                            var lessonLabel = new TextBlock
                            {
                                Text = $"  Урок {noun.Lesson}",
                                FontSize = 11,
                                Foreground = ThemeManager.GetTextSecondary(),
                                Margin = new Thickness(8, 8, 0, 4)
                            };
                            wordsPanel.Children.Add(lessonLabel);
                        }

                        var wordCard = CreateWordCard(noun.Id, $"{noun.Article} {noun.Word}",
                            noun.Translation ?? "", noun.IsHidden, "noun");
                        wordsPanel.Children.Add(wordCard);
                    }
                }

                // ✅ ТОЛЬКО ПРИЛАГАТЕЛЬНЫЕ
                if (allAdj.Count > 0)
                {
                    if (allNouns.Count > 0)
                    {
                        var divider = new Separator
                        {
                            Margin = new Thickness(0, 12, 0, 8),
                            Background = ThemeManager.GetBorder()
                        };
                        wordsPanel.Children.Add(divider);
                    }

                    var adjHeader = new TextBlock
                    {
                        Text = "💚 Прилагательные",
                        FontSize = 14,
                        FontWeight = FontWeights.SemiBold,
                        Foreground = ThemeManager.GetText(),
                        Margin = new Thickness(0, 0, 0, 8)
                    };
                    wordsPanel.Children.Add(adjHeader);

                    int currentLesson = -1;
                    foreach (var adj in allAdj)
                    {
                        if (currentLesson != adj.Lesson)
                        {
                            currentLesson = adj.Lesson;
                            var lessonLabel = new TextBlock
                            {
                                Text = $"  Урок {adj.Lesson}",
                                FontSize = 11,
                                Foreground = ThemeManager.GetTextSecondary(),
                                Margin = new Thickness(8, 8, 0, 4)
                            };
                            wordsPanel.Children.Add(lessonLabel);
                        }

                        var wordCard = CreateWordCard(adj.Id, adj.Word,
                            adj.Translation ?? "", adj.IsHidden, "adjective");
                        wordsPanel.Children.Add(wordCard);
                    }
                }

                // ✅ ТОЛЬКО ГЛАГОЛЫ
                if (allVerbs.Count > 0)
                {
                    if (allNouns.Count > 0 || allAdj.Count > 0)
                    {
                        var divider = new Separator
                        {
                            Margin = new Thickness(0, 12, 0, 8),
                            Background = ThemeManager.GetBorder()
                        };
                        wordsPanel.Children.Add(divider);
                    }

                    var verbHeader = new TextBlock
                    {
                        Text = "🔴 Глаголы",
                        FontSize = 14,
                        FontWeight = FontWeights.SemiBold,
                        Foreground = ThemeManager.GetText(),
                        Margin = new Thickness(0, 0, 0, 8)
                    };
                    wordsPanel.Children.Add(verbHeader);

                    int currentLesson = -1;
                    foreach (var verb in allVerbs)
                    {
                        if (currentLesson != verb.Lesson)
                        {
                            currentLesson = verb.Lesson;
                            var lessonLabel = new TextBlock
                            {
                                Text = $"  Урок {verb.Lesson}",
                                FontSize = 11,
                                Foreground = ThemeManager.GetTextSecondary(),
                                Margin = new Thickness(8, 8, 0, 4)
                            };
                            wordsPanel.Children.Add(lessonLabel);
                        }

                        var wordCard = CreateWordCard(verb.Id, verb.Word,
                            verb.Translation ?? "", verb.IsHidden, "verb");
                        wordsPanel.Children.Add(wordCard);
                    }
                }

                if (allNouns.Count == 0 && allAdj.Count == 0 && allVerbs.Count == 0)
                {
                    var emptyText = new TextBlock
                    {
                        Text = "Нет слов в выбранных уроках и типах",
                        Foreground = ThemeManager.GetTextSecondary(),
                        FontSize = 12
                    };
                    wordsPanel.Children.Add(emptyText);
                }
            }

            scrollViewer.Content = wordsPanel;
            mainStack.Children.Add(scrollViewer);

            var doneButton = new Button
            {
                Content = "✓ Готово",
                FontSize = 14,
                Padding = new Thickness(20, 10, 20, 10),
                Background = new SolidColorBrush(Color.FromRgb(76, 175, 80)),
                Foreground = Brushes.White,
                Cursor = Cursors.Hand,
                BorderThickness = new Thickness(0),
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 10, 0, 0)
            };
            doneButton.Click += (s, e) =>
            {
                if (_currentTrainerMode == "articles" && _articlesViewModel != null)
                    _articlesViewModel.RefreshAllNouns();
                else if (_currentTrainerMode == "translation" && _translationViewModel != null)
                    _translationViewModel.RefreshWords();

                _wordsManagementModal?.Close();
            };

            mainStack.Children.Add(doneButton);

            mainBorder.Child = mainStack;
            return mainBorder;
        }


        // ИСПРАВЛЕНО: Для совместимости со старыми версиями .NET
        private Border CreateWordCard(int wordId, string word, string translation, bool isHidden, string wordType)
        {
            var border = new Border
            {
                Background = ThemeManager.GetSurface(),
                BorderBrush = ThemeManager.GetBorder(),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(12),
                Margin = new Thickness(0, 0, 0, 8)
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var leftStack = new StackPanel { VerticalAlignment = VerticalAlignment.Center };

            var wordBlock = new TextBlock
            {
                Text = word,
                FontSize = 12,
                FontWeight = FontWeights.SemiBold,
                Foreground = isHidden ? ThemeManager.GetTextSecondary() : ThemeManager.GetText(),
                Opacity = isHidden ? 0.5 : 1.0,
                FontStyle = isHidden ? FontStyles.Italic : FontStyles.Normal
            };
            leftStack.Children.Add(wordBlock);

            if (!string.IsNullOrEmpty(translation))
            {
                var translationBlock = new TextBlock
                {
                    Text = translation,
                    FontSize = 11,
                    Foreground = ThemeManager.GetTextSecondary(),
                    Margin = new Thickness(0, 4, 0, 0)
                };
                leftStack.Children.Add(translationBlock);
            }

            Grid.SetColumn(leftStack, 0);
            grid.Children.Add(leftStack);

            var hideButton = new Button
            {
                Content = isHidden ? "👁️‍🗨️ Показать" : "👁️ Скрыть",
                FontSize = 10,
                Padding = new Thickness(8, 4, 8, 4),
                Background = isHidden ? new SolidColorBrush(Color.FromRgb(76, 175, 80))
                                      : new SolidColorBrush(Color.FromRgb(244, 67, 54)),
                Foreground = Brushes.White,
                Cursor = Cursors.Hand,
                BorderThickness = new Thickness(0),
                Tag = $"{wordType}_{wordId}"
            };

            hideButton.Click += (s, e) =>
            {
                ToggleWord(wordType, wordId);
                _wordsManagementModal?.Close();
                ShowWordsManagementModal();
            };

            var buttonBorder = new Border
            {
                Background = hideButton.Background,
                CornerRadius = new CornerRadius(4),
                Child = hideButton
            };

            Grid.SetColumn(buttonBorder, 1);
            grid.Children.Add(buttonBorder);

            border.Child = grid;
            return border;
        }

        private void ToggleWord(string wordType, int wordId)
        {
            using var context = new TrainerDbContext();

            if (wordType == "noun")
            {
                var noun = context.GermanNouns.FirstOrDefault(w => w.Id == wordId);
                if (noun != null) { noun.IsHidden = !noun.IsHidden; context.SaveChanges(); }
            }
            else if (wordType == "adjective")
            {
                var adj = context.GermanAdjectives.FirstOrDefault(w => w.Id == wordId);
                if (adj != null) { adj.IsHidden = !adj.IsHidden; context.SaveChanges(); }
            }
            else if (wordType == "verb")
            {
                var verb = context.GermanVerbs.FirstOrDefault(w => w.Id == wordId);
                if (verb != null) { verb.IsHidden = !verb.IsHidden; context.SaveChanges(); }
            }
        }

        // ===== ОСТАЛЬНЫЕ МЕТОДЫ (АРТИКЛИ И ПЕРЕВОДЫ) =====

        private void ButtonClick_der(object sender, RoutedEventArgs e)
        {
            _articlesViewModel?.SelectArticle("der");
            HighlightSelectedButton("der");
            UpdateCheckButtonState();
        }

        private void ButtonClick_die(object sender, RoutedEventArgs e)
        {
            _articlesViewModel?.SelectArticle("die");
            HighlightSelectedButton("die");
            UpdateCheckButtonState();
        }

        private void ButtonClick_das(object sender, RoutedEventArgs e)
        {
            _articlesViewModel?.SelectArticle("das");
            HighlightSelectedButton("das");
            UpdateCheckButtonState();
        }
        private void ManageWordsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowWordsManagementModal();
        }

        private void HighlightSelectedButton(string article)
        {
            BtnDer.BorderBrush = Brushes.Transparent;
            BtnDie.BorderBrush = Brushes.Transparent;
            BtnDas.BorderBrush = Brushes.Transparent;

            var highlightBrush = new SolidColorBrush(Colors.White);

            switch (article)
            {
                case "der":
                    BtnDer.BorderBrush = highlightBrush;
                    break;
                case "die":
                    BtnDie.BorderBrush = highlightBrush;
                    break;
                case "das":
                    BtnDas.BorderBrush = highlightBrush;
                    break;
            }
        }

        private void CheckAnswer_Click(object sender, RoutedEventArgs e)
        {
            bool isArticlesMode = ArticlePage.Visibility == Visibility.Visible;
            bool isTranslationMode = SettingsPage.Visibility == Visibility.Visible && SettingsTitle.Text.Contains("Режим переводов");

            if (isTranslationMode)
            {
                if (_translationViewModel!.IsAnswered)
                {
                    _translationViewModel.NextQuestion();
                    LoadTranslationContent();
                }
                else
                {
                    _translationViewModel.CheckAnswer();
                    LoadTranslationContent();
                }
                UpdateCheckButtonState();
                return;
            }

            if (isArticlesMode)
            {
                if (_articlesViewModel!.IsAnswered)
                {
                    _articlesViewModel.NextQuestion();
                    ResetButtonHighlights();
                }
                else
                {
                    _articlesViewModel.CheckAnswer();
                }
                UpdateCheckButtonState();
            }
        }

        private void UpdateCheckButtonState()
        {
            bool isArticlesMode = ArticlePage.Visibility == Visibility.Visible;
            bool isTranslationMode = SettingsPage.Visibility == Visibility.Visible && SettingsTitle.Text.Contains("Режим переводов");

            CheckAnswerButtonArticles.Visibility = Visibility.Collapsed;
            CheckAnswerButtonTranslation.Visibility = Visibility.Collapsed;

            if (isArticlesMode && _articlesViewModel != null)
            {
                bool hasSelected = !string.IsNullOrEmpty(_articlesViewModel.SelectedArticle);
                bool isAnswered = _articlesViewModel.IsAnswered;

                CheckAnswerButtonArticles.Visibility = Visibility.Visible;
                CheckAnswerButtonArticles.IsEnabled = hasSelected || isAnswered;
                CheckAnswerButtonArticles.Content = isAnswered ? "→ Дальше" : "✓ Проверить";
                CheckAnswerButtonArticles.ToolTip = isAnswered ? "Показать следующее слово" : "Проверить ответ";
            }

            if (isTranslationMode && _translationViewModel != null)
            {
                bool hasSelected = !string.IsNullOrEmpty(_translationViewModel.SelectedAnswer);
                bool isAnswered = _translationViewModel.IsAnswered;

                CheckAnswerButtonTranslation.Visibility = Visibility.Visible;
                CheckAnswerButtonTranslation.IsEnabled = hasSelected || isAnswered;
                CheckAnswerButtonTranslation.Content = isAnswered ? "→ Дальше" : "✓ Проверить";
                CheckAnswerButtonTranslation.ToolTip = isAnswered ? "Показать следующее слово" : "Проверить ответ";
            }
        }

        private void ResetButtonHighlights()
        {
            BtnDer.BorderBrush = Brushes.Transparent;
            BtnDie.BorderBrush = Brushes.Transparent;
            BtnDas.BorderBrush = Brushes.Transparent;
        }

        private void LoadTranslationContent()
        {
            SettingsContentPanel.Children.Clear();
            if (_translationViewModel == null) return;

            var mainBorder = new Border
            {
                Background = ThemeManager.GetSurface(),
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(30),
                Margin = new Thickness(0, 0, 0, 20)
            };

            var mainStack = new StackPanel();

            var title = new TextBlock
            {
                Text = "💬 Режим переводов",
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Foreground = ThemeManager.GetText(),
                Margin = new Thickness(0, 0, 0, 40),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            mainStack.Children.Add(title);

            var wordDisplay = new TextBlock
            {
                Text = _translationViewModel?.CurrentWordDisplay ?? "...",
                FontSize = 64,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(50, 182, 198)),
                Margin = new Thickness(0, 0, 0, 60),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            mainStack.Children.Add(wordDisplay);

            if (_translationViewModel?.AnswerOptions != null)
            {
                var buttonsGrid = new Grid();
                buttonsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                buttonsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                buttonsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(80) });
                buttonsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
                buttonsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(80) });

                for (int i = 0; i < _translationViewModel.AnswerOptions.Count; i++)
                {
                    var answer = _translationViewModel.AnswerOptions[i];
                    var button = CreateAnswerButton(answer, i);

                    int row = i >= 2 ? 0 : 2;
                    int col = i % 2;

                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);

                    if (col == 0)
                    {
                        var margin = button.Margin;
                        button.Margin = new Thickness(10, margin.Top, margin.Right, margin.Bottom);
                    }

                    buttonsGrid.Children.Add(button);
                }

                mainStack.Children.Add(buttonsGrid);
            }

            var feedbackText = new TextBlock
            {
                Text = _translationViewModel?.Feedback ?? "",
                FontSize = 18,
                Foreground = ThemeManager.GetText(),
                Margin = new Thickness(0, 50, 0, 0),
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontWeight = FontWeights.Medium,
                MinHeight = 30
            };
            mainStack.Children.Add(feedbackText);

            mainBorder.Child = mainStack;
            SettingsContentPanel.Children.Add(mainBorder);

            if (SettingsHeader != null)
            {
                var statsPanel = SettingsHeader.Child as StackPanel;
                if (statsPanel == null)
                {
                    statsPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
                    SettingsHeader.Child = statsPanel;
                }

                statsPanel.Children.Clear();

                var correctBadge = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(76, 175, 80)),
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(15, 8, 15, 8),
                    Margin = new Thickness(0, 0, 10, 0)
                };

                var correctStack = new StackPanel { Orientation = Orientation.Horizontal };
                correctStack.Children.Add(new TextBlock { Text = "✓ ", FontSize = 18, Foreground = Brushes.White, FontWeight = FontWeights.Bold });
                correctStack.Children.Add(new TextBlock { Text = _translationViewModel?.CorrectCount.ToString() ?? "0", FontSize = 18, FontWeight = FontWeights.Bold, Foreground = Brushes.White });
                correctBadge.Child = correctStack;
                statsPanel.Children.Add(correctBadge);

                var wrongBadge = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(244, 67, 54)),
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(15, 8, 15, 8),
                    Margin = new Thickness(0, 0, 10, 0)
                };

                var wrongStack = new StackPanel { Orientation = Orientation.Horizontal };
                wrongStack.Children.Add(new TextBlock { Text = "✗ ", FontSize = 18, Foreground = Brushes.White, FontWeight = FontWeights.Bold });
                wrongStack.Children.Add(new TextBlock { Text = _translationViewModel?.WrongCount.ToString() ?? "0", FontSize = 18, FontWeight = FontWeights.Bold, Foreground = Brushes.White });
                wrongBadge.Child = wrongStack;
                statsPanel.Children.Add(wrongBadge);

                var percentBadge = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(33, 150, 243)),
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(15, 8, 15, 8)
                };

                var percentStack = new StackPanel { Orientation = Orientation.Horizontal };
                percentStack.Children.Add(new TextBlock { Text = "📊 ", FontSize = 18 });
                percentStack.Children.Add(new TextBlock { Text = _translationViewModel?.SuccessPercentage ?? "0%", FontSize = 18, FontWeight = FontWeights.Bold, Foreground = Brushes.White });
                percentBadge.Child = percentStack;
                statsPanel.Children.Add(percentBadge);
            }

            UpdateCheckButtonState();
        }

        private Border CreateAnswerButton(string answer, int index)
        {
            var buttonBorder = new Border
            {
                Background = ThemeManager.GetBackground(),
                CornerRadius = new CornerRadius(8),
                BorderBrush = ThemeManager.GetBorder(),
                BorderThickness = new Thickness(2),
                Padding = new Thickness(0),
                Margin = new Thickness(10, 0, 10, 0),
                Cursor = Cursors.Hand,
                Tag = answer
            };

            var button = new Button
            {
                Content = answer,
                FontSize = 18,
                FontWeight = FontWeights.SemiBold,
                Foreground = ThemeManager.GetText(),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Padding = new Thickness(20),
                Tag = answer
            };

            button.Click += (s, e) =>
            {
                if (_translationViewModel != null && !_translationViewModel.IsAnswered)
                {
                    _translationViewModel.SelectedAnswer = answer;

                    HighlightAnswerButton(button,buttonBorder, answer);
                    UpdateCheckButtonState();
                }
            };

            button.MouseEnter += (s, e) =>
            {
                if (_translationViewModel != null && !_translationViewModel.IsAnswered)
                {
                    buttonBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(100, 200, 210));
                    button.Foreground = ThemeManager.GetBackground();
                    buttonBorder.BorderThickness = new Thickness(2);

                    button.Opacity = 0.9;
                }
            };

            button.MouseLeave += (s, e) =>
            {
                if (_translationViewModel != null && !_translationViewModel.IsAnswered)
                {
                    if (_translationViewModel.SelectedAnswer != answer)
                    {
                        buttonBorder.BorderBrush = ThemeManager.GetBorder();
                        buttonBorder.BorderThickness = new Thickness(2);
                    }
                    button.Foreground = ThemeManager.GetText();
                    button.Opacity = 1.0;
                }

            };

            buttonBorder.Child = button;
            return buttonBorder;
        }

        private void HighlightAnswerButton(Button button, Border selectedBorder, string answer)
        {
            foreach (var border in FindVisualChildren<Border>(SettingsContentPanel))
            {
                if (border.Tag is string && border.Tag.ToString() == answer)
                {
                    border.BorderBrush = new SolidColorBrush(Color.FromRgb(50, 182, 198));
                    border.BorderThickness = new Thickness(3);
                    button.Foreground = ThemeManager.GetBackground();
                }
                else if (border.Tag is string)
                {
                    border.BorderBrush = ThemeManager.GetBorder();
                    border.BorderThickness = new Thickness(2);
                }
            }
        }

        // ===== МЕТОДЫ НАСТРОЕК =====
        private void LoadSettingsContent()
        {
            SettingsContentPanel.Children.Clear();
            using var context = new TrainerDbContext();
            var lessonsSection = CreateLessonsSection(context);
            SettingsContentPanel.Children.Add(lessonsSection);

            var separator = new Separator
            {
                Margin = new Thickness(0, 30, 0, 30),
                Background = ThemeManager.GetBorder()
            };

            SettingsContentPanel.Children.Add(separator);

            var wordsSection = CreateWordsManagementSection(context);
            SettingsContentPanel.Children.Add(wordsSection);
        }

        private Border CreateLessonsSection(TrainerDbContext context)
        {
            var border = new Border
            {
                Background = ThemeManager.GetSurface(),
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(30),
                Margin = new Thickness(0, 0, 0, 20)
            };

            var stackPanel = new StackPanel();

            var title = new TextBlock
            {
                Text = "📚 Выбор уроков и типов слов для тренировки",
                FontSize = 22,
                FontWeight = FontWeights.Bold,
                Foreground = ThemeManager.GetText(),
                Margin = new Thickness(0, 0, 0, 20)
            };
            stackPanel.Children.Add(title);

            var subtitle = new TextBlock
            {
                Text = "Выберите какие уроки и типы слов использовать в тренировке.",
                FontSize = 14,
                Foreground = ThemeManager.GetTextSecondary(),
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 20)
            };
            stackPanel.Children.Add(subtitle);

            var wordTypesTitle = new TextBlock
            {
                Text = "📝 Типы слов:",
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                Foreground = ThemeManager.GetText(),
                Margin = new Thickness(0, 0, 0, 10)
            };
            stackPanel.Children.Add(wordTypesTitle);

            var nounCheckbox = new CheckBox
            {
                Content = "📘 Существительные (Nouns)",
                FontSize = 14,
                Margin = new Thickness(20, 0, 0, 8),
                Foreground = ThemeManager.GetText(),
                IsChecked = AppSettings.EnabledWordTypes.Contains("Noun") || AppSettings.EnabledWordTypes.Count == 0,
                Tag = "Noun"
            };
            stackPanel.Children.Add(nounCheckbox);

            var adjCheckbox = new CheckBox
            {
                Content = "💚 Прилагательные (Adjectives)",
                FontSize = 14,
                Margin = new Thickness(20, 0, 0, 8),
                Foreground = ThemeManager.GetText(),
                IsChecked = AppSettings.EnabledWordTypes.Contains("Adjective") || AppSettings.EnabledWordTypes.Count == 0,
                Tag = "Adjective"
            };
            stackPanel.Children.Add(adjCheckbox);

            var verbCheckbox = new CheckBox
            {
                Content = "🔴 Глаголы (Verbs)",
                FontSize = 14,
                Margin = new Thickness(20, 0, 0, 20),
                Foreground = ThemeManager.GetText(),
                IsChecked = AppSettings.EnabledWordTypes.Contains("Verb") || AppSettings.EnabledWordTypes.Count == 0,
                Tag = "Verb"
            };
            stackPanel.Children.Add(verbCheckbox);

            var divider = new Border
            {
                Height = 1,
                Background = ThemeManager.GetBorder(),
                Margin = new Thickness(0, 20, 0, 20)
            };
            stackPanel.Children.Add(divider);

            var lessonsTitle = new TextBlock
            {
                Text = "📖 Уроки:",
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                Foreground = ThemeManager.GetText(),
                Margin = new Thickness(0, 0, 0, 10)
            };
            stackPanel.Children.Add(lessonsTitle);

            var lessons = context.GermanNouns.Select(w => w.Lesson)
                .Union(context.GermanAdjectives.Select(w => w.Lesson))
                .Union(context.GermanVerbs.Select(w => w.Lesson))
                .Distinct()
                .OrderBy(l => l)
                .ToList();

            foreach (var lesson in lessons)
            {
                var nounsCount = context.GermanNouns.Count(w => w.Lesson == lesson && !w.IsHidden);
                var adjCount = context.GermanAdjectives.Count(w => w.Lesson == lesson && !w.IsHidden);
                var verbsCount = context.GermanVerbs.Count(w => w.Lesson == lesson && !w.IsHidden);
                var totalCount = nounsCount + adjCount + verbsCount;

                var checkbox = new CheckBox
                {
                    Content = $"📄 Урок {lesson} ({totalCount} слов)",
                    FontSize = 14,
                    Margin = new Thickness(20, 0, 0, 8),
                    Foreground = ThemeManager.GetText(),
                    IsChecked = AppSettings.EnabledLessons.Contains(lesson) || AppSettings.EnabledLessons.Count == 0,
                    Tag = lesson
                };
                stackPanel.Children.Add(checkbox);
            }

            var saveButton = new Button
            {
                Content = "💾 Сохранить",
                FontSize = 16,
                Padding = new Thickness(20),
                Margin = new Thickness(0, 20, 0, 0),
                Background = new SolidColorBrush(Color.FromRgb(76, 175, 80)),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            var saveButtonBorder = new Border
            {
                CornerRadius = new CornerRadius(8),
                Child = saveButton
            };

            saveButton.Click += (s, e) =>
            {
                AppSettings.EnabledLessons.Clear();
                AppSettings.EnabledWordTypes.Clear();

                if (nounCheckbox.IsChecked == true) AppSettings.EnabledWordTypes.Add("Noun");
                if (adjCheckbox.IsChecked == true) AppSettings.EnabledWordTypes.Add("Adjective");
                if (verbCheckbox.IsChecked == true) AppSettings.EnabledWordTypes.Add("Verb");

                foreach (var child in stackPanel.Children)
                {
                    if (child is CheckBox cb && cb.Tag is int lessonNumber)
                    {
                        if (cb.IsChecked == true) AppSettings.EnabledLessons.Add(lessonNumber);
                    }
                }

                AppSettings.SaveSettings();

                if (_articlesViewModel != null) _articlesViewModel.RefreshAllNouns();
                if (_translationViewModel != null) _translationViewModel.RefreshWords();

                string enabledTypes = string.Join(", ", (AppSettings.EnabledWordTypes.Count == 0 ?
                    new[] { "Все типы" } :
                    AppSettings.EnabledWordTypes.Select(t => t == "Noun" ? "Существительные" :
                                                            t == "Adjective" ? "Прилагательные" :
                                                            "Глаголы")));

                MessageBox.Show($"✓ Сохранено!\n\nТипы слов: {enabledTypes}\nУроки: {(AppSettings.EnabledLessons.Count == 0 ? "Все" : string.Join(", ", AppSettings.EnabledLessons))}",
                    "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            };

            stackPanel.Children.Add(saveButtonBorder);
            border.Child = stackPanel;
            return border;
        }

        private Border CreateWordsManagementSection(TrainerDbContext context)
        {
            var border = new Border
            {
                Background = ThemeManager.GetSurface(),
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(30)
            };

            var stackPanel = new StackPanel();

            var title = new TextBlock
            {
                Text = "🛠️ Управление словами",
                FontSize = 22,
                FontWeight = FontWeights.Bold,
                Foreground = ThemeManager.GetText(),
                Margin = new Thickness(0, 0, 0, 20)
            };
            stackPanel.Children.Add(title);

            var lessons = context.GermanNouns.Select(w => w.Lesson)
                .Union(context.GermanAdjectives.Select(w => w.Lesson))
                .Union(context.GermanVerbs.Select(w => w.Lesson))
                .Distinct()
                .OrderBy(l => l)
                .ToList();

            foreach (var lesson in lessons)
            {
                var lessonGroup = CreateLessonGroup(context, lesson);
                stackPanel.Children.Add(lessonGroup);

                if (lessons.Last() != lesson)
                {
                    var sep = new Separator
                    {
                        Margin = new Thickness(0, 20, 0, 20),
                        Background = ThemeManager.GetBorder()
                    };
                    stackPanel.Children.Add(sep);
                }
            }

            border.Child = stackPanel;
            return border;
        }

        private Border CreateLessonGroup(TrainerDbContext context, int lesson)
        {
            var border = new Border
            {
                Background = ThemeManager.GetBackground(),
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(20),
                Margin = new Thickness(0, 0, 0, 20),
                BorderBrush = ThemeManager.GetBorder(),
                BorderThickness = new Thickness(1)
            };

            var mainStack = new StackPanel();

            var lessonTitle = new TextBlock
            {
                Text = $"Урок {lesson}",
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Foreground = ThemeManager.GetText(),
                Margin = new Thickness(0, 0, 0, 20)
            };
            mainStack.Children.Add(lessonTitle);

            var nouns = context.GermanNouns.Where(w => w.Lesson == lesson).OrderBy(w => w.Word).ToList();
            if (nouns.Count > 0)
            {
                var nounsExpander = CreateWordTypeExpander(nouns.Cast<object>().ToList(), $"📘 Существительные ({nouns.Count})", "Noun");
                mainStack.Children.Add(nounsExpander);
            }

            var adjectives = context.GermanAdjectives.Where(w => w.Lesson == lesson).OrderBy(w => w.Word).ToList();
            if (adjectives.Count > 0)
            {
                if (nouns.Count > 0)
                {
                    mainStack.Children.Add(new Separator { Margin = new Thickness(0, 15, 0, 15), Background = ThemeManager.GetBorder() });
                }
                var adjExpander = CreateWordTypeExpander(adjectives.Cast<object>().ToList(), $"💚 Прилагательные ({adjectives.Count})", "Adjective");
                mainStack.Children.Add(adjExpander);
            }

            var verbs = context.GermanVerbs.Where(w => w.Lesson == lesson).OrderBy(w => w.Word).ToList();
            if (verbs.Count > 0)
            {
                if (nouns.Count > 0 || adjectives.Count > 0)
                {
                    mainStack.Children.Add(new Separator { Margin = new Thickness(0, 15, 0, 15), Background = ThemeManager.GetBorder() });
                }
                var verbsExpander = CreateWordTypeExpander(verbs.Cast<object>().ToList(), $"🔴 Глаголы ({verbs.Count})", "Verb");
                mainStack.Children.Add(verbsExpander);
            }

            border.Child = mainStack;
            return border;
        }

        private Expander CreateWordTypeExpander(List<object> words, string title, string wordType)
        {
            var expander = new Expander
            {
                Header = title,
                FontSize = 14,
                FontWeight = FontWeights.SemiBold,
                Foreground = ThemeManager.GetText(),
                Background = ThemeManager.GetBackground(),
                BorderThickness = new Thickness(1),
                BorderBrush = ThemeManager.GetBorder(),
                Margin = new Thickness(0, 0, 0, 10),
                Padding = new Thickness(10)
            };

            var contentStack = new StackPanel { Margin = new Thickness(0, 10, 0, 0) };

            foreach (var word in words)
            {
                Border? card = null;

                if (wordType == "Noun" && word is GermanNoun noun)
                    card = CreateNounCard(noun);
                else if (wordType == "Adjective" && word is GermanAdjective adj)
                    card = CreateAdjectiveCard(adj);
                else if (wordType == "Verb" && word is GermanVerb verb)
                    card = CreateVerbCard(verb);

                if (card != null)
                    contentStack.Children.Add(card);
            }

            expander.Content = contentStack;
            return expander;
        }

        private Border CreateNounCard(GermanNoun noun)
        {
            var border = new Border
            {
                Background = ThemeManager.GetBackground(),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(0),
                Margin = new Thickness(0, 0, 0, 8),
                BorderBrush = ThemeManager.GetBorder(),
                BorderThickness = new Thickness(1)
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(70) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(110) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(70) });

            var articleBadge = new Border
            {
                Background = GetArticleColor(noun.Article),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(6),
                Margin = new Thickness(6),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var articleText = new TextBlock
            {
                Text = noun.Article,
                FontSize = 11,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            articleBadge.Child = articleText;
            Grid.SetColumn(articleBadge, 0);
            grid.Children.Add(articleBadge);

            var wordText = new TextBlock
            {
                Text = noun.Word,
                FontSize = 13,
                FontWeight = FontWeights.SemiBold,
                Foreground = ThemeManager.GetText(),
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(6, 0, 0, 0)
            };
            Grid.SetColumn(wordText, 1);
            grid.Children.Add(wordText);

            var translationText = new TextBlock
            {
                Text = noun.Translation ?? "",
                FontSize = 12,
                Foreground = noun.Translation != null ? ThemeManager.GetText() : ThemeManager.GetTextSecondary(),
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(6, 0, 0, 0),
                FontStyle = noun.Translation == null ? FontStyles.Italic : FontStyles.Normal,
                TextTrimming = TextTrimming.CharacterEllipsis
            };
            Grid.SetColumn(translationText, 2);
            grid.Children.Add(translationText);

            var statsText = new TextBlock
            {
                Text = $"{noun.CorrectAnswers}/{noun.WrongAnswers}",
                FontSize = 10,
                Foreground = ThemeManager.GetTextSecondary(),
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(4, 0, 0, 0)
            };
            Grid.SetColumn(statsText, 3);
            grid.Children.Add(statsText);

            var deleteButton = new Button
            {
                Content = "❌",
                FontSize = 11,
                Width = 26,
                Height = 26,
                Margin = new Thickness(2),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand,
                ToolTip = "Удалить слово",
                Padding = new Thickness(0)
            };

            deleteButton.Click += (s, e) => DeleteNoun_Click(noun.Id);
            Grid.SetColumn(deleteButton, 4);
            grid.Children.Add(deleteButton);

            border.Child = grid;
            return border;
        }

        private Border CreateAdjectiveCard(GermanAdjective adj)
        {
            var border = new Border
            {
                Background = ThemeManager.GetBackground(),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(0),
                Margin = new Thickness(0, 0, 0, 8),
                BorderBrush = ThemeManager.GetBorder(),
                BorderThickness = new Thickness(1)
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(110) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(70) });

            var wordText = new TextBlock
            {
                Text = adj.Word,
                FontSize = 13,
                FontWeight = FontWeights.SemiBold,
                Foreground = ThemeManager.GetText(),
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(8, 0, 0, 0)
            };
            Grid.SetColumn(wordText, 0);
            grid.Children.Add(wordText);

            var translationText = new TextBlock
            {
                Text = adj.Translation,
                FontSize = 12,
                Foreground = ThemeManager.GetText(),
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(8, 0, 0, 0),
                TextTrimming = TextTrimming.CharacterEllipsis
            };
            Grid.SetColumn(translationText, 1);
            grid.Children.Add(translationText);

            var statsText = new TextBlock
            {
                Text = $"{adj.CorrectAnswers}/{adj.WrongAnswers}",
                FontSize = 10,
                Foreground = ThemeManager.GetTextSecondary(),
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(4, 0, 0, 0)
            };
            Grid.SetColumn(statsText, 2);
            grid.Children.Add(statsText);

            var deleteButton = new Button
            {
                Content = "❌",
                FontSize = 11,
                Width = 26,
                Height = 26,
                Margin = new Thickness(2),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand,
                ToolTip = "Удалить слово",
                Padding = new Thickness(0)
            };

            deleteButton.Click += (s, e) => DeleteAdjective_Click(adj.Id);
            Grid.SetColumn(deleteButton, 3);
            grid.Children.Add(deleteButton);

            border.Child = grid;
            return border;
        }

        private Border CreateVerbCard(GermanVerb verb)
        {
            var border = new Border
            {
                Background = ThemeManager.GetBackground(),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(0),
                Margin = new Thickness(0, 0, 0, 8),
                BorderBrush = ThemeManager.GetBorder(),
                BorderThickness = new Thickness(1)
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(110) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(70) });

            var wordText = new TextBlock
            {
                Text = verb.Word,
                FontSize = 13,
                FontWeight = FontWeights.SemiBold,
                Foreground = ThemeManager.GetText(),
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(8, 0, 0, 0)
            };
            Grid.SetColumn(wordText, 0);
            grid.Children.Add(wordText);

            var translationText = new TextBlock
            {
                Text = verb.Translation,
                FontSize = 12,
                Foreground = ThemeManager.GetText(),
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(8, 0, 0, 0),
                TextTrimming = TextTrimming.CharacterEllipsis
            };
            Grid.SetColumn(translationText, 1);
            grid.Children.Add(translationText);

            var statsText = new TextBlock
            {
                Text = $"{verb.CorrectAnswers}/{verb.WrongAnswers}",
                FontSize = 10,
                Foreground = ThemeManager.GetTextSecondary(),
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(4, 0, 0, 0)
            };
            Grid.SetColumn(statsText, 2);
            grid.Children.Add(statsText);

            var deleteButton = new Button
            {
                Content = "❌",
                FontSize = 11,
                Width = 26,
                Height = 26,
                Margin = new Thickness(2),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand,
                ToolTip = "Удалить слово",
                Padding = new Thickness(0)
            };

            deleteButton.Click += (s, e) => DeleteVerb_Click(verb.Id);
            Grid.SetColumn(deleteButton, 3);
            grid.Children.Add(deleteButton);

            border.Child = grid;
            return border;
        }

        private SolidColorBrush GetArticleColor(string article)
        {
            return article.ToLower() switch
            {
                "der" => new SolidColorBrush(Color.FromRgb(33, 150, 243)),
                "die" => new SolidColorBrush(Color.FromRgb(233, 30, 99)),
                "das" => new SolidColorBrush(Color.FromRgb(156, 39, 176)),
                _ => new SolidColorBrush(Color.FromRgb(96, 125, 139))
            };
        }

        private void DeleteNoun_Click(int nounId)
        {
            var result = MessageBox.Show("Вы уверены, что хотите удалить это слово?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                using var context = new TrainerDbContext();
                var noun = context.GermanNouns.FirstOrDefault(w => w.Id == nounId);
                if (noun != null)
                {
                    context.GermanNouns.Remove(noun);
                    context.SaveChanges();
                }

                LoadSettingsContent();

                if (_articlesViewModel != null)
                    _articlesViewModel.RefreshAllNouns();

                MessageBox.Show("Слово удалено!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteAdjective_Click(int adjectiveId)
        {
            var result = MessageBox.Show("Вы уверены, что хотите удалить это слово?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                using var context = new TrainerDbContext();
                var adj = context.GermanAdjectives.FirstOrDefault(w => w.Id == adjectiveId);
                if (adj != null)
                {
                    context.GermanAdjectives.Remove(adj);
                    context.SaveChanges();
                }

                LoadSettingsContent();

                MessageBox.Show("Слово удалено!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteVerb_Click(int verbId)
        {
            var result = MessageBox.Show("Вы уверены, что хотите удалить это слово?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                using var context = new TrainerDbContext();
                var verb = context.GermanVerbs.FirstOrDefault(w => w.Id == verbId);
                if (verb != null)
                {
                    context.GermanVerbs.Remove(verb);
                    context.SaveChanges();
                }

                LoadSettingsContent();

                MessageBox.Show("Слово удалено!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            ThemeManager.OnThemeChanged -= ApplyTheme;
            base.OnClosed(e);
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject? depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null)
                    {
                        if (child is T)
                            yield return (T)child;

                        foreach (T childOfChild in FindVisualChildren<T>(child))
                            yield return childOfChild;
                    }
                }
            }
        }
    }
}