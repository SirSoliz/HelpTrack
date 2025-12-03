using AutoMapper;
using HelpTrack.Application.DTOs;
using HelpTrack.Application.Services.Interfaces;
using HelpTrack.Infraestructure.Models;
using HelpTrack.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
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
            
            // Asignar valores por defecto y fechas
            entity.FechaCreacion = DateTime.Now;
            entity.FechaAsignacion = DateTime.Now; 
            
            // Valores por defecto si no vienen en el DTO (o si son 0)
            // Asumiendo IDs válidos en la base de datos:
            if (entity.IdEstadoActual == 0) entity.IdEstadoActual = 1; // 1 = Abierto/Nuevo
            if (entity.IdSla == 0) entity.IdSla = 1; 
            if (entity.IdEtiqueta == 0) entity.IdEtiqueta = 1; 
            
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

        public async Task<ICollection<TicketHistoryDTO>> GetHistoryAsync()
        {
            var list = await _repository.GetHistoryListAsync();
            var collection = _mapper.Map<ICollection<TicketHistoryDTO>>(list);
            return collection;
        }

        public async Task<ICollection<HistorialTicketDTO>> GetTicketHistoryLogAsync(int ticketId)
        {
            var list = await _repository.GetHistoryLogAsync(ticketId);
            var collection = _mapper.Map<ICollection<HistorialTicketDTO>>(list);
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

        public async Task AssignAsync(AsignacionTicketDTO dto)
        {
            // Check if assignment already exists
            var existingAssignment = await _repository.GetAssignmentByTicketIdAsync(dto.IdTicket);

            if (existingAssignment != null)
            {
                // Update existing assignment manually to avoid modifying the Key (IdAsignacion)
                existingAssignment.IdTecnico = dto.IdTecnico;
                existingAssignment.Metodo = dto.Metodo;
                existingAssignment.Prioridad = dto.Prioridad;
                existingAssignment.FechaAsignacion = DateTime.Now;
                
                await _repository.UpdateAssignmentAsync(existingAssignment);
            }
            else
            {
                // Create new assignment
                var assignment = _mapper.Map<AsignacionesTicket>(dto);
                assignment.FechaAsignacion = DateTime.Now;
                await _repository.AddAssignmentAsync(assignment);
            }

            // Update ticket status to "Asignado" (ID 2)
            var ticket = await _repository.FindByIdAsync(dto.IdTicket);
            if (ticket != null)
            {
                ticket.IdEstadoActual = 2; // Asignado
                ticket.FechaAsignacion = DateTime.Now;
                await _repository.UpdateAsync(ticket);
            }
        }
    }
}
