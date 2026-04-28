using IscrizioneManager.Core.Services;

namespace IscrizioneManager.Core.Repositories;

public class SupabaseIscrittiListaRepository : IIscrittiListaRepository
{
    public async Task<List<VIscrizioneCompleta>> GetListAsync()
    {
        return (await ClientHolder.Client
            .GetAll<VIscrizioneCompleta>()
            .Select("*")
            .Get()).Models;
    }

    public async Task AggiornaPagatoAsync(VIscrizioneCompleta item)
    {
        await ClientHolder.Client
            .GetAll<Iscrizione>()
            .Where(x => x.Id == item.IdIscrizione)
            .Set(x => x.Pagato, item.Pagato)
            .Update();
    }
}
