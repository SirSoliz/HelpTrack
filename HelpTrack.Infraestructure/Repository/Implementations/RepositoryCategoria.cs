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
            return entity.IdCategoria;
        }

        public async Task<Categorias> FindByIdAsync(int id)
        {
            var @object = await _context.Set<Categorias>().FindAsync(id);
            return @object!;
        }

        public async Task<ICollection<Categorias>> ListAsync()
        {
            var collection = await _context.Set<Categorias>().ToListAsync();
            return collection;
        }

        public async Task UpdateAsync(Categorias entity)
        {
            await _context.SaveChangesAsync();
        }
    }
}
