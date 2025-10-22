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
    public class ServiceTicket : IServiceTicket
    {
        private readonly IRepositoryTicket _repository;
        private readonly IMapper _mapper;
        public ServiceTicket(IRepositoryTicket repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<int> AddAsync(TicketDTO dto)
        {
            var entity = _mapper.Map<Tickets>(dto);
            // Return ID Generado
            return await _repository.AddAsync(entity);
        }

        public async Task<TicketDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<TicketDTO>(@object);
            return objectMapped;
        }

        public async Task<ICollection<TicketDTO>> ListAsync()
        {
            //Obtener datos del repositorio
            var list = await _repository.ListAsync();

            var collection = _mapper.Map<ICollection<TicketDTO>>(list);
            // Return lista
            return collection;
        }

        public async Task UpdateAsync(int id, TicketDTO dto)
        {
            //Obtenga el modelo original a actualizar
            var @object = await _repository.FindByIdAsync(id);
            //       source, destination
            var entity = _mapper.Map(dto, @object!);

            await _repository.UpdateAsync(entity);
        }
    }
}
