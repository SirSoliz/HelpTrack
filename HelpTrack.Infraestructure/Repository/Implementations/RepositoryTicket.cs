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
            await _context.Set<Tickets>().AddAsync(entity);
            return (int)entity.IdTicket;
        }

        public async Task<Tickets> FindByIdAsync(int id)
        {
            //throw new NotImplementedException();
            var @object = await _context.Set<Tickets>().FindAsync(id);
            return @object!;
        }

        public async Task<ICollection<Tickets>> ListAsync()
        {
            var collection = await _context.Set<Tickets>().ToListAsync();
            return collection;
        }

        public async Task UpdateAsync(Tickets entity)
        {
            await _context.SaveChangesAsync();
        }
    }
}
