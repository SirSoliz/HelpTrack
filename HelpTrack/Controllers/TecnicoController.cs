using HelpTrack.Application.DTOs;
using HelpTrack.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;
using Microsoft.Extensions.Localization;
using HelpTrack.Resources;

namespace HelpTrack.Web.Controllers
{
    public class TecnicoController : Controller
    {
        private readonly IServiceTecnico _serviceTecnico;
        private readonly IServiceEspecialidad _especialidadService;  
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private const int PageSize = 10; // Número de elementos por página

        public TecnicoController(IServiceTecnico serviceTecnico, IServiceEspecialidad especialidadService, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _serviceTecnico = serviceTecnico;
            _especialidadService = especialidadService;
            _sharedLocalizer = sharedLocalizer;

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
                ModelState.AddModelError("", _sharedLocalizer["ErrorLoadingTechnicians"]);
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
        public async Task<IActionResult> Edit(int id, [Bind("IdTecnico,Alias,Disponible,NivelCarga,Usuario")] TecnicoDTO tecnico, string[] EspecialidadesSeleccionadas)
        {
            if (id != tecnico.IdTecnico)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the current technician to preserve any user data that's not in the form
                    var existingTecnico = await _serviceTecnico.FindByIdAsync(id);
                    if (existingTecnico == null)
                    {
                        return NotFound();
                    }

                    // Update the user data
                    if (tecnico.Usuario != null)
                    {
                        // Preserve the user ID
                        tecnico.Usuario.IdUsuario = existingTecnico.Usuario.IdUsuario;

                        // Update the user data in the database
                        await _serviceTecnico.UpdateAsync(id, tecnico, EspecialidadesSeleccionadas ?? Array.Empty<string>());
                    }
                    else
                    {
                        // If no user data was provided, just update the technician data
                        await _serviceTecnico.UpdateAsync(id, tecnico, EspecialidadesSeleccionadas ?? Array.Empty<string>());
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", _sharedLocalizer["ErrorUpdatingTechnician"]);
                    // Log the error
                    Console.WriteLine($"Error al actualizar técnico: {ex.Message}");
                }
            }

            // If we got this far, something failed, redisplay form with errors
            ViewBag.Especialidades = await _especialidadService.ListAsync();
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
                        ModelState.AddModelError("", _sharedLocalizer["EmailRequired"]);
                        ViewBag.Especialidades = await _especialidadService.ListAsync();
                        return View(tecnico);
                    }

                    // Verificar si ya existe un usuario con ese email
                    var emailExiste = await _serviceTecnico.ExisteEmailAsync(tecnico.Usuario.Email);
                    if (emailExiste)
                    {
                        ModelState.AddModelError("Usuario.Email", _sharedLocalizer["EmailExists"]);
                        ViewBag.Especialidades = await _especialidadService.ListAsync();
                        return View(tecnico);
                    }


                    var idTecnico = await _serviceTecnico.AddAsync(tecnico);

                    if (EspecialidadesSeleccionadas != null && EspecialidadesSeleccionadas.Length > 0)
                    {
                        await _serviceTecnico.UpdateAsync(idTecnico, tecnico, EspecialidadesSeleccionadas);
                    }

                    // Agregar mensaje de éxito
                    TempData["SuccessMessage"] = _sharedLocalizer["TechnicianCreatedSuccess"].Value;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", string.Format(_sharedLocalizer["ErrorCreatingTechnician"], ex.Message));
                    if (ex.InnerException != null)
                    {
                        ModelState.AddModelError("", string.Format(_sharedLocalizer["ErrorDetails"], ex.InnerException.Message));
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
                    TempData["Error"] = _sharedLocalizer["ErrorDeletingTechnicianNotFound"].Value;
                    return RedirectToAction(nameof(Index));
                }

                TempData["Success"] = _sharedLocalizer["TechnicianDeletedSuccess"].Value;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the error
                TempData["Error"] = _sharedLocalizer["ErrorDeletingTechnician"].Value;
                return RedirectToAction(nameof(Index));
            }
        }


    }
}