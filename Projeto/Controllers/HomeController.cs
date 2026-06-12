using Microsoft.AspNetCore.Mvc;
using Projeto.Models;
using Projeto.Services;

namespace Projeto.Controllers;

public class HomeController : Controller
{
    private readonly SugestaoService _sugestaoService;
    private readonly ContentSafetyService _contentSafetyService;

    public HomeController(SugestaoService sugestaoService, ContentSafetyService contentSafetyService)
    {
        _sugestaoService = sugestaoService;
        _contentSafetyService = contentSafetyService;
    }

    public IActionResult Index()
    {
        var sugestao = _sugestaoService.GetAll();
        return View(sugestao);
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarSugestao(Sugestao sugestao)
    {
        if (ModelState.IsValid)
        {
            var safetyCheck = await _contentSafetyService.ValidateProductAsync(sugestao);
            
            if (!safetyCheck.IsSafe)
            {
                TempData["ErrorMessage"] = safetyCheck.Message;
                return RedirectToAction(nameof(Index));
            }

            _sugestaoService.Add(sugestao);
            TempData["SuccessMessage"] = "Sugestão validada e cadastrada com sucesso!";
        }
        return RedirectToAction(nameof(Index));
    }
}
