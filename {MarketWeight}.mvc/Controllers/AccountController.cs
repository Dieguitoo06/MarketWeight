using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using MarketWeight.Core.Persistencia;

namespace _MarketWeight_.mvc.Controllers;

public class AccountController : Controller
{
    private readonly IRepoUsuario _repoUsuario;

    public AccountController(IRepoUsuario repoUsuario)
    {
        _repoUsuario = repoUsuario;
    }
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError(string.Empty, "Credenciales inválidas");
            return View();
        }

        var usuario = _repoUsuario.Obtener()
            .FirstOrDefault(u => u.Email == u.Email && u.Password == u.Password);

        if (usuario is null)
        {
            ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos");
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}"),
            new Claim(ClaimTypes.Email, usuario.Email)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Register(string nombre, string apellido, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError(string.Empty, "Datos inválidos");
            return View();
        }
        // Verificar duplicado por email
        var yaExiste = _repoUsuario.Obtener().Any(u => u.Email == email);
        if (yaExiste)
        {
            ModelState.AddModelError(string.Empty, "El email ya está registrado");
            return View();
        }
        _repoUsuario.Alta(new MarketWeight.Core.Usuario
        {
            Nombre = nombre,
            Apellido = apellido,
            Email = email,
            Password = password,
            Saldo = 0
        });
        TempData["Message"] = "Cuenta creada. Inicie sesión.";
        return RedirectToAction("Login");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}


