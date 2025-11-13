using HelpTrack.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrack.Application.Services.Interfaces
{
    public interface IServicePrioridades
    {
        Task<ICollection<PrioridadDTO>> ListAsync();
        Task<PrioridadDTO> FindByIdAsync(int id);
        Task<int> AddAsync(PrioridadDTO dto);
        Task UpdateAsync(int id, PrioridadDTO dto);

    }

}
