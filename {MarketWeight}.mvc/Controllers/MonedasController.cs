using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using _MarketWeight_.mvc.Models;
using MarketWeight.Core.Persistencia;
using MarketWeight.Core;

namespace _MarketWeight_.mvc.Controllers;

[Authorize]
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
            IdMoneda = m.IdMoneda,
            Precio = m.Precio,
            Cantidad = m.Cantidad,
            Nombre = m.Nombre
        }).ToList();
        return View(model);
    }

    public IActionResult Details(uint id)
    {
        var moneda = _repoMoneda.Detalle(id);
        if (moneda is null) return NotFound();
        var model = new MonedaDto
        {
            IdMoneda = moneda.IdMoneda,
            Nombre = moneda.Nombre,
            Precio = moneda.Precio,
            Cantidad = moneda.Cantidad
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new MonedaDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(MonedaDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var entity = new Moneda
        {
            Nombre = dto.Nombre,
            Precio = dto.Precio,
            Cantidad = dto.Cantidad
        };
        _repoMoneda.Alta(entity);
        return RedirectToAction(nameof(Index));
    }
}


