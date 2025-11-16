namespace MarketWeight.Core
{
    public class UsuarioMoneda
    {

        public  uint idUsuario { get; set; }
        public uint idMoneda { get; set; }
        public decimal Cantidad { get; set; }
        // Nombre y Precio se agregan para mostrar detalles junto con la billetera
        public string? Nombre { get; set; }
        public decimal? Precio { get; set; }
    }
}

