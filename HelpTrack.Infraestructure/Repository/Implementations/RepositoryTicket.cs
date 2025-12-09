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

        public async Task<ICollection<Tickets>> GetHistoryListAsync()
        {
            var collection = await _context.Tickets
                .Include(t => t.IdEstadoActualNavigation)
                .Include(t => t.AsignacionesTicket)
                    .ThenInclude(a => a.IdTecnicoNavigation)
                        .ThenInclude(tec => tec.IdTecnicoNavigation) // Usuario
                .ToListAsync();
            return collection;
        }

        public async Task<ICollection<HistorialTicket>> GetHistoryLogAsync(int ticketId)
        {
            return await _context.HistorialTicket
                .Include(h => h.IdEstadoNavigation)
                .Include(h => h.IdUsuarioAccionNavigation)
                .Where(h => h.IdTicket == ticketId)
                .OrderByDescending(h => h.FechaEvento)
                .ToListAsync();
        }

        public async Task UpdateAsync(Tickets entity)
        {
            _context.Tickets.Update(entity);
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

        public async Task AddHistoryAsync(HistorialTicket history)
        {
            await _context.HistorialTicket.AddAsync(history);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.AsignacionesTicket)
                .Include(t => t.ImagenesTicket)
                .Include(t => t.HistorialTicket)
                .FirstOrDefaultAsync(t => t.IdTicket == id);

            if (ticket == null)
            {
                return false;
            }

            try
            {
                // Delete related records first (due to foreign key constraints)
                
                // Delete assignment (singular)
                if (ticket.AsignacionesTicket != null)
                {
                    _context.AsignacionesTicket.Remove(ticket.AsignacionesTicket);
                }

                // Delete images
                if (ticket.ImagenesTicket.Any())
                {
                    _context.ImagenesTicket.RemoveRange(ticket.ImagenesTicket);
                }

                // Delete history
                if (ticket.HistorialTicket.Any())
                {
                    _context.HistorialTicket.RemoveRange(ticket.HistorialTicket);
                }

                // Finally, delete the ticket
                _context.Tickets.Remove(ticket);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el ticket: {ex.Message}");
                return false;
            }
        }
    }
}
