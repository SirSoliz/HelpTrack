using HelpTrack.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrack.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryEstadoTicket
    {
        Task<EstadosTicket?> FindByIdAsync(short id);
        Task<ICollection<EstadosTicket>> ListAsync();
        Task<int> AddAsync(EstadosTicket entity);
        Task UpdateAsync(EstadosTicket entity);
        Task DeleteAsync(EstadosTicket entity);
    }
}
