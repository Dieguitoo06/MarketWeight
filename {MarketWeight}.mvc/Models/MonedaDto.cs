using System.ComponentModel.DataAnnotations;

namespace _MarketWeight_.mvc.Models;

public class MonedaDto
{
    public uint IdMoneda { get; set; }

    [Required]
    public decimal Precio { get; set; }

    [Required]
    public decimal Cantidad { get; set; }

    [Required]
    public string Nombre { get; set; } = string.Empty;
}


