using Microsoft.AspNetCore.Mvc;
using _MarketWeight_.mvc.Models;
using MarketWeight.Core.Persistencia;
using MarketWeight.Core;

namespace _MarketWeight_.mvc.Controllers;

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
        var usuario = _repoUsuario.Detalle(id);
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
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new UsuarioDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(UsuarioDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var entity = new Usuario
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Email = dto.Email,
            Password = dto.Password,
            Saldo = dto.Saldo
        };
        _repoUsuario.Alta(entity);
        return RedirectToAction(nameof(Index));
    }
}


