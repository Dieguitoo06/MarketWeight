using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Cryptography;
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

        var emailNorm = email.Trim();
        var passNorm = password.Trim();
        // Hash para coincidir con almacenamiento CHAR(64) si se usa SHA-256
        string Hash(string s)
        {
            using var sha = SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(s);
            var hash = sha.ComputeHash(bytes);
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }
        var passHashed = Hash(passNorm);
        var usuarios = _repoUsuario.Obtener();
        var usuarioPorEmail = usuarios.FirstOrDefault(u => string.Equals(u.Email?.Trim(), emailNorm, StringComparison.OrdinalIgnoreCase));
        if (usuarioPorEmail is null)
        {
            ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos");
            return View();
        }
        // Aceptar tanto texto plano (legado) como hash SHA-256 (64 chars)
        var stored = usuarioPorEmail.Password?.Trim();
        var ok = string.Equals(stored, passNorm, StringComparison.Ordinal)
                 || string.Equals(stored, passHashed, StringComparison.OrdinalIgnoreCase);
        if (!ok)
        {
            ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos");
            return View();
        }

        // Asegura que no quede sesión previa
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuarioPorEmail.IdUsuario.ToString()),
            new Claim(ClaimTypes.Name, $"{usuarioPorEmail.Nombre} {usuarioPorEmail.Apellido}"),
            new Claim(ClaimTypes.Email, usuarioPorEmail.Email),
            new Claim(ClaimTypes.Role, usuarioPorEmail.EsAdmin ? "Admin" : "User")
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
        var yaExiste = _repoUsuario.Obtener().Any(u => string.Equals(u.Email?.Trim(), email.Trim(), StringComparison.OrdinalIgnoreCase));
        if (yaExiste)
        {
            ModelState.AddModelError(string.Empty, "El email ya está registrado");
            return View();
        }
        // Hash de password en SHA-256 hex para almacenar 64 chars
        string Hash(string s)
        {
            using var sha = SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(s.Trim());
            var hash = sha.ComputeHash(bytes);
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }
        _repoUsuario.Alta(new MarketWeight.Core.Usuario
        {
            Nombre = nombre,
            Apellido = apellido,
            Email = email,
            Password = Hash(password),
            Saldo = 0,
            EsAdmin = false
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


