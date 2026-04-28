namespace IscrizioneManager.Core.Repositories;

public interface IIscrittiListaRepository
{
    Task<List<VIscrizioneCompleta>> GetListAsync();
    Task AggiornaPagatoAsync(VIscrizioneCompleta item);
}
