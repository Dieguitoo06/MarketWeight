using System.Data;
using MarketWeight.Ado.Dapper;
using MarketWeight.Core.Persistencia;
using MySqlConnector;
using Microsoft.AspNetCore.Authentication.Cookies;

/// <summary>
/// Program.cs - Configuración y punto de entrada de la aplicación web MarketWeight
/// Configura los servicios, autenticación, middleware y rutas de la aplicación
/// </summary>

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios a la aplicación
builder.Services.AddControllersWithViews();

/// <summary>
/// Configurar autenticación con cookies
/// Utiliza esquema de autenticación por cookies para mantener las sesiones de usuario
/// </summary>
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Rutas de redirección para login y acceso denegado
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        
        // Permitir que la cookie se renueve con cada petición si está dentro del período de expiración
        options.SlidingExpiration = true;
        
        /// <summary>
        /// ExpireTimeSpan: Tiempo máximo de inactividad antes de que expire la sesión (30 minutos)
        /// Si el usuario no realiza ninguna acción en 30 minutos, debe volver a iniciar sesión
        /// </summary>
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        
        /// <summary>
        /// Configuración de seguridad de la cookie de autenticación
        /// </summary>
        // Nombre de la cookie
        options.Cookie.Name = "MarketWeight.Auth";
        
        // HttpOnly previene que JavaScript acceda a la cookie (protección contra XSS)
        options.Cookie.HttpOnly = true;
        
        // SameAsRequest: Para desarrollo local con HTTP. En producción debe cambiar a Always con HTTPS
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    });

/// <summary>
/// Configurar la cadena de conexión a la base de datos MySQL
/// </summary>
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string 'MySqlConnection' no está configurada.");
}

// Registrar IDbConnection con MySqlConnection
builder.Services.AddScoped<IDbConnection>(_ => new MySqlConnection(connectionString));

/// <summary>
/// Registrar los repositorios como servicios inyectables (patrón Dependency Injection)
/// </summary>
builder.Services.AddScoped<IRepoUsuario, RepoUsuario>();
builder.Services.AddScoped<IRepoMoneda, RepoMoneda>();
builder.Services.AddScoped<IRepoHistorial, RepoHistorial>();


var app = builder.Build();

/// <summary>
/// Configurar el middleware de la aplicación (pipeline HTTP)
/// El orden es importante: se ejecutan en el mismo orden en que se definen
/// </summary>

// Manejo de excepciones en ambiente de producción
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // HSTS (HTTP Strict Transport Security) - Valor por defecto: 30 días
    // En producción, considerar aumentar este valor
    app.UseHsts();
}

// Redirigir HTTP a HTTPS (seguridad en tránsito)
app.UseHttpsRedirection();

// Servir archivos estáticos (CSS, JS, imágenes, etc.)
app.UseStaticFiles();

// Enrutamiento - Mapear URLs a controladores
app.UseRouting();

// Autenticación - Validar identidad del usuario
app.UseAuthentication();

// Autorización - Validar permisos del usuario
app.UseAuthorization();

/// <summary>
/// Ruta por defecto: Home/Index
/// Patrón: {controller}/{action}/{id?}
/// </summary>
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Iniciar la aplicación
app.Run();
