using System.ComponentModel.DataAnnotations;

namespace _MarketWeight_.mvc.Models;

public class UsuarioDto
{
    public uint IdUsuario { get; set; }

    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public string Apellido { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public decimal Saldo { get; set; }
}



