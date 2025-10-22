using HelpTrack.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpTrackWeb.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly IServiceCategoria _serviceCategoria;
        public CategoriaController(IServiceCategoria serviceCategoria)
        {
            _serviceCategoria = serviceCategoria;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var collection = await _serviceCategoria.ListAsync();
            return View(collection);
        }

        public async Task<IActionResult> Details(int id)
        {
            var @object = await _serviceCategoria.FindByIdAsync(id);
            ViewBag.NotificationMessage = HelpTrackWeb.Web.Util.SweetAlertHelper.Mensaje("Exito",
                "Se ha cargado la informacion del autor " + id + ".",
                HelpTrackWeb.Web.Util.SweetAlertMessageType.info);
            return View(@object);
        }

        // GET: CategoriaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoriaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategoriaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoriaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategoriaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
