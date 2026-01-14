using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GermanTrainer.Models;
using System;
using System.Reflection.Emit;

namespace GermanTrainer.Data.Configuration
{
    partial class GermanVerbConfiguration
    {
        public static GermanVerb[] GetGermanVerbBatch1()
        {
            return new GermanVerb[]
            {
                new GermanVerb { Id = 1, Lesson = 1, Word = "heißen",       Translation = "зваться, называться",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
                new GermanVerb { Id = 2, Lesson = 1, Word = "kommen",       Translation = "приходить, приезжать",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
                new GermanVerb { Id = 3, Lesson = 1, Word = "wohnen",       Translation = "жить, проживать",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
                new GermanVerb { Id = 4, Lesson = 1, Word = "schreiben",    Translation = "писать",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
                new GermanVerb { Id = 5, Lesson = 1, Word = "buchstabieren", Translation = "произносить/писать по буквам",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
                new GermanVerb { Id = 6, Lesson = 1, Word = "machen",       Translation = "делать",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
                new GermanVerb { Id = 7, Lesson = 1, Word = "lernen",       Translation = "учить, изучать",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false }
            };
        }

        public static GermanVerb[] GetGermanVerbBatch2()
        {
            return new GermanVerb[]
            {
                    new GermanVerb { Id = 8,  Lesson = 2, Word = "sprechen", Translation = "говорить",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 9,  Lesson = 2, Word = "arbeiten", Translation = "работать",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 10, Lesson = 2, Word = "lernen",   Translation = "учить, учиться",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 11, Lesson = 2, Word = "lieben",   Translation = "любить",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 12, Lesson = 2, Word = "leben",    Translation = "жить",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 13, Lesson = 2, Word = "suchen",   Translation = "искать",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 14, Lesson = 2, Word = "kosten",   Translation = "стоить",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 15, Lesson = 2, Word = "schicken", Translation = "отправлять",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 16, Lesson = 2, Word = "liegen",   Translation = "находиться, лежать",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false }
        };
        }

        public static GermanVerb[] GetGermanVerbBatch3()
        {
            return new GermanVerb[]
            {
                    new GermanVerb { Id = 17, Lesson = 3, Word = "haben",    Translation = "иметь",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 18, Lesson = 3, Word = "brauchen", Translation = "нуждаться, нуждаться в",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 19, Lesson = 3, Word = "kaufen",   Translation = "покупать",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 20, Lesson = 3, Word = "bezahlen", Translation = "платить, оплачивать",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false }
            };
        }

        public static GermanVerb[] GetGermanVerbBatch4()
        {
            return new GermanVerb[]
            {
                    new GermanVerb { Id = 21, Lesson = 4, Word = "studieren",  Translation = "учиться (в вузе)",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 22, Lesson = 4, Word = "faulenzen",  Translation = "бездельничать",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 23, Lesson = 4, Word = "schlafen",   Translation = "спать",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 24, Lesson = 4, Word = "essen",      Translation = "есть",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 25, Lesson = 4, Word = "lesen",      Translation = "читать",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 26, Lesson = 4, Word = "sehen",      Translation = "смотреть, видеть",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 27, Lesson = 4, Word = "nehmen",     Translation = "брать",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 28, Lesson = 4, Word = "fahren",     Translation = "ехать, ездить",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 29, Lesson = 4, Word = "treffen",    Translation = "встречаться, встречать",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanVerb { Id = 30, Lesson = 4, Word = "gehen",      Translation = "идти",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 31, Lesson = 4, Word = "bleiben",    Translation = "оставаться",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 32, Lesson = 4, Word = "chillen",    Translation = "расслабляться, чиллить",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 33, Lesson = 4, Word = "besichtigen",Translation = "осматривать (город, музей и т. д.)",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 34, Lesson = 4, Word = "besuchen",   Translation = "посещать, навещать",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 35, Lesson = 4, Word = "essen (zu Mittag)", Translation = "обедать",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 36, Lesson = 4, Word = "trinken",    Translation = "пить",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 37, Lesson = 4, Word = "kennen",     Translation = "знать, быть знакомым",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false }
            };
        }

        public static GermanVerb[] GetGermanVerbBatch5()
        {
            return new GermanVerb[]
            {
                    // хобби, спорт
    new GermanVerb { Id = 38, Lesson = 5, Word = "spielen",        Translation = "играть",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 39, Lesson = 5, Word = "joggen",         Translation = "бегать трусцой",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 40, Lesson = 5, Word = "malen",          Translation = "рисовать (красками)",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 41, Lesson = 5, Word = "tanzen",         Translation = "танцевать",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 42, Lesson = 5, Word = "surfen",         Translation = "сидеть в интернете; серфить",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 43, Lesson = 5, Word = "grillen",        Translation = "жарить на гриле",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 44, Lesson = 5, Word = "hören",          Translation = "слушать",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 45, Lesson = 5, Word = "schwimmen",      Translation = "плавать",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    // время / расписание
    new GermanVerb { Id = 46, Lesson = 5, Word = "beginnen",       Translation = "начинаться",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 47, Lesson = 5, Word = "enden",          Translation = "заканчиваться",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 48, Lesson = 5, Word = "gehen",          Translation = "идти",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 49, Lesson = 5, Word = "klingeln",       Translation = "звенеть (о будильнике, звонке)",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    // отделяемые глаголы и др. действия
    new GermanVerb { Id = 50, Lesson = 5, Word = "anrufen",        Translation = "звонить по телефону",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 51, Lesson = 5, Word = "anfangen",       Translation = "начинать",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 52, Lesson = 5, Word = "aufhören",       Translation = "переставать, прекращать",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 53, Lesson = 5, Word = "aufstehen",      Translation = "вставать",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 54, Lesson = 5, Word = "aufräumen",      Translation = "убирать (в комнате)",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 55, Lesson = 5, Word = "einkaufen",      Translation = "закупаться, делать покупки",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 56, Lesson = 5, Word = "mitnehmen",      Translation = "брать с собой",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 57, Lesson = 5, Word = "fernsehen",      Translation = "смотреть телевизор",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 58, Lesson = 5, Word = "ausgehen",       Translation = "выходить куда‑то (в город)",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 59, Lesson = 5, Word = "wegfahren",      Translation = "уезжать",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 60, Lesson = 5, Word = "mitkommen",      Translation = "идти/поехать вместе",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 61, Lesson = 5, Word = "stattfinden",    Translation = "состояться, происходить",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 62, Lesson = 5, Word = "ausfallen",      Translation = "отменяться",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 63, Lesson = 5, Word = "einkaufen gehen",Translation = "ходить за покупками",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 64, Lesson = 5, Word = "spazieren gehen",Translation = "гулять (прогуливаться)",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    // раздел D, ещё глаголы
    new GermanVerb { Id = 65, Lesson = 5, Word = "reparieren",     Translation = "ремонтировать",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 66, Lesson = 5, Word = "kochen",         Translation = "готовить, варить",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false }
            };
        }

        public static GermanVerb[] GetGermanVerbBatch6()
        {
            return new GermanVerb[]
            {

            };
        }

        public static GermanVerb[] GetGermanVerbBatch7()
        {
            return new GermanVerb[]
            {

            };
        }
        public static GermanVerb[] GetGermanVerbBatch8()
        {
            return new GermanVerb[]
            {
    new GermanVerb { Id = 67, Lesson = 8, Word = "sagen",        Translation = "сказать", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 68, Lesson = 8, Word = "wehtun",       Translation = "болеть", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 69, Lesson = 8, Word = "aufmachen",    Translation = "открывать", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 70, Lesson = 8, Word = "sollen",       Translation = "быть должным; следует", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 71, Lesson = 8, Word = "geben",        Translation = "давать", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 72, Lesson = 8, Word = "untersuchen",  Translation = "обследовать", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 73, Lesson = 8, Word = "operieren",    Translation = "оперировать", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanVerb { Id = 74, Lesson = 8, Word = "messen",       Translation = "измерять", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false }
        };
        }
        public static GermanVerb[] GetGermanVerbBatch9()
        {
            return new GermanVerb[]
            {

            };
        }
    }
}