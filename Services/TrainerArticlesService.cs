using GermanTrainer.Data;
using GermanTrainer.Models;

namespace GermanTrainer.Services
{
    public class TrainerArticlesService
    {
        private static readonly Random Random = new();

        public List<GermanNoun> GetAllNouns()
        {
            using var context = new TrainerDbContext();
            return context.GermanNouns.OrderBy(w => w.Lesson).ThenBy(w => w.Word).ToList();
        }

        public List<GermanNoun> GetVisibleNouns()
        {
            using var context = new TrainerDbContext();
            var enabledLessons = AppSettings.EnabledLessons.ToList();

            var query = context.GermanNouns.Where(w => !w.IsHidden);
            if (enabledLessons.Count > 0)
                query = query.Where(w => enabledLessons.Contains(w.Lesson));

            return query.ToList();
        }

        public GermanNoun? GetNounById(int id)
        {
            using var context = new TrainerDbContext();
            return context.GermanNouns.FirstOrDefault(w => w.Id == id);
        }

        public GermanNoun? GetNextNoun()
        {
            var words = GetVisibleNouns();
            if (words.Count == 0) return null;

            var totalWeight = words.Sum(CalculateWordWeight);
            if (totalWeight <= 0) return words[Random.Next(words.Count)];

            var randomValue = Random.NextDouble() * totalWeight;
            double currentValue = 0;

            foreach (var word in words)
            {
                currentValue += CalculateWordWeight(word);
                if (randomValue <= currentValue)
                    return word;
            }

            return words[^1];
        }

        private static double CalculateWordWeight(GermanNoun word)
        {
            if (word.TimesShown == 0) return 1.0;

            var errorRate = (double)word.WrongAnswers / word.TimesShown;
            var difficultyFactor = word.DifficultyCoefficient / 10.0;
            return (errorRate + 1) * (difficultyFactor + 0.5);
        }

        public void RecordAnswer(GermanNoun noun, bool isCorrect)
        {
            using var context = new TrainerDbContext();
            var dbNoun = context.GermanNouns.FirstOrDefault(w => w.Id == noun.Id);
            if (dbNoun == null) return;

            dbNoun.TimesShown++;
            if (isCorrect)
                dbNoun.CorrectAnswers++;
            else
                dbNoun.WrongAnswers++;

            if (dbNoun.DifficultyCoefficient < 10)
                dbNoun.DifficultyCoefficient++;

            dbNoun.LastShownAt = DateTime.Now;
            context.SaveChanges();
        }

        public void AddNoun(string word, string article, string? translation = null, string? pluralForm = null, int lesson = 1)
        {
            if (!IsValidArticle(article))
                throw new ArgumentException("Артикль должен быть: der, die или das");

            using var context = new TrainerDbContext();
            context.GermanNouns.Add(new GermanNoun
            {
                Word = word.Trim(),
                Article = article.ToLowerInvariant(),
                Translation = translation ?? string.Empty,
                PluralForm = pluralForm,
                Lesson = lesson,
                DifficultyCoefficient = 5,
                CreatedAt = DateTime.Now,
                IsHidden = false
            });
            context.SaveChanges();
        }

        public void RemoveNoun(int nounId)
        {
            using var context = new TrainerDbContext();
            var noun = context.GermanNouns.FirstOrDefault(w => w.Id == nounId);
            if (noun == null) return;

            context.GermanNouns.Remove(noun);
            context.SaveChanges();
        }

        public void ToggleNounVisibility(int nounId)
        {
            using var context = new TrainerDbContext();
            var noun = context.GermanNouns.FirstOrDefault(w => w.Id == nounId);
            if (noun == null) return;

            noun.IsHidden = !noun.IsHidden;
            context.SaveChanges();
        }

        public void ToggleNounVisibility(int nounId, bool isHidden)
        {
            using var context = new TrainerDbContext();
            var noun = context.GermanNouns.FirstOrDefault(w => w.Id == nounId);
            if (noun == null) return;

            noun.IsHidden = isHidden;
            context.SaveChanges();
        }

        public (int totalNouns, int correctTotal, int wrongTotal, double averageSuccessRate) GetNounStatistics()
        {
            using var context = new TrainerDbContext();
            var nouns = context.GermanNouns.ToList();
            if (nouns.Count == 0)
                return (0, 0, 0, 0);

            return (nouns.Count,
                    nouns.Sum(w => w.CorrectAnswers),
                    nouns.Sum(w => w.WrongAnswers),
                    nouns.Average(w => w.SuccessRate));
        }

        public void ResetNounStatistics(int nounId)
        {
            using var context = new TrainerDbContext();
            var noun = context.GermanNouns.FirstOrDefault(w => w.Id == nounId);
            if (noun == null) return;

            noun.TimesShown = 0;
            noun.CorrectAnswers = 0;
            noun.WrongAnswers = 0;
            noun.DifficultyCoefficient = 5;
            noun.LastShownAt = null;
            context.SaveChanges();
        }

        private static bool IsValidArticle(string? article) =>
            article?.ToLowerInvariant() is "der" or "die" or "das";
    }
}
