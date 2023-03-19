

using BO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BlImplementation;

internal class Order : BlApi.IOrder
{
    private DalApi.IDal? dal = DalApi.Factory.Get();

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order GetDetailsOrder(int idOrder)
    {
        if (idOrder > 0) // vaild id.
        {
            try
            {
                DO.Order order = dal?.Order.Get(orderFunc => orderFunc?.Id == idOrder) ?? throw new BO.DalConfigException("Error in configuration process"); // asking for a DO order with this order id.
                var (items, sum) = getOrders(order.Id);

                BO.Order retOrdrt = new BO.Order
                {
                    Id = order.Id,
                    CustomerName = order.CustomerName,
                    CustomerEmail = order.CustomerEmail,
                    CustomerAdress = order.CustomerAdress,
                    OrderDate = order.OrderDate,
                    Status = GetStatus(order),
                    ShipDate = order.ShipDate,
                    DeliveryDate = order.DeliveryDate,

                    Items = items.Select(_orderItem =>
                    {
                        DO.OrderItem orderItem = (DO.OrderItem)_orderItem!;

                        return new BO.OrderItem
                        {
                            OrderId = orderItem.OrderId,
                            ProductId = orderItem.ProductId,
                            Name = dal.Product.Get(orderItemFunc => orderItemFunc?.ProductId == orderItem.ProductId).Name,
                            Price = orderItem.Price,
                            Amount = orderItem.Amount,
                            TotalPrice = orderItem.Price * orderItem.Amount
                        };

                    }).ToList(),

                    TotalPrice = (double)sum!,
                };

                return retOrdrt;
            }
            catch (DO.EntityNotExistException ex)
            {
                throw new BO.EntityNotExistException($"Order with id: {idOrder} doesn't exsist in data source", ex);
            }
        }
        else // unvalide id
        {
            throw new BO.IdNotValidException("not valid id for order");
        }
    }

