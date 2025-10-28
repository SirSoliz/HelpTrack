using HelpTrack.Infraestructure.Models;

public interface IRepositoryTecnico
{
    Task<Tecnicos?> FindByIdAsync(int id);  // Método requerido
    Task<ICollection<Tecnicos>> ListAsync();
    Task<int> AddAsync(Tecnicos entity);
    Task UpdateAsync(Tecnicos entity);
    Task<ICollection<Tecnicos>> SearchAsync(string searchTerm);
}