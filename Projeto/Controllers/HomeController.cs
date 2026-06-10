using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Projeto.Models;

namespace Projeto.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
