using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using GermanTrainer.Models;
using GermanTrainer.Services;

namespace GermanTrainer.ViewModels
{
    public class TrainerArticlesViewModel : INotifyPropertyChanged
    {
        private readonly TrainerArticlesService _service;

        private GermanNoun? _currentNoun;
        private string? _selectedArticle;
        private string? _feedback;
        private bool _isAnswered;
        private int _correctCount;
        private int _wrongCount;
        private ObservableCollection<GermanNoun> _allNouns;

        public TrainerArticlesViewModel()
        {
            _service = new TrainerArticlesService();
            _allNouns = new ObservableCollection<GermanNoun>(_service.GetVisibleNouns());

            LoadNextQuestion();
        }

        public GermanNoun? CurrentNoun
        {
            get => _currentNoun;
            set { _currentNoun = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentNounDisplay)); }
        }

        public string CurrentNounDisplay => CurrentNoun?.Word ?? "Загрузка...";

        public string? SelectedArticle
        {
            get => _selectedArticle;
            set { _selectedArticle = value; OnPropertyChanged(); }
        }

        public string? Feedback
        {
            get => _feedback;
            set { _feedback = value; OnPropertyChanged(); }
        }

        public bool IsAnswered
        {
            get => _isAnswered;
            set { _isAnswered = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanAnswer)); }
        }

        public bool CanAnswer => !IsAnswered;

        public int CorrectCount
        {
            get => _correctCount;
            set { _correctCount = value; OnPropertyChanged(); OnPropertyChanged(nameof(SuccessPercentage)); }
        }

        public int WrongCount
        {
            get => _wrongCount;
            set { _wrongCount = value; OnPropertyChanged(); OnPropertyChanged(nameof(SuccessPercentage)); }
        }

        public string SuccessPercentage
        {
            get
            {
                var total = CorrectCount + WrongCount;
                if (total == 0) return "0%";
                return $"{Math.Round((double)CorrectCount / total * 100, 1)}%";
            }
        }

        public ObservableCollection<GermanNoun> AllNouns
        {
            get => _allNouns;
            set { _allNouns = value; OnPropertyChanged(); }
        }

        public void SelectArticle(string article)
        {
            if (!IsAnswered)
            {
                SelectedArticle = article;
            }
        }

        public void CheckAnswer()
        {
            if (CurrentNoun == null || SelectedArticle == null)
                return;

            IsAnswered = true;

            bool isCorrect = SelectedArticle == CurrentNoun.Article;

            if (isCorrect)
            {
                CorrectCount++;
                Feedback = $"✓ Правильно! {CurrentNoun.Article} {CurrentNoun.Word}";
                if (!string.IsNullOrEmpty(CurrentNoun.Translation))
                {
                    Feedback += $" ({CurrentNoun.Translation})";
                }
            }
            else
            {
                WrongCount++;
                Feedback = $"✗ Неправильно. Правильный ответ: {CurrentNoun.Article} {CurrentNoun.Word}";
                if (!string.IsNullOrEmpty(CurrentNoun.Translation))
                {
                    Feedback += $" ({CurrentNoun.Translation})";
                }
            }

            _service.RecordAnswer(CurrentNoun, isCorrect);

            RefreshNounsList();
        }

        public void NextQuestion()
        {
            LoadNextQuestion();
        }

        public void ResetQuiz()
        {
            CorrectCount = 0;
            WrongCount = 0;
            SelectedArticle = null;
            Feedback = null;
            IsAnswered = false;
            LoadNextQuestion();
        }

        public void AddNewNoun(string word, string article, string? translation = null)
        {
            try
            {
                _service.AddNoun(word, article, translation);
                RefreshNounsList();
                Feedback = $"Слово '{word}' добавлено!";
            }
            catch (ArgumentException ex)
            {
                Feedback = $"Ошибка: {ex.Message}";
            }
        }

        public void RefreshAllNouns()
        {
            RefreshNounsList();
            LoadNextQuestion();
        }

        private void LoadNextQuestion()
        {
            CurrentNoun = _service.GetNextNoun();
            SelectedArticle = null;
            Feedback = null;
            IsAnswered = false;

            if (CurrentNoun == null)
            {
                Feedback = "Нет доступных слов для тренировки! Добавьте новые слова или снимите скрытие с существующих.";
            }
        }

        private void RefreshNounsList()
        {
            AllNouns = new ObservableCollection<GermanNoun>(_service.GetVisibleNouns());
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
