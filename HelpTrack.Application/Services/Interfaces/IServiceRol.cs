using HelpTrack.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpTrack.Application.Services.Interfaces
{
    public interface IServiceRol
    {
        Task<ICollection<RolDTO>> ListAsync();
        Task<RolDTO> FindByIdAsync(int id);
        Task<int> AddAsync(RolDTO dto);
        Task UpdateAsync(int id, RolDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
