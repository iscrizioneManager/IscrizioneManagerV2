using IscrizioneManager.Core.Services;

namespace IscrizioneManager.Web.Services
{
  public class ModuloIscrizioneViewModel
  {

    public async Task<List<Settimana>> GetSettimaneAsync()
    {
      var settimane = await ClientHolder.Client
 .GetAll<Settimana>()
 .Select("*")
 .Get();

      var result = new List<Settimana>();
      foreach (var t in settimane.Models)
      {
        if (t.CostoIntero != null)
        {
          result.Add(new Settimana()
          {
            Id = t.Id,
            Desc = $"{t.Desc} (Intero)",
            CostoIntero = t.CostoIntero,
            CostoBase = null,
            DataInizio = t.DataInizio,
            DataFine = t.DataFine
          });
        }
        if (t.CostoBase != null)
        {
          result.Add(new Settimana()
          {
            Id = -t.Id,
            Desc = $"{t.Desc} (Base)",
            CostoIntero = null,
            CostoBase = t.CostoBase,
            DataInizio = t.DataInizio,
            DataFine = t.DataFine
          });
        }
      }

      return result;
    }

    public async Task<List<Taglia>> GetTaglieAsync()
    {
      var taglie = await ClientHolder.Client
        .GetAll<Taglia>()
        .Select("*")
        .Get();
      return taglie.Models;
    }

    public async Task<List<ConsensoDto>> GetConsensiAsync()
    {
      var tipiConsensi = await ClientHolder.Client
        .GetAll<TipoConsenso>()
        .Select("*")
        .Get();

      return tipiConsensi.Models.Select(x => new ConsensoDto() { IdTipoConsenso = x.Id, Descrizione = x.Descrizione }).ToList();
    }

    public async Task<List<GenitoreDto>> GetGenitoriListAsync()
    {
      var genitori = await ClientHolder.Client
       .GetAll<Genitore>()
       .Select("*")
       .Get();

      return genitori.Models?.Select(x => new GenitoreDto
      {
        IdGenitore = x.Id,
        Nome = x.Nome,
        Cognome = x.Cognome,
        Telefono = x.Telefono,
        Sesso = x.Sesso
      })?.ToList() ?? new List<GenitoreDto>();
    }
  }
}
