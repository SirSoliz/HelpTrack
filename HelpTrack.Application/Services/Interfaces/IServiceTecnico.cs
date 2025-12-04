using HelpTrack.Application.DTOs;
using HelpTrack.Infraestructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpTrack.Application.Services.Interfaces
{
    public interface IServiceTecnico
    {
        Task<ICollection<TecnicoDTO>> ListAsync();
        Task<TecnicoDTO> FindByIdAsync(int id);
        Task<int> AddAsync(TecnicoDTO dto);
        Task UpdateAsync(int id, TecnicoDTO dto, string[] selectedCategorias);
        Task<ICollection<TecnicoDTO>> SearchAsync(string searchTerm);

        Task<bool> ExisteEmailAsync(string email);
        Task<bool> DeleteAsync(int id);
        Task<Tecnicos?> GetTechnicianWithLeastWorkloadAsync();
        Task RecalculateAllWorkloadsAsync();
    }
}