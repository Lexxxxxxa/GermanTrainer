using GermanTrainer.Models;
using GermanTrainer.Data;

namespace GermanTrainer.Services
{
    public class TrainerArticlesService
    {
        private readonly TrainerDbContext _context;

        public TrainerArticlesService()
        {
            _context = new TrainerDbContext();
        }

        public List<GermanNoun> GetAllNouns()
        {
            return _context.GermanNouns.OrderBy(w => w.Lesson).ThenBy(w => w.Word).ToList();
        }

        public List<GermanNoun> GetVisibleNouns()
        {
            var enabledLessons = AppSettings.EnabledLessons;
            if (enabledLessons.Count == 0)
            {
                return _context.GermanNouns.Where(w => !w.IsHidden).ToList();
            }

            return _context.GermanNouns
                .Where(w => !w.IsHidden && enabledLessons.Contains(w.Lesson))
                .ToList();
        }

        // ✓ НОВЫЙ МЕТОД: Получить все существительные (включая скрытые)
        public List<GermanNoun> GetAllNounsIncludingHidden()
        {
            return _context.GermanNouns.OrderBy(w => w.Lesson).ThenBy(w => w.Word).ToList();
        }

        // ✓ НОВЫЙ МЕТОД: Получить существительное по ID
        public GermanNoun? GetNounById(int id)
        {
            return _context.GermanNouns.FirstOrDefault(w => w.Id == id);
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

        public GermanNoun? GetNextNoun()
        {
            var words = GetVisibleNouns();
            if (words.Count == 0) return null;

            var totalWeight = words.Sum(w => CalculateWordWeight(w));
            if (totalWeight == 0) return words.OrderBy(x => Guid.NewGuid()).FirstOrDefault();

            var random = new Random();
            var randomValue = random.NextDouble() * totalWeight;
            double currentValue = 0;

            foreach (var word in words)
            {
                currentValue += CalculateWordWeight(word);
                if (randomValue <= currentValue)
                {
                    return word;
                }
            }

            return words.Last();
        }

        private double CalculateWordWeight<T>(T word) where T : class
        {
            dynamic w = word;
            if (w.TimesShown == 0) return 1.0;

            var errorRate = (double)w.WrongAnswers / w.TimesShown;
            var difficultyFactor = w.DifficultyCoefficient / 10.0;
            return (errorRate + 1) * (difficultyFactor + 0.5);
        }

        public void RecordAnswer(GermanNoun noun, bool isCorrect)
        {
            var dbNoun = _context.GermanNouns.FirstOrDefault(w => w.Id == noun.Id);
            if (dbNoun != null)
            {
                dbNoun.TimesShown++;
                if (isCorrect)
                {
                    dbNoun.CorrectAnswers++;
                }
                else
                {
                    dbNoun.WrongAnswers++;
                }

                if (dbNoun.DifficultyCoefficient < 10)
                {
                    dbNoun.DifficultyCoefficient++;
                }

                dbNoun.LastShownAt = DateTime.Now;
                _context.SaveChanges();
            }
        }

        public void AddNoun(string word, string article, string? translation = null, string? pluralForm = null, int lesson = 1)
        {
            if (!IsValidArticle(article))
                throw new ArgumentException("Артикль должен быть: der, die или das");

            var newNoun = new GermanNoun
            {
                Word = word.Trim(),
                Article = article.ToLower(),
                Translation = translation ?? string.Empty,
                PluralForm = pluralForm,
                Lesson = lesson,
                DifficultyCoefficient = 5,
                CreatedAt = DateTime.Now,
                IsHidden = false
            };

            _context.GermanNouns.Add(newNoun);
            _context.SaveChanges();
        }

        public void RemoveNoun(int nounId)
        {
            var noun = _context.GermanNouns.FirstOrDefault(w => w.Id == nounId);
            if (noun != null)
            {
                _context.GermanNouns.Remove(noun);
                _context.SaveChanges();
            }
        }

        public void ToggleNounVisibility(int nounId, bool isHidden)
        {
            var noun = _context.GermanNouns.FirstOrDefault(w => w.Id == nounId);
            if (noun != null)
            {
                noun.IsHidden = isHidden;
                _context.SaveChanges();
            }
        }

        public (int totalNouns, int correctTotal, int wrongTotal, double averageSuccessRate) GetNounStatistics()
        {
            var nouns = _context.GermanNouns.ToList();
            if (nouns.Count == 0)
                return (0, 0, 0, 0);

            var correctTotal = nouns.Sum(w => w.CorrectAnswers);
            var wrongTotal = nouns.Sum(w => w.WrongAnswers);
            var averageSuccessRate = nouns.Average(w => w.SuccessRate);

            return (nouns.Count, correctTotal, wrongTotal, averageSuccessRate);
        }

        public void ResetNounStatistics(int nounId)
        {
            var noun = _context.GermanNouns.FirstOrDefault(w => w.Id == nounId);
            if (noun != null)
            {
                noun.TimesShown = 0;
                noun.CorrectAnswers = 0;
                noun.WrongAnswers = 0;
                noun.DifficultyCoefficient = 5;
                noun.LastShownAt = null;
                _context.SaveChanges();
            }
        }

        private bool IsValidArticle(string article)
        {
            return article?.ToLower() switch
            {
                "der" or "die" or "das" => true,
                _ => false
            };
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}