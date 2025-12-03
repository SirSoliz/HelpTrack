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
        private readonly IServiceTecnico _serviceTecnico;
        private readonly ILogger<TicketController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;
        private const int PageSize = 10;

        public TicketController(
            IServiceTicket serviceTicket,
            IServiceEstadoTicket serviceEstadoTicket,
            IServicePrioridades servicePrioridades,
            IServiceCategoria serviceCategoria,
            IServiceTecnico serviceTecnico,
            ILogger<TicketController> logger,
            IWebHostEnvironment hostEnvironment)
        {
            _serviceTicket = serviceTicket;
            _serviceEstadoTicket = serviceEstadoTicket;
            _servicePrioridades = servicePrioridades;
            _serviceCategoria = serviceCategoria;
            _serviceTecnico = serviceTecnico;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        // Método auxiliar para cargar prioridades, categorías y estados en ViewBag
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

        // Metodo de ayuda para guardar imagenes 
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
                        IdUsuario = 1
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
                    // Establecer usuario predeterminado si no se proporciona
                    if (ticketDTO.IdUsuarioCreacion == 0) ticketDTO.IdUsuarioCreacion = 1;

                    // Manejar imagenes 
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
                    // Manejar nuevas imágenes
                    if (ticketDTO.NuevasImagenes != null && ticketDTO.NuevasImagenes.Any())
                    {
                        var imagenes = await SaveImagesAsync(ticketDTO.NuevasImagenes);
                        //Necesitamos agregarlos al ticket existente.
                        //Dado que UpdateAsync podría reemplazar la colección o fusionarla, debemos asegurarnos de que el servicio gestione la adición de nuevos elementos a la colección.
                        //Por ahora, los agregamos a la colección del DTO.
                        foreach (var img in imagenes)
                        {
                            img.IdTicket = id;
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

        // GET: Ticket/History
        [HttpGet]
        public async Task<IActionResult> History()
        {
            try
            {
                var history = await _serviceTicket.GetHistoryAsync();
                return View(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar el historial de tickets");
                return View(new List<TicketHistoryDTO>());
            }
        }

        // GET: Ticket/HistoryDetails/5
        [HttpGet]
        public async Task<IActionResult> HistoryDetails(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                // Obtener información general del ticket (reutilizando el servicio existente)
                var allHistory = await _serviceTicket.GetHistoryAsync();
                var ticketInfo = allHistory.FirstOrDefault(t => t.IdTicket == id.Value);

                if (ticketInfo == null) return NotFound();

                // Obtener el log de cambios del ticket
                var historyLog = await _serviceTicket.GetTicketHistoryLogAsync(id.Value);

                var model = new HelpTrack.Web.ViewModels.TicketHistoryDetailsViewModel
                {
                    TicketInfo = ticketInfo,
                    HistoryLog = historyLog
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar los detalles del historial del ticket");
                return NotFound();
            }
        }

        // GET: Ticket/Assign/5
        [HttpGet]
        public async Task<IActionResult> Assign(int? id)
        {
            if (id == null) return NotFound();

            var ticket = await _serviceTicket.FindByIdAsync(id.Value);
            if (ticket == null) return NotFound();

            var tecnicos = await _serviceTecnico.ListAsync();
            var prioridades = await _servicePrioridades.ListAsync();
            
            var model = new AsignacionTicketDTO
            {
                IdTicket = ticket.IdTicket,
                Ticket = ticket,
                Prioridad = ticket.IdPrioridad,
                TecnicosDisponibles = new SelectList(tecnicos.Select(t => new
                {
                    IdTecnico = t.IdTecnico,
                    NombreCompleto = $"{t.Usuario?.Nombre} ({t.Alias})"
                }), "IdTecnico", "NombreCompleto"),
                PrioridadesDisponibles = new SelectList(prioridades, "IdPrioridad", "Nombre", ticket.IdPrioridad)
            };

            return View(model);
        }

        // POST: Ticket/Assign
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(AsignacionTicketDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceTicket.AssignAsync(model);
                    TempData["SuccessMessage"] = "Ticket asignado exitosamente";
                    return RedirectToAction(nameof(Details), new { id = model.IdTicket });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al asignar el ticket");
                    var innerMessage = ex.InnerException != null ? ex.InnerException.Message : "";
                    ModelState.AddModelError("", $"Error al asignar el ticket: {ex.Message} -> {innerMessage}");
                }
            }

            // Recargar datos si falla la validación
            var ticket = await _serviceTicket.FindByIdAsync(model.IdTicket);
            model.Ticket = ticket;
            var tecnicos = await _serviceTecnico.ListAsync();
            var prioridades = await _servicePrioridades.ListAsync();
            
            model.TecnicosDisponibles = new SelectList(tecnicos.Select(t => new
            {
                IdTecnico = t.IdTecnico,
                NombreCompleto = $"{t.Usuario?.Nombre} ({t.Alias})"
            }), "IdTecnico", "NombreCompleto");
            model.PrioridadesDisponibles = new SelectList(prioridades, "IdPrioridad", "Nombre");

            return View(model);

        }
    }
}