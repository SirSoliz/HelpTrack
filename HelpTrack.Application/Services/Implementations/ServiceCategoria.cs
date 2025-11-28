using AutoMapper;
using HelpTrack.Application.DTOs;
using HelpTrack.Application.Services.Interfaces;
using HelpTrack.Infraestructure.Models;
using HelpTrack.Infraestructure.Repository.Implementations;
using HelpTrack.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrack.Application.Services.Implementations
{
    public class ServiceCategoria : IServiceCategoria
    {
        private readonly IRepositoryCategoria _repository;
        private readonly IMapper _mapper;
        private readonly IRepositorySla _repositorySla;

        public ServiceCategoria(IRepositoryCategoria repository, IMapper mapper, IRepositorySla repositorySla)
        {
            _repository = repository;
            _mapper = mapper;
            _repositorySla = repositorySla;
        }
        public async Task<int> AddAsync(CategoriaDTO dto)
        {
            var entity = _mapper.Map<Categorias>(dto);

            // Cargar el SLA relacionado
            var sla = await _repositorySla.FindByIdAsync(dto.IdSla);
            if (sla == null)
            {
                throw new ArgumentException($"No se encontró un SLA con el ID {dto.IdSla}");
            }

            entity.IdSlaNavigation = sla;

            return await _repository.AddAsync(entity);
        }
        public async Task<CategoriaDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);

            var objectMapped = _mapper.Map<CategoriaDTO>(@object);

            return objectMapped;
        }

        public async Task<ICollection<CategoriaDTO>> ListAsync()
        {            
            var list = await _repository.ListAsync();

            var collection = _mapper.Map<ICollection<CategoriaDTO>>(list);

            return collection;
        }

        public async Task UpdateAsync(int id, CategoriaDTO dto)
        {
            var @object = await _repository.FindByIdAsync(id);

            var entity = _mapper.Map(dto, @object!);

            await _repository.UpdateAsync(entity);
        }

        public async Task<CategoriaDTO> GetByIdWithDetailsAsync(int id)
        {
            var categoria = await _repository.FindByIdAsync(id);
            return _mapper.Map<CategoriaDTO>(categoria);
        }
    }
}
