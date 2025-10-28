using HelpTrack.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpTrack.Application.Services.Interfaces
{
    public interface IServiceEspecialidad
    {
        Task<ICollection<EspecialidadDTO>> ListAsync();
        Task<EspecialidadDTO> FindByIdAsync(short id);
        Task<int> AddAsync(EspecialidadDTO dto);
        Task UpdateAsync(short id, EspecialidadDTO dto);
        Task<ICollection<EspecialidadDTO>> SearchAsync(string searchTerm);
    }
}