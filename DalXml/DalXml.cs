using DalApi;

namespace Dal;

sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();

    private DalXml() { }

    public IProduct Product { get; } = new DalXmlProduct();

    public IOrder Order { get; } = new DalXmlOrder();

    public IOrderItem OrderItem { get; } = new DalXmlOrderItem();
}
