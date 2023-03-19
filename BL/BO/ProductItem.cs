namespace BO;

/// <summary>
/// claas for Product Item.
/// </summary>
public class ProductItem
{
    /// <summary>
    /// Name of product.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of product.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Price of product.
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// Category of product.
    /// </summary>
    public Categories? Category { get; set; }

    /// <summary>
    /// Image of product.
    /// </summary>
    public string? Image { get; set; }

    /// <summary>
    /// Amount of Product Item
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// Checks whether there is enough in stock or not.
    /// </summary>
    public bool InStock { get; set; }

    public override string ToString() => $@"
Product id: {Id}
Name: {Name}
Category: {Category}
Price: {Price}
Amount in cart:{Amount}
Is in stock: {InStock}
";
}
