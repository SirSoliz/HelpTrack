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
    public class ServiceEstadoTicket : IServiceEstadoTicket
    {
        private readonly IRepositoryEstadoTicket _repository;
        private readonly IMapper _mapper;

        public ServiceEstadoTicket(IRepositoryEstadoTicket repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<EstadoTicketDTO>> ListAsync()
        {
            var EstadoTicketes = await _repository.ListAsync();
            return _mapper.Map<ICollection<EstadoTicketDTO>>(EstadoTicketes);
        }

        public async Task<EstadoTicketDTO> FindByIdAsync(short id)
        {
            var EstadoTicket = await _repository.FindByIdAsync(id);
            if (EstadoTicket == null)
            {
                throw new KeyNotFoundException($"No se encontró la EstadoTicket con ID {id}");
            }
            return _mapper.Map<EstadoTicketDTO>(EstadoTicket);
        }

        public async Task<int> AddAsync(EstadoTicketDTO dto)
        {
            var entity = _mapper.Map<EstadosTicket>(dto);
            return await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(short id, EstadoTicketDTO dto)
        {
            var entity = await _repository.FindByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"No se encontró la EstadoTicket con ID {id}");
            }

            _mapper.Map(dto, entity);
            await _repository.UpdateAsync(entity);
        }

        public async Task<ICollection<EstadoTicketDTO>> SearchAsync(string searchTerm)
        {
            var EstadoTicketes = await _repository.ListAsync();
            return _mapper.Map<ICollection<EstadoTicketDTO>>(EstadoTicketes)
                .Where(e =>
                    e.Nombre != null && e.Nombre.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (e.Nombre != null && e.Nombre.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                )
                .ToList();
        }
    }
}
