using AutoMapper;
using HelpTrack.Application.DTOs;
using HelpTrack.Application.Services.Interfaces;
using HelpTrack.Infraestructure.Models;
using HelpTrack.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpTrack.Application.Services.Implementations
{
    public class ServiceEspecialidad : IServiceEspecialidad
    {
        private readonly IRepositoryEspecialidad _repository;
        private readonly IMapper _mapper;

        public ServiceEspecialidad(IRepositoryEspecialidad repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<EspecialidadDTO>> ListAsync()
        {
            var especialidades = await _repository.ListAsync();
            return _mapper.Map<ICollection<EspecialidadDTO>>(especialidades);
        }

        public async Task<EspecialidadDTO> FindByIdAsync(short id)
        {
            var especialidad = await _repository.FindByIdAsync(id);
            if (especialidad == null)
            {
                throw new KeyNotFoundException($"No se encontró la especialidad con ID {id}");
            }
            return _mapper.Map<EspecialidadDTO>(especialidad);
        }

        public async Task<int> AddAsync(EspecialidadDTO dto)
        {
            var entity = _mapper.Map<Especialidades>(dto);
            return await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(short id, EspecialidadDTO dto)
        {
            var entity = await _repository.FindByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"No se encontró la especialidad con ID {id}");
            }

            _mapper.Map(dto, entity);
            await _repository.UpdateAsync(entity);
        }

        public async Task<ICollection<EspecialidadDTO>> SearchAsync(string searchTerm)
        {
            var especialidades = await _repository.ListAsync();
            return _mapper.Map<ICollection<EspecialidadDTO>>(especialidades)
                .Where(e =>
                    e.Nombre != null && e.Nombre.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (e.Descripcion != null && e.Descripcion.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                )
                .ToList();
        }
    }
}