using HelpTrack.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrack.Application.Services.Interfaces
{
    public interface IServiceTicket
    {
        Task<ICollection<TicketDTO>> ListAsync();
        Task<TicketDTO> FindByIdAsync(int id);
        Task<int> AddAsync(TicketDTO dto);
        Task UpdateAsync(int id, TicketDTO dto);

    }
}
