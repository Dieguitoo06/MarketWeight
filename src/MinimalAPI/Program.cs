using MarketWeight.Ado.Dapper;
using MarketWeight.Core;
using MySqlConnector;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<IDbConnection>(_ => new MySqlConnection(connectionString));
builder.Services.AddScoped<RepoUsuario>();
builder.Services.AddScoped<RepoMoneda>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Endpoints para Usuario
app.MapGet("/usuarios", (RepoUsuario repo) => repo.Obtener());

app.MapGet("/usuarios/{id:int}", (RepoUsuario repo, int id) =>
{
    if (id < 0) return Results.BadRequest("El id no puede ser negativo.");
    var usuario = repo.Detalle((uint)id);
    return usuario is not null ? Results.Ok(usuario) : Results.NotFound();
});

app.MapPost("/usuarios", (RepoUsuario repo, Usuario usuario) =>
{
    try
    {
        repo.Alta(usuario);
        return Results.Created($"/usuarios/{usuario.Email}", usuario);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

// Endpoints para Moneda
app.MapGet("/monedas", (RepoMoneda repo) => repo.Obtener());

app.MapGet("/monedas/{id:int}", (RepoMoneda repo, int id) =>
{
    if (id < 0) return Results.BadRequest("El id no puede ser negativo.");
    var moneda = repo.Detalle((uint)id);
    return moneda is not null ? Results.Ok(moneda) : Results.NotFound();
});

app.MapPost("/monedas", (RepoMoneda repo, Moneda moneda) =>
{
    try
    {
        repo.Alta(moneda);
        return Results.Created($"/monedas/{moneda.Nombre}", moneda);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/", () => "Hello World!");

app.Run();
