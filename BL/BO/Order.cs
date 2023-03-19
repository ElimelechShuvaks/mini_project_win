using OtherFunction;
namespace BO;

/// <summary>
/// class for Orders.
/// </summary>

public class Order
{
    /// <summary>
    /// id of order
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// CustomerName of order
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    /// CustomerEmail of order
    /// </summary>
    public string? CustomerEmail { get; set; }

    /// <summary>
    /// CustomerAdress of order
    /// </summary>
    public string? CustomerAdress { get; set; }

    /// <summary>
    /// status of order
    /// </summary>
    public OrderStatus? Status { get; set; }

    /// <summary>
    /// OrderDate of order
    /// </summary>
    public DateTime? OrderDate { get; set; }

    /// <summary>
    /// ShipDate of order
    /// </summary>
    public DateTime? ShipDate { get; set; }

    /// <summary>
    /// DeliveryDate of order
    /// </summary>
    public DateTime? DeliveryDate { get; set; }

    /// <summary>
    /// Items of order
    /// </summary>
    public List<OrderItem>? Items { get; set; }

    /// <summary>
    /// TotalPrice of order
    /// </summary>
    public double TotalPrice { get; set; }

    public override string ToString()
    {
        return $@"
Order Number: {Id}

Customer Name: {CustomerName}
Customer Email: {CustomerEmail}
Customer Adress: {CustomerAdress}

Order status: {Status}

Order Date: {OrderDate}
Ship Date: {ShipDate?.ToShortDateString()}
Delivery Date: {DeliveryDate}

Items:
{Items?.GetToStrings()}

Total price of order: {TotalPrice} ";
    }
}
