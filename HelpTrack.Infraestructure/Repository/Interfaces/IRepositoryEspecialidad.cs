// HelpTrack.Infraestructure/Repository/Interfaces/IRepositoryEspecialidad.cs
using HelpTrack.Infraestructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpTrack.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryEspecialidad
    {
        Task<ICollection<Especialidades>> FindByIdsAsync(IEnumerable<int> ids);

        Task<Especialidades?> FindByIdAsync(int id);
        Task<ICollection<Especialidades>> ListAsync();
        Task<int> AddAsync(Especialidades entity);
        Task UpdateAsync(Especialidades entity);
        Task DeleteAsync(Especialidades entity);
    }
}