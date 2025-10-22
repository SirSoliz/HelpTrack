using HelpTrack.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace HelpTrack.Web.Controllers
{
    public class TicketController : Controller
    {
        private readonly IServiceTicket _serviceTicket;
        public TicketController(IServiceTicket serviceTicket)
        {
            _serviceTicket = serviceTicket;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var collection = await _serviceTicket.ListAsync();
            return View(collection);
        }

        public async Task<IActionResult> Details(int id)
        {
            var @object = await _serviceTicket.FindByIdAsync(id);
            ViewBag.NotificationMessage = HelpTrackWeb.Web.Util.SweetAlertHelper.Mensaje("Exito",
                "Se ha cargado la informacion del autor " + id + ".",
                HelpTrackWeb.Web.Util.SweetAlertMessageType.info);
            return View(@object);
        }
    }
}