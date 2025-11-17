using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using _MarketWeight_.mvc.Models;
using MarketWeight.Core.Persistencia;
using System.Security.Claims;
using MarketWeight.Core;

namespace _MarketWeight_.mvc.Controllers;

/// <summary>
/// Controlador para la gestión de criptomonedas
/// Permite listar, ver detalles, comprar y crear monedas (solo administradores)
/// </summary>
[Authorize]
public class MonedasController : Controller
{
    private readonly IRepoMoneda _repoMoneda;
    private readonly IRepoUsuario _repoUsuario;

    /// <summary>
    /// Constructor que inyecta los repositorios necesarios
    /// </summary>
    public MonedasController(IRepoMoneda repoMoneda, IRepoUsuario repoUsuario)
    {
        _repoMoneda = repoMoneda;
        _repoUsuario = repoUsuario;
    }

    /// <summary>
    /// GET: Lista todas las criptomonedas disponibles
    /// </summary>
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

    /// <summary>
    /// GET: Muestra los detalles de una criptomoneda específica
    /// </summary>
    /// <param name="id">ID de la moneda</param>
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

    /// <summary>
    /// GET: Muestra el formulario de compra de criptomonedas
    /// </summary>
    [HttpGet]
    public IActionResult Buy()
    {
        var monedas = _repoMoneda.Obtener();
        ViewData["Monedas"] = monedas.ToList();
        return View();
    }

    /// <summary>
    /// POST: Procesa la compra de criptomonedas por parte del usuario autenticado
    /// Valida disponibilidad de stock y saldo del usuario
    /// </summary>
    /// <param name="idMoneda">ID de la moneda a comprar</param>
    /// <param name="cantidad">Cantidad de monedas a comprar</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Buy(uint idMoneda, decimal cantidad)
    {
        if (cantidad <= 0)
        {
            TempData["Message"] = "Cantidad inválida";
            return RedirectToAction(nameof(Buy));
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userIdClaim) || !uint.TryParse(userIdClaim, out var userId))
        {
            return RedirectToAction("Login", "Account", new { returnUrl = Url.Action(nameof(Buy)) });
        }

        var moneda = _repoMoneda.Detalle(idMoneda);
        if (moneda is null)
        {
            return NotFound();
        }

        if (cantidad > moneda.Cantidad)
        {
            TempData["Message"] = "No hay stock suficiente de la moneda seleccionada";
            return RedirectToAction(nameof(Buy));
        }

        try
        {
            var usuario = _repoUsuario.Detalle(userId);
            if (usuario is null)
            {
                return NotFound();
            }
            
            var precioTotal = moneda.Precio * cantidad;
            
            _repoUsuario.Compra(userId, cantidad, idMoneda);
            
            // Guardar datos de la compra en ViewData para mostrar en la confirmación
            ViewData["NombreMoneda"] = moneda.Nombre;
            ViewData["Cantidad"] = cantidad.ToString("N2");
            ViewData["PrecioUnitario"] = moneda.Precio.ToString("N2");
            ViewData["PrecioTotal"] = precioTotal.ToString("N2");
            ViewData["SaldoAnterior"] = usuario.Saldo.ToString("N2");
            ViewData["SaldoNuevo"] = (usuario.Saldo - precioTotal).ToString("N2");
            
            return View("Confirmation");
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"Error al comprar: {ex.Message}";
            return RedirectToAction(nameof(Buy));
        }
    }

    /// <summary>
    /// GET: Muestra el formulario para crear una nueva criptomoneda (solo administradores)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View(new MonedaDto());
    }

    /// <summary>
    /// POST: Crea una nueva criptomoneda en el sistema (solo administradores)
    /// </summary>
    /// <param name="dto">Datos de la nueva moneda</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
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


