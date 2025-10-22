using HelpTrack.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace HelpTrack.Web.Controllers
{
    public class AutorController : Controller
    {
        private readonly IServiceTecnico _serviceAutor;
        public AutorController(IServiceTecnico serviceAutor)
        {
            _serviceAutor = serviceAutor;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var collection = await _serviceAutor.ListAsync();
            return View(collection);
        }
    }
}