using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using _MarketWeight_.mvc.Models;
using MarketWeight.Core.Persistencia;

namespace _MarketWeight_.mvc.Controllers;

[Authorize]
public class HistorialesController : Controller
{
    private readonly IRepoHistorial _repoHistorial;

    public HistorialesController(IRepoHistorial repoHistorial)
    {
        _repoHistorial = repoHistorial;
    }

    public IActionResult Index()
    {
        var registros = _repoHistorial.Obtener();
        var model = registros.Select(h => new HistorialDto
        {
            IdHistorial = h.IdHistorial,
            IdUsuario = h.IdUsuario,
            IdMoneda = h.idMoneda,
            Cantidad = h.Cantidad,
            Compra = h.Compra,
            FechaHora = h.FechaHora
        }).ToList();
        return View(model);
    }

    public IActionResult Details(uint id)
    {
        var h = _repoHistorial.Detalle(id);
        if (h is null) return NotFound();
        var model = new HistorialDto
        {
            IdHistorial = h.IdHistorial,
            IdUsuario = h.IdUsuario,
            IdMoneda = h.idMoneda,
            Cantidad = h.Cantidad,
            Compra = h.Compra,
            FechaHora = h.FechaHora
        };
        return View(model);
    }
}


