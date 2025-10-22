using HelpTrack.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace HelpTrack.Web.Controllers
{
    public class TecnicoController : Controller
    {
        private readonly IServiceTecnico _serviceTecnico;
        public TecnicoController(IServiceTecnico serviceTecnico)
        {
            _serviceTecnico = serviceTecnico;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var collection = await _serviceTecnico.ListAsync();
            return View(collection);
        }

        public async Task<IActionResult> Details(int id)
        {
            var @object = await _serviceTecnico.FindByIdAsync(id);
            ViewBag.NotificationMessage = HelpTrackWeb.Web.Util.SweetAlertHelper.Mensaje("Exito",
                "Se ha cargado la informacion del autor " + id + ".",
                HelpTrackWeb.Web.Util.SweetAlertMessageType.info);
            return View(@object);
        }
    }
}