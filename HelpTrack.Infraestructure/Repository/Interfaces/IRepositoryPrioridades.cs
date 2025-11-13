using HelpTrack.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrack.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryPrioridades
    {
        Task<ICollection<Prioridades>> ListAsync();
        Task<Prioridades> FindByIdAsync(int id);
        Task<int> AddAsync(Prioridades entity);
        Task UpdateAsync(Prioridades entity);
    }
}
