using HelpTrack.Infraestructure.Data;
using HelpTrack.Infraestructure.Models;
using HelpTrack.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpTrack.Infraestructure.Repository.Implementations
{
    public class RepositoryTecnico : IRepositoryTecnico
    {
        private readonly HelpTrackContext _context;

        public RepositoryTecnico(HelpTrackContext context)
        {
            _context = context;
        }

        public async Task<Tecnicos?> FindByIdAsync(int id)
        {
            return await _context.Tecnicos
                .Include(t => t.IdTecnicoNavigation)  // Incluir la relación con Usuarios
                .FirstOrDefaultAsync(t => t.IdTecnico == id);
        }

        public async Task<ICollection<Tecnicos>> ListAsync()
        {
            return await _context.Tecnicos
                .Include(t => t.IdTecnicoNavigation)  // Incluir la relación con Usuarios
                .ToListAsync();
        }

        public async Task<int> AddAsync(Tecnicos entity)
        {
            await _context.Tecnicos.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdTecnico;
        }

        public async Task UpdateAsync(Tecnicos entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Tecnicos>> SearchAsync(string searchTerm)
        {
            return await _context.Tecnicos
                .Include(t => t.IdTecnicoNavigation)  // Include related user data
                .Where(t => 
                    EF.Functions.Like(t.Alias, $"%{searchTerm}%") ||
                    (t.IdTecnicoNavigation != null && 
                     (EF.Functions.Like(t.IdTecnicoNavigation.Nombre, $"%{searchTerm}%") ||
                      EF.Functions.Like(t.IdTecnicoNavigation.Email, $"%{searchTerm}%")))
                )
                .ToListAsync();
        }
    }
}
