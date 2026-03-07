using IscrizioneManager.Core.Services;
using IscrizioniManager;
using IscrizioniManager.Data;
using IscrizioniManager.Dtos;
using IscrizioniManager.Models;

public class SquadraBambinoController
{
  public SquadraBambinoController()
  {
  }

  public static async Task<List<Squadra>> GetSquadreAsync()
  {
    var bambini = await ClientHolder.Client
      .GetAll<Bambino>()
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

    foreach (var s in squadra.Models)
    {
      s.Bambini = squadraBambino.Models
        .Where(sb => sb.IdSquadra == s.Id)
        .Join(bambini.Models, sb => sb.IdBambino, b => b.Id, (sb, b) => b)
        .ToList();
    }

    squadra.Models.Add(new Squadra()
    {
      Nome = "Senza squadra", 
      Color = "#bbb",
      Bambini = bambini.Models.Where(x => !squadraBambino.Models.Select(y => y.IdBambino).Contains(x.Id)).ToList()
    });

    return squadra.Models;
  }
}
