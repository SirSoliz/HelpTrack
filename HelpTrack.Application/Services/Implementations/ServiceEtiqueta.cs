// HelpTrack.Application/Services/Implementations/ServiceEtiqueta.cs
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
    public class ServiceEtiqueta : IServiceEtiqueta
    {
        private readonly IRepositoryEtiqueta _repository;
        private readonly IMapper _mapper;

        public ServiceEtiqueta(IRepositoryEtiqueta repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<EtiquetaDTO>> ListAsync()
        {
            var etiquetas = await _repository.ListAsync();
            return _mapper.Map<ICollection<EtiquetaDTO>>(etiquetas);
        }

        public async Task<EtiquetaDTO> FindByIdAsync(short id)
        {
            var etiqueta = await _repository.FindByIdAsync(id);
            if (etiqueta == null)
            {
                throw new KeyNotFoundException($"No se encontró la etiqueta con ID {id}");
            }
            return _mapper.Map<EtiquetaDTO>(etiqueta);
        }

        public async Task<int> AddAsync(EtiquetaDTO dto)
        {
            var entity = _mapper.Map<Etiquetas>(dto);
            return await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(short id, EtiquetaDTO dto)
        {
            var entity = await _repository.FindByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"No se encontró la etiqueta con ID {id}");
            }

            _mapper.Map(dto, entity);
            await _repository.UpdateAsync(entity);
        }

        public async Task<ICollection<EtiquetaDTO>> SearchAsync(string searchTerm)
        {
            var etiquetas = await _repository.ListAsync();
            return _mapper.Map<ICollection<EtiquetaDTO>>(etiquetas)
                .Where(e =>
                    e.Nombre != null && e.Nombre.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (e.Descripcion != null && e.Descripcion.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                )
                .ToList();
        }
    }
}
