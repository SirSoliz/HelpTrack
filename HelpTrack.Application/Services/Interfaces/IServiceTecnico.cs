using HelpTrack.Application.DTOs;
using HelpTrack.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HelpTrack.Application.Services.Interfaces
{
    public interface IServiceTecnico
    {
        Task<ICollection<TecnicoDTO>> ListAsync();
        Task<TecnicoDTO> FindByIdAsync(int id);
        Task<int> AddAsync(TecnicoDTO dto);
        Task UpdateAsync(int id, TecnicoDTO  dto);
    }
}
