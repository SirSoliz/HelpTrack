// HelpTrack.Application/Services/Interfaces/IServiceEtiqueta.cs
using HelpTrack.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpTrack.Application.Services.Interfaces
{
    public interface IServiceEtiqueta
    {
        Task<ICollection<EtiquetaDTO>> ListAsync();
        Task<EtiquetaDTO> FindByIdAsync(short id);
        Task<int> AddAsync(EtiquetaDTO dto);
        Task UpdateAsync(short id, EtiquetaDTO dto);
        Task<ICollection<EtiquetaDTO>> SearchAsync(string searchTerm);
    }
}
