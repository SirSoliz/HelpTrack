using HelpTrack.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrack.Application.Services.Interfaces
{
    public interface IServiceEstadoTicket
    {
        Task<ICollection<EstadoTicketDTO>> ListAsync();
        Task<EstadoTicketDTO> FindByIdAsync(short id);
        Task<int> AddAsync(EstadoTicketDTO dto);
        Task UpdateAsync(short id, EstadoTicketDTO dto);
        Task<ICollection<EstadoTicketDTO>> SearchAsync(string searchTerm);
    }
}
