namespace MinimalAPI.Funcionalidades.DTOs;

public class HistorialDto
{
    public required uint IdUsuario { get; set; }
    public required uint IdMoneda { get; set; }
    public required decimal Cantidad { get; set; }
    public required bool Compra { get; set; }
    public required DateTime FechaHora { get; set; }
}


