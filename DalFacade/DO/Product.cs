namespace DO;

/// <summary>
/// Details of a product
/// </summary>
public struct Product
{
    /// <summary>
    /// Product id
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// product name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// product price
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// product category
    /// </summary>
    public Categories? Category { get; set; }

    /// <summary>
    /// product image
    /// </summary>
    public string? Image { get; set; }

    /// <summary>
    /// amount in stock
    /// </summary>
    public int InStock { get; set; }

    public override string ToString() => $@"Product ProductId: {ProductId}" + '\n' +
                                            $@"Name: {Name}" + '\n' + 
                                            $@"Category: {Category}" + '\n' +
                                            $@"Price: {Price}" + '\n' +
                                            $@"Amount in stock: {InStock}";}
