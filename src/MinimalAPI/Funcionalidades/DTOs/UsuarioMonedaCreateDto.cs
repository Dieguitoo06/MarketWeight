namespace MinimalAPI.Funcionalidades.DTOs;

public class UsuarioMonedaCreateDto
{
    public required uint IdUsuario { get; set; }
    public required uint IdMoneda { get; set; }
    public required decimal Cantidad { get; set; }
}


