using System.ComponentModel.DataAnnotations;

namespace _MarketWeight_.mvc.Models;

public class HistorialDto
{
    public uint IdHistorial { get; set; }
    public uint IdUsuario { get; set; }
    public uint IdMoneda { get; set; }
    public decimal Cantidad { get; set; }
    public bool Compra { get; set; }
    public DateTime FechaHora { get; set; }
}



