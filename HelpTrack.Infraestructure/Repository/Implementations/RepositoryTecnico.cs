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
    public class RepositoryTecnico : IRepositoryTecnico
    {
        private readonly HelpTrackContext _context;
        public RepositoryTecnico(HelpTrackContext context)
        {
            _context = context;
        }
        public async Task<Tecnicos> FindByIdAsync(int id)
        {
            //throw new NotImplementedException();
            var @object = await _context.Set<Tecnicos>().FindAsync(id);
            return @object!;
        }
        public async Task<ICollection<Tecnicos>> ListAsync()
        {
            //Select * from Autor
            var collection = await _context.Set<Tecnicos>().ToListAsync();
            return collection;
        }

        public async Task<int> AddAsync(Tecnicos entity)
        {
            await _context.Set<Tecnicos>().AddAsync(entity);
            return entity.IdTecnico;
        }

        public async Task UpdateAsync(Tecnicos entity)
        {
            //Las relaciones a actualizar depende de la consulta utilizada en el servicio

            await _context.SaveChangesAsync();
        }
    }
}
