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
        private const int PageSize = 10; // Número de elementos por página

        public TecnicoController(IServiceTecnico serviceTecnico)
        {
            _serviceTecnico = serviceTecnico;
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
    }
}