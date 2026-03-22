using IscrizioneManager.Core.Services;
using Newtonsoft.Json;

namespace IscrizioniManager.Controllers
{
  public class IscrittiListaController
  {
    public IscrittiListaController()
    {
    }

    public async Task<List<VIscrizioneCompleta>> GetList()
    {
      return (await ClientHolder.Client
            .GetAll<VIscrizioneCompleta>()
            .Select("*")
            .Get()).Models;
    }

    public static async Task AggiornaPagatoAsync(VIscrizioneCompleta item)
    {
      await ClientHolder.Client
        .GetAll<Iscrizione>()
        .Where(x => x.Id == item.IdIscrizione)
        .Set(x => x.Pagato, item.Pagato)
        .Update();
    }

    public static async Task<List<VIscrizioneCompleta>> LoadIscrittiAsync()
    {
      var eventMetadata = await IscrizioneCompletaController.GetEventMetadataAsync();
      var anniScolastici = await IscrizioneCompletaController.GetAnniScolasticiAsync(JsonConvert.DeserializeObject<int[]>(eventMetadata.GradiScuolaAllowed));
      var iscritti = (await new IscrittiListaController().GetList())
         .GroupBy(x => new { x.IdIscrizione, x.BCognome, x.BNome, x.DataNascita, x.Anno, x.IdBambino, x.Note, x.Pagato, x.event_id })
         .Select(x => new VIscrizioneCompleta()
         {
           BNome = x.Key.BNome,
           BCognome = x.Key.BCognome,
           DataNascita = x.Key.DataNascita,
           Anno = x.Key.Anno,
           IdBambino = x.Key.IdBambino,
           IdIscrizione = x.Key.IdIscrizione,
           Note = x.Key.Note,
           Pagato = x.Key.Pagato,
           event_id = x.Key.event_id,
         })
         .ToList();

      // assegna descrizione anno e formato data
      iscritti.ForEach(x =>
      {
        x.AnnoDesc = anniScolastici.FirstOrDefault(a => a.Id != null && a.Id == x.Anno)?.Desc ?? "";
        x.DataNascitaDesc = x.DataNascita.ToString("dd/MM/yyyy");
      });

      return iscritti;
    }
  }
}
