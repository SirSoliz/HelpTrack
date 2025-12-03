using HelpTrack.Infraestructure.Data;
using HelpTrack.Infraestructure.Models;
using HelpTrack.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrack.Infraestructure.Repository.Implementations
{
    public class RepositoryTicket : IRepositoryTicket
    {
        private readonly HelpTrackContext _context;
        public RepositoryTicket(HelpTrackContext context)
        {
            _context = context;
        }
        public async Task<int> AddAsync(Tickets entity)
        {
            // Removed manual ID assignment logic to allow DB auto-increment
            await _context.Tickets.AddAsync(entity);

            await _context.SaveChangesAsync();

            return (int)entity.IdTicket;
        }

        public async Task<Tickets> FindByIdAsync(int id)
        {
            var @object = await _context.Tickets
                .Include(t => t.ImagenesTicket)
                .Include(t => t.IdPrioridadNavigation)
                .Include(t => t.IdCategoriaNavigation)
                .Include(t => t.IdEstadoActualNavigation)
                .Include(t => t.IdUsuarioCreacionNavigation)
                .FirstOrDefaultAsync(t => t.IdTicket == id);
            return @object!;
        }

        public async Task<ICollection<Tickets>> ListAsync()
        {
            var collection = await _context.Tickets
                .Include(t => t.IdPrioridadNavigation)
                .Include(t => t.IdEstadoActualNavigation)
                .ToListAsync();
            return collection;
        }

        public async Task UpdateAsync(Tickets entity)
        {
            // For updates, we need to ensure the context is tracking the entity correctly.
            // If the entity was detached or created from a DTO, we might need to attach it.
            // However, since we are using AutoMapper to map ONTO the existing entity in the Service,
            // the entity passed here should already be tracked if it came from FindByIdAsync.
            
            // If it's a new context instance or detached entity, we might need:
            // _context.Entry(entity).State = EntityState.Modified;
            
            await _context.SaveChangesAsync();
        }

        public async Task AddAssignmentAsync(AsignacionesTicket assignment)
        {
            await _context.AsignacionesTicket.AddAsync(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task<AsignacionesTicket?> GetAssignmentByTicketIdAsync(int ticketId)
        {
            return await _context.AsignacionesTicket
                .FirstOrDefaultAsync(a => a.IdTicket == ticketId);
        }

        public async Task UpdateAssignmentAsync(AsignacionesTicket assignment)
        {
            _context.AsignacionesTicket.Update(assignment);
            await _context.SaveChangesAsync();
        }
    }
}
