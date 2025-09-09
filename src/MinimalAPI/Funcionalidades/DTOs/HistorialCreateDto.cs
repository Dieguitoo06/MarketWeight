namespace MinimalAPI.Funcionalidades.DTOs;

public class HistorialCreateDto
{
    public required uint IdUsuario { get; set; }
    public required uint IdMoneda { get; set; }
    public required decimal Cantidad { get; set; }
    public required bool Compra { get; set; }
}


