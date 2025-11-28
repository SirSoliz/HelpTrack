using HelpTrack.Application.DTOs;
using HelpTrack.Application.Services.Interfaces;
using HelpTrack.Infraestructure.Repository.Implementations;
using HelpTrack.Infraestructure.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using X.PagedList.Extensions;

namespace HelpTrack.Web.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly IServiceCategoria _serviceCategoria;
        private readonly IRepositorySla _repositorySla;

        private const int PageSize = 10;

        public CategoriaController(IServiceCategoria serviceCategoria, IRepositorySla repositorySla)
        {
            _serviceCategoria = serviceCategoria ?? throw new ArgumentNullException(nameof(serviceCategoria));
            _repositorySla = repositorySla ?? throw new ArgumentNullException(nameof(repositorySla));

        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page, string searchString)
        {
            try
            {
                int pageNumber = page ?? 1;
                var categorias = await _serviceCategoria.ListAsync();

                if (!string.IsNullOrEmpty(searchString))
                {
                    categorias = categorias
                        .Where(c => c.Nombre.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                  (c.Descripcion != null &&
                                   c.Descripcion.Contains(searchString, StringComparison.OrdinalIgnoreCase)))
                        .ToList();
                }

                var pagedList = categorias.ToPagedList(pageNumber, PageSize);
                return View(pagedList);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error al cargar las categorías.");
                return View(new PagedList<CategoriaDTO>(new List<CategoriaDTO>(), 1, 1));
            }
        }

        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _serviceCategoria.GetByIdWithDetailsAsync(id.Value);

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)  // Cambiado de short? a int?
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _serviceCategoria.GetByIdWithDetailsAsync(id.Value);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoriaDTO categoriaDTO)  // Cambiado de short a int
        {
            if (id != categoriaDTO.IdCategoria)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceCategoria.UpdateAsync(id, categoriaDTO);
                    return RedirectToAction(nameof(Details), new { id = categoriaDTO.IdCategoria });
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocurrió un error al actualizar la categoría.");
                }
            }
            return View(categoriaDTO);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Obtener la lista de SLAs
            var slas = await _repositorySla.ListAsync();
            ViewBag.SLAs = new SelectList(slas, "IdSla", "Nombre");
            return View(new CategoriaDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoriaDTO categoriaDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceCategoria.AddAsync(categoriaDTO);
                    TempData["SuccessMessage"] = "Categoría creada exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    var slas = await _repositorySla.ListAsync();
                    ViewBag.SLAs = new SelectList(slas, "IdSla", "Nombre", categoriaDTO.IdSla);
                    ModelState.AddModelError("", $"Error al crear la categoría: {ex.Message}");
                }
            }
            return View(categoriaDTO);
        }
        public IActionResult Delete(int? id) => RedirectToAction(nameof(Index));
    }
}