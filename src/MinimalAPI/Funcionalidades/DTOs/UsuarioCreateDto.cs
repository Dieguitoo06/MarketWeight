namespace MinimalAPI.Funcionalidades.DTOs;

public class UsuarioCreateDto
{
    public required string Nombre { get; set; }
    public required string Apellido { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}


