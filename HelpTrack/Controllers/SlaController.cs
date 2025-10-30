using HelpTrack.Application.DTOs;
using HelpTrack.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;

namespace HelpTrackWeb.Controllers
{
    public class SlaController : Controller
    {
        private readonly IServiceSla _serviceSla;
        private const int PageSize = 10;
        public SlaController(IServiceSla serviceSla)
        {
            _serviceSla = serviceSla;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int? page, string searchString)
        {
            try
            {
                int pageNumber = page ?? 1;
                var slas = await _serviceSla.ListAsync();

                if (!string.IsNullOrEmpty(searchString))
                {
                    slas = slas
                        .Where(c => c.Nombre.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                  (c.Nombre != null &&
                                   c.Nombre.Contains(searchString, StringComparison.OrdinalIgnoreCase)))
                        .ToList();
                }

                var pagedList = slas.ToPagedList(pageNumber, PageSize);
                return View(pagedList);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error al cargar las categorías.");
                return View(new PagedList<SlaDTO>(new List<SlaDTO>(), 1, 1));
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var @object = await _serviceSla.FindByIdAsync(id);
            ViewBag.NotificationMessage = HelpTrackWeb.Web.Util.SweetAlertHelper.Mensaje("Exito",
                "Se ha cargado la informacion del autor " + id + ".",
                HelpTrackWeb.Web.Util.SweetAlertMessageType.info);
            return View(@object);
        }
    }
}

