using Microsoft.AspNetCore.Mvc;
using Projeto.Interfaces;

namespace Projeto.Controllers
{
    public class SugestaoController : Controller
    {
        private readonly ISugestaoService _service;

        public SugestaoController(ISugestaoService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public void Votar(int PostId)
        {
            int.TryParse(HttpContext.Session.GetString("Id"), out int UsuarioId);
            _service.Votar(UsuarioId, PostId);
        }
    }
}