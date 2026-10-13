using GermanTrainer.Data;
using GermanTrainer.Models;

namespace GermanTrainer.Services
{
    public class TrainerTranslationService
    {
        private static readonly Random Random = new();

        private List<GermanNoun> _nouns = new();
        private List<GermanAdjective> _adjectives = new();
        private List<GermanVerb> _verbs = new();
        private List<object> _allWords = new();

        public TrainerTranslationService()
        {
            LoadWords();
        }

        public object? GetNextWord()
        {
            if (_allWords.Count == 0)
                return null;

            return _allWords[Random.Next(_allWords.Count)];
        }

        public List<string> GenerateWrongAnswers(string correctAnswer, int count)
        {
            var distinct = _nouns.Select(n => n.Translation)
                .Concat(_adjectives.Select(a => a.Translation))
                .Concat(_verbs.Select(v => v.Translation))
                .Where(t => !string.IsNullOrEmpty(t) && t != correctAnswer)
                .Distinct()
                .ToList();

            var wrongAnswers = distinct.Count >= count
                ? distinct.OrderBy(_ => Random.Next()).Take(count).ToList()
                : distinct;

            while (wrongAnswers.Count < count)
                wrongAnswers.Add($"вариант {wrongAnswers.Count + 1}");

            return wrongAnswers;
        }

        public void RecordAnswer(object? word, bool isCorrect)
        {
            if (word == null) return;

            using var context = new TrainerDbContext();

            switch (word)
            {
                case GermanNoun noun:
                    var dbNoun = context.GermanNouns.FirstOrDefault(w => w.Id == noun.Id);
                    if (dbNoun == null) return;
                    if (isCorrect) dbNoun.CorrectAnswers++; else dbNoun.WrongAnswers++;
                    noun.CorrectAnswers = dbNoun.CorrectAnswers;
                    noun.WrongAnswers = dbNoun.WrongAnswers;
                    break;

                case GermanAdjective adj:
                    var dbAdj = context.GermanAdjectives.FirstOrDefault(w => w.Id == adj.Id);
                    if (dbAdj == null) return;
                    if (isCorrect) dbAdj.CorrectAnswers++; else dbAdj.WrongAnswers++;
                    adj.CorrectAnswers = dbAdj.CorrectAnswers;
                    adj.WrongAnswers = dbAdj.WrongAnswers;
                    break;

                case GermanVerb verb:
                    var dbVerb = context.GermanVerbs.FirstOrDefault(w => w.Id == verb.Id);
                    if (dbVerb == null) return;
                    if (isCorrect) dbVerb.CorrectAnswers++; else dbVerb.WrongAnswers++;
                    verb.CorrectAnswers = dbVerb.CorrectAnswers;
                    verb.WrongAnswers = dbVerb.WrongAnswers;
                    break;

                default:
                    return;
            }

            context.SaveChanges();
        }

        public void ReloadWords()
        {
            LoadWords();
        }

        private void LoadWords()
        {
            using var context = new TrainerDbContext();
            var lessons = AppSettings.EnabledLessons.ToList();
            bool filterLessons = lessons.Count > 0;

            _nouns = AppSettings.IsWordTypeEnabled(WordType.Noun)
                ? context.GermanNouns
                    .Where(w => !w.IsHidden && (!filterLessons || lessons.Contains(w.Lesson)))
                    .ToList()
                : new List<GermanNoun>();

            _adjectives = AppSettings.IsWordTypeEnabled(WordType.Adjective)
                ? context.GermanAdjectives
                    .Where(w => !w.IsHidden && (!filterLessons || lessons.Contains(w.Lesson)))
                    .ToList()
                : new List<GermanAdjective>();

            _verbs = AppSettings.IsWordTypeEnabled(WordType.Verb)
                ? context.GermanVerbs
                    .Where(w => !w.IsHidden && (!filterLessons || lessons.Contains(w.Lesson)))
                    .ToList()
                : new List<GermanVerb>();

            _allWords = new List<object>();
            _allWords.AddRange(_nouns);
            _allWords.AddRange(_adjectives);
            _allWords.AddRange(_verbs);
        }
    }
}
