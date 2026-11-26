using GermanTrainer.Core.Models;

namespace GermanTrainer.Infrastructure.Data.Configuration;

public static class VerbPrepositionSeeds
{
    public static readonly (string Word, string Display, string Prep, GrammaticalCase Case, string Meaning)[] Entries =
    [
        ("warten", "warten", "auf", GrammaticalCase.Akkusativ, "ждать кого-либо / чего-либо"),
        ("sprechen", "sprechen", "mit", GrammaticalCase.Dativ, "разговаривать с собеседником"),
        ("sprechen", "sprechen", "über", GrammaticalCase.Akkusativ, "обсуждать какую-либо тему"),
        ("reden", "reden", "mit", GrammaticalCase.Dativ, "разговаривать с собеседником"),
        ("reden", "reden", "über", GrammaticalCase.Akkusativ, "обсуждать какую-либо тему"),
        ("arbeiten", "arbeiten", "an", GrammaticalCase.Dativ, "работать над задачей / проектом"),
        ("arbeiten", "arbeiten", "bei", GrammaticalCase.Dativ, "работать у работодателя / в компании"),
        ("leben", "leben", "von", GrammaticalCase.Dativ, "жить за счёт источника дохода"),
        ("suchen", "suchen", "nach", GrammaticalCase.Dativ, "разыскивать кого-либо / что-либо"),
        ("beginnen", "beginnen", "mit", GrammaticalCase.Dativ, "начинать с чего-либо"),
        ("aufhören", "aufhören", "mit", GrammaticalCase.Dativ, "прекращать какое-либо занятие"),
        ("helfen", "helfen", "bei", GrammaticalCase.Dativ, "помогать в выполнении дела"),
        ("träumen", "träumen", "von", GrammaticalCase.Dativ, "мечтать о чём-либо"),
        ("spielen", "spielen", "mit", GrammaticalCase.Dativ, "играть с партнёром"),
        ("spielen", "spielen", "um", GrammaticalCase.Akkusativ, "играть на ставку / ради выигрыша"),
        ("schreiben", "schreiben", "an", GrammaticalCase.Akkusativ, "писать адресату"),
        ("kämpfen", "kämpfen", "für", GrammaticalCase.Akkusativ, "бороться в поддержку идеи / права"),
        ("kämpfen", "kämpfen", "gegen", GrammaticalCase.Akkusativ, "бороться против противника / явления"),
        ("kämpfen", "kämpfen", "um", GrammaticalCase.Akkusativ, "бороться за получение чего-либо"),
        ("achten (auf)", "achten", "auf", GrammaticalCase.Akkusativ, "обращать внимание на кого-либо / что-либо"),
        ("anfangen", "anfangen", "mit", GrammaticalCase.Dativ, "начинать с какого-либо дела"),
        ("danken", "danken", "für", GrammaticalCase.Akkusativ, "благодарить за что-либо"),
        ("gehören", "gehören", "zu", GrammaticalCase.Dativ, "принадлежать к группе / относиться к чему-либо"),
        ("passen", "passen", "zu", GrammaticalCase.Dativ, "подходить к чему-либо / сочетаться"),
        ("sprechen", "sprechen", "von", GrammaticalCase.Dativ, "говорить о ком-либо / чём-либо"),
        ("antworten", "antworten", "auf", GrammaticalCase.Akkusativ, "отвечать на вопрос / сообщение"),
        ("sich ärgern", "sich ärgern", "über", GrammaticalCase.Akkusativ, "злиться из-за чего-либо"),
        ("aufpassen", "aufpassen", "auf", GrammaticalCase.Akkusativ, "присматривать за кем-либо / чем-либо"),
        ("sich aufregen", "sich aufregen", "über", GrammaticalCase.Akkusativ, "возмущаться из-за чего-либо"),
        ("ausgeben", "ausgeben", "für", GrammaticalCase.Akkusativ, "тратить деньги на что-либо"),
        ("sich bedanken", "sich bedanken", "bei", GrammaticalCase.Dativ, "благодарить кого-либо"),
        ("sich bedanken", "sich bedanken", "für", GrammaticalCase.Akkusativ, "благодарить за что-либо"),
        ("sich beklagen", "sich beklagen", "bei", GrammaticalCase.Dativ, "жаловаться кому-либо"),
        ("sich beklagen", "sich beklagen", "über", GrammaticalCase.Akkusativ, "жаловаться на кого-либо / что-либо"),
        ("sich beschweren", "sich beschweren", "bei", GrammaticalCase.Dativ, "обращаться с жалобой к кому-либо"),
        ("sich beschweren", "sich beschweren", "über", GrammaticalCase.Akkusativ, "жаловаться на причину недовольства"),
        ("sich bewerben", "sich bewerben", "um", GrammaticalCase.Akkusativ, "подавать заявление на должность"),
        ("berichten", "berichten", "über", GrammaticalCase.Akkusativ, "сообщать о событии / теме"),
        ("bitten", "bitten", "um", GrammaticalCase.Akkusativ, "просить о чём-либо"),
        ("denken", "denken", "an", GrammaticalCase.Akkusativ, "думать о ком-либо / чём-либо"),
        ("einladen", "einladen", "zu", GrammaticalCase.Dativ, "приглашать на мероприятие"),
        ("sich entscheiden", "sich entscheiden", "für", GrammaticalCase.Akkusativ, "выбирать что-либо / решаться в пользу"),
        ("sich entschuldigen", "sich entschuldigen", "für", GrammaticalCase.Akkusativ, "извиняться за что-либо"),
        ("sich erholen", "sich erholen", "von", GrammaticalCase.Dativ, "восстанавливаться после чего-либо"),
        ("sich erinnern", "sich erinnern", "an", GrammaticalCase.Akkusativ, "вспоминать о ком-либо / чём-либо"),
        ("sich erkundigen", "sich erkundigen", "bei", GrammaticalCase.Dativ, "справляться у кого-либо"),
        ("sich erkundigen", "sich erkundigen", "nach", GrammaticalCase.Dativ, "наводить справки о чём-либо"),
        ("fragen", "fragen", "nach", GrammaticalCase.Dativ, "спрашивать о ком-либо / чём-либо"),
        ("sich freuen", "sich freuen", "auf", GrammaticalCase.Akkusativ, "радоваться предстоящему событию"),
        ("sich freuen", "sich freuen", "über", GrammaticalCase.Akkusativ, "радоваться уже случившемуся / имеющемуся"),
        ("sich gewöhnen", "sich gewöhnen", "an", GrammaticalCase.Akkusativ, "привыкать к чему-либо"),
        ("glauben", "glauben", "an", GrammaticalCase.Akkusativ, "верить в кого-либо / что-либо"),
        ("gratulieren", "gratulieren", "zu", GrammaticalCase.Dativ, "поздравлять с событием"),
        ("hoffen", "hoffen", "auf", GrammaticalCase.Akkusativ, "надеяться на что-либо"),
        ("sich informieren", "sich informieren", "über", GrammaticalCase.Akkusativ, "получать информацию о чём-либо"),
        ("sich interessieren", "sich interessieren", "für", GrammaticalCase.Akkusativ, "интересоваться чем-либо"),
        ("sich kümmern", "sich kümmern", "um", GrammaticalCase.Akkusativ, "заботиться о ком-либо / чём-либо"),
        ("lachen", "lachen", "über", GrammaticalCase.Akkusativ, "смеяться над кем-либо / чем-либо"),
        ("leiden", "leiden", "an", GrammaticalCase.Dativ, "страдать заболеванием"),
        ("leiden", "leiden", "unter", GrammaticalCase.Dativ, "страдать от обстоятельств / условий"),
        ("nachdenken", "nachdenken", "über", GrammaticalCase.Akkusativ, "размышлять над вопросом"),
        ("riechen", "riechen", "nach", GrammaticalCase.Dativ, "пахнуть чем-либо"),
        ("schmecken", "schmecken", "nach", GrammaticalCase.Dativ, "иметь вкус чего-либо"),
        ("sorgen", "sorgen", "für", GrammaticalCase.Akkusativ, "заботиться об обеспечении / благополучии"),
        ("sich sorgen", "sich sorgen", "um", GrammaticalCase.Akkusativ, "беспокоиться о ком-либо / чём-либо"),
        ("teilnehmen", "teilnehmen", "an", GrammaticalCase.Dativ, "участвовать в мероприятии"),
        ("telefonieren", "telefonieren", "mit", GrammaticalCase.Dativ, "разговаривать по телефону с собеседником"),
        ("sich verabreden", "sich verabreden", "mit", GrammaticalCase.Dativ, "договариваться о встрече с кем-либо"),
        ("sich verlassen", "sich verlassen", "auf", GrammaticalCase.Akkusativ, "полагаться на кого-либо / что-либо"),
        ("sich verlieben", "sich verlieben", "in", GrammaticalCase.Akkusativ, "влюбляться в кого-либо"),
        ("sich vorbereiten", "sich vorbereiten", "auf", GrammaticalCase.Akkusativ, "готовиться к событию / испытанию"),
        ("warnen", "warnen", "vor", GrammaticalCase.Dativ, "предупреждать об опасности"),
        ("sich wundern", "sich wundern", "über", GrammaticalCase.Akkusativ, "удивляться чему-либо"),
        ("sich unterhalten", "sich unterhalten", "mit", GrammaticalCase.Dativ, "беседовать с собеседником"),
        ("sich unterhalten", "sich unterhalten", "über", GrammaticalCase.Akkusativ, "беседовать на какую-либо тему"),
        ("sich streiten", "sich streiten", "mit", GrammaticalCase.Dativ, "ссориться с кем-либо"),
        ("sich streiten", "sich streiten", "um", GrammaticalCase.Akkusativ, "спорить за обладание чем-либо"),
        ("verzichten", "verzichten", "auf", GrammaticalCase.Akkusativ, "отказываться от чего-либо"),
        ("sich anmelden", "sich anmelden", "für", GrammaticalCase.Akkusativ, "записываться на курс / мероприятие"),
        ("sich einsetzen", "sich einsetzen", "für", GrammaticalCase.Akkusativ, "выступать в поддержку дела / идеи")
    ];

