// HelpTrack.Infraestructure/Repository/Interfaces/IRepositoryEtiqueta.cs
using HelpTrack.Infraestructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpTrack.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryEtiqueta
    {
        Task<Etiquetas?> FindByIdAsync(short id);
        Task<ICollection<Etiquetas>> ListAsync();
        Task<int> AddAsync(Etiquetas entity);
        Task UpdateAsync(Etiquetas entity);
        Task DeleteAsync(Etiquetas entity);
    }
}
