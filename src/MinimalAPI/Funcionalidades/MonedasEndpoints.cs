using MarketWeight.Ado.Dapper;
using MarketWeight.Core;
using MarketWeight.Core.Persistencia;
using MinimalAPI.Funcionalidades.DTOs;

namespace MinimalAPI.Funcionalidades;

public static class MonedasEndpoints
{
    public static void MapMonedasEndpoints(this WebApplication app)
    {
        app.MapGet("/monedas", (IRepoMoneda repo) =>
        {
            var monedas = repo.Obtener();
            var dtos = monedas.Select(m => new MonedaDto
            {
                Precio = m.Precio,
                Cantidad = m.Cantidad,
                Nombre = m.Nombre
            });
            return Results.Ok(dtos);
        });

        app.MapGet("/monedas/{id:int}", (IRepoMoneda repo, int id) =>
        {
            if (id < 0) return Results.BadRequest("El id no puede ser negativo.");
            var moneda = repo.Detalle((uint)id);
            if (moneda is null) return Results.NotFound();
            var dto = new MonedaDto
            {
                Precio = moneda.Precio,
                Cantidad = moneda.Cantidad,
                Nombre = moneda.Nombre
            };
            return Results.Ok(dto);
        });

        app.MapPost("/monedas", (IRepoMoneda repo, MonedaCreateDto nueva) =>
        {
            try
            {
                var moneda = new Moneda
                {
                    Precio = nueva.Precio,
                    Cantidad = nueva.Cantidad,
                    Nombre = nueva.Nombre
                };
                repo.Alta(moneda);
                var dto = new MonedaDto
                {
                    Precio = moneda.Precio,
                    Cantidad = moneda.Cantidad,
                    Nombre = moneda.Nombre
                };
                return Results.Created($"/monedas/{moneda.Nombre}", dto);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
    }
}


