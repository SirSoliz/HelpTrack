// HelpTrack.Infraestructure/Repository/Implementations/RepositoryEtiqueta.cs
using HelpTrack.Infraestructure.Data;
using HelpTrack.Infraestructure.Models;
using HelpTrack.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpTrack.Infraestructure.Repository.Implementations
{
    public class RepositoryEtiqueta : IRepositoryEtiqueta
    {
        private readonly HelpTrackContext _context;

        public RepositoryEtiqueta(HelpTrackContext context)
        {
            _context = context;
        }

        public async Task<Etiquetas?> FindByIdAsync(short id)
        {
            return await _context.Etiquetas
                .Include(e => e.IdCategoria)
                .FirstOrDefaultAsync(e => e.IdEtiqueta == id);
        }

        public async Task<ICollection<Etiquetas>> ListAsync()
        {
            return await _context.Etiquetas
                .Include(e => e.IdCategoria)
                .ToListAsync();
        }

        public async Task<int> AddAsync(Etiquetas entity)
        {
            await _context.Etiquetas.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdEtiqueta;
        }

        public async Task UpdateAsync(Etiquetas entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Etiquetas entity)
        {
            _context.Etiquetas.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
