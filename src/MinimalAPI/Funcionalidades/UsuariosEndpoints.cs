using MarketWeight.Ado.Dapper;
using MarketWeight.Core;
using MarketWeight.Core.Persistencia;
using MinimalAPI.Funcionalidades.DTOs;

namespace MinimalAPI.Funcionalidades;

public static class UsuariosEndpoints
{
    public static void MapUsuariosEndpoints(this WebApplication app)
    {
        app.MapGet("/usuarios", (IRepoUsuario repo) =>
        {
            var usuarios = repo.Obtener();
            var dtos = usuarios.Select(u => new UsuarioDto
            {
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Email = u.Email,
                Saldo = u.Saldo
            });
            return Results.Ok(dtos);
        });

        app.MapGet("/usuarios/{id:int}", (IRepoUsuario repo, int id) =>
        {
            if (id < 0) return Results.BadRequest("El id no puede ser negativo.");
            var usuario = repo.Detalle((uint)id);
            if (usuario is null) return Results.NotFound();
            var dto = new UsuarioDto
            {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Saldo = usuario.Saldo
            };
            return Results.Ok(dto);
        });

        app.MapPost("/usuarios", (IRepoUsuario repo, UsuarioCreateDto nuevo) =>
        {
            try
            {
                var usuario = new Usuario
                {
                    Nombre = nuevo.Nombre,
                    Apellido = nuevo.Apellido,
                    Email = nuevo.Email,
                    Password = nuevo.Password,
                    Saldo = 0
                };
                repo.Alta(usuario);
                var dto = new UsuarioDto
                {
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Email = usuario.Email,
                    Saldo = usuario.Saldo
                };
                return Results.Created($"/usuarios/{usuario.Email}", dto);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
    }
}


