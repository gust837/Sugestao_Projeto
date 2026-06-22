using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Projeto.Interfaces;
using Projeto.Models;

namespace Projeto.Controllers
{
    public class FeedController : Controller
    {
        private readonly ISugestaoService _service;

        public FeedController(ISugestaoService service)
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
        public async Task<IActionResult> Index(string? status, string? lugar)
        {
            if (VerificarSessaoFalse()) return RedirectToAction("Index", "Login");
            ViewBag.Admin = HttpContext.Session.GetString("Admin") == "true";

            int.TryParse(HttpContext.Session.GetString("UsuarioId"), out int usuarioId);
            var votosUsuario = await _service.ListarVotosUsuario(usuarioId);
            ViewBag.VotosUsuario = new HashSet<int>(votosUsuario);

            IEnumerable<Sugestao> sugestoes;

            if (!string.IsNullOrEmpty(status))
                sugestoes = await _service.ListarSugestaoPorStatus(status);
            else if (!string.IsNullOrEmpty(lugar))
                sugestoes = await _service.ListarSugestaoPorLugar(lugar);
            else
                sugestoes = await _service.ListarSugestoes();

            return View(sugestoes);
        }

        [HttpGet]
        public async Task<IActionResult> NovoPost()
        {
            if (VerificarSessaoFalse()) return RedirectToAction("Index", "Login");

            ViewBag.Categorias = await _service.ListarCategorias();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NovoPost(Sugestao s, string? categorias, IFormFile arquivoImagem)
        {
            if (VerificarSessaoFalse()) return RedirectToAction("Index", "Login");

            int.TryParse(HttpContext.Session.GetString("UsuarioId"), out int usuarioId);
            s.UsuarioId = usuarioId;
            s.DataStatus = DateTime.Today;
            s.DataSugestao = DateTime.Today;

            await _service.CriarSugestao(s, categorias, arquivoImagem);
            return RedirectToAction("Index", "Feed");
        }

        [HttpPost]
        public async Task<IActionResult> EditarSugestao(int id, string status)
        {
            if (VerificarSessaoAdminFalse()) return RedirectToAction("Index", "Login");

            await _service.EditarStatusSugestao(id, status);

            return RedirectToAction("Index", "Feed");
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirSugestao(int id)
        {
            if (VerificarSessaoAdminFalse()) return RedirectToAction("Index", "Login");

            await _service.ExcluirSugestao(id);

            return RedirectToAction("Index", "Feed");
        }

        [HttpPost]
        public async Task<IActionResult> Votar(int postId)
        {
            int.TryParse(HttpContext.Session.GetString("UsuarioId"), out int usuarioId);
            await _service.Votar(usuarioId, postId);
            return RedirectToAction("Index", "Feed");
        }
    }
}