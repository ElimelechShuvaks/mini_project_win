namespace BO;


/// <summary>
/// data of Order item.
/// </summary>
public class OrderItem
{
    /// <summary>
    /// id of order item
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// id of product
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Name of product
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Price of product
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// Amount of product
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// TotalPrice of amount of the product
    /// </summary>
    public double TotalPrice { get; set; }

    public override string ToString()
    {
        return $@"
Order Id: {OrderId}
Product Id: {ProductId}
Name :{Name}
Price: {Price}
Amount: {Amount}
Total price: {TotalPrice} ";
    }
}
