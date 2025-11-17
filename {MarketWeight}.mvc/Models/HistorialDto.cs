using System.ComponentModel.DataAnnotations;

namespace _MarketWeight_.mvc.Models;

/// <summary>
/// DTO para representar un registro de historial de transacciones
/// Contiene información sobre compras y ventas de criptomonedas realizadas por usuarios
/// </summary>
public class HistorialDto
{
    /// <summary>
    /// Identificador único del registro en el historial
    /// </summary>
    public uint IdHistorial { get; set; }

    /// <summary>
    /// ID del usuario que realizó la transacción
    /// </summary>
    public uint IdUsuario { get; set; }

    /// <summary>
    /// ID de la criptomoneda involucrada en la transacción
    /// </summary>
    public uint IdMoneda { get; set; }

    /// <summary>
    /// Cantidad de monedas transaccionadas
    /// </summary>
    public decimal Cantidad { get; set; }

    /// <summary>
    /// Indicador de si la transacción fue una compra (true) o venta (false)
    /// </summary>
    public bool Compra { get; set; }

    /// <summary>
    /// Fecha y hora en que se realizó la transacción
    /// </summary>
    public DateTime FechaHora { get; set; }
}



