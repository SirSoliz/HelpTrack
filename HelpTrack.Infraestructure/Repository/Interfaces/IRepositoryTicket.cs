using HelpTrack.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrack.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryTicket
    {
        Task<ICollection<Tickets>> ListAsync();
        Task<ICollection<Tickets>> GetHistoryListAsync();
        Task<ICollection<HistorialTicket>> GetHistoryLogAsync(int ticketId);
        Task<Tickets> FindByIdAsync(int id);
        Task<int> AddAsync(Tickets entity);
        Task UpdateAsync(Tickets entity);
        Task AddAssignmentAsync(AsignacionesTicket assignment);
        Task<AsignacionesTicket?> GetAssignmentByTicketIdAsync(int ticketId);
        Task UpdateAssignmentAsync(AsignacionesTicket assignment);
    }
}
