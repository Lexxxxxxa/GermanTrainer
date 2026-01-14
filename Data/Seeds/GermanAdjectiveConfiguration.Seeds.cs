using GermanTrainer.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace GermanTrainer.Data.Configuration
{
    partial class GermanAdjectiveConfiguration
    {
        public static GermanAdjective[] GetGermanAdjectiveBatch1()
        {
            return new GermanAdjective[]
            {
                new GermanAdjective { Id = 1, Lesson = 1, Word = "neu", Translation = "новый",
                                      DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
                new GermanAdjective { Id = 2, Lesson = 1, Word = "gut", Translation = "хороший",
                                      DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false }
            };
        }

        public static GermanAdjective[] GetGermanAdjectiveBatch2()
        {
            return new GermanAdjective[]
            {
                    new GermanAdjective { Id = 3, Lesson = 2, Word = "richtig",     Translation = "правильно, правильный",
                          DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 4, Lesson = 2, Word = "kaputt",      Translation = "сломанный, испорченный",
                          DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 5, Lesson = 2, Word = "interessant", Translation = "интересный",
                          DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 6, Lesson = 2, Word = "frei",        Translation = "свободный",
                          DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false }
            };
        }

        public static GermanAdjective[] GetGermanAdjectiveBatch3()
        {
            return new GermanAdjective[]
            {
                    new GermanAdjective { Id = 7,  Lesson = 3, Word = "modern",      Translation = "современный",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 8,  Lesson = 3, Word = "groß",        Translation = "большой",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 9,  Lesson = 3, Word = "klein",       Translation = "маленький",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 10, Lesson = 3, Word = "schön",       Translation = "красивый",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 11, Lesson = 3, Word = "hässlich",    Translation = "некрасивый",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 12, Lesson = 3, Word = "bequem",      Translation = "удобный",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 13, Lesson = 3, Word = "unbequem",    Translation = "неудобный",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanAdjective { Id = 14, Lesson = 3, Word = "gelb",        Translation = "жёлтый",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 15, Lesson = 3, Word = "rot",         Translation = "красный",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 16, Lesson = 3, Word = "rosa",        Translation = "розовый",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 17, Lesson = 3, Word = "lila",        Translation = "лиловый, фиолетовый",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 18, Lesson = 3, Word = "blau",        Translation = "синий",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 19, Lesson = 3, Word = "grün",        Translation = "зелёный",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 20, Lesson = 3, Word = "braun",       Translation = "коричневый",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 21, Lesson = 3, Word = "schwarz",     Translation = "чёрный",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 22, Lesson = 3, Word = "weiß",        Translation = "белый",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 23, Lesson = 3, Word = "grau",        Translation = "серый",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 24, Lesson = 3, Word = "orange",      Translation = "оранжевый",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanAdjective { Id = 25, Lesson = 3, Word = "super",       Translation = "супер, классный",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 26, Lesson = 3, Word = "toll",        Translation = "классный",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 27, Lesson = 3, Word = "gemütlich",   Translation = "уютный",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 28, Lesson = 3, Word = "ganz schön",  Translation = "довольно красивый",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 29, Lesson = 3, Word = "schlecht",    Translation = "плохой",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 30, Lesson = 3, Word = "langweilig",  Translation = "скучный",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 31, Lesson = 3, Word = "furchtbar",   Translation = "ужасный",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanAdjective { Id = 32, Lesson = 3, Word = "hell",        Translation = "светлый",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 33, Lesson = 3, Word = "dunkel",      Translation = "тёмный",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 34, Lesson = 3, Word = "warm",        Translation = "тёплый",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 35, Lesson = 3, Word = "kalt",        Translation = "холодный",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 36, Lesson = 3, Word = "ruhig",       Translation = "тихий, спокойный",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 37, Lesson = 3, Word = "laut",        Translation = "громкий",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 38, Lesson = 3, Word = "günstig",     Translation = "недорогой, выгодный",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false }
            };
        }

        public static GermanAdjective[] GetGermanAdjectiveBatch4()
        {
            return new GermanAdjective[]
            {

            };
        }
        public static GermanAdjective[] GetGermanAdjectiveBatch5()
        {
            return new GermanAdjective[]
            {
                    new GermanAdjective
    {
        Id = 39,
        Lesson = 5,
        Word = "spät",
        Translation = "поздно",
        DifficultyCoefficient = 5,
        TimesShown = 0,
        CorrectAnswers = 0,
        WrongAnswers = 0,
        IsHidden = false
    },
    new GermanAdjective
    {
        Id = 40,
        Lesson = 5,
        Word = "vielleicht",
        Translation = "возможно",
        DifficultyCoefficient = 5,
        TimesShown = 0,
        CorrectAnswers = 0,
        WrongAnswers = 0,
        IsHidden = false
    },
    new GermanAdjective
    {
        Id = 41,
        Lesson = 5,
        Word = "zusammen",
        Translation = "вместе",
        DifficultyCoefficient = 5,
        TimesShown = 0,
        CorrectAnswers = 0,
        WrongAnswers = 0,
        IsHidden = false
    },
    new GermanAdjective
    {
        Id = 42,
        Lesson = 5,
        Word = "später",
        Translation = "позже",
        DifficultyCoefficient = 5,
        TimesShown = 0,
        CorrectAnswers = 0,
        WrongAnswers = 0,
        IsHidden = false
    }
            };
        }
        public static GermanAdjective[] GetGermanAdjectiveBatch6()
        {
            return new GermanAdjective[]
            {

            };
        }
        public static GermanAdjective[] GetGermanAdjectiveBatch7()
        {
            return new GermanAdjective[]
            {
            };
        }
        public static GermanAdjective[] GetGermanAdjectiveBatch8()
        {
            return new GermanAdjective[]
            {
    new GermanAdjective { Id = 43, Lesson = 8, Word = "dringend",    Translation = "срочно", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 44, Lesson = 8, Word = "regelmäßig", Translation = "регулярно", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 45, Lesson = 8, Word = "krank",      Translation = "больной", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 46, Lesson = 8, Word = "sofort",     Translation = "сразу, немедленно", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 47, Lesson = 8, Word = "besser",     Translation = "лучше", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 48, Lesson = 8, Word = "erkältet",   Translation = "простуженный", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanAdjective { Id = 49, Lesson = 8, Word = "verletzt",   Translation = "раненый; травмированный", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false }
        };
        }
        public static GermanAdjective[] GetGermanAdjectiveBatch9()
        {
            return new GermanAdjective[]
            {
            };
        }
    }
}