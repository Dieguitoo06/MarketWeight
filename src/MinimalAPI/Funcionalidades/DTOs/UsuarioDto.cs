namespace MinimalAPI.Funcionalidades.DTOs;

public class UsuarioDto
{
    public required string Nombre { get; set; }
    public required string Apellido { get; set; }
    public required string Email { get; set; }
    public decimal Saldo { get; set; }
}


