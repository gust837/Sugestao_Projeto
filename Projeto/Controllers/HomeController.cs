using Microsoft.AspNetCore.Mvc;
using Projeto.Models;
using Projeto.Services;

namespace Projeto.Controllers;

public class HomeController : Controller
{
    private readonly SugestaoService _sugestaoService;
    private readonly ContentSafetyService _contentSafetyService;
    private readonly IWebHostEnvironment _env;

    public HomeController(SugestaoService sugestaoService, ContentSafetyService contentSafetyService, IWebHostEnvironment env)
    {
        _sugestaoService = sugestaoService;
        _contentSafetyService = contentSafetyService;
        _env = env;
    }

    public IActionResult Index()
    {
        var sugestao = _sugestaoService.GetAll();
        return View(sugestao);
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarSugestao(Sugestao sugestao, IFormFile? imagem = null)
    {
        if (ModelState.IsValid)
        {
            // Valida texto e imagem (se houver) com o Gemini
            var safetyCheck = await _contentSafetyService.ValidacaoSugestaoAsync(sugestao, imagem);

            if (!safetyCheck.IsSafe)
            {
                TempData["ErrorMessage"] = safetyCheck.Message;
                return RedirectToAction(nameof(Index));
            }

            // Salva o arquivo no servidor somente após aprovação da IA
            if (imagem != null && imagem.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(imagem.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await imagem.CopyToAsync(stream);

                sugestao.Imagem = uniqueFileName; // guarda só o nome, não o caminho completo
            }

            _sugestaoService.Adicionar(sugestao);
            TempData["SuccessMessage"] = "Sugestão validada e cadastrada com sucesso!";
        }

        return RedirectToAction(nameof(Index));
    }
}
