namespace DO;

/// <summary>
/// data of Order item.
/// </summary>
public struct OrderItem
{
    /// <summary>
    /// Item id
    /// </summary>
    public int ItemId { get; set; }

    /// <summary>
    /// Product id
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// order id
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// amount of the product in order item
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// price of 1 product
    /// </summary>
    public double Price { get; set; }

    public override string ToString()
    {
        return $@"Order item Id: {ItemId}
Product Id: {ProductId}
Order Id: {OrderId}
Price: {Price}
Amount: {Amount}"; }

}
