using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using _MarketWeight_.mvc.Models;
using MarketWeight.Core.Persistencia;
using MarketWeight.Core;

namespace _MarketWeight_.mvc.Controllers;

/// <summary>
/// Controlador para la gestión de la billetera de criptomonedas de usuarios
/// Permite ver y gestionar las monedas que posee cada usuario
/// </summary>
[Authorize]
public class UsuarioMonedasController : Controller
{
    private readonly IRepoUsuario _repoUsuario;

    /// <summary>
    /// Constructor que inyecta el repositorio de usuarios
    /// </summary>
    public UsuarioMonedasController(IRepoUsuario repoUsuario)
    {
        _repoUsuario = repoUsuario;
    }

    /// <summary>
    /// GET: Lista todas las monedas en las billeteras de todos los usuarios
    /// </summary>
    public IActionResult Index()
    {
        var registros = _repoUsuario.ObtenerUsuarioMoneda();
        var model = registros.Select(r => new UsuarioMonedaDto
        {
            IdUsuario = r.idUsuario,
            IdMoneda = r.idMoneda,
            Cantidad = r.Cantidad
        }).ToList();
        return View(model);
    }

    /// <summary>
    /// GET: Muestra los detalles de una moneda específica en la billetera de un usuario
    /// </summary>
    /// <param name="idUsuario">ID del usuario propietario de la moneda</param>
    /// <param name="idMoneda">ID de la moneda en la billetera</param>
    public IActionResult Details(uint idUsuario, uint idMoneda)
    {
        var registros = _repoUsuario.ObtenerUsuarioMoneda();
        var r = registros.FirstOrDefault(x => x.idUsuario == idUsuario && x.idMoneda == idMoneda);
        if (r is null) return NotFound();
        var model = new UsuarioMonedaDto
        {
            IdUsuario = r.idUsuario,
            IdMoneda = r.idMoneda,
            Cantidad = r.Cantidad
        };
        return View(model);
    }
}


