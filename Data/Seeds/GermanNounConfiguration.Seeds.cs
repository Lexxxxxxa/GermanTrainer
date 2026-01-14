using GermanTrainer.Models;
using Microsoft.EntityFrameworkCore;

namespace GermanTrainer.Data.Configuration
{
    partial class GermanNounConfiguration
    {
        public static GermanNoun[] GetGermanNounBatch1()
        {
            return new GermanNoun[]
            {
                new GermanNoun { Id = 1,  Lesson = 1, Word = "Beruf",       Article = "der", Translation = "профессия",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

                new GermanNoun { Id = 2,  Lesson = 1, Word = "Arzt",        Article = "der", Translation = "врач (мужчина)",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
                new GermanNoun { Id = 3,  Lesson = 1, Word = "Ärztin",      Article = "die", Translation = "врач (женщина)",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

                new GermanNoun { Id = 4,  Lesson = 1, Word = "Lehrer",      Article = "der", Translation = "учитель",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
                new GermanNoun { Id = 5,  Lesson = 1, Word = "Lehrerin",    Article = "die", Translation = "учительница",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

                new GermanNoun { Id = 6,  Lesson = 1, Word = "Verkäufer",   Article = "der", Translation = "продавец",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
                new GermanNoun { Id = 7,  Lesson = 1, Word = "Verkäuferin", Article = "die", Translation = "продавщица",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

                new GermanNoun { Id = 8,  Lesson = 1, Word = "Ingenieur",   Article = "der", Translation = "инженер",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
                new GermanNoun { Id = 9,  Lesson = 1, Word = "Ingenieurin", Article = "die", Translation = "инженер (женщина)",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

                new GermanNoun { Id = 10, Lesson = 1, Word = "Familienname", Article = "der", Translation = "фамилия",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
                new GermanNoun { Id = 11, Lesson = 1, Word = "Vorname",      Article = "der", Translation = "имя",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
                new GermanNoun { Id = 12, Lesson = 1, Word = "Land",         Article = "das", Translation = "страна",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

                new GermanNoun { Id = 13, Lesson = 1, Word = "Name",         Article = "der", Translation = "имя; название",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
                new GermanNoun { Id = 14, Lesson = 1, Word = "Haus",         Article = "das", Translation = "дом",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

                new GermanNoun { Id = 15, Lesson = 1, Word = "Entschuldigung", Article = "die", Translation = "извините; извинение",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

                new GermanNoun { Id = 16, Lesson = 1, Word = "Frau",        Article = "die", Translation = "миссис; госпожа; женщина",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
                new GermanNoun { Id = 17, Lesson = 1, Word = "Herr",        Article = "der", Translation = "мистер; господин",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

                new GermanNoun { Id = 18, Lesson = 1, Word = "Deutsch",     Article = "",    Translation = "немецкий язык",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

                new GermanNoun { Id = 19, Lesson = 1, Word = "Zahl",        Article = "die", Translation = "число",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
                new GermanNoun { Id = 20, Lesson = 1, Word = "Nummer",      Article = "die", Translation = "номер",
                                 DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false }
            };
        }

        public static GermanNoun[] GetGermanNounBatch2()
        {
            return new GermanNoun[]
            {
                    new GermanNoun { Id = 21, Lesson = 2, Word = "Heimat",          Article = "die", Translation = "родина",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 22, Lesson = 2, Word = "CD",              Article = "die", Translation = "диск",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 23, Lesson = 2, Word = "Flasche",         Article = "die", Translation = "бутылка",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 24, Lesson = 2, Word = "Tür",             Article = "die", Translation = "дверь",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 25, Lesson = 2, Word = "Uhr",             Article = "die", Translation = "часы",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 26, Lesson = 2, Word = "Tafel",           Article = "die", Translation = "доска",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 27, Lesson = 2, Word = "Tasche",          Article = "die", Translation = "сумка",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 28, Lesson = 2, Word = "Nationalität",    Article = "die", Translation = "национальность",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 29, Lesson = 2, Word = "Sprache",         Article = "die", Translation = "язык",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 30, Lesson = 2, Word = "Muttersprache",   Article = "die", Translation = "родной язык",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 31, Lesson = 2, Word = "Arbeit",          Article = "die", Translation = "работа",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 32, Lesson = 2, Word = "Stift",           Article = "der", Translation = "ручка",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 33, Lesson = 2, Word = "Kugelschreiber",  Article = "der", Translation = "шариковая ручка",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 34, Lesson = 2, Word = "Kuli",            Article = "der", Translation = "ручка (кулі)",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 35, Lesson = 2, Word = "Laptop",          Article = "der", Translation = "ноутбук",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 36, Lesson = 2, Word = "Tablet",          Article = "das", Translation = "планшет",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 37, Lesson = 2, Word = "Schlüssel",       Article = "der", Translation = "ключ",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 38, Lesson = 2, Word = "Tisch",           Article = "der", Translation = "стол",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 39, Lesson = 2, Word = "Stuhl",           Article = "der", Translation = "стул",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 40, Lesson = 2, Word = "Buch",            Article = "das", Translation = "книга",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 41, Lesson = 2, Word = "Heft",            Article = "das", Translation = "тетрадь",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 42, Lesson = 2, Word = "Handy",           Article = "das", Translation = "телефон (мобильный)",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 43, Lesson = 2, Word = "Fenster",         Article = "das", Translation = "окно",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 44, Lesson = 2, Word = "Lampe",           Article = "die", Translation = "лампа",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 45, Lesson = 2, Word = "Brille",          Article = "die", Translation = "очки",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 46, Lesson = 2, Word = "Vorwahl",         Article = "die", Translation = "код города (телефонный)",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 47, Lesson = 2, Word = "Telefonnummer",   Article = "die", Translation = "телефонный номер",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 48, Lesson = 2, Word = "Kita",            Article = "die", Translation = "детский сад",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 49, Lesson = 2, Word = "Platz",           Article = "der", Translation = "место",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 50, Lesson = 2, Word = "Adresse",         Article = "die", Translation = "адрес",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 51, Lesson = 2, Word = "Straße",          Article = "die", Translation = "улица",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 52, Lesson = 2, Word = "Postleitzahl",    Article = "die", Translation = "почтовый индекс",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 53, Lesson = 2, Word = "E-Mail-Adresse",  Article = "die", Translation = "e-mail адрес",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 54, Lesson = 2, Word = "Euro",            Article = "der", Translation = "евро",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 55, Lesson = 2, Word = "Anmeldeformular", Article = "das", Translation = "регистрационная форма",
                     DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false }
            };
        }

        public static GermanNoun[] GetGermanNounBatch3()
        {
            return new GermanNoun[]
            {
                    new GermanNoun { Id = 56, Lesson = 3, Word = "Wohnung",      Article = "die", Translation = "квартира",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 57, Lesson = 3, Word = "Wohnzimmer",   Article = "das", Translation = "гостиная",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 58, Lesson = 3, Word = "Schlafzimmer", Article = "das", Translation = "спальня",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 59, Lesson = 3, Word = "Küche",        Article = "die", Translation = "кухня",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 60, Lesson = 3, Word = "Möbel",        Article = "die", Translation = "мебель",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 61, Lesson = 3, Word = "Schrank",      Article = "der", Translation = "шкаф",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 62, Lesson = 3, Word = "Sessel",       Article = "der", Translation = "кресло",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 63, Lesson = 3, Word = "Sofa",         Article = "das", Translation = "диван",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 64, Lesson = 3, Word = "Regal",        Article = "das", Translation = "полка, стеллаж",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 65, Lesson = 3, Word = "Teppich",      Article = "der", Translation = "ковёр",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 66, Lesson = 3, Word = "Bild",         Article = "das", Translation = "картина",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 67, Lesson = 3, Word = "Vorhang",      Article = "der", Translation = "штора",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 68, Lesson = 3, Word = "Wand",         Article = "die", Translation = "стена",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 69, Lesson = 3, Word = "Fernseher",    Article = "der", Translation = "телевизор",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 70, Lesson = 3, Word = "Herd",         Article = "der", Translation = "плита",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 71, Lesson = 3, Word = "Spüle",        Article = "die", Translation = "раковина (кухонная)",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 72, Lesson = 3, Word = "Spülmaschine", Article = "die", Translation = "посудомоечная машина",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 73, Lesson = 3, Word = "Mikrowelle",   Article = "die", Translation = "микроволновка",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 74, Lesson = 3, Word = "Waschmaschine",Article = "die", Translation = "стиральная машина",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 75, Lesson = 3, Word = "Farbe",        Article = "die", Translation = "краска, цвет",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 76, Lesson = 3, Word = "Kühlschrank",  Article = "der", Translation = "холодильник",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 77, Lesson = 3, Word = "Blume",        Article = "die", Translation = "цветок",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 78, Lesson = 3, Word = "Erdgeschoss",  Article = "das", Translation = "первый этаж, цокольный этаж",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 79, Lesson = 3, Word = "Stock",        Article = "der", Translation = "этаж",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 80, Lesson = 3, Word = "Dachgeschoss", Article = "das", Translation = "мансарда, верхний этаж",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 81, Lesson = 3, Word = "Geschäft",     Article = "das", Translation = "магазин",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 82, Lesson = 3, Word = "Miete",        Article = "die", Translation = "арендная плата",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 83, Lesson = 3, Word = "Nebenkosten",  Article = "die", Translation = "коммунальные расходы",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 84, Lesson = 3, Word = "Quadratmeter", Article = "der", Translation = "квадратный метр",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 85, Lesson = 3, Word = "Balkon",       Article = "der", Translation = "балкон",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 86, Lesson = 3, Word = "Garten",       Article = "der", Translation = "сад",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 87, Lesson = 3, Word = "Nähe",         Article = "die", Translation = "близость (в выражении „in der Nähe“)",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false }
            };
        }
        public static GermanNoun[] GetGermanNounBatch4()
        {
            return new GermanNoun[]
            {
                    new GermanNoun { Id = 88, Lesson = 4, Word = "Vater",        Article = "der", Translation = "отец",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 89, Lesson = 4, Word = "Mutter",       Article = "die", Translation = "мать",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 90, Lesson = 4, Word = "Eltern",       Article = "die", Translation = "родители",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 91, Lesson = 4, Word = "Bruder",       Article = "der", Translation = "брат",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 92, Lesson = 4, Word = "Schwester",    Article = "die", Translation = "сестра",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 93, Lesson = 4, Word = "Geschwister",  Article = "die", Translation = "братья и сёстры",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 94, Lesson = 4, Word = "Onkel",        Article = "der", Translation = "дядя",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 95, Lesson = 4, Word = "Tante",        Article = "die", Translation = "тётя",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 96, Lesson = 4, Word = "Sohn",         Article = "der", Translation = "сын",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 97, Lesson = 4, Word = "Tochter",      Article = "die", Translation = "дочь",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 98, Lesson = 4, Word = "Foto",         Article = "das", Translation = "фото",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 99, Lesson = 4, Word = "Freizeit",     Article = "die", Translation = "досуг, свободное время",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 100, Lesson = 4, Word = "Familie",     Article = "die", Translation = "семья",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 101, Lesson = 4, Word = "Schokolade",  Article = "die", Translation = "шоколад",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 102, Lesson = 4, Word = "Pizza",       Article = "die", Translation = "пицца",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 103, Lesson = 4, Word = "Film",        Article = "der", Translation = "фильм",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 104, Lesson = 4, Word = "Zeit",        Article = "die", Translation = "время",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 105, Lesson = 4, Word = "Sehenswürdigkeit", Article = "die", Translation = "достопримечательность",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 106, Lesson = 4, Word = "Wochenende",  Article = "das", Translation = "выходные",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 107, Lesson = 4, Word = "Tag",         Article = "der", Translation = "день",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 108, Lesson = 4, Word = "Straßenfest", Article = "das", Translation = "уличный праздник",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 109, Lesson = 4, Word = "Kaffee",      Article = "der", Translation = "кофе",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false }
            };
        }
        public static GermanNoun[] GetGermanNounBatch5()
        {
            return new GermanNoun[]
            {
                    // время и расписание
    new GermanNoun { Id = 110, Lesson = 5, Word = "Kurs",         Article = "der", Translation = "курс",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 111, Lesson = 5, Word = "Pause",        Article = "die", Translation = "перерыв, пауза",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    // транспорт, будильник, самолет, детектив
    new GermanNoun { Id = 112, Lesson = 5, Word = "Zug",          Article = "der", Translation = "поезд",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 113, Lesson = 5, Word = "Radiowecker",  Article = "der", Translation = "радио‑будильник",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 114, Lesson = 5, Word = "Flugzeug",     Article = "das", Translation = "самолёт",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 115, Lesson = 5, Word = "Krimi",        Article = "der", Translation = "детектив (фильм/книга)",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    // кино, газета, прогулка, экскурсия
    new GermanNoun { Id = 116, Lesson = 5, Word = "Kino",         Article = "das", Translation = "кино, кинотеатр",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 117, Lesson = 5, Word = "Zeitung",      Article = "die", Translation = "газета",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 118, Lesson = 5, Word = "Ausflug",      Article = "der", Translation = "поездка, экскурсия",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    // неделя и дни недели
    new GermanNoun { Id = 119, Lesson = 5, Word = "Woche",        Article = "die", Translation = "неделя",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 120, Lesson = 5, Word = "Montag",       Article = "der", Translation = "понедельник",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 121, Lesson = 5, Word = "Dienstag",     Article = "der", Translation = "вторник",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 122, Lesson = 5, Word = "Mittwoch",     Article = "der", Translation = "среда",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 123, Lesson = 5, Word = "Donnerstag",   Article = "der", Translation = "четверг",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 124, Lesson = 5, Word = "Freitag",      Article = "der", Translation = "пятница",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 125, Lesson = 5, Word = "Samstag",      Article = "der", Translation = "суббота",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 126, Lesson = 5, Word = "Sonntag",      Article = "der", Translation = "воскресенье",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    // домашка, велосипед, билет, вечер вторника
    new GermanNoun { Id = 127, Lesson = 5, Word = "Hausaufgabe",  Article = "die", Translation = "домашнее задание",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 128, Lesson = 5, Word = "Fahrrad",      Article = "das", Translation = "велосипед",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 129, Lesson = 5, Word = "Fahrkarte",    Article = "die", Translation = "проездной билет",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 130, Lesson = 5, Word = "Dienstagabend",Article = "der", Translation = "вечер вторника",
        DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false }
            };
        }
        public static GermanNoun[] GetGermanNounBatch6()
        {
            return new GermanNoun[]
            {

            };
        }
        public static GermanNoun[] GetGermanNounBatch7()
        {
            return new GermanNoun[]
            {

            };
        }
        public static GermanNoun[] GetGermanNounBatch8()
        {
            return new GermanNoun[]
            {
    new GermanNoun { Id = 131, Lesson = 8, Word = "Hausarzt",         Article = "der",   Translation = "семейный врач (терапевт)", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 132, Lesson = 8, Word = "Hausärztin",       Article = "die",   Translation = "семейный врач (женщина)", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 133, Lesson = 8, Word = "Zahnarzt",         Article = "der",   Translation = "стоматолог", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 134, Lesson = 8, Word = "Zahnärztin",       Article = "die",   Translation = "стоматолог (женщина)", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 135, Lesson = 8, Word = "Sprechzeiten",     Article = "die",   Translation = "часы приёма", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 136, Lesson = 8, Word = "Zahnschmerzen",    Article = "die",   Translation = "зубная боль", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 137, Lesson = 8, Word = "Halsschmerzen",    Article = "die",   Translation = "боль в горле", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 138, Lesson = 8, Word = "Bauchschmerzen",   Article = "die",   Translation = "боль в животе", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 139, Lesson = 8, Word = "Rückenschmerzen",  Article = "die",   Translation = "боль в спине", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 140, Lesson = 8, Word = "Praxis",           Article = "die",   Translation = "врачебная практика; кабинет", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 141, Lesson = 8, Word = "Kopf",             Article = "der",   Translation = "голова", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 142, Lesson = 8, Word = "Auge",             Article = "das",   Translation = "глаз", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 143, Lesson = 8, Word = "Nase",             Article = "die",   Translation = "нос", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 144, Lesson = 8, Word = "Mund",             Article = "der",   Translation = "рот", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 145, Lesson = 8, Word = "Ohr",              Article = "das",   Translation = "ухо", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 146, Lesson = 8, Word = "Hals",             Article = "der",   Translation = "горло; шея", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 147, Lesson = 8, Word = "Bein",             Article = "das",   Translation = "нога", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 148, Lesson = 8, Word = "Fuß",              Article = "der",   Translation = "ступня", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 149, Lesson = 8, Word = "Arm",              Article = "der",   Translation = "рука", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 150, Lesson = 8, Word = "Finger",           Article = "der",   Translation = "палец", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 151, Lesson = 8, Word = "Rücken",           Article = "der",   Translation = "спина", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 152, Lesson = 8, Word = "Bauch",            Article = "der",   Translation = "живот", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 153, Lesson = 8, Word = "Schnupfen",        Article = "der",   Translation = "насморк", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 154, Lesson = 8, Word = "Husten",           Article = "der",   Translation = "кашель", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 155, Lesson = 8, Word = "Fieber",           Article = "das",   Translation = "температура; лихорадка", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 156, Lesson = 8, Word = "Erkältung",        Article = "die",   Translation = "простуда", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 157, Lesson = 8, Word = "Grippe",           Article = "die",   Translation = "грипп", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 158, Lesson = 8, Word = "Kontrolle",        Article = "die",   Translation = "контроль; осмотр", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 159, Lesson = 8, Word = "Tablette",         Article = "die",   Translation = "таблетка", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 160, Lesson = 8, Word = "Gesundheit",       Article = "die",   Translation = "здоровье", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 161, Lesson = 8, Word = "Krankenkasse",     Article = "die",   Translation = "больничная касса; медстраховка", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 162, Lesson = 8, Word = "Zahnkontrolle",    Article = "die",   Translation = "проверка зубов", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 163, Lesson = 8, Word = "Bonusheft",        Article = "das",   Translation = "бонусная книжка (у стоматолога)", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 164, Lesson = 8, Word = "Gesundheitskarte", Article = "die",   Translation = "медицинская карта (страховая)", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 165, Lesson = 8, Word = "Krankschreibung",  Article = "die",   Translation = "больничный (справка)", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 166, Lesson = 8, Word = "Arbeitgeber",      Article = "der/die", Translation = "работодатель", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 167, Lesson = 8, Word = "Medikament",       Article = "das",   Translation = "лекарство", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 168, Lesson = 8, Word = "Rezept",           Article = "das",   Translation = "рецепт", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 169, Lesson = 8, Word = "Apotheke",         Article = "die",   Translation = "аптека", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 170, Lesson = 8, Word = "Entschuldigungsschreiben",   Article = "die",   Translation = "записка с извинением", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 171, Lesson = 8, Word = "Unterricht",       Article = "der",   Translation = "урок; занятие", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 172, Lesson = 8, Word = "Notarzt",          Article = "der",   Translation = "врач скорой помощи", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 173, Lesson = 8, Word = "Unfall",           Article = "der",   Translation = "авария; несчастный случай", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 174, Lesson = 8, Word = "Minute",           Article = "die",   Translation = "минута", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 175, Lesson = 8, Word = "Ecke",             Article = "die",   Translation = "угол", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 176, Lesson = 8, Word = "Feuer",            Article = "das",   Translation = "огонь; пожар", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 177, Lesson = 8, Word = "Feuerwehr",        Article = "die",   Translation = "пожарная служба", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 178, Lesson = 8, Word = "Krankenwagen",     Article = "der",   Translation = "скорая помощь (машина)", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },

    new GermanNoun { Id = 179, Lesson = 8, Word = "Notruf",           Article = "der",   Translation = "экстренный вызов", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
    new GermanNoun { Id = 180, Lesson = 8, Word = "Notfall",          Article = "der",   Translation = "неотложный случай", DifficultyCoefficient = 5, TimesShown = 0, CorrectAnswers = 0, WrongAnswers = 0, IsHidden = false },
        };
        }
        public static GermanNoun[] GetGermanNounBatch9()
        {
            return new GermanNoun[]
            {
            };
        }
    }
}