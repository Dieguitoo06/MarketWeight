namespace MarketWeight.Core.Persistencia;

public interface IRepoUsuario :
    IRepoAlta<Usuario>,
    IRepoListado<Usuario>,
    IRepoDetalle<Usuario, uint>
{
    public void Compra(uint idusuario, decimal cantidad, uint idmoneda)
    {}

    public void Vender(uint idusuario, decimal cantidad, uint idmoneda)
    {}

    public void Ingreso(uint idusuario, decimal saldo)
    {}
    public void Transferencia( uint idmoneda, decimal cantidad, uint idusuarioTransfiere, uint idusuarioTransferido)
    {}

    public IEnumerable<Usuario> ObtenerPorCondicion (string condicion);

    public IEnumerable<UsuarioMoneda> ObtenerUsuarioMoneda();

    public IEnumerable<UsuarioMoneda> ObtenerPorCondicionUsuarioMoneda (uint? userid, decimal? cantidad);

    public Usuario? DetalleCompleto(uint idUsuario);

    Task CompraAsync(uint idusuario, decimal cantidad, uint idmoneda);
    Task VenderAsync(uint idusuario, decimal cantidad, uint idmoneda);
    Task IngresoAsync(uint idusuario, decimal saldo);
    Task TransferenciaAsync(uint idmoneda, decimal cantidad, uint idusuarioTransfiere, uint idusuarioTransferido);
    Task<IEnumerable<Usuario>> ObtenerPorCondicionAsync(string condicion);
    Task<IEnumerable<UsuarioMoneda>> ObtenerUsuarioMonedaAsync();
    Task<IEnumerable<UsuarioMoneda>> ObtenerPorCondicionUsuarioMonedaAsync(uint? userid, decimal? cantidad);
    Task<Usuario?> DetalleCompletoAsync(uint idUsuario);
}
