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
    public class RepositoryEstadoTicket : IRepositoryEstadoTicket
    {
        private readonly HelpTrackContext _context;

        public RepositoryEstadoTicket(HelpTrackContext context)
        {
            _context = context;
        }

        public async Task<EstadosTicket?> FindByIdAsync(short id)
        {
            return await _context.EstadosTicket
                .Include(e => e.IdEstado)
                .Include(e => e.Nombre)
                .FirstOrDefaultAsync(e => e.IdEstado == id);
        }

        public async Task<ICollection<EstadosTicket>> ListAsync()
        {
            return await _context.EstadosTicket
                .Include(e => e.IdEstado)
                .Include(e => e.Nombre)
                .ToListAsync();
        }

        public async Task<int> AddAsync(EstadosTicket entity)
        {
            await _context.EstadosTicket.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdEstado;
        }

        public async Task UpdateAsync(EstadosTicket entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(EstadosTicket entity)
        {
            _context.EstadosTicket.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
