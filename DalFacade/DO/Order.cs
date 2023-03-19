namespace DO;

/// <summary>
/// struct to represent data of an order
/// </summary>
public struct Order
{
    /// <summary>
    /// order id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Customer name
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    /// Customer email
    /// </summary>
    public string? CustomerEmail { get; set; }

    /// <summary>
    /// Customer adress
    /// </summary>
    public string? CustomerAdress { get; set; }

    /// <summary>
    /// Order date
    /// </summary>
    public DateTime? OrderDate { get; set; }

    /// <summary>
    /// Ship date
    /// </summary>
    public DateTime? ShipDate { get; set; }

    /// <summary>
    /// Delivery date
    /// </summary>
    public DateTime? DeliveryDate { get; set; }

    public override string ToString()
    {
        return $@"Order Number: {Id}
Customer Name: {CustomerName}
Customer Email: {CustomerEmail}
Customer Adress: {CustomerAdress}
Order Date: {OrderDate}
Ship Date: {ShipDate?.ToShortDateString()}
Delivery Date: {DeliveryDate}
";
    }
}
