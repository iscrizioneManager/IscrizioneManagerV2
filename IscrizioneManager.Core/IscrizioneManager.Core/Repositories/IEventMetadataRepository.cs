using IscrizioneManager.Core.Models;

namespace IscrizioneManager.Core.Repositories;

public interface IEventMetadataRepository
{
    Task<EventoMetadata> GetEventMetadataAsync();
    Task<List<AnnoScolastico>> GetAnniScolasticiAsync(int[] gradiAmmessi);
}
