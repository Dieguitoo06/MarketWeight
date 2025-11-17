using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using _MarketWeight_.mvc.Models;
using MarketWeight.Core.Persistencia;

namespace _MarketWeight_.mvc.Controllers;

/// <summary>
/// Controlador para ver el historial de transacciones de criptomonedas
/// Muestra todas las compras y ventas registradas en el sistema
/// </summary>
[Authorize]
public class HistorialesController : Controller
{
    private readonly IRepoHistorial _repoHistorial;

    /// <summary>
    /// Constructor que inyecta el repositorio de historial
    /// </summary>
    public HistorialesController(IRepoHistorial repoHistorial)
    {
        _repoHistorial = repoHistorial;
    }

    /// <summary>
    /// GET: Lista todos los registros de historial de transacciones
    /// </summary>
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

    /// <summary>
    /// GET: Muestra los detalles de un registro específico del historial
    /// </summary>
    /// <param name="id">ID del registro del historial</param>
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


