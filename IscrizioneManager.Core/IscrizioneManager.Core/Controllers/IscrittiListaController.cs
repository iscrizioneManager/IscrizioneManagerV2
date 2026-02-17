using IscrizioneManager.Core.Services;

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
  }
}
