namespace BO;

/// <summary>
/// class for OrderForList.
/// </summary>
public class OrderForList
{

    /// <summary>
    /// id of Order
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// CustomerName of Order
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    /// status of Order
    /// </summary>
    public OrderStatus? Status { get; set; }

    /// <summary>
    /// Amount of items in Order
    /// </summary>
    public int AmountOfItems { get; set; }

    /// <summary>
    /// Total price of Order
    /// </summary>
    public double TotalPrice { get; set; }

    public override string ToString()
    {
        return $@"
Order id: {OrderId}
Customer name: {CustomerName}
Order status: {Status}
Amount of items: {AmountOfItems}
Total price: {TotalPrice}:
";
    }
}
