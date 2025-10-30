using HelpTrack.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrack.Infraestructure.Repository.Interfaces
{
    public interface IRepositorySla
    {
        Task<ICollection<Sla>> ListAsync();
        Task<Sla> FindByIdAsync(int id);
        Task<int> AddAsync(Sla entity);
        Task UpdateAsync(Sla entity);
    }
}
