using System.Formats.Asn1;
using MarketWeight.Core;
using MarketWeight.Core.Persistencia;
using MySqlConnector;

namespace MarketWeight.Ado.Dapper.Test;

public class RepoUsuarioTest : TestBase
{
    IRepoUsuario _repo;
    public RepoUsuarioTest() : base()
        => _repo = new RepoUsuario(Conexion);
    
    [Fact]

    public void TraerOK()
    {
        var usuarios = _repo.Obtener();
        
        Assert.NotEmpty(usuarios);
        Assert.Contains(usuarios,
            m => m.Nombre == "Ana");
    }

    [Fact]

    public void IngresarDineroOK()
    {
        _repo.Ingreso(1, 7707m);
        _repo.Ingreso(2, 420m);
        _repo.Ingreso(3, 5000m);
        _repo.Ingreso(4, 6666m);
        
        var usuario1 = _repo.Detalle(1);
        var usuario2 = _repo.Detalle(2);
        var usuario3 = _repo.Detalle(3);
        var usuario4 = _repo.Detalle(4);
        Assert.True(usuario1 != null && usuario1.Saldo >= 7707m);
        Assert.True(usuario2 != null && usuario2.Saldo >= 420m);
        Assert.True(usuario3 != null && usuario3.Saldo >= 5000m);
        Assert.True(usuario4 != null && usuario4.Saldo >= 6666m);
    }

    [Fact]
    public void AltaUsuarioOK()
    {
        Usuario usuarioWalter = new()
        {
            Nombre = "Walte",
            Apellido = "Beníte",
            Email = "waltercoocker@gmail.com",
            Password = "314159265358979"
        };

        Usuario usuarioJorge = new()
        {
            Nombre = "Jorge",
            Apellido = "Casco",
            Email = "JorgeCasco@gmail.com",
            Password = "jorge123"
        };

        Usuario usuarioGuido = new()
        {
            Nombre = "Guido",
            Apellido = "Gavilán",
            Email = "guidopepin@gmail.com",
            Password = "guidopepin123"
        };

        Usuario usuarioCarlos = new()
        {
            Nombre = "Carlos",
            Apellido = "Bello",
            Email = "carloselbello@gmail.com",
            Password = "carlos123"
        };
        _repo.Alta(usuarioWalter);
        _repo.Alta(usuarioJorge);
        _repo.Alta(usuarioGuido);
        _repo.Alta(usuarioCarlos);

        var usuarios = _repo.Obtener();
        
        Assert.Contains(usuarios, u => u.Email == "waltercoocker@gmail.com");
        Assert.Contains(usuarios, u => u.Email == "JorgeCasco@gmail.com");
        Assert.Contains(usuarios, u => u.Email == "guidopepin@gmail.com");
        Assert.Contains(usuarios, u => u.Email == "carloselbello@gmail.com");
    }

    [Fact]
    public void ComprarMonedaOK()
    {
        _repo.Compra(3, 0.5m, 2);
        _repo.Compra(2, 1m, 3);
        _repo.Compra(2, 0.5m, 1);
        
        var monedasUsuario3 = _repo.ObtenerPorCondicionUsuarioMoneda(3, null);
        var monedasUsuario2 = _repo.ObtenerPorCondicionUsuarioMoneda(2, null);
        Assert.NotEmpty(monedasUsuario3);
        Assert.NotEmpty(monedasUsuario2);
    }

    [Fact]
    public void ComprarMonedaFail()
    {
        var error =  Assert.ThrowsAny<Exception> (()=>_repo.Compra(6, 0.5m, 1));
        Assert.Contains("Insuficiente", error.Message);
    }

    [Fact]
    public void VenderMonedaOK()
    {
        _repo.Vender(2, 0.1m, 1);
        var monedasUsuario2 = _repo.ObtenerPorCondicionUsuarioMoneda(2, null);
        Assert.NotNull(monedasUsuario2);
    }

    [Fact]
    public void VenderMonedaFail()
    {
        var error =  Assert.ThrowsAny<Exception> (()=>_repo.Vender(5, 0.5m, 2));
        Assert.Contains("Insuficiente", error.Message);

        error =  Assert.ThrowsAny<Exception> (()=>_repo.Vender(6, 0.5m, 5));
        Assert.Contains("Insuficiente", error.Message);

    }

    [Fact]
    public void ObtenerPorCondicionOK()
    {
        var usuarios = _repo.ObtenerPorCondicion("saldo >= 1000");
        Assert.NotEmpty(usuarios);
    }
    [Fact]

    public void TransferenciaOK()
    {
        var usuariosMoneda1 = _repo.ObtenerPorCondicionUsuarioMoneda(2, 0.5m);/*string? userid, decimal cantidad*/
        _repo.Compra(2, 2.5m, 1);

        Assert.NotEmpty(usuariosMoneda1);

        _repo.Transferencia(2, 0.5m, 2, 6);

        var usuariosMoneda2 = _repo.ObtenerPorCondicionUsuarioMoneda(6, 0.5m);
        Assert.NotEmpty(usuariosMoneda2);
    }

        [Fact]
        public void TransferenciaFAIL()
    {

        var error =  Assert.ThrowsAny<Exception> (()=>_repo.Transferencia(2, 0.5m, 8, 6));
        Assert.Equal("Cantidad Insuficiente!", error.Message);

    }

     [Fact]
    public void DetalleCompletoOK()
    {
        var usuario =_repo.DetalleCompleto(1);
        Assert.NotNull(usuario);

    }

    [Fact]
    public void DetalleCompletoBilleteraOK()
    {
        var usuario =_repo.DetalleCompleto(2);
        Assert.NotNull(usuario);
        Assert.NotNull(usuario.Billetera);
        Assert.NotNull(usuario.Transacciones);
        Assert.NotEmpty(usuario.Billetera);
        Assert.NotEmpty(usuario.Transacciones);

    }

}