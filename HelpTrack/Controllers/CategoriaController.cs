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
                    ModelState.AddModelError("", $"Error al crear la categoría: {ex.Message}");
                }
            }
            // Si hay error, vuelve a mostrar el formulario con los datos ingresados
            return View(categoriaDTO);
        }
        public IActionResult Delete(int? id) => RedirectToAction(nameof(Index));
    }
}