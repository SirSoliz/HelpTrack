using AutoMapper;
using HelpTrack.Application.DTOs;
using HelpTrack.Application.Services.Interfaces;
using HelpTrack.Infraestructure.Models;
using HelpTrack.Infraestructure.Repository.Implementations;
using HelpTrack.Infraestructure.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using X.PagedList.Extensions;
using Microsoft.Extensions.Localization;
using HelpTrack.Application.Resources;
using HelpTrack.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace HelpTrack.Web.Controllers
{
    [Authorize]
    [RequireAdmin]
    public class CategoriaController : Controller
    {
        private readonly IServiceCategoria _serviceCategoria;
        private readonly IRepositorySla _repositorySla;
        private readonly IRepositoryEtiqueta _repositoryEtiqueta;
        private readonly IRepositoryEspecialidad _repositoryEspecialidad;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;



        private const int PageSize = 10;

        public CategoriaController(IServiceCategoria serviceCategoria, IRepositorySla repositorySla, 
           IRepositoryEtiqueta repositoryEtiqueta, IRepositoryEspecialidad repositoryEspecialidad, IMapper mapper, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _serviceCategoria = serviceCategoria ?? throw new ArgumentNullException(nameof(serviceCategoria));
            _repositorySla = repositorySla ?? throw new ArgumentNullException(nameof(repositorySla));
            _repositoryEtiqueta = repositoryEtiqueta;
            _repositoryEspecialidad = repositoryEspecialidad;
            _mapper = mapper;
            _sharedLocalizer = sharedLocalizer;
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
                ModelState.AddModelError("", _sharedLocalizer["ErrorLoadingCategories"]);
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
                    ModelState.AddModelError("", _sharedLocalizer["ErrorUpdatingCategory"]);
                }
            }
            return View(categoriaDTO);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Obtener la lista de SLAs
            var slas = await _repositorySla.ListAsync();
            var etiquetas = await _repositoryEtiqueta.ListAsync();
            var especialidades = await _repositoryEspecialidad.ListAsync();

            ViewBag.SLAs = new SelectList(slas, "IdSla", "Nombre");
            ViewBag.Etiquetas = new MultiSelectList(etiquetas, "IdEtiqueta", "Nombre");
            ViewBag.Especialidades = new MultiSelectList(especialidades, "IdEspecialidad", "Nombre");

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
                    // Mapear las etiquetas y especialidades seleccionadas
                    if (categoriaDTO.EtiquetasSeleccionadas != null && categoriaDTO.EtiquetasSeleccionadas.Any())
                    {
                        var etiquetas = await _repositoryEtiqueta
                            .FindByIdsAsync(categoriaDTO.EtiquetasSeleccionadas);
                        categoriaDTO.Etiquetas = _mapper.Map<ICollection<EtiquetaDTO>>(etiquetas);
                    }

                    if (categoriaDTO.EspecialidadesSeleccionadas != null && categoriaDTO.EspecialidadesSeleccionadas.Any())
                    {
                        var especialidades = await _repositoryEspecialidad
                            .FindByIdsAsync(categoriaDTO.EspecialidadesSeleccionadas);
                        categoriaDTO.Especialidades = _mapper.Map<ICollection<EspecialidadDTO>>(especialidades);
                    }

                    await _serviceCategoria.AddAsync(categoriaDTO);
                    TempData["SuccessMessage"] = _sharedLocalizer["CategoryCreatedSuccess"].Value;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    await CargarListasDesplegables();
                    ModelState.AddModelError("", string.Format(_sharedLocalizer["ErrorCreatingCategory"], ex.Message));
                }
            }
            else
            {
                await CargarListasDesplegables();
            }
            return View(categoriaDTO);
        }

        private async Task CargarListasDesplegables()
        {
            var slas = await _repositorySla.ListAsync();
            var etiquetas = await _repositoryEtiqueta.ListAsync();
            var especialidades = await _repositoryEspecialidad.ListAsync();

            ViewBag.SLAs = new SelectList(slas, "IdSla", "Nombre");
            ViewBag.Etiquetas = new MultiSelectList(etiquetas, "IdEtiqueta", "Nombre");
            ViewBag.Especialidades = new MultiSelectList(especialidades, "IdEspecialidad", "Nombre");
        }
        public IActionResult Delete(int? id) => RedirectToAction(nameof(Index));
    }
}