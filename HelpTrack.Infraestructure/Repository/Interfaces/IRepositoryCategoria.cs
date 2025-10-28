using HelpTrack.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrack.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryCategoria
    {
        Task<ICollection<Categorias>> ListAsync();
        Task<Categorias?> FindByIdAsync(int id);
        Task<int> AddAsync(Categorias entity);
        Task UpdateAsync(Categorias entity);
    }
}
