using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using _MarketWeight_.mvc.Models;
using MarketWeight.Core.Persistencia;
using MarketWeight.Core;
using System.Security.Claims;

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
            Saldo = u.Saldo,
            EsAdmin = u.EsAdmin
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
            Saldo = usuario.Saldo,
            EsAdmin = usuario.EsAdmin
        };
        ViewData["Billetera"] = usuario.Billetera ?? new List<UsuarioMoneda>();
        return View(model);
    }

    [HttpGet]
    public IActionResult Me()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userIdClaim) || !uint.TryParse(userIdClaim, out var userId))
            return RedirectToAction("Login", "Account", new { returnUrl = Url.Action(nameof(Me)) });

        return RedirectToAction(nameof(Details), new { id = userId });
    }

    // Crear usuario vía UI deshabilitado; los usuarios se crean por Register

    [HttpGet]
    public IActionResult Ingresar()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userIdClaim) || !uint.TryParse(userIdClaim, out var userId))
            return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Ingresar") });

        var usuario = _repoUsuario.Detalle(userId);
        if (usuario is null) return NotFound();
        ViewData["Saldo"] = usuario.Saldo;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Ingresar(decimal monto)
    {
        if (monto <= 0)
        {
            TempData["Message"] = "Monto inválido";
            return RedirectToAction(nameof(Ingresar));
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userIdClaim) || !uint.TryParse(userIdClaim, out var userId))
            return RedirectToAction("Login", "Account", new { returnUrl = Url.Action(nameof(Ingresar)) });

        _repoUsuario.Ingreso(userId, monto);
        TempData["Message"] = "Saldo ingresado correctamente";
        return RedirectToAction("Details", new { id = userId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public IActionResult ToggleAdmin(uint id)
    {
        var usuario = _repoUsuario.Detalle(id);
        if (usuario is null) return NotFound();

        // No permitir que un admin se quite a sí mismo los permisos
        var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserIdClaim != null && uint.TryParse(currentUserIdClaim, out var currentUserId) && currentUserId == id)
        {
            TempData["Message"] = "No puedes modificar tus propios permisos de administrador";
            return RedirectToAction(nameof(Details), new { id });
        }

        usuario.EsAdmin = !usuario.EsAdmin;
        _repoUsuario.Modificar(usuario);
        
        var message = usuario.EsAdmin ? "Usuario promovido a administrador" : "Usuario degradado a usuario normal";
        TempData["Message"] = message;
        return RedirectToAction(nameof(Details), new { id });
    }
    // SeedWallet eliminado
}


