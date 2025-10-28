using HelpTrack.Application.DTOs;
using HelpTrack.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
        private const int PageSize = 10;

        public CategoriaController(IServiceCategoria serviceCategoria)
        {
            _serviceCategoria = serviceCategoria ??
                throw new ArgumentNullException(nameof(serviceCategoria));
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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var categoria = await _serviceCategoria.FindByIdAsync(id.Value);
                return categoria == null ? NotFound() : View(categoria);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error al cargar los detalles de la categoría.";
                return RedirectToAction(nameof(Index));
            }
        }

        // Métodos sin implementación (solo para mostrar los botones)
        public IActionResult Edit(int? id) => RedirectToAction(nameof(Index));
        public IActionResult Delete(int? id) => RedirectToAction(nameof(Index));
    }
}