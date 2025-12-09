using HelpTrack.Infraestructure.Data;
using HelpTrack.Infraestructure.Models;
using HelpTrack.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpTrack.Infraestructure.Repository.Implementations
{
    public class RepositoryRol : IRepositoryRol
    {
        private readonly HelpTrackContext _context;

        public RepositoryRol(HelpTrackContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Roles>> ListAsync()
        {
            return await _context.Roles
                .OrderBy(r => r.Nombre)
                .ToListAsync();
        }

        public async Task<Roles> FindByIdAsync(int id)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.IdRol == id);
        }

        public async Task<int> AddAsync(Roles entity)
        {
            _context.Roles.Add(entity);
            await _context.SaveChangesAsync();
            return entity.IdRol;
        }

        public async Task UpdateAsync(Roles entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await FindByIdAsync(id);
            if (entity == null)
                return false;

            _context.Roles.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Roles
                .AnyAsync(r => r.IdRol == id);
        }
    }
}