    private (IEnumerable<DO.OrderItem?>, double?) getOrders(int orderId)
    {
        IEnumerable<DO.OrderItem?> orderItems = dal?.OrderItem.GetList(orderItem => orderItem?.OrderId == orderId) ?? throw new BO.DalConfigException("Error in configuration process");
        return (orderItems, orderItems.Sum(orderItem => orderItem?.Amount * orderItem?.Price));
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<BO.OrderForList> OrderForListRequest()
    {
        try
        {
            return dal?.Order.GetList().Select(order =>
            {
                DO.Order _order = (DO.Order)order!;
                var (items, sum) = getOrders(_order.Id);

                return new BO.OrderForList
                {
                    OrderId = _order.Id,
                    CustomerName = _order.CustomerName,
                    Status = GetStatus(_order),
                    AmountOfItems = items.Count(),
                    TotalPrice = (double)sum!,
                };

            }).ToList() ?? throw new BO.DalConfigException("Error in configuration process");
        }
        catch (BO.BlExceptions ex)
        {
            throw ex;
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order OrderShippingUpdate(int idOrder)
    {
        try
        {
            BO.Order boOrder = GetDetailsOrder(idOrder);

            if (boOrder.Status is BO.OrderStatus.Confirmed)
            {
                DO.Order order = dal?.Order.Get(orderFunc => orderFunc?.Id == idOrder) ?? throw new BO.DalConfigException("Error in configuration process"); ;
                order.ShipDate = DateTime.Now;

                dal.Order.Update(order);

                boOrder.ShipDate = DateTime.Now;
                boOrder.Status = OrderStatus.Shipied;
                return boOrder;
            }
            else
            {
                throw new BO.StatusErrorException($"can't change the status to shiped becouse the status is {boOrder.Status}");
            }
        }
        catch (BO.BlExceptions ex)
        {
            throw ex;
        }
        catch (DO.EntityNotExistException ex)
        {
            throw new BO.EntityNotExistException(ex.Message, ex);
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order OrderDeliveryUpdate(int idOrder)
    {
        try
        {
            BO.Order boOrder = GetDetailsOrder(idOrder);

            if (boOrder.Status == BO.OrderStatus.Shipied)
            {
                DO.Order order = dal?.Order.Get(orderFunc => orderFunc?.Id == idOrder) ?? throw new BO.DalConfigException("Error in configuration process"); ;

                order.DeliveryDate = DateTime.Now;
                dal.Order.Update(order);

                boOrder.DeliveryDate = DateTime.Now;

                boOrder.Status = OrderStatus.Deliveried;

                return boOrder;
            }
            else
            {
                throw new BO.StatusErrorException($"can't change the status to deliveried becouse the status is {boOrder.Status}");
            }
        }
        catch (BO.BlExceptions ex)
        {
            throw ex;
        }
        catch (DO.EntityNotExistException ex)
        {
            throw new BO.EntityNotExistException(ex.Message, ex);
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.OrderTracking OrderTracking(int idOrder)
    {
        try
        {
            DO.Order order = dal?.Order.Get(orderFunc => orderFunc?.Id == idOrder) ?? throw new BO.DalConfigException("Error in configuration process"); ;
            OrderTracking orderTracking = helpOrderTracking(order);

            return orderTracking;
        }
        catch (DO.EntityNotExistException ex)
        {
            throw new BO.EntityNotExistException(ex.Message, ex);
        }
    }

    private OrderTracking helpOrderTracking(DO.Order order)
    {
        BO.OrderTracking orderTracking = new BO.OrderTracking
        {
            OrderId = order.Id,
            Status = GetStatus(order),
            TuplesOfDateAndDescription = new List<Tuple<DateTime?, string?>>()
        };

        switch (orderTracking.Status)
        {
            case BO.OrderStatus.Confirmed:

                orderTracking.TuplesOfDateAndDescription.Add(Tuple.Create(order.OrderDate, "The order has been created")!);
                break;

            case BO.OrderStatus.Shipied:

                orderTracking.TuplesOfDateAndDescription.Add(Tuple.Create(order.OrderDate, "The order has been created")!);
                orderTracking.TuplesOfDateAndDescription.Add(Tuple.Create(order.ShipDate, "The order has been sent")!);
                break;

            case BO.OrderStatus.Deliveried:
                orderTracking.TuplesOfDateAndDescription.Add(Tuple.Create(order.OrderDate, "The order has been created")!);
                orderTracking.TuplesOfDateAndDescription.Add(Tuple.Create(order.ShipDate, "The order has been sent")!);
                orderTracking.TuplesOfDateAndDescription.Add(Tuple.Create(order.DeliveryDate, "The order is deliveried")!);
                break;

            default:
                break;
        }

        return orderTracking;
    }

    /// <summary>
    /// help function to colculate the order status of a given DO order.
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    private BO.OrderStatus GetStatus(DO.Order order)
    {
        if (order.DeliveryDate != null)
            return BO.OrderStatus.Deliveried;

        if (order.ShipDate != null)
            return BO.OrderStatus.Shipied;

        return BO.OrderStatus.Confirmed;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order OrderUpdate(int orderId, int productId, int newAmount)
    {
        try
        {
            if (GetStatus(dal?.Order.Get(orderFunc => orderFunc?.Id == orderId) ?? throw new BO.DalConfigException("Error in configuration process")) != BO.OrderStatus.Confirmed) // check if the order is'n sent.
                throw new BO.StatusErrorException("can't updating the order becouse it's alredy sent.");

            if (!dal.Order.GetList().ToList().Any(order => order?.Id == orderId)) // check if it exsit an order with this id.
                throw new BO.EntityNotExistException($"There is no order with such id: {orderId}");

            DO.OrderItem orderItem = dal.OrderItem.Get(item => item?.OrderId == orderId && item?.ProductId == productId);

            DO.Product product = dal.Product.Get(productFunc => productFunc?.ProductId == productId);

            if (orderItem.Equals(default(DO.OrderItem))) // there is'n an order item with these ids, than add a new order item with this produc
            {
                if (product.InStock < newAmount) // check if there is enough in stock
                    throw new BO.NotExsitInStockException("the product is out of stock");

                orderItem.OrderId = orderId;
                orderItem.ProductId = product.ProductId;
                orderItem.Amount = newAmount;
                orderItem.Price = product.Price * newAmount;

                dal.OrderItem.Add(orderItem);

                product.InStock = product.InStock - newAmount;
            }
            else // the product alredy exist in the order, than 
            {
                if (newAmount > orderItem.Amount && product.InStock < newAmount) // check if there is enough in stock
                    throw new BO.NotExsitInStockException("the product is out of stock for this amount, you can try again with less amount");

                product.InStock -= newAmount - orderItem.Amount;

                orderItem.Amount = newAmount;

                if (newAmount == 0)
                {
                    dal.OrderItem.Delete(orderItem.ItemId);
                }
                else
                {
                    dal.OrderItem.Update(orderItem);
                }

            }

            dal.Product.Update(product);
            return GetDetailsOrder(orderId);
        }
        catch (BO.BlExceptions ex)
        {
            throw ex;
        }
        catch (DO.EntityNotExistException ex)
        {
            throw new BO.EntityNotExistException(ex.Message, ex);
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<OrderForList?> GetOrderAndOrderByName(IEnumerable<OrderForList?> orderForLists)
    {
        return from item in orderForLists
               orderby item.CustomerName
               select item;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<BO.OrderStatistics> GetOrderStatiscs(IEnumerable<OrderForList?> orderForLists)
    {
        return from item in orderForLists
               group item by item.Status into j
               select new OrderStatistics
               {
                   Status = j.Key,
                   CountSumStatus = j.Count()
               };
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public int? nextOrderSending()
    {
        try
        {
            IEnumerable<OrderTracking> orderTrackings = dal!.Order.GetList(order => order?.DeliveryDate is null).Select(order => helpOrderTracking(order.GetValueOrDefault()));
            return orderTrackings.MinBy(orderTracking =>
                                        orderTracking.TuplesOfDateAndDescription![orderTracking.TuplesOfDateAndDescription.Count() - 1].Item1
                                        .GetValueOrDefault())?.OrderId;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}