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
    public class RepositoryPrioridades : IRepositoryPrioridades
    {
        private readonly HelpTrackContext _context;
        public RepositoryPrioridades(HelpTrackContext context)
        {
            _context = context;
        }
        public async Task<int> AddAsync(Prioridades entity)
        {

            await _context.Set<Prioridades>().AddAsync(entity);

            return entity.IdPrioridad;
        }

        public async Task<Prioridades> FindByIdAsync(int id)
        {
            //throw new NotImplementedException();
            var @object = await _context.Set<Prioridades>().FindAsync(id);
            return @object!;
        }

        public async Task<ICollection<Prioridades>> ListAsync()
        {
            var collection = await _context.Set<Prioridades>().ToListAsync();
            return collection;
        }

        public async Task UpdateAsync(Prioridades entity)
        {
            await _context.SaveChangesAsync();
        }
    }
}
