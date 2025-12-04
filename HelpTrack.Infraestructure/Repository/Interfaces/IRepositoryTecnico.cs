using HelpTrack.Infraestructure.Models;

public interface IRepositoryTecnico
{
    Task<Tecnicos?> FindByIdAsync(int id);  // Método requerido
    Task<ICollection<Tecnicos>> ListAsync();
    Task<ICollection<Tecnicos>> ListWithAssignmentsAsync();
    Task<int> AddAsync(Tecnicos entity);
    Task UpdateAsync(Tecnicos entity, string[] selectedCategorias);
    Task<ICollection<Tecnicos>> SearchAsync(string searchTerm);
    Task<bool> DeleteAsync(int id);
    Task IncrementWorkloadAsync(int technicianId);
    Task DecrementWorkloadAsync(int technicianId);
    Task RecalculateAllWorkloadsAsync();
}