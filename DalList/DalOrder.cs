using DalApi;
using DO;
using System.Runtime.CompilerServices;

namespace Dal;

internal class DalOrder : IOrder
{
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(Order newOrder)
    {
        newOrder.Id = DataSource.Num_runOrder;
        DataSource._orders.Add(newOrder);
        return newOrder.Id;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int idNum)
    {
        DataSource._orders.Remove(Get(orderFunc => orderFunc?.Id == idNum));
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Order order)
    {
        int index = existOrder(order.Id);

        if (index != -1)
        {
            DataSource._orders[index] = order;
            return;
        }
        OtherFunctions.exceptionNotFound("order", order.Id);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    private int existOrder(int id)
    {
        int index = DataSource._orders.FindIndex(order => order?.Id == id);
        return index;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public Order Get(Func<Order?, bool>? func)
    {
        Order? order = DataSource._orders.FirstOrDefault(func!);
        if (order is not null)
        {
            return (Order)order;
        }
        throw new EntityNotExistException("There is no order that meets these conditions in the database.");
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Order?> GetList(Func<Order?, bool>? func = null)
    {
        bool check = func is null;
        return check ? DataSource._orders.Select(order => order) : DataSource._orders.Where(func!);
    }
}

