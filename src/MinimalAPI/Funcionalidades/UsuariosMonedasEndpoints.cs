using MarketWeight.Ado.Dapper;
using MarketWeight.Core;
using MarketWeight.Core.Persistencia;
using MinimalAPI.Funcionalidades.DTOs;

namespace MinimalAPI.Funcionalidades;

public static class UsuariosMonedasEndpoints
{
    public static void MapUsuariosMonedasEndpoints(this WebApplication app)
    {
        app.MapGet("/usuarios-monedas", (IRepoUsuario repo) =>
        {
            var registros = repo.ObtenerUsuarioMoneda();
            var dtos = registros.Select(um => new UsuarioMonedaDto
            {
                IdUsuario = um.idUsuario,
                IdMoneda = um.idMoneda,
                Cantidad = um.Cantidad
            });
            return Results.Ok(dtos);
        });

        app.MapGet("/usuarios-monedas/filtrar", (IRepoUsuario repo, uint? userId, decimal? cantidad) =>
        {
            try
            {
                var registros = repo.ObtenerPorCondicionUsuarioMoneda(userId, cantidad);
                var dtos = registros.Select(um => new UsuarioMonedaDto
                {
                    IdUsuario = um.idUsuario,
                    IdMoneda = um.idMoneda,
                    Cantidad = um.Cantidad
                });
                return Results.Ok(dtos);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
    }
}


