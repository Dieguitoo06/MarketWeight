using System.ComponentModel.DataAnnotations;

namespace _MarketWeight_.mvc.Models;

/// <summary>
/// DTO para representar una criptomoneda en el sistema
/// Contiene información sobre el precio, cantidad disponible y nombre de la moneda
/// </summary>
public class MonedaDto
{
    /// <summary>
    /// Identificador único de la criptomoneda
    /// </summary>
    public uint IdMoneda { get; set; }

    /// <summary>
    /// Precio unitario de la criptomoneda
    /// </summary>
    [Required]
    public decimal Precio { get; set; }

    /// <summary>
    /// Cantidad disponible de la criptomoneda en el sistema
    /// </summary>
    [Required]
    public decimal Cantidad { get; set; }

    /// <summary>
    /// Nombre de la criptomoneda (ej: Bitcoin, Ethereum)
    /// </summary>
    [Required]
    public string Nombre { get; set; } = string.Empty;
}


