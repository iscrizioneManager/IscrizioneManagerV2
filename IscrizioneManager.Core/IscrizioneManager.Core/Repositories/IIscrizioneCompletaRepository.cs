namespace IscrizioneManager.Core.Repositories;

public interface IIscrizioneCompletaRepository
{
    Task<bool> CreateAsync(ModuloIscrizioneDto dto);
    Task<bool> UpdateAsync(ModuloIscrizioneDto dto);
    Task<bool> DeleteAsync(int idBambino);
    Task<ModuloIscrizioneDto> GetAsync(int idBambino);
}
