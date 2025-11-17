namespace _MarketWeight_.mvc.Models;

/// <summary>
/// Modelo para mostrar información de errores en la aplicación
/// Se utiliza en la página de error para mostrar el ID de la solicitud
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// ID único de la solicitud que causó el error
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Indica si el RequestId debe mostrarse en la interfaz de usuario
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
