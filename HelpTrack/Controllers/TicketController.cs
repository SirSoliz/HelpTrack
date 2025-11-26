using HelpTrack.Application.DTOs;
using HelpTrack.Application.Services.Interfaces;
using HelpTrack.Infraestructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using X.PagedList;
using X.PagedList.Extensions;

namespace HelpTrack.Web.Controllers
{
    public class TicketController : Controller
    {
        private readonly IServiceTicket _serviceTicket;
        private readonly IServiceEstadoTicket _serviceEstadoTicket;
        private readonly IServicePrioridades _servicePrioridades;
        private readonly IServiceCategoria _serviceCategoria;
        private readonly ILogger<TicketController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;
        private const int PageSize = 10;

        public TicketController(
            IServiceTicket serviceTicket,
            IServiceEstadoTicket serviceEstadoTicket,
            IServicePrioridades servicePrioridades,
            IServiceCategoria serviceCategoria,
            ILogger<TicketController> logger,
            IWebHostEnvironment hostEnvironment)
        {
            _serviceTicket = serviceTicket;
            _serviceEstadoTicket = serviceEstadoTicket;
            _servicePrioridades = servicePrioridades;
            _serviceCategoria = serviceCategoria;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        // Helper method to load priorities, categories, and states into ViewBag
        private async Task LoadFormDataAsync(int? selectedPrioridadId = null, int? selectedCategoriaId = null, int? selectedEstadoId = null)
        {
            var prioridades = await _servicePrioridades.ListAsync();
            ViewBag.Prioridades = prioridades
                .Select(p => new SelectListItem
                {
                    Value = p.IdPrioridad.ToString(),
                    Text = p.Nombre,
                    Selected = selectedPrioridadId.HasValue && p.IdPrioridad == selectedPrioridadId.Value
                })
                .ToList();

            var categorias = await _serviceCategoria.ListAsync();
            ViewBag.Categorias = categorias
                .Select(c => new SelectListItem
                {
                    Value = c.IdCategoria.ToString(),
                    Text = c.Nombre,
                    Selected = selectedCategoriaId.HasValue && c.IdCategoria == selectedCategoriaId.Value
                })
                .ToList();

            var estados = await _serviceEstadoTicket.ListAsync();
            ViewBag.Estados = estados
                .Select(e => new SelectListItem
                {
                    Value = e.IdEstado.ToString(),
                    Text = e.Nombre,
                    Selected = selectedEstadoId.HasValue && e.IdEstado == selectedEstadoId.Value
                })
                .ToList();
        }

        // Helper method to save images
        private async Task<List<ImagenesTicket>> SaveImagesAsync(List<IFormFile>? images)
        {
            var savedImages = new List<ImagenesTicket>();
            if (images == null || !images.Any()) return savedImages;

            string wwwRootPath = _hostEnvironment.WebRootPath;
            string uploadPath = Path.Combine(wwwRootPath, "imagenes", "tickets");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            foreach (var file in images)
            {
                if (file.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(uploadPath, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    savedImages.Add(new ImagenesTicket
                    {
                        NombreArchivo = file.FileName,
                        TipoContenido = file.ContentType,
                        UrlArchivo = $"/imagenes/tickets/{fileName}",
                        FechaCreacion = DateTime.Now,
                        IdUsuario = 1 // Default user, replace with logged in user
                    });
                }
            }

            return savedImages;
        }

        // GET: Ticket/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ticketDTO = await _serviceTicket.FindByIdAsync(id.Value);
            if (ticketDTO == null) return NotFound();

            await LoadFormDataAsync(ticketDTO.IdPrioridad, ticketDTO.IdCategoria, ticketDTO.IdEstadoActual);
            return View(ticketDTO);
        }

        // GET: Ticket/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var ticketDTO = new TicketDTO();
            await LoadFormDataAsync();
            return View(ticketDTO);
        }

        // POST: Ticket/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketDTO ticketDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Set default user if not provided
                    if (ticketDTO.IdUsuarioCreacion == 0) ticketDTO.IdUsuarioCreacion = 1;

                    // Handle Images
                    if (ticketDTO.NuevasImagenes != null && ticketDTO.NuevasImagenes.Any())
                    {
                        var imagenes = await SaveImagesAsync(ticketDTO.NuevasImagenes);
                        foreach (var img in imagenes)
                        {
                            ticketDTO.ImagenesTicket.Add(img);
                        }
                    }

                    await _serviceTicket.AddAsync(ticketDTO);
                    TempData["SuccessMessage"] = "Ticket creado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al crear el ticket");
                    var innerMessage = ex.InnerException != null ? ex.InnerException.Message : "";
                    ModelState.AddModelError("", $"Error al crear el ticket: {ex.Message} -> {innerMessage}");
                }
            }

            await LoadFormDataAsync(ticketDTO?.IdPrioridad, ticketDTO?.IdCategoria, ticketDTO?.IdEstadoActual);
            return View(ticketDTO);
        }

        // GET: Ticket/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ticketDTO = await _serviceTicket.FindByIdAsync(id.Value);
            if (ticketDTO == null) return NotFound();

            await LoadFormDataAsync(ticketDTO.IdPrioridad, ticketDTO.IdCategoria, ticketDTO.IdEstadoActual);
            return View(ticketDTO);
        }

        // POST: Ticket/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TicketDTO ticketDTO)
        {
            if (id != ticketDTO.IdTicket) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle New Images
                    if (ticketDTO.NuevasImagenes != null && ticketDTO.NuevasImagenes.Any())
                    {
                        var imagenes = await SaveImagesAsync(ticketDTO.NuevasImagenes);
                        // We need to add these to the existing ticket. 
                        // Since UpdateAsync might replace the collection or merge, 
                        // we should ensure the service handles adding new items to the collection.
                        // For now, we add them to the DTO's collection.
                        foreach (var img in imagenes)
                        {
                            img.IdTicket = id; // Ensure ID is set
                            ticketDTO.ImagenesTicket.Add(img);
                        }
                    }

                    await _serviceTicket.UpdateAsync(id, ticketDTO);
                    return RedirectToAction(nameof(Details), new { id = ticketDTO.IdTicket });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al actualizar el ticket");
                    var innerMessage = ex.InnerException != null ? ex.InnerException.Message : "";
                    ModelState.AddModelError("", $"Error al actualizar el ticket: {ex.Message} -> {innerMessage}");
                }
            }

            await LoadFormDataAsync(ticketDTO.IdPrioridad, ticketDTO.IdCategoria, ticketDTO.IdEstadoActual);
            return View(ticketDTO);
        }

        // GET: Ticket/Index
        [HttpGet]
        public async Task<IActionResult> Index(int? page, string searchString)
        {
            try
            {
                int pageNumber = page ?? 1;
                var tickets = await _serviceTicket.ListAsync();

                if (!string.IsNullOrEmpty(searchString))
                {
                    tickets = tickets
                        .Where(t => t.Titulo.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                    (t.Descripcion != null && t.Descripcion.Contains(searchString, StringComparison.OrdinalIgnoreCase)))
                        .ToList();
                }

                var pagedList = tickets.ToPagedList(pageNumber, PageSize);
                return View(pagedList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar la lista de tickets");
                ModelState.AddModelError("", "Error al cargar los tickets.");
                return View(new PagedList<TicketDTO>(new List<TicketDTO>(), 1, 1));
            }
        }
    }
}