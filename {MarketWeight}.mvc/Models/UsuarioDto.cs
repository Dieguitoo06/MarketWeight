using System.ComponentModel.DataAnnotations;

namespace _MarketWeight_.mvc.Models;

/// <summary>
/// DTO para representar un usuario del sistema
/// Contiene información de perfil, credenciales y estado del usuario
/// </summary>
public class UsuarioDto
{
    /// <summary>
    /// Identificador único del usuario
    /// </summary>
    public uint IdUsuario { get; set; }

    /// <summary>
    /// Nombre del usuario
    /// </summary>
    [Required]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Apellido del usuario
    /// </summary>
    [Required]
    public string Apellido { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del usuario (debe ser único en el sistema)
    /// </summary>
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña del usuario (no se almacena en cláro)
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Saldo disponible del usuario para compras
    /// </summary>
    public decimal Saldo { get; set; }
    
    /// <summary>
    /// Indica si el usuario tiene permisos de administrador
    /// </summary>
    public bool EsAdmin { get; set; } = false;
}



