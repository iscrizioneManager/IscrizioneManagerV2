using IscrizioneManager.Core.Services;
using IscrizioniManager.Models;

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
      await ClientHolder.Client.GetAll<Iscrizione>()
          .Where(x => x.Id == item.IdIscrizione)
          .Update(new Iscrizione
          {
            Id = item.IdIscrizione.Value,
            Anno = (AnnoScolastico?)item.Anno,
            IdBambino = item.IdBambino,
            Note = item.Note,
            Pagato = item.Pagato,
            event_id = item.event_id
          });
    }

    public static async Task<List<VIscrizioneCompleta>> LoadIscrittiAsync()
    {
      var anniScolastici = Enum.GetValues(typeof(AnnoScolastico))
          .Cast<AnnoScolastico>()
          .Select(e => new AnnoScolasticoItem((int)e)).ToList();
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
           event_id = x.Key.event_id
         })
         .ToList();

      // assegna descrizione anno e formato data
      iscritti.ForEach(x =>
      {
        x.AnnoDesc = anniScolastici.FirstOrDefault(a => a.Value != null && a.Value == x.Anno)?.Description ?? "";
        x.DataNascitaDesc = x.DataNascita.ToString("dd/MM/yyyy");
      });

      return iscritti;
    }
  }
}
