using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using GermanTrainer.Data;
using GermanTrainer.Models;
using GermanTrainer.Services;

namespace GermanTrainer.ViewModels
{
    public class TrainerTranslationViewModel : INotifyPropertyChanged
    {
        private readonly TrainerTranslationService _service;
        private object? _currentWord;
        private string? _selectedAnswer;
        private string? _feedback;
        private bool _isAnswered;
        private int _correctCount;
        private int _wrongCount;
        private List<string> _answerOptions;
        private string? _correctAnswer;

        public event PropertyChangedEventHandler? PropertyChanged;

        public TrainerTranslationViewModel()
        {
            _service = new TrainerTranslationService();
            _answerOptions = new List<string>();
            LoadNextQuestion();
        }

        public object? CurrentWord
        {
            get => _currentWord;
            set { _currentWord = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentWordDisplay)); }
        }

        public string CurrentWordDisplay => CurrentWord switch
        {
            GermanNoun n => $"{n.Article} {n.Word}",
            GermanAdjective a => a.Word,
            GermanVerb v => v.Word,
            _ => "Загрузка..."
        };

        public string? SelectedAnswer
        {
            get => _selectedAnswer;
            set { _selectedAnswer = value; OnPropertyChanged(); }
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

        public List<string> AnswerOptions
        {
            get => _answerOptions;
            set { _answerOptions = value; OnPropertyChanged(); }
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

        public void SelectAnswer(string answer)
        {
            if (!IsAnswered)
            {
                SelectedAnswer = answer;
            }
        }

        public void CheckAnswer()
        {
            if (CurrentWord == null || SelectedAnswer == null)
                return;

            IsAnswered = true;
            bool isCorrect = SelectedAnswer == _correctAnswer;

            if (isCorrect)
            {
                CorrectCount++;
                Feedback = $"✓ Правильно! {_correctAnswer}";
            }
            else
            {
                WrongCount++;
                Feedback = $"✗ Неправильно. Правильный ответ: {_correctAnswer}";
            }

            _service.RecordAnswer(CurrentWord, isCorrect);
        }

        public void NextQuestion()
        {
            LoadNextQuestion();
        }

        public void ResetQuiz()
        {
            CorrectCount = 0;
            WrongCount = 0;
            SelectedAnswer = null;
            Feedback = null;
            IsAnswered = false;
            LoadNextQuestion();
        }

        // ✓ ОБНОВЛЕННЫЙ МЕТОД: ТЕПЕРЬ ПЕРЕЗАГРУЖАЕТ СЛОВА С НОВЫМИ НАСТРОЙКАМИ
        public void RefreshWords()
        {
            _service.ReloadWords();
            LoadNextQuestion();
        }

        private void LoadNextQuestion()
        {
            CurrentWord = _service.GetNextWord();
            _correctAnswer = GetTranslation(CurrentWord);
            AnswerOptions = GenerateAnswerOptions(_correctAnswer);
            SelectedAnswer = null;
            Feedback = null;
            IsAnswered = false;

            if (CurrentWord == null)
            {
                Feedback = "Нет доступных слов для тренировки! Выберите уроки и типы слов в настройках.";
            }
        }

        private string? GetTranslation(object? word) => word switch
        {
            GermanNoun n => n.Translation,
            GermanAdjective a => a.Translation,
            GermanVerb v => v.Translation,
            _ => null
        };

        private List<string> GenerateAnswerOptions(string? correctAnswer)
        {
            if (string.IsNullOrEmpty(correctAnswer))
                return new List<string>();

            var options = new List<string> { correctAnswer };
            var wrongAnswers = _service.GenerateWrongAnswers(correctAnswer, 3);
            options.AddRange(wrongAnswers);

            // Перемешиваем
            var random = new Random();
            return options.OrderBy(x => random.Next()).ToList();
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
