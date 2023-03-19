using DalApi;
namespace Dal;

sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();

    private DalList()
    {
        Product = new DalProduct();
        Order = new DalOrder();
        OrderItem = new DalOrderitem();
    }

    public IProduct Product { get; }
    public IOrder Order { get; }
    public IOrderItem OrderItem { get; }
}
