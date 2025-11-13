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
    public class ServicePrioridades : IServicePrioridades
    {
        private readonly IRepositoryPrioridades _repository;
        private readonly IMapper _mapper;
        public ServicePrioridades(IRepositoryPrioridades repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<int> AddAsync(PrioridadDTO dto)
        {

            var entity = _mapper.Map<Prioridades>(dto);
            // Return ID Generado
            return await _repository.AddAsync(entity);
        }

        public async Task<PrioridadDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<PrioridadDTO>(@object);
            return objectMapped;
        }

        public async Task<ICollection<PrioridadDTO>> ListAsync()
        {
            //Obtener datos del repositorio
            var list = await _repository.ListAsync(); // List<Prioridades>
            return _mapper.Map<ICollection<PrioridadDTO>>(list); // Ahora sí mapea correctamente
        }

        public async Task UpdateAsync(int id, PrioridadDTO dto)
        {
            //Obtenga el modelo original a actualizar
            var @object = await _repository.FindByIdAsync(id);
            //       source, destination
            var entity = _mapper.Map(dto, @object!);

            await _repository.UpdateAsync(entity);
        }
    }
}
