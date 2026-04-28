namespace IscrizioneManager.Core.Repositories;

public interface ISquadraBambinoRepository
{
    Task<List<Squadra>> GetSquadreAsync();
    Task SetSquadraAsync(int bambinoId, int newSquadraId);
    Task SaveAllAssignmentsAsync(List<Squadra> squadre);
}
