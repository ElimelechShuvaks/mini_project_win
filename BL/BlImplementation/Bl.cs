using BlApi;
namespace BlImplementation;

/// <summary>
/// inplementation of IBl interface, that holds the 3 main entities Product, Order and Cart.
/// </summary>
sealed internal class Bl : IBl
{
    internal Bl()
    {
        Product = new Product();
        Order = new Order();
        Cart = new Cart();
    }

    public IProduct Product { get; }

    public IOrder Order { get; }

    public ICart Cart { get; }
}
