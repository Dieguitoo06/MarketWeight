using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using _MarketWeight_.mvc.Models;
using MarketWeight.Core.Persistencia;
using MarketWeight.Core;

namespace _MarketWeight_.mvc.Controllers;

[Authorize]
public class UsuariosController : Controller
{
    private readonly IRepoUsuario _repoUsuario;

    public UsuariosController(IRepoUsuario repoUsuario)
    {
        _repoUsuario = repoUsuario;
    }

    public IActionResult Index()
    {
        var usuarios = _repoUsuario.Obtener();
        var model = usuarios.Select(u => new UsuarioDto
        {
            IdUsuario = u.IdUsuario,
            Nombre = u.Nombre,
            Apellido = u.Apellido,
            Email = u.Email,
            Password = string.Empty,
            Saldo = u.Saldo
        }).ToList();
        return View(model);
    }

    public IActionResult Details(uint id)
    {
        var usuario = _repoUsuario.DetalleCompleto(id);
        if (usuario is null) return NotFound();
        var model = new UsuarioDto
        {
            IdUsuario = usuario.IdUsuario,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Email = usuario.Email,
            Password = string.Empty,
            Saldo = usuario.Saldo
        };
        ViewData["Billetera"] = usuario.Billetera ?? new List<UsuarioMoneda>();
        return View(model);
    }

    // Crear usuario vía UI deshabilitado; los usuarios se crean por Register

    // SeedWallet eliminado
}


