using HelpTrack.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpTrack.Application.Services.Interfaces
{
    public interface IServiceTecnico
    {
        Task<ICollection<TecnicoDTO>> ListAsync();
        Task<TecnicoDTO> FindByIdAsync(int id);
        Task<int> AddAsync(TecnicoDTO dto);
        Task UpdateAsync(int id, TecnicoDTO dto);
        Task<ICollection<TecnicoDTO>> SearchAsync(string searchTerm);
    }
}