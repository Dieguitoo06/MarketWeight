using Microsoft.AspNetCore.Mvc;
using _MarketWeight_.mvc.Models;
using MarketWeight.Core.Persistencia;

namespace _MarketWeight_.mvc.Controllers;

public class MonedasController : Controller
{
    private readonly IRepoMoneda _repoMoneda;

    public MonedasController(IRepoMoneda repoMoneda)
    {
        _repoMoneda = repoMoneda;
    }

    public IActionResult Index()
    {
        var monedas = _repoMoneda.Obtener();
        var model = monedas.Select(m => new MonedaDto
        {
            Precio = m.Precio,
            Cantidad = m.Cantidad,
            Nombre = m.Nombre
        }).ToList();
        return View(model);
    }
}


