using DalApi;
using DO;
using System.Runtime.CompilerServices;

namespace Dal;

internal class DalXmlOrderItem : IOrderItem
{
    string orderItemPath = @"..\xml\orderItems.xml";
    string configPath = @"..\xml\config.xml";

    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(DO.OrderItem newOrderItem)
    {
        newOrderItem.ItemId = XmlTools.GetConfigNumber(configPath, "num_runOrderitem");

        List<OrderItem?> list = XmlTools.LoadListFromXMLSerializer<OrderItem>(orderItemPath);
        list.Add(newOrderItem);

        XmlTools.SaveListToXMLSerializer(list, orderItemPath);

        return newOrderItem.ItemId;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        List<OrderItem?> orderItems = XmlTools.LoadListFromXMLSerializer<OrderItem>(orderItemPath);
        OrderItem? orderItem = existOrderItem(orderItems, id);

        if (orderItem is null)
        {
            throw new DO.IdNotExistException($"Order Item with id: {id} does not exist in database.");
        }

        orderItems.Remove(orderItem);
        XmlTools.SaveListToXMLSerializer(orderItems, orderItemPath);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public DO.OrderItem Get(Func<DO.OrderItem?, bool>? func)
    {
        List<OrderItem?> orderItems = XmlTools.LoadListFromXMLSerializer<OrderItem>(orderItemPath);
        OrderItem? orderItem = orderItems.FirstOrDefault(func!);

        return orderItem ?? throw new EntityNotExistException("There is not an order item that meets these conditions in the database");
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<DO.OrderItem?> GetList(Func<DO.OrderItem?, bool>? func = null)
    {
        List<OrderItem?> orderItems = XmlTools.LoadListFromXMLSerializer<OrderItem>(orderItemPath);
        bool check = func is null;

        return check ? orderItems.Select(item => item) : orderItems.Where(func!);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(DO.OrderItem orderItem)
    {
        Delete(orderItem.ItemId);
        Add(orderItem);
    }

    private OrderItem? existOrderItem(List<OrderItem?> orderItems, int id)
    {
        return orderItems.FirstOrDefault(item => item?.ItemId == id);
    }
}
