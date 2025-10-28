using HelpTrack.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HelpTrack.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryTecnico
    {
        Task<ICollection<Tecnicos>> ListAsync();
        Task<Tecnicos?> FindByIdAsync(int id);
        Task<int> AddAsync(Tecnicos entity);
        Task UpdateAsync(Tecnicos entity);
        Task<ICollection<Tecnicos>> SearchAsync(string searchTerm);
    }
}
