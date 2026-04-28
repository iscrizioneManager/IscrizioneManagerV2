using IscrizioneManager.Core.Repositories;

public class SquadraBambinoController
{
    internal static ISquadraBambinoRepository _repo = new SupabaseSquadraBambinoRepository();

    public static async Task<List<Squadra>> GetSquadreAsync()
        => await _repo.GetSquadreAsync();

    public static async Task SetSquadraAsync(int bambinoId, int newSquadraId)
        => await _repo.SetSquadraAsync(bambinoId, newSquadraId);

    public static async Task SaveAllAssignmentsAsync(List<Squadra> squadre)
        => await _repo.SaveAllAssignmentsAsync(squadre);
}
