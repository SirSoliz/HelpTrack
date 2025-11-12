using AutoMapper;
using HelpTrack.Application.DTOs;
using HelpTrack.Application.Services.Interfaces;
using HelpTrack.Infraestructure.Models;
using HelpTrack.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpTrack.Infraestructure.Data;

namespace HelpTrack.Application.Services.Implementations
{
    public class ServiceTecnico : IServiceTecnico
    {
        private readonly IRepositoryTecnico _repositoryTecnico;
        private readonly IMapper _mapper;
        private readonly HelpTrackContext _context;

        public ServiceTecnico(IRepositoryTecnico repositoryTecnico, IMapper mapper, HelpTrackContext context)
        {
            _repositoryTecnico = repositoryTecnico;
            _mapper = mapper;
            _context = context;
        }

        public async Task<TecnicoDTO> FindByIdAsync(int id)
        {
            var tecnico = await _repositoryTecnico.FindByIdAsync(id);
            if (tecnico == null)
            {
                throw new KeyNotFoundException($"No se encontró el técnico con ID {id}");
            }
            return _mapper.Map<TecnicoDTO>(tecnico);
        }

        public async Task<ICollection<TecnicoDTO>> ListAsync()
        {
            var tecnicos = await _repositoryTecnico.ListAsync();
            return _mapper.Map<ICollection<TecnicoDTO>>(tecnicos);
        }

        public async Task<int> AddAsync(TecnicoDTO dto)
        {
            try
            {
                // Validar que el email no sea nulo o vacío
                if (string.IsNullOrWhiteSpace(dto.Usuario?.Email))
                {
                    throw new ArgumentException("El correo electrónico es requerido");
                }

                // Validar que el nombre no sea nulo o vacío
                if (string.IsNullOrWhiteSpace(dto.Usuario?.Nombre))
                {
                    throw new ArgumentException("El nombre es requerido");
                }

                // Primero creamos el usuario
                var usuario = new Usuarios
                {
                    Nombre = dto.Usuario.Nombre.Trim(),
                    Email = dto.Usuario.Email.Trim().ToLower(), // Aseguramos formato consistente
                    Activo = true,
                    FechaCreacion = DateTime.Now,
                    UltimoInicioSesion = null,
                    Contrasena = HashPassword("ContraseñaTemporal123!") // Contraseña temporal
                };

                // Luego creamos el técnico
                var tecnico = new Tecnicos
                {
                    Alias = dto.Alias?.Trim(), // Aseguramos que no haya espacios en blanco
                    Disponible = dto.Disponible,
                    NivelCarga = dto.NivelCarga,
                    IdTecnicoNavigation = usuario
                };

                // Guardamos el técnico (lo que también guardará el usuario debido a la relación)
                return await _repositoryTecnico.AddAsync(tecnico);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error en ServiceTecnico.AddAsync: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                throw; // Relanzar la excepción para manejarla en el controlador
            }
        }

        // Método para hashear la contraseña (deberías usar un servicio de autenticación adecuado)
        private byte[] HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task UpdateAsync(int id, TecnicoDTO dto, string[] selectedCategorias)
        {
            var tecnico = await _repositoryTecnico.FindByIdAsync(id);
            if (tecnico == null)
            {
                throw new KeyNotFoundException($"No se encontró el técnico con ID {id}");
            }

            // Actualizar propiedades básicas
            tecnico.Alias = dto.Alias;
            tecnico.Disponible = dto.Disponible;
            tecnico.NivelCarga = dto.NivelCarga;

            // Actualizar datos del usuario 
            if (tecnico.IdTecnicoNavigation != null && dto.Usuario != null)
            {
                tecnico.IdTecnicoNavigation.Nombre = dto.Usuario.Nombre;
                tecnico.IdTecnicoNavigation.Email = dto.Usuario.Email;
            }

            await _repositoryTecnico.UpdateAsync(tecnico, selectedCategorias);
        }

        public async Task<ICollection<TecnicoDTO>> SearchAsync(string searchTerm)
        {
            var tecnicos = await _repositoryTecnico.SearchAsync(searchTerm);
            return _mapper.Map<ICollection<TecnicoDTO>>(tecnicos);
        }

        public async Task<bool> ExisteEmailAsync(string email)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

    }
}