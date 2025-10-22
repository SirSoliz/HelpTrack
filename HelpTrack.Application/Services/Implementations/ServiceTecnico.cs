using AutoMapper;
using HelpTrack.Application.DTOs;
using HelpTrack.Application.Services.Interfaces;
using HelpTrack.Infraestructure.Models;
using HelpTrack.Infraestructure.Repository.Implementations;
using HelpTrack.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace  HelpTrack.Application.Services.Implementations
{
    public class ServiceTecnico : IServiceTecnico
    {
        private readonly IRepositoryTecnico _repository;
        private readonly IMapper _mapper;
        public ServiceTecnico(IRepositoryTecnico repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<TecnicoDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<TecnicoDTO>(@object);
            return objectMapped;
        }
        public async Task<ICollection<TecnicoDTO>> ListAsync()
        {
            //Obtener datos del repositorio
            var list = await _repository.ListAsync();
            // Map List<Autor> a ICollection<BodegaDTO>
            var collection = _mapper.Map<ICollection<TecnicoDTO>>(list);
            // Return lista
            return collection;
        }

        public async Task<int> AddAsync(TecnicoDTO dto)
        {
            // Map AutorDTO a Autor
            var entity = _mapper.Map<Tecnicos>(dto);
            // Return ID Generado
            return await _repository.AddAsync(entity);
        }
        public async Task UpdateAsync(int id, TecnicoDTO dto)
        {
            //Obtenga el modelo original a actualizar
            var @object = await _repository.FindByIdAsync(id);
            //       source, destination
            var entity = _mapper.Map(dto, @object!);


            await _repository.UpdateAsync(entity);
        }
    }
}
