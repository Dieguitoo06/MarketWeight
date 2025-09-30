using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using _MarketWeight_.mvc.Models;
using MarketWeight.Core.Persistencia;
using MarketWeight.Core;

namespace _MarketWeight_.mvc.Controllers;

[Authorize]
public class UsuarioMonedasController : Controller
{
    private readonly IRepoUsuario _repoUsuario;

    public UsuarioMonedasController(IRepoUsuario repoUsuario)
    {
        _repoUsuario = repoUsuario;
    }

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


