using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using _MarketWeight_.mvc.Models;
using MarketWeight.Core.Persistencia;
using System.Security.Claims;
using MarketWeight.Core;

namespace _MarketWeight_.mvc.Controllers;


[Authorize]
public class MonedasController : Controller
{
    private readonly IRepoMoneda _repoMoneda;
    private readonly IRepoUsuario _repoUsuario;


    public MonedasController(IRepoMoneda repoMoneda, IRepoUsuario repoUsuario)
    {
        _repoMoneda = repoMoneda;
        _repoUsuario = repoUsuario;
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
    public IActionResult Buy()
    {
        var monedas = _repoMoneda.Obtener();
        ViewData["Monedas"] = monedas.ToList();
        return View();
    }

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

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View(new MonedaDto());
    }

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


