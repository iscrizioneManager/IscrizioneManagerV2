using IscrizioneManager.Core.Services;
using IscrizioniManager.Models;

namespace IscrizioneManager.Core.Repositories;

public class SupabaseSquadraBambinoRepository : ISquadraBambinoRepository
{
    public async Task<List<Squadra>> GetSquadreAsync()
    {
        var bambini = await ClientHolder.Client.GetAll<Bambino>().Select("*").Get();
        var iscrizioni = await ClientHolder.Client.GetAll<Iscrizione>().Select("*").Get();
        var squadraBambino = await ClientHolder.Client.GetAll<SquadraBambino>().Select("*").Get();
        var squadra = await ClientHolder.Client.GetAll<Squadra>().Select("*").Get();
        var anniScolastici = (await ClientHolder.Client.BaseFrom<IscrizioneManager.Core.Models.AnnoScolastico>().Select("*").Get()).Models;

        Bambino MakeBambino(Bambino x)
        {
            var anno = (int?)iscrizioni.Models.SingleOrDefault(y => y.IdBambino == x.Id)?.Anno;
            return new Bambino
            {
                Id = x.Id,
                Nome = x.Nome,
                Cognome = x.Cognome,
                Anno = anno,
                AnnoDesc = anniScolastici.FirstOrDefault(a => a.Id == anno)?.Desc ?? ""
            };
        }

        var squadre = new List<Squadra>();
        foreach (var s in squadra.Models)
        {
            s.Bambini = squadraBambino.Models
                .Where(sb => sb.IdSquadra == s.Id)
                .Join(bambini.Models, sb => sb.IdBambino, b => b.Id, (sb, b) => b)
                .Select(MakeBambino)
                .OrderBy(x => x.Anno)
                .ThenBy(x => x.Cognome)
                .ThenBy(x => x.Nome)
                .ToList();
            squadre.Add(s);
        }

        squadre.Add(new Squadra
        {
            Nome = "Senza squadra",
            Color = "#bbb",
            Bambini = bambini.Models
                .Where(x => !squadraBambino.Models.Select(y => y.IdBambino).Contains(x.Id))
                .Select(MakeBambino)
                .OrderBy(x => x.Anno)
                .ThenBy(x => x.Cognome)
                .ThenBy(x => x.Nome)
                .ToList()
        });

        return squadre;
    }

    public async Task SetSquadraAsync(int bambinoId, int newSquadraId)
    {
        await ClientHolder.Client
            .GetAll<SquadraBambino>()
            .Where(x => x.IdBambino == bambinoId)
            .Delete();

        if (newSquadraId != 0)
            await ClientHolder.Client
                .GetAll<SquadraBambino>()
                .Insert(new SquadraBambino
                {
                    IdSquadra = newSquadraId,
                    IdBambino = bambinoId,
                    event_id = ClientHolder.Client._eventId
                });
    }

    public async Task SaveAllAssignmentsAsync(List<Squadra> squadre)
    {
        await ClientHolder.Client.GetAll<SquadraBambino>().Delete();

        foreach (var s in squadre.Where(s => s.Id != 0))
            foreach (var b in s.Bambini)
                await ClientHolder.Client
                    .GetAll<SquadraBambino>()
                    .Insert(new SquadraBambino
                    {
                        IdSquadra = s.Id,
                        IdBambino = b.Id,
                        event_id = ClientHolder.Client._eventId
                    });
    }
}
