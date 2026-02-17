using IscrizioneManager.Core.Services;
using IscrizioniManager;
using IscrizioniManager.Data;
using IscrizioniManager.Dtos;

public class SquadraBambinoController
{
  public SquadraBambinoController()
  {
  }

  public async Task<SquadraBambino?> AddChildToTeamAsync(BambinoSquadraDto dto)
  {
    // Controlla se l'associazione esiste già
    var existingResp = await ClientHolder.Client
        .GetAll<SquadraBambino>()
        .Select("*")
        .Where(x => x.IdBambino == dto.IdBambino && x.IdSquadra == dto.IdSquadra)
        .Single();

    if (existingResp != null)
      throw new Exception("Il bambino è già associato a questa squadra.");

    // Inserisci nuova associazione
    var relazione = new SquadraBambino
    {
      IdBambino = dto.IdBambino,
      IdSquadra = dto.IdSquadra
    };
    var resp = await ClientHolder.Client.GetAll<SquadraBambino>().Insert(relazione);

    return resp.Models.FirstOrDefault();
  }
}
