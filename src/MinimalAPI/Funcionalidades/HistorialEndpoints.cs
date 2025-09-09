using MarketWeight.Ado.Dapper;
using MarketWeight.Core;
using MarketWeight.Core.Persistencia;
using MinimalAPI.Funcionalidades.DTOs;

namespace MinimalAPI.Funcionalidades;

public static class HistorialEndpoints
{
    public static void MapHistorialEndpoints(this WebApplication app)
    {
        app.MapGet("/historial", (IRepoHistorial repo) =>
        {
            var registros = repo.Obtener();
            var dtos = registros.Select(h => new HistorialDto
            {
                IdUsuario = h.IdUsuario,
                IdMoneda = h.idMoneda,
                Cantidad = h.Cantidad,
                Compra = h.Compra,
                FechaHora = h.FechaHora
            });
            return Results.Ok(dtos);
        });

        app.MapGet("/historial/{id:int}", (IRepoHistorial repo, int id) =>
        {
            if (id < 0) return Results.BadRequest("El id no puede ser negativo.");
            var registro = repo.Detalle((uint)id);
            if (registro is null) return Results.NotFound();
            var dto = new HistorialDto
            {
                IdUsuario = registro.IdUsuario,
                IdMoneda = registro.idMoneda,
                Cantidad = registro.Cantidad,
                Compra = registro.Compra,
                FechaHora = registro.FechaHora
            };
            return Results.Ok(dto);
        });

        app.MapPost("/historial", (IRepoHistorial repo, HistorialCreateDto nuevo) =>
        {
            try
            {
                var entidad = new Historial
                {
                    IdUsuario = nuevo.IdUsuario,
                    idMoneda = nuevo.IdMoneda,
                    Cantidad = nuevo.Cantidad,
                    Compra = nuevo.Compra,
                    FechaHora = DateTime.UtcNow
                };
                repo.Alta(entidad);
                var dto = new HistorialDto
                {
                    IdUsuario = entidad.IdUsuario,
                    IdMoneda = entidad.idMoneda,
                    Cantidad = entidad.Cantidad,
                    Compra = entidad.Compra,
                    FechaHora = entidad.FechaHora
                };
                return Results.Created($"/historial/{dto.IdUsuario}-{dto.IdMoneda}-{dto.FechaHora:o}", dto);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
    }
}


