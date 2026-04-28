using IscrizioneManager.Core.Models;
using IscrizioneManager.Core.Repositories;

public class IscrizioneCompletaController
{
    public IscrizioneCompletaController() { }

    internal static IIscrizioneCompletaRepository _repo = new SupabaseIscrizioneCompletaRepository();
    internal static IEventMetadataRepository _metaRepo = new SupabaseEventMetadataRepository();

    public static async Task<EventoMetadata> GetEventMetadataAsync()
        => await _metaRepo.GetEventMetadataAsync();

    public static async Task<List<AnnoScolastico>> GetAnniScolasticiAsync(int[] gradiAmmessi)
        => await _metaRepo.GetAnniScolasticiAsync(gradiAmmessi);

    public static async Task<bool> CreateAsync(ModuloIscrizioneDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        return await _repo.CreateAsync(dto);
    }

    public static async Task<bool> UpdateAsync(ModuloIscrizioneDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        return await _repo.UpdateAsync(dto);
    }

    public static async Task<bool> DeleteAsync(int idBambino)
    {
        if (idBambino <= 0) throw new ArgumentException("Id non valido", nameof(idBambino));
        return await _repo.DeleteAsync(idBambino);
    }

    public static async Task<ModuloIscrizioneDto> GetAsync(int idBambino)
        => await _repo.GetAsync(idBambino);
}
