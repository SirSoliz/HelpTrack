using HelpTrack.Application.DTOs;
using HelpTrack.Application.Services.Implementations;
using HelpTrack.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using X.PagedList;
using X.PagedList.Extensions;

namespace HelpTrack.Web.Controllers
{
    public class TicketController : Controller
    {
        private readonly IServiceTicket _serviceTicket;
        private readonly IServiceEstadoTicket _serviceEstadoTicket;
        private readonly IServicePrioridades _servicePrioridades;
        private const int PageSize = 10;
        public TicketController(IServiceTicket serviceTicket, IServiceEstadoTicket serviceEstadoTicket, IServicePrioridades servicePrioridades)
        {
            _serviceTicket = serviceTicket;
            _serviceEstadoTicket = serviceEstadoTicket;
            _servicePrioridades = servicePrioridades;
        }
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
                        .Where(c => c.Titulo.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                  (c.Descripcion != null &&
                                   c.Descripcion.Contains(searchString, StringComparison.OrdinalIgnoreCase)))
                        .ToList();
                }

                var pagedList = tickets.ToPagedList(pageNumber, PageSize);
                return View(pagedList);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error al cargar los tickets.");
                return View(new PagedList<TicketDTO>(new List<TicketDTO>(), 1, 1));
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var ticketDTO = await _serviceTicket.FindByIdAsync(id);
            if (ticketDTO == null) return NotFound();

            var prioridades = await _servicePrioridades.ListAsync();
            ViewBag.Prioridades = prioridades
                .Select(p => new SelectListItem
                {
                    Value = p.IdPrioridad.ToString(),
                    Text = p.Nombre,
                    Selected = p.IdPrioridad == ticketDTO.IdPrioridad
                })
                .ToList();

            // Mensaje de notificación (si querés mantenerlo)
            ViewBag.NotificationMessage = HelpTrackWeb.Web.Util.SweetAlertHelper.Mensaje("Exito",
                "Se ha cargado la información del ticket " + id + ".",
                HelpTrackWeb.Web.Util.SweetAlertMessageType.info);

            return View(ticketDTO);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ticketDTO = await _serviceTicket.FindByIdAsync(id.Value);
            if (ticketDTO == null) return NotFound();

            var prioridades = await _servicePrioridades.ListAsync();
            ViewBag.Prioridades = prioridades
                .Select(p => new SelectListItem
                {
                    Value = p.IdPrioridad.ToString(),
                    Text = p.Nombre,
                    Selected = p.IdPrioridad == ticketDTO.IdPrioridad
                })
                .ToList();

            return View(ticketDTO);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TicketDTO ticketDTO)  // Cambiado de short a int
        {
            if (id != ticketDTO.IdTicket) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceTicket.UpdateAsync(id, ticketDTO);
                    return RedirectToAction(nameof(Details), new { id = ticketDTO.IdTicket });
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Error al actualizar el ticket.");
                }
            }

            var prioridades = await _servicePrioridades.ListAsync();
            ViewBag.Prioridades = prioridades
                .Select(p => new SelectListItem
                {
                    Value = p.IdPrioridad.ToString(),
                    Text = p.Nombre,
                    Selected = p.IdPrioridad == ticketDTO.IdPrioridad
                })
                .ToList();

            return View(ticketDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var ticketDTO = new TicketDTO();

            var prioridades = await _servicePrioridades.ListAsync();
            ViewBag.Prioridades = prioridades
                .Select(p => new SelectListItem
                {
                    Value = p.IdPrioridad.ToString(),
                    Text = p.Nombre
                })
                .ToList();

            return View(ticketDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketDTO ticketDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceTicket.AddAsync(ticketDTO);
                    TempData["SuccessMessage"] = "Ticket creado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al crear el ticket: {ex.Message}");
                }
            }

            var prioridades = await _servicePrioridades.ListAsync();
            ViewBag.Prioridades = prioridades
                .Select(p => new SelectListItem
                {
                    Value = p.IdPrioridad.ToString(),
                    Text = p.Nombre
                })
                .ToList();

            return View(ticketDTO);
        }
    }
}