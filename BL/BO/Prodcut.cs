
//using Windows.UI.Xaml.Media;

namespace BO;

/// <summary>
/// Main logical entity of a product (Product)
/// - for screens of product details (for management) and operations on a product.
/// </summary>
public class Product
{
    /// <summary>
    /// Product id.
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
    /// InStock in product.
    /// </summary>
    public int InStock { get; set; }


    public override string ToString() => $@"
Product id: {Id}
Name: {Name}
Category: {Category}
Price: {Price}
Amount in stock: {InStock}
";
}
