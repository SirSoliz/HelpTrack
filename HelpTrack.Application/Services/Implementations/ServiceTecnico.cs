using AutoMapper;
using HelpTrack.Application.DTOs;
using HelpTrack.Application.Services.Interfaces;
using HelpTrack.Infraestructure.Models;
using HelpTrack.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var @object = await _repositoryTecnico.FindByIdAsync(id);
            var objectMapped = _mapper.Map<TecnicoDTO>(@object);
            return objectMapped;
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
            var @object = await _repositoryTecnico.FindByIdAsync(id);
            var entity = _mapper.Map(dto, @object!);
            await _repositoryTecnico.UpdateAsync(entity);
        }

        public async Task<ICollection<TecnicoDTO>> SearchAsync(string searchTerm)
        {
            var tecnicos = await _repositoryTecnico.SearchAsync(searchTerm);
            return _mapper.Map<ICollection<TecnicoDTO>>(tecnicos);
        }
    }
}
