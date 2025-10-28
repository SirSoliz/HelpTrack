using HelpTrack.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;

namespace HelpTrack.Web.Controllers
{
    public class TecnicoController : Controller
    {
        private readonly IServiceTecnico _serviceTecnico;
        private const int PageSize = 10; // Número de elementos por página

        public TecnicoController(IServiceTecnico serviceTecnico)
        {
            _serviceTecnico = serviceTecnico;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            int pageNumber = page ?? 1;
            var collection = await _serviceTecnico.ListAsync();
            var pagedList = collection.ToPagedList(pageNumber, PageSize);
            return View(pagedList);
        }

        public async Task<IActionResult> Details(int id)
        {
            var @object = await _serviceTecnico.FindByIdAsync(id);
            ViewBag.NotificationMessage = HelpTrackWeb.Web.Util.SweetAlertHelper.Mensaje("Exito",
                "Se ha cargado la informacion del técnico " + id + ".",
                HelpTrackWeb.Web.Util.SweetAlertMessageType.info);
            return View(@object);
        }
    }
}