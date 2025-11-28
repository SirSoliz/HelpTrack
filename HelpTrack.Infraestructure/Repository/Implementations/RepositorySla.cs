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
    public class RepositorySla : IRepositorySla
    {
        private readonly HelpTrackContext _context;
        public RepositorySla(HelpTrackContext context)
        {
            _context = context;
        }
        public async Task<int> AddAsync(Sla entity)
        {
            await _context.Set<Sla>().AddAsync(entity);
            return (int)entity.IdSla;
        }

        public async Task<Sla?> FindByIdAsync(int id)
        {
            return await _context.Sla.FindAsync(id);

        }

        public async Task<ICollection<Sla>> ListAsync()
        {
            var collection = await _context.Set<Sla>().ToListAsync();
            return collection;
        }

        public async Task UpdateAsync(Sla entity)
        {
            await _context.SaveChangesAsync();
        }
    }
}
