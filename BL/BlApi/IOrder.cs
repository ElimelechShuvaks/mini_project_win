using BO;
using System.Text.RegularExpressions;

namespace BlApi;

/// <summary>
/// Interface for order.
/// </summary>
public interface IOrder
{
    /// <summary>
    /// takes an order id, search a Suitable DO order and try to return a BO order.
    /// </summary>
    /// <param name="idOrder"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Order GetDetailsOrder(int idOrder);

    /// <summary>
    /// update the shipDate in DO order and also return a BO order.
    /// </summary>
    /// <param name="idOrder"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Order OrderShippingUpdate(int idOrder);

    /// <summary>
    /// update the delivery Date in DO order and also return a BO order.
    /// </summary>
    /// <param name="idOrder"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Order OrderDeliveryUpdate(int idOrder);

    /// <summary>
    /// Order Tracking.
    /// </summary>
    /// <param name="IdOrder"></param>
    /// <returns></returns>
    public OrderTracking OrderTracking(int idOrder);

    /// <summary>
    /// try to take a list of DO orders and return a list of OrderForList.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<OrderForList?> OrderForListRequest();

    //the bonus function.

    /// <summary>
    /// option for the maneger to update an order
    /// </summary>
    /// <param name="idOrder"></param>
    /// <param name="newAmount"></param>
    /// <returns></returns>
    public Order OrderUpdate(int orderId, int productId, int newAmount);

    /// <summary>
    /// The func order the List by name.
    /// </summary>
    /// <param name="orderForLists"></param>
    /// <returns></returns>
    public IEnumerable<OrderForList?> GetOrderAndOrderByName(IEnumerable<OrderForList?> orderForLists);


    /// <summary>
    /// the function Division into groups according to order status
    /// </summary>
    /// <param name="orderForLists"></param>
    /// <returns></returns>
    public IEnumerable< BO.OrderStatistics> GetOrderStatiscs(IEnumerable<OrderForList?> orderForLists);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int? nextOrderSending();
}
