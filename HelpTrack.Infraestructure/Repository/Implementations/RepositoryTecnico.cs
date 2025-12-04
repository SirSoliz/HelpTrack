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
            .Include(t =>t.IdTecnicoNavigation)  // Incluir datos del usuario
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

    public async Task UpdateAsync(Tecnicos entity, string[] selectedCategorias)
    {
        // Obtener el técnico actual con sus relaciones
        var tecnicoActual = await _context.Tecnicos
            .Include(t => t.IdEspecialidad)
            .Include(t => t.IdTecnicoNavigation)
            .FirstOrDefaultAsync(t => t.IdTecnico == entity.IdTecnico);

        if (tecnicoActual == null)
        {
            throw new KeyNotFoundException($"No se encontró el técnico con ID {entity.IdTecnico}");
        }

        // Actualizar propiedades del técnico
        _context.Entry(tecnicoActual).CurrentValues.SetValues(entity);

        // Actualizar propiedades del usuario asociado
        if (entity.IdTecnicoNavigation != null)
        {
            _context.Entry(tecnicoActual.IdTecnicoNavigation).CurrentValues.SetValues(entity.IdTecnicoNavigation);
        }


        // Actualizar especialidades
        if (selectedCategorias != null)
        {
            var especialidadesSeleccionadasHS = new HashSet<string>(selectedCategorias);
            var especialidadesTecnico = new HashSet<int>(tecnicoActual.IdEspecialidad.Select(e => e.IdEspecialidad));

            foreach (var especialidad in _context.Especialidades)
            {
                if (especialidadesSeleccionadasHS.Contains(especialidad.IdEspecialidad.ToString()))
                {
                    if (!especialidadesTecnico.Contains(especialidad.IdEspecialidad))
                    {
                        tecnicoActual.IdEspecialidad.Add(especialidad);
                    }
                }
                else
                {
                    if (especialidadesTecnico.Contains(especialidad.IdEspecialidad))
                    {
                        var especialidadToRemove = tecnicoActual.IdEspecialidad
                            .Single(e => e.IdEspecialidad == especialidad.IdEspecialidad);
                        tecnicoActual.IdEspecialidad.Remove(especialidadToRemove);
                    }
                }
            }
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TecnicoExists(entity.IdTecnico))
            {
                throw new KeyNotFoundException($"No se encontró el técnico con ID {entity.IdTecnico}");
            }
            else
            {
                throw;
            }
        }
    }

    private bool TecnicoExists(int id)
    {
        return _context.Tecnicos.Any(e => e.IdTecnico == id);
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

    // En RepositoryTecnico.cs
    public async Task<bool> DeleteAsync(int id)
    {
        var tecnico = await _context.Tecnicos
            .Include(t => t.IdTecnicoNavigation)  // Incluir el usuario asociado
            .Include(t => t.IdEspecialidad)       // Incluir las especialidades
            .FirstOrDefaultAsync(t => t.IdTecnico == id);

        if (tecnico == null)
        {
            return false;
        }

        try
        {
            // Eliminar las relaciones de especialidades
            if (tecnico.IdEspecialidad != null && tecnico.IdEspecialidad.Any())
            {
                // Eliminar las relaciones de especialidades
                foreach (var especialidad in tecnico.IdEspecialidad.ToList())
                {
                    tecnico.IdEspecialidad.Remove(especialidad);
                }
            }

            // Eliminar el técnico
            _context.Tecnicos.Remove(tecnico);

            // Eliminar el usuario asociado
            if (tecnico.IdTecnicoNavigation != null)
            {
                _context.Usuarios.Remove(tecnico.IdTecnicoNavigation);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            // Log the error
            Console.WriteLine($"Error al eliminar el técnico: {ex.Message}");
            return false;
        }
    }

    public async Task IncrementWorkloadAsync(int technicianId)
    {
        var tecnico = await _context.Tecnicos.FindAsync(technicianId);
        if (tecnico != null)
        {
            tecnico.NivelCarga += 1;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DecrementWorkloadAsync(int technicianId)
    {
        var tecnico = await _context.Tecnicos.FindAsync(technicianId);
        if (tecnico != null && tecnico.NivelCarga > 0)
        {
            tecnico.NivelCarga -= 1;
            await _context.SaveChangesAsync();
        }
    }

    public async Task RecalculateAllWorkloadsAsync()
    {
        // Get the "Cerrado" state ID to exclude closed tickets
        var estadoCerrado = await _context.EstadosTicket
            .FirstOrDefaultAsync(e => e.Nombre == "Cerrado");
        int idEstadoCerrado = estadoCerrado?.IdEstado ?? -1;

        // Get all technicians with their assignments
        var tecnicos = await _context.Tecnicos
            .Include(t => t.AsignacionesTicket)
                .ThenInclude(a => a.IdTicketNavigation)
            .ToListAsync();

        foreach (var tecnico in tecnicos)
        {
            // Count active (non-closed) tickets assigned to this technician
            int activeTickets = tecnico.AsignacionesTicket
                .Count(a => a.IdTicketNavigation.IdEstadoActual != idEstadoCerrado);

            tecnico.NivelCarga = activeTickets;
        }

        await _context.SaveChangesAsync();
    }
}