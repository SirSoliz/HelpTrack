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
    public class ServiceSla : IServiceSla
    {
        private readonly IRepositorySla _repository;
        private readonly IMapper _mapper;
        public ServiceSla(IRepositorySla repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<int> AddAsync(SlaDTO dto)
        {
            var entity = _mapper.Map<Sla>(dto);
            // Return ID Generado
            return await _repository.AddAsync(entity);
        }

        public async Task<SlaDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<SlaDTO>(@object);
            return objectMapped;
        }

        public async Task<ICollection<SlaDTO>> ListAsync()
        {
            //Obtener datos del repositorio
            var list = await _repository.ListAsync();

            var collection = _mapper.Map<ICollection<SlaDTO>>(list);
            // Return lista
            return collection;
        }

        public async Task UpdateAsync(int id, SlaDTO dto)
        {
            //Obtenga el modelo original a actualizar
            var @object = await _repository.FindByIdAsync(id);
            //       source, destination
            var entity = _mapper.Map(dto, @object!);

            await _repository.UpdateAsync(entity);
        }
    }
}
