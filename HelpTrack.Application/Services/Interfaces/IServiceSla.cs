using HelpTrack.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrack.Application.Services.Interfaces
{
    public interface IServiceSla
    {
        Task<ICollection<SlaDTO>> ListAsync();
        Task<SlaDTO> FindByIdAsync(int id);
        Task<int> AddAsync(SlaDTO dto);
        Task UpdateAsync(int id, SlaDTO dto);

    }
}
