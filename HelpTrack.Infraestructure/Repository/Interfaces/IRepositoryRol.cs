using HelpTrack.Infraestructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpTrack.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryRol
    {
        Task<ICollection<Roles>> ListAsync();
        Task<Roles> FindByIdAsync(int id);
        Task<int> AddAsync(Roles entity);
        Task UpdateAsync(Roles entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
