using HelpTrack.Infraestructure.Data;
using HelpTrack.Infraestructure.Models;
using Microsoft.EntityFrameworkCore;

public class RepositoryTecnico : IRepositoryTecnico
{
    private readonly HelpTrackContext _context;

    public RepositoryTecnico(HelpTrackContext context)
    {
        _context = context;
    }

    // Implementación del método requerido por la interfaz
    public async Task<Tecnicos?> FindByIdAsync(int id)
    {
        return await _context.Tecnicos
            .Include(t => t.IdTecnicoNavigation)  // Incluir datos del usuario
            .Include(t => t.IdEspecialidad)       // Incluir especialidades
            .FirstOrDefaultAsync(t => t.IdTecnico == id);
    }

    // Versión sobrecargada opcional con parámetros adicionales
    public async Task<Tecnicos?> FindByIdAsync(int id, string? includeProperties = null)
    {
        IQueryable<Tecnicos> query = _context.Tecnicos;

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }
        }

        return await query.FirstOrDefaultAsync(t => t.IdTecnico == id);
    }

    // Resto de los métodos de la interfaz...
    public async Task<ICollection<Tecnicos>> ListAsync()
    {
        return await _context.Tecnicos
            .Include(t => t.IdTecnicoNavigation)
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
            .Include(t => t.IdTecnicoNavigation)
            .Where(t =>
                EF.Functions.Like(t.Alias, $"%{searchTerm}%") ||
                (t.IdTecnicoNavigation != null &&
                 (EF.Functions.Like(t.IdTecnicoNavigation.Nombre, $"%{searchTerm}%")))
            )
            .ToListAsync();
    }
}