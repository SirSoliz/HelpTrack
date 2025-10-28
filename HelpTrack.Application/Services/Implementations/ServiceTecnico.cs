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
    public class ServiceTecnico : IServiceTecnico
    {
        private readonly IRepositoryTecnico _repositoryTecnico;
        private readonly IMapper _mapper;

        public ServiceTecnico(IRepositoryTecnico repositoryTecnico, IMapper mapper)
        {
            _repositoryTecnico = repositoryTecnico;
            _mapper = mapper;
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
            var entity = _mapper.Map<Tecnicos>(dto);
            return await _repositoryTecnico.AddAsync(entity);
        }

        public async Task UpdateAsync(int id, TecnicoDTO dto)
        {
            var existingTecnico = await _repositoryTecnico.FindByIdAsync(id);
            if (existingTecnico == null)
            {
                throw new KeyNotFoundException($"No se encontró el técnico con ID {id} para actualizar");
            }

            _mapper.Map(dto, existingTecnico);
            await _repositoryTecnico.UpdateAsync(existingTecnico);
        }

        public async Task<ICollection<TecnicoDTO>> SearchAsync(string searchTerm)
        {
            var tecnicos = await _repositoryTecnico.SearchAsync(searchTerm);
            return _mapper.Map<ICollection<TecnicoDTO>>(tecnicos);
        }
    }
}