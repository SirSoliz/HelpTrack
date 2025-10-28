using HelpTrack.Infraestructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpTrack.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryUsuario
    {
        Task<Usuarios?> FindByIdAsync(int id);
        Task<ICollection<Usuarios>> ListAsync();
        Task<int> AddAsync(Usuarios entity);
        Task UpdateAsync(Usuarios entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<Usuarios?> FindByEmailAsync(string email);
    }
}
