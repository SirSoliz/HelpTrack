using AutoMapper;
using HelpTrack.Application.DTOs;
using HelpTrack.Application.Services.Interfaces;
using HelpTrack.Infraestructure.Models;
using HelpTrack.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpTrack.Application.Services.Implementations
{
    public class ServiceUsuario : IServiceUsuario
    {
        private readonly IRepositoryUsuario _repositoryUsuario;
        private readonly IMapper _mapper;

        public ServiceUsuario(IRepositoryUsuario repositoryUsuario, IMapper mapper)
        {
            _repositoryUsuario = repositoryUsuario ?? throw new ArgumentNullException(nameof(repositoryUsuario));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UsuarioDTO> FindByIdAsync(int id)
        {
            var usuario = await _repositoryUsuario.FindByIdAsync(id);
            return _mapper.Map<UsuarioDTO>(usuario);
        }

        public async Task<ICollection<UsuarioDTO>> ListAsync()
        {
            var usuarios = await _repositoryUsuario.ListAsync();
            return _mapper.Map<ICollection<UsuarioDTO>>(usuarios);
        }

        public async Task<int> AddAsync(UsuarioDTO dto)
        {
            var usuario = _mapper.Map<Usuarios>(dto);
            return await _repositoryUsuario.AddAsync(usuario);
        }

        public async Task UpdateAsync(int id, UsuarioDTO dto)
        {
            var usuario = await _repositoryUsuario.FindByIdAsync(id);
            if (usuario == null)
                throw new KeyNotFoundException("Usuario no encontrado");

            _mapper.Map(dto, usuario);
            await _repositoryUsuario.UpdateAsync(usuario);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repositoryUsuario.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _repositoryUsuario.ExistsAsync(id);
        }

        public async Task<Usuarios?> FindByEmailAsync(string email)
        {
            return await _repositoryUsuario.FindByEmailAsync(email);
        }

        public async Task<bool> UpdateLastLoginAsync(int id)
        {
            var usuario = await _repositoryUsuario.FindByIdAsync(id);
            if (usuario == null)
                return false;

            usuario.UltimoInicioSesion = DateTime.Now;
            await _repositoryUsuario.UpdateAsync(usuario);
            return true;
        }

        public async Task<UsuarioDTO?> LoginAsync(string email, string password)
        {
            var usuario = await _repositoryUsuario.FindByEmailAsync(email);
            if (usuario == null) return null;

            if (VerifyPassword(password, usuario.Contrasena))
            {
                return _mapper.Map<UsuarioDTO>(usuario);
            }
            return null;
        }

        public async Task<UsuarioDTO> RegisterAsync(UsuarioDTO userDto, string password)
        {
            var usuario = _mapper.Map<Usuarios>(userDto);
            usuario.Contrasena = HashPassword(password);
            usuario.FechaCreacion = DateTime.Now;
            usuario.Activo = true;

            await _repositoryUsuario.AddAsync(usuario);
            return _mapper.Map<UsuarioDTO>(usuario);
        }

        private byte[] HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                return sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPassword(string password, byte[]? storedHash)
        {
            if (storedHash == null || storedHash.Length == 0) return false;
            var hash = HashPassword(password);
            return hash.SequenceEqual(storedHash);
        }

        public async Task<bool> UpdatePasswordAsync(string email, string newPassword)
        {
            var usuario = await _repositoryUsuario.FindByEmailAsync(email);
            if (usuario == null) return false;

            usuario.Contrasena = HashPassword(newPassword);
            await _repositoryUsuario.UpdateAsync(usuario);
            return true;
        }
    }
}
