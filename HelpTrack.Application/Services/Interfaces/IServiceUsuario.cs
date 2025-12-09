using HelpTrack.Application.DTOs;
using HelpTrack.Infraestructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpTrack.Application.Services.Interfaces
{
    public interface IServiceUsuario
    {
        Task<UsuarioDTO> FindByIdAsync(int id);
        Task<ICollection<UsuarioDTO>> ListAsync();
        Task<int> AddAsync(UsuarioDTO dto);
        Task UpdateAsync(int id, UsuarioDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> UpdateLastLoginAsync(int id);
        Task<Usuarios?> FindByEmailAsync(string email);
        Task<UsuarioDTO?> LoginAsync(string email, string password);
        Task<UsuarioDTO> RegisterAsync(UsuarioDTO user, string password);
        Task<bool> UpdatePasswordAsync(string email, string newPassword);
        Task<ICollection<Roles>> GetUsuarioRolesAsync(int usuarioId);
        Task<bool> AssignRoleToUsuarioAsync(int usuarioId, int rolId);
        Task<bool> RemoveRoleFromUsuarioAsync(int usuarioId, int rolId);
    }
}
