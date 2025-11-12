using HelpTrack.Application.DTOs;
using HelpTrack.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;

namespace HelpTrack.Web.Controllers
{
    public class TecnicoController : Controller
    {
        private readonly IServiceTecnico _serviceTecnico;
        private readonly IServiceEspecialidad _especialidadService;  
        private const int PageSize = 10; // Número de elementos por página

        public TecnicoController(IServiceTecnico serviceTecnico, IServiceEspecialidad especialidadService)
        {
            _serviceTecnico = serviceTecnico;
            _especialidadService = especialidadService;

        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page, string searchString)
        {
            try
            {
                int pageNumber = page ?? 1;
                ICollection<TecnicoDTO> tecnicos;

                if (!string.IsNullOrEmpty(searchString))
                {
                    tecnicos = await _serviceTecnico.SearchAsync(searchString);
                    ViewData["CurrentFilter"] = searchString; // Mantener el término de búsqueda
                }
                else
                {
                    tecnicos = await _serviceTecnico.ListAsync();
                }

                var pagedList = tecnicos.ToPagedList(pageNumber, PageSize);
                return View(pagedList);
            }
            catch (Exception ex)
            {
                // Manejar el error apropiadamente
                ModelState.AddModelError("", "Error al cargar los técnicos.");
                return View(new PagedList<TecnicoDTO>(new List<TecnicoDTO>(), 1, 1));
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var tecnico = await _serviceTecnico.FindByIdAsync(id);
            if (tecnico == null)
            {
                return NotFound();
            }
            return View(tecnico);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tecnico = await _serviceTecnico.FindByIdAsync(id.Value);
            if (tecnico == null)
            {
                return NotFound();
            }

            // Cargar las especialidades disponibles
            ViewBag.Especialidades = await _especialidadService.ListAsync();

            return View(tecnico);
        }

        // POST: Tecnico/Edit/IdTecnico
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTecnico,Alias,Disponible,NivelCarga, Usuario.Nombre, Usuario.Email")] TecnicoDTO tecnico, string[] EspecialidadesSeleccionadas)
        {
            if (id != tecnico.IdTecnico)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceTecnico.UpdateAsync(id, tecnico, EspecialidadesSeleccionadas ?? Array.Empty<string>()); ;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al actualizar el técnico. Por favor, intente nuevamente.");
                }
            }
            // Si llegamos aquí, algo falló, recargar las especialidades
            tecnico = await _serviceTecnico.FindByIdAsync(id);
            return View(tecnico);
        }

        // GET: Tecnico/Create
        public async Task<IActionResult> Create()
        {
            // Cargar las especialidades disponibles
            ViewBag.Especialidades = await _especialidadService.ListAsync();
            return View(new TecnicoDTO
            {
                Usuario = new UsuarioDTO(),
                Disponible = true,
                NivelCarga = 0
            });
        }

        // POST: Tecnico/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Alias,Disponible,NivelCarga,Usuario")] TecnicoDTO tecnico, string[] EspecialidadesSeleccionadas)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Validar que el usuario y su email no sean nulos
                    if (tecnico.Usuario == null || string.IsNullOrWhiteSpace(tecnico.Usuario.Email))
                    {
                        ModelState.AddModelError("", "El correo electrónico es requerido");
                        ViewBag.Especialidades = await _especialidadService.ListAsync();
                        return View(tecnico);
                    }

                    // Verificar si ya existe un usuario con ese email
                    var emailExiste = await _serviceTecnico.ExisteEmailAsync(tecnico.Usuario.Email);
                    if (emailExiste)
                    {
                        ModelState.AddModelError("Usuario.Email", "Ya existe un usuario con este correo electrónico");
                        ViewBag.Especialidades = await _especialidadService.ListAsync();
                        return View(tecnico);
                    }

                    var idTecnico = await _serviceTecnico.AddAsync(tecnico);

                    if (EspecialidadesSeleccionadas != null && EspecialidadesSeleccionadas.Length > 0)
                    {
                        await _serviceTecnico.UpdateAsync(idTecnico, tecnico, EspecialidadesSeleccionadas);
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al crear el técnico: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        ModelState.AddModelError("", $"Detalles: {ex.InnerException.Message}");
                    }
                }
            }

            // Si llegamos aquí, algo falló, recargar las especialidades
            ViewBag.Especialidades = await _especialidadService.ListAsync();
            return View(tecnico);
        }

        //Metodo para eliminar un técnico
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _serviceTecnico.DeleteAsync(id);
                if (!result)
                {
                    TempData["Error"] = "No se pudo eliminar el técnico o no se encontró.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Success"] = "Técnico eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the error
                TempData["Error"] = "Ocurrió un error al intentar eliminar el técnico.";
                return RedirectToAction(nameof(Index));
            }
        }


    }
}