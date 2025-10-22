using HelpTrack.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrack.Application.Services.Interfaces
{
    public interface IServiceCategoria
    {
        Task<ICollection<CategoriaDTO>> ListAsync();
        Task<CategoriaDTO> FindByIdAsync(int id);
        Task<int> AddAsync(CategoriaDTO dto);
        Task UpdateAsync(int id, CategoriaDTO dto);
    }
}
