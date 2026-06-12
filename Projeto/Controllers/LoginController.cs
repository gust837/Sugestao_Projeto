using Microsoft.AspNetCore.Mvc;
using Projeto.Interfaces;
using Projeto.Models;

namespace Projeto.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioService _service;

        public LoginController(IUsuarioService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(string Nome, string Cpf, string Email, string Senha)
        {
            Usuario u = new Usuario(Nome, Cpf, Email, Senha, false);
            _service.CriarUsuario(u);

            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public async Task<IActionResult> Logar(string email, string senha)
        {
            Usuario? usuario = await _service.BuscarUsuario(email, senha);

            if (usuario != null)
            {
                HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
                HttpContext.Session.SetString("Admin", usuario.Adm.ToString().ToLower());
                return RedirectToAction("Index", "Sugestao");
            }
            ViewBag.Erro = "Email ou senha incorretos";
            return RedirectToAction("Index", "Login");
        }
    }
}