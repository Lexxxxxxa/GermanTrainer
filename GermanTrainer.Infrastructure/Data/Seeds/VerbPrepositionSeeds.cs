using GermanTrainer.Models;

namespace GermanTrainer.Data;

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
        ("achten (auf)", "achten", "auf", GrammaticalCase.Akkusativ, "обращать внимание на кого-либо / что-либо")
    ];

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
