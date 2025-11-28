// HelpTrack.Infraestructure/Repository/Implementations/RepositoryEspecialidad.cs
using HelpTrack.Infraestructure.Data;
using HelpTrack.Infraestructure.Models;
using HelpTrack.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpTrack.Infraestructure.Repository.Implementations
{
    public class RepositoryEspecialidad : IRepositoryEspecialidad
    {
        private readonly HelpTrackContext _context;

        public RepositoryEspecialidad(HelpTrackContext context)
        {
            _context = context;
        }

        public async Task<Especialidades?> FindByIdAsync(int id)
        {
            return await _context.Especialidades
                .Include(e => e.IdCategoria)
                .Include(e => e.IdTecnico)
                .FirstOrDefaultAsync(e => e.IdEspecialidad == id);
        }
        public async Task<ICollection<Especialidades>> FindByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.Especialidades
                .Where(e => ids.Contains(e.IdEspecialidad))
                .ToListAsync();
        }

        public async Task<ICollection<Especialidades>> ListAsync()
        {
            return await _context.Especialidades
                .Include(e => e.IdCategoria)
                .Include(e => e.IdTecnico)
                .ToListAsync();
        }

        public async Task<int> AddAsync(Especialidades entity)
        {
            await _context.Especialidades.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdEspecialidad;
        }

        public async Task UpdateAsync(Especialidades entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Especialidades entity)
        {
            _context.Especialidades.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}