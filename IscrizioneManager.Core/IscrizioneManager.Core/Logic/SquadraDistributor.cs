using IscrizioniManager.Models;

namespace IscrizioneManager.Core.Logic;

public static class SquadraDistributor
{
    public static List<Squadra> Distribuisci(
        List<Squadra> squadre,
        Func<IEnumerable<Bambino>, IEnumerable<Bambino>>? shuffler = null)
    {
        shuffler ??= b => b.OrderBy(_ => Guid.NewGuid());

        var tutti = squadre.SelectMany(s => s.Bambini).ToList();
        var targetSquadre = squadre.Where(s => s.Id != 0).ToList();

        foreach (var s in targetSquadre)
            s.Bambini.Clear();

        var gruppiPerAnno = tutti.GroupBy(b => b.Anno).OrderBy(g => g.Key);
        int index = 0;

        foreach (var gruppo in gruppiPerAnno)
        {
            foreach (var bambino in shuffler(gruppo))
            {
                targetSquadre[index % targetSquadre.Count].Bambini.Add(bambino);
                index++;
            }
        }

        foreach (var s in targetSquadre)
        {
            s.Bambini = s.Bambini
                .OrderByDescending(x => x.Anno)
                .ThenBy(x => $"{x.Cognome} {x.Nome}")
                .ToList();
        }

        var senzaSquadra = squadre.SingleOrDefault(s => s.Id == 0);
        if (senzaSquadra != null)
            senzaSquadra.Bambini.Clear();

        return squadre;
    }
}
