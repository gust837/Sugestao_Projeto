using Microsoft.AspNetCore.Mvc;
using Projeto.Interfaces;
using Projeto.Models;

namespace Projeto.Controllers
{
    public class SugestaoController : Controller
    {
        private readonly ISugestaoService _service;

        public SugestaoController(ISugestaoService service)
        {
            _service = service;
        }

        public bool VerificarSessaoFalse()
        {
            return HttpContext.Session.GetString("UsuarioId") == null;
        }

        public bool VerificarSessaoAdminFalse()
        {
            return HttpContext.Session.GetString("Admin") != "true";
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (VerificarSessaoFalse()) return RedirectToAction("Index", "Login");
            return View(await _service.ListarSugestoes());
        }

        [HttpGet]
        public IActionResult NovaSugestao()
        {
            if (VerificarSessaoFalse()) return RedirectToAction("Index", "Login");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NovaSugestao(Sugestao s, string? categorias, IFormFile arquivoImagem)
        {
            if (VerificarSessaoFalse()) return RedirectToAction("Index", "Login");

            int.TryParse(HttpContext.Session.GetString("UsuarioId"), out int usuarioId);
            s.UsuarioId = usuarioId;
            s.DataStatus = DateTime.Today;
            s.DataSugestao = DateTime.Today;

            await _service.CriarSugestao(s, categorias, arquivoImagem);
            return RedirectToAction("Index", "Sugestao");
        }

        [HttpPost]
        public async Task<IActionResult> EditarSugestao(int id, string status)
        {
            if (VerificarSessaoAdminFalse()) return RedirectToAction("Index", "Login");

            await _service.EditarStatusSugestao(id, status);

            return RedirectToAction("Index", "Sugestao");
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirSugestao(int id)
        {
            if (VerificarSessaoAdminFalse()) return RedirectToAction("Index", "Login");

            await _service.ExcluirSugestao(id);

            return RedirectToAction("Index", "Sugestao");
        }

        [HttpPost]
        public async Task<IActionResult> Votar(int postId)
        {
            int.TryParse(HttpContext.Session.GetString("UsuarioId"), out int usuarioId);
            await _service.Votar(usuarioId, postId);
            return RedirectToAction("Index", "Sugestao");
        }
    }
}