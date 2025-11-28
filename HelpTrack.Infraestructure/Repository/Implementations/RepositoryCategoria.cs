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
    public class RepositoryCategoria : IRepositoryCategoria
    {
        private readonly HelpTrackContext _context; 
        public RepositoryCategoria(HelpTrackContext context)
        {
            _context = context;
        }
        public async Task<int> AddAsync(Categorias entity)
        {
            await _context.Set<Categorias>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdCategoria;
        }

        public async Task<Categorias?> FindByIdAsync(int id)
        {
            short key = (short)id;
            return await _context.Categorias
               .Include(c => c.IdSlaNavigation)
               .Include(c => c.IdEspecialidad)
               .Include(c => c.IdEtiqueta)
               .FirstOrDefaultAsync(c => c.IdCategoria == key);
        }


        public async Task<ICollection<Categorias>> ListAsync()
        {
            return await _context.Categorias
                .Include(c => c.IdSlaNavigation)  // Incluir datos del SLA
                .Include(c => c.IdEspecialidad)  // Incluir especialidades
                .Include(c => c.IdEtiqueta)  // Incluir etiquetas
                .ToListAsync();
        }
        public async Task UpdateAsync(Categorias entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
