using Microsoft.AspNetCore.Mvc;
using Projeto.Models;
using Projeto.Services;

namespace Projeto.Controllers;

public class HomeController : Controller
{
    private readonly SugestaoService _sugestaoService;
    private readonly IWebHostEnvironment _env;

    public HomeController(SugestaoService sugestaoService, IWebHostEnvironment env)
    {
        _sugestaoService = sugestaoService;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var sugestoes = await _sugestaoService.ListarSugestoes();
        return View(sugestoes);
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarSugestao(Sugestao sugestao, IFormFile? imagem = null)
    {
        if (ModelState.IsValid)
        {
            var resultado = await _sugestaoService.CriarSugestao(sugestao, null, imagem);

            if (!resultado.Ok)
            {
                TempData["ErrorMessage"] = resultado.Mensagem;
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = resultado.Mensagem;
        }

        return RedirectToAction(nameof(Index));
    }
}
