namespace MarketWeight.Core.Persistencia;

public interface IRepoModificar<T>
{
    void Modificar(T elemento);
    Task ModificarAsync(T elemento);
}
