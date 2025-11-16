using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MarketWeight.Core.Persistencia;

namespace _MarketWeight_.mvc.Controllers;

public class AccountController : Controller
{
    private readonly IRepoUsuario _repoUsuario;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IRepoUsuario repoUsuario, ILogger<AccountController> logger)
    {
        _repoUsuario = repoUsuario;
        _logger = logger;
    }

    /// <summary>
    /// Calcula el hash SHA-256 de una contraseña de forma consistente
    /// </summary>
    private static string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return string.Empty;

        // Normalizar: solo trim (no lowercase) para que coincida con el trigger DB
        var normalized = password.Trim();

        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(normalized);
        var hash = sha.ComputeHash(bytes);
        return string.Concat(hash.Select(b => b.ToString("x2")));
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult DebugHash(string password = "123")
    {
        var hash = HashPassword(password);
        return Ok(new { password, hash, length = hash.Length });
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult DebugUser(string email = "abc@gmail.com")
    {
        var user = _repoUsuario.ObtenerPorEmail(email);
        if (user == null)
            return NotFound("Usuario no encontrado");
        
        return Ok(new { 
            email = user.Email, 
            storedHash = user.Password,
            hashLength = user.Password?.Length,
            hashLowercase = user.Password?.ToLowerInvariant()
        });
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
            ModelState.AddModelError(string.Empty, "Email y contraseña son requeridos");
            return View();
        }

        try
        {
            var emailNorm = email.Trim().ToLowerInvariant();
            var passHashed = HashPassword(password);
            
            _logger.LogInformation($"Intento de login - Email: {emailNorm}");
            
            // Obtener usuario específico por email
            var usuarioPorEmail = _repoUsuario.ObtenerPorEmail(emailNorm);
            
            if (usuarioPorEmail is null)
            {
                _logger.LogWarning($"Usuario no encontrado: {emailNorm}");
                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos");
                return View();
            }

            // Normalizar ambos hashes para comparación segura
            var storedHash = usuarioPorEmail.Password?.Trim().ToLowerInvariant() ?? string.Empty;
            var incomingHash = passHashed.ToLowerInvariant();
            
            _logger.LogInformation($"Hash almacenado (primeros 10 chars): {storedHash.Substring(0, Math.Min(10, storedHash.Length))}");
            _logger.LogInformation($"Hash calculado (primeros 10 chars): {incomingHash.Substring(0, Math.Min(10, incomingHash.Length))}");
            
            // Comparar hashes
            if (storedHash != incomingHash)
            {
                _logger.LogWarning($"Contraseña incorrecta para: {emailNorm}");
                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos");
                return View();
            }

            _logger.LogInformation($"Login exitoso para: {emailNorm}");

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
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en Login");
            ModelState.AddModelError(string.Empty, "Error al procesar el login");
            return View();
        }
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
        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido) || 
            string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError(string.Empty, "Todos los campos son requeridos");
            return View();
        }

        try
        {
            var emailNorm = email.Trim().ToLowerInvariant();
            
            // Verificar duplicado por email
            var yaExiste = _repoUsuario.ObtenerPorEmail(emailNorm);
            if (yaExiste is not null)
            {
                ModelState.AddModelError(string.Empty, "El email ya está registrado");
                return View();
            }

            // Enviar la contraseña en texto plano al INSERT: el trigger MySQL aplicará SHA2(,256)
            _logger.LogInformation($"Registrando nuevo usuario - Email: {emailNorm}");

            _repoUsuario.Alta(new MarketWeight.Core.Usuario
            {
                Nombre = nombre.Trim(),
                Apellido = apellido.Trim(),
                Email = emailNorm,
                Password = password.Trim(), // dejar sin hashear: el trigger DB hará SHA2
                Saldo = 0,
                EsAdmin = false
            });

            TempData["Message"] = "Cuenta creada exitosamente. Por favor inicia sesión.";
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en Register");
            ModelState.AddModelError(string.Empty, "Error al crear la cuenta");
            return View();
        }
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


