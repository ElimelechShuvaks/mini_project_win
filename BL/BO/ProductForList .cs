

namespace BO;

public class ProductForList
{

    /// <summary>
    ///  Id of Product.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///  Name of Product.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///  Price of Product.
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    ///  Category of Product.
    /// </summary>
    public Categories? Category { get; set; }

    /// <summary>
    /// Image of product.
    /// </summary>
    //public string? Image { get; set; }

    public override string ToString() => $@"
Product id: {Id}
Name: {Name}
Category: {Category}
Price: {Price}
";
}
