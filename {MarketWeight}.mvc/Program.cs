using System.Data;
using MarketWeight.Ado.Dapper;
using MarketWeight.Core.Persistencia;
using MySqlConnector;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        options.SlidingExpiration = true;
        
        // Configurar tiempo de expiración de la cookie
        // ExpireTimeSpan: tiempo máximo de inactividad antes de que expire la sesión (30 minutos)
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        
        // Configuración de seguridad de la cookie
        options.Cookie.Name = "MarketWeight.Auth";
        options.Cookie.HttpOnly = true;
        // Para presentaciones locales por HTTP puedes usar SameAsRequest temporalmente.
        // En producción debe permanecer 'Always' con HTTPS.
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    });

var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string 'MySqlConnection' no está configurada.");
}

builder.Services.AddScoped<IDbConnection>(_ => new MySqlConnection(connectionString));

builder.Services.AddScoped<IRepoUsuario, RepoUsuario>();
builder.Services.AddScoped<IRepoMoneda, RepoMoneda>();
builder.Services.AddScoped<IRepoHistorial, RepoHistorial>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
