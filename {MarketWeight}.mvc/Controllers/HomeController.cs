using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using _MarketWeight_.mvc.Models;

namespace _MarketWeight_.mvc.Controllers;

/// <summary>
/// Controlador principal de la aplicación
/// Maneja las vistas públicas como Home y Privacy
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    /// <summary>
    /// Constructor que inyecta el logger
    /// </summary>
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// GET: Página de inicio de la aplicación
    /// </summary>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// GET: Página de privacidad
    /// </summary>
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
