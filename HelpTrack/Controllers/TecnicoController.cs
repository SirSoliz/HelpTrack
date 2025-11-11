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

    }
}