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

namespace HelpTrack.Application.Services.Implementations
{
    public class ServiceCategoria : IServiceCategoria
    {
        private readonly IRepositoryCategoria _repository;
        private readonly IMapper _mapper;
        public ServiceCategoria(IRepositoryCategoria repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<int> AddAsync(CategoriaDTO dto)
        {
            var entity = _mapper.Map<Categorias>(dto);

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

        public async Task<CategoriaDTO> GetByIdWithDetailsAsync(short id)
        {
            var categoria = await _repository.FindByIdAsync(id);
            return _mapper.Map<CategoriaDTO>(categoria);
        }
    }
}
