namespace _MarketWeight_.mvc.Models;

/// <summary>
/// DTO para representar una criptomoneda en la billetera de un usuario
/// Establece la relación entre un usuario y las monedas que posee
/// </summary>
public class UsuarioMonedaDto
{
    /// <summary>
    /// ID del usuario propietario de la moneda
    /// </summary>
    public uint IdUsuario { get; set; }

    /// <summary>
    /// ID de la criptomoneda que posee el usuario
    /// </summary>
    public uint IdMoneda { get; set; }

    /// <summary>
    /// Cantidad de esa criptomoneda que posee el usuario
    /// </summary>
    public decimal Cantidad { get; set; }
}



