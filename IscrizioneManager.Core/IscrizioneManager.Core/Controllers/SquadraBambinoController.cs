using IscrizioneManager.Core.Services;
using IscrizioniManager;
using IscrizioniManager.Data;
using IscrizioniManager.Dtos;
using IscrizioniManager.Models;

public class SquadraBambinoController
{
  public static async Task<List<Squadra>> GetSquadreAsync()
  {
    var bambini = await ClientHolder.Client
      .GetAll<Bambino>()
      .Select("*")
      .Get();
    var iscrizioni = await ClientHolder.Client
      .GetAll<Iscrizione>()
      .Select("*")
      .Get();
    var squadraBambino = await ClientHolder.Client
      .GetAll<SquadraBambino>()
      .Select("*")
      .Get();
    var squadra = await ClientHolder.Client
      .GetAll<Squadra>()
      .Select("*")
      .Get();

    List<Squadra> squadre = new List<Squadra>();
    foreach (var s in squadra.Models)
    {
      s.Bambini = squadraBambino.Models
        .Where(sb => sb.IdSquadra == s.Id)
        .Join(bambini.Models, sb => sb.IdBambino, b => b.Id, (sb, b) => b)
        .Select(x => new Bambino(){ Id = x.Id, Nome = x.Nome,Cognome = x.Cognome, Anno = (int?)iscrizioni.Models.SingleOrDefault(y => y.IdBambino == x.Id)?.Anno })
        .ToList();
      squadre.Add(s);
    }

    squadre.Add(new Squadra()
    {
      Nome = "Senza squadra", 
      Color = "#bbb",
      Bambini = bambini.Models.Where(x => !squadraBambino.Models.Select(y => y.IdBambino).Contains(x.Id))
        .Select(x => new Bambino() { Id = x.Id, Nome = x.Nome, Cognome = x.Cognome, Anno = (int?)iscrizioni.Models.SingleOrDefault(y => y.IdBambino == x.Id)?.Anno })
        .ToList()
    });

    return squadre;
  }
}
