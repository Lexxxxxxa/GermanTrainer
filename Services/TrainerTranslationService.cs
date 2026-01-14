using System;
using System.Collections.Generic;
using System.Linq;
using GermanTrainer.Data;
using GermanTrainer.Models;

namespace GermanTrainer.Services
{
    public class TrainerTranslationService
    {
        private readonly TrainerDbContext _context;
        private List<GermanNoun> _nouns;
        private List<GermanAdjective> _adjectives;
        private List<GermanVerb> _verbs;
        private List<object> _allWords;

        public TrainerTranslationService()
        {
            _context = new TrainerDbContext();
            _nouns = new List<GermanNoun>();
            _adjectives = new List<GermanAdjective>();
            _verbs = new List<GermanVerb>();
            _allWords = new List<object>();
            LoadWords();
        }

        public object? GetNextWord()
        {
            if (_allWords == null || _allWords.Count == 0)
                return null;

            var random = new Random();
            return _allWords[random.Next(_allWords.Count)];
        }

        // ✓ НОВЫЙ МЕТОД: Получить все существительные
        public List<GermanNoun> GetAllNouns()
        {
            return _context.GermanNouns.OrderBy(w => w.Lesson).ThenBy(w => w.Word).ToList();
        }

        // ✓ НОВЫЙ МЕТОД: Получить все прилагательные
        public List<GermanAdjective> GetAllAdjectives()
        {
            return _context.GermanAdjectives.OrderBy(w => w.Lesson).ThenBy(w => w.Word).ToList();
        }

        // ✓ НОВЫЙ МЕТОД: Получить все глаголы
        public List<GermanVerb> GetAllVerbs()
        {
            return _context.GermanVerbs.OrderBy(w => w.Lesson).ThenBy(w => w.Word).ToList();
        }

        // ✓ НОВЫЙ МЕТОД: Переключить видимость существительного
        public void ToggleNounVisibility(int nounId)
        {
            var noun = _context.GermanNouns.FirstOrDefault(w => w.Id == nounId);
            if (noun != null)
            {
                noun.IsHidden = !noun.IsHidden;
                _context.SaveChanges();
            }
        }

        // ✓ НОВЫЙ МЕТОД: Переключить видимость прилагательного
        public void ToggleAdjectiveVisibility(int adjectiveId)
        {
            var adj = _context.GermanAdjectives.FirstOrDefault(w => w.Id == adjectiveId);
            if (adj != null)
            {
                adj.IsHidden = !adj.IsHidden;
                _context.SaveChanges();
            }
        }

        // ✓ НОВЫЙ МЕТОД: Переключить видимость глагола
        public void ToggleVerbVisibility(int verbId)
        {
            var verb = _context.GermanVerbs.FirstOrDefault(w => w.Id == verbId);
            if (verb != null)
            {
                verb.IsHidden = !verb.IsHidden;
                _context.SaveChanges();
            }
        }

        public List<string> GenerateWrongAnswers(string correctAnswer, int count)
        {
            var random = new Random();
            var wrongAnswers = new List<string>();

            // Собираем все переводы (исключая правильный)
            var allTranslations = new List<string>();
            allTranslations.AddRange(_nouns.Select(n => n.Translation).Where(t => t != null && t != correctAnswer)!);
            allTranslations.AddRange(_adjectives.Select(a => a.Translation).Where(t => t != null && t != correctAnswer)!);
            allTranslations.AddRange(_verbs.Select(v => v.Translation).Where(t => t != null && t != correctAnswer)!);

            // Выбираем случайные уникальные варианты
            var distinct = allTranslations.Distinct().ToList();
            if (distinct.Count >= count)
            {
                wrongAnswers = distinct.OrderBy(x => random.Next()).Take(count).ToList();
            }
            else if (distinct.Count > 0)
            {
                wrongAnswers = distinct;
                // Добавляем заглушки если не хватает
                while (wrongAnswers.Count < count)
                {
                    wrongAnswers.Add($"вариант {wrongAnswers.Count + 1}");
                }
            }
            else
            {
                // На случай если в БД мало слов
                for (int i = 1; i <= count; i++)
                {
                    wrongAnswers.Add($"вариант {i}");
                }
            }

            return wrongAnswers;
        }

        public void RecordAnswer(object? word, bool isCorrect)
        {
            if (word == null) return;

            if (word is GermanNoun noun)
            {
                if (isCorrect) noun.CorrectAnswers++; else noun.WrongAnswers++;
            }
            else if (word is GermanAdjective adj)
            {
                if (isCorrect) adj.CorrectAnswers++; else adj.WrongAnswers++;
            }
            else if (word is GermanVerb verb)
            {
                if (isCorrect) verb.CorrectAnswers++; else verb.WrongAnswers++;
            }

            _context.SaveChanges();
        }

        // ✓ НОВЫЙ МЕТОД: ПЕРЕЗАГРУЗИТЬ СЛОВА ПОСЛЕ ИЗМЕНЕНИЯ НАСТРОЕК
        public void ReloadWords()
        {
            LoadWords();
        }

        private void LoadWords()
        {
            _nouns = new List<GermanNoun>();
            _adjectives = new List<GermanAdjective>();
            _verbs = new List<GermanVerb>();
            _allWords = new List<object>();

            // ✓ ПРОВЕРЯЕМ, КАКИЕ ТИПЫ СЛОВ ВКЛЮЧЕНЫ
            bool includeNouns = AppSettings.EnabledWordTypes.Count == 0 || AppSettings.EnabledWordTypes.Contains("Noun");
            bool includeAdjectives = AppSettings.EnabledWordTypes.Count == 0 || AppSettings.EnabledWordTypes.Contains("Adjective");
            bool includeVerbs = AppSettings.EnabledWordTypes.Count == 0 || AppSettings.EnabledWordTypes.Contains("Verb");

            // ✓ ЗАГРУЖАЕМ СУЩЕСТВИТЕЛЬНЫЕ
            if (includeNouns)
            {
                _nouns = _context.GermanNouns.Where(w => !w.IsHidden).ToList();
                if (AppSettings.EnabledLessons.Any())
                {
                    _nouns = _nouns.Where(w => AppSettings.EnabledLessons.Contains(w.Lesson)).ToList();
                }

                _allWords.AddRange(_nouns.Cast<object>());
            }

            // ✓ ЗАГРУЖАЕМ ПРИЛАГАТЕЛЬНЫЕ
            if (includeAdjectives)
            {
                _adjectives = _context.GermanAdjectives.Where(w => !w.IsHidden).ToList();
                if (AppSettings.EnabledLessons.Any())
                {
                    _adjectives = _adjectives.Where(w => AppSettings.EnabledLessons.Contains(w.Lesson)).ToList();
                }

                _allWords.AddRange(_adjectives.Cast<object>());
            }

            // ✓ ЗАГРУЖАЕМ ГЛАГОЛЫ
            if (includeVerbs)
            {
                _verbs = _context.GermanVerbs.Where(w => !w.IsHidden).ToList();
                if (AppSettings.EnabledLessons.Any())
                {
                    _verbs = _verbs.Where(w => AppSettings.EnabledLessons.Contains(w.Lesson)).ToList();
                }

                _allWords.AddRange(_verbs.Cast<object>());
            }
        }
    }
}