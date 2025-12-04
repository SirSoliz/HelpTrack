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
        private readonly IRepositoryTecnico _repositoryTecnico;
        private readonly IMapper _mapper;
        public ServiceTicket(IRepositoryTicket repository, IRepositoryTecnico repositoryTecnico, IMapper mapper)
        {
            _repository = repository;
            _repositoryTecnico = repositoryTecnico;
            _mapper = mapper;
        }
        public async Task<int> AddAsync(TicketDTO dto)
        {
            var entity = _mapper.Map<Tickets>(dto);

            // Manually map images because AutoMapper ignores them
            if (dto.ImagenesTicket != null && dto.ImagenesTicket.Any())
            {
                entity.ImagenesTicket = dto.ImagenesTicket;
            }
            
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

            // Manually add new images
            if (dto.ImagenesTicket != null && dto.ImagenesTicket.Any())
            {
                foreach (var img in dto.ImagenesTicket)
                {
                    entity.ImagenesTicket.Add(img);
                }
            }

            await _repository.UpdateAsync(entity);
        }

        public async Task AssignAsync(AsignacionTicketDTO dto)
        {
            // Check if assignment already exists
            var existingAssignment = await _repository.GetAssignmentByTicketIdAsync(dto.IdTicket);

            if (existingAssignment != null)
            {
                // If reassigning to a different technician, decrement the old technician's workload
                int oldTechnicianId = existingAssignment.IdTecnico;
                if (oldTechnicianId != dto.IdTecnico)
                {
                    await _repositoryTecnico.DecrementWorkloadAsync(oldTechnicianId);
                }

                // Update existing assignment manually to avoid modifying the Key (IdAsignacion)
                existingAssignment.IdTecnico = dto.IdTecnico;
                existingAssignment.Metodo = dto.Metodo;
                existingAssignment.Prioridad = dto.Prioridad;
                existingAssignment.FechaAsignacion = DateTime.Now;
                
                await _repository.UpdateAssignmentAsync(existingAssignment);

                // Increment the new technician's workload (only if it's a different technician)
                if (oldTechnicianId != dto.IdTecnico)
                {
                    await _repositoryTecnico.IncrementWorkloadAsync(dto.IdTecnico);
                }
            }
            else
            {
                // Create new assignment
                var assignment = _mapper.Map<AsignacionesTicket>(dto);
                assignment.FechaAsignacion = DateTime.Now;
                await _repository.AddAssignmentAsync(assignment);

                // Increment the technician's workload for new assignment
                await _repositoryTecnico.IncrementWorkloadAsync(dto.IdTecnico);
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

        public async Task<bool> DeleteAsync(int id)
        {
            // Get the assignment to know which technician to decrement workload from
            var assignment = await _repository.GetAssignmentByTicketIdAsync(id);
            
            // Delete the ticket (this also deletes assignments, images, and history)
            var result = await _repository.DeleteAsync(id);

            // If deletion was successful and ticket was assigned, decrement technician's workload
            if (result && assignment != null)
            {
                await _repositoryTecnico.DecrementWorkloadAsync(assignment.IdTecnico);
            }

            return result;
        }
    }
}