    public static readonly (string Word, string Display, string Prep, GrammaticalCase Case, string Meaning)[] Expansion =
    [

    ];

    public static void AddCourseExpansion(TrainerDbContext db)
    {
        var verbs = db.GermanVerbs.OrderBy(v => v.Id).ToList();
        var existing = db.VerbPrepositions.ToList();

        foreach (var entry in Entries)
        {
            foreach (var verb in verbs.Where(v => Matches(v.Word, entry.Word)))
            {
                bool alreadyExists = existing.Any(p =>
                    p.GermanVerbId == verb.Id &&
                    p.Preposition == entry.Prep &&
                    p.Case == entry.Case &&
                    p.Meaning == entry.Meaning);

                if (alreadyExists)
                    continue;

                var item = new VerbPreposition
                {
                    GermanVerbId = verb.Id,
                    VerbDisplay = entry.Display,
                    Preposition = entry.Prep,
                    Case = entry.Case,
                    Meaning = entry.Meaning
                };

                db.VerbPrepositions.Add(item);
                existing.Add(item);
            }
        }

        db.SaveChanges();
    }

    private static bool Matches(string word, string target)
    {
        word = word.Replace("(sich)", "sich").Trim();
        target = target.Replace(" (auf)", "");
        if (word.EndsWith(" sich", StringComparison.Ordinal)) word = "sich " + word[..^5];
        if (target.StartsWith("sich ", StringComparison.Ordinal) && word == target[5..]) return true;
        return word == target || word == target + " auf" || word == target + " über" ||
            word == target + " für" || word == target + " an" || word == target + " um";
    }

    public static void AddMissing(TrainerDbContext db)
    {
        var verbs = db.GermanVerbs.OrderBy(v => v.Id).ToList();
        var existing = db.VerbPrepositions.ToList();
        foreach (var entry in Entries)
        {
            var verb = verbs.FirstOrDefault(v => v.Word == entry.Word);
            if (verb == null || existing.Any(p => p.GermanVerbId == verb.Id &&
                p.Preposition == entry.Prep && p.Case == entry.Case && p.Meaning == entry.Meaning)) continue;
            var construction = new VerbPreposition
            {
                GermanVerbId = verb.Id, VerbDisplay = entry.Display, Preposition = entry.Prep,
                Case = entry.Case, Meaning = entry.Meaning
            };
            db.VerbPrepositions.Add(construction);
            existing.Add(construction);
        }
        db.SaveChanges();
    }
}
