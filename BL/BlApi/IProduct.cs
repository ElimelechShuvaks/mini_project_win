using BO;
namespace BlApi;

/// <summary>
/// interface for Product.
/// </summary>
public interface IProduct
{
    /// <summary>
    /// Get list of "ProductForList" for manger and client.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<ProductForList?> ProductListRequest(Func<ProductForList?, bool>? func = null);

    /// <summary>
    /// Get list of ProductForList for manger.
    /// </summary>
    /// <param name="IdProduct"></param>
    /// <returns></returns>
    public Product ProductDetailsManger(int idProduct);

    /// <summary>
    /// Get list of ProductForList for Client
    /// </summary>
    /// <param name="IdProduct"></param>
    /// <returns></returns>
    public ProductItem ProductDetailsClient(Cart cart, int idProduct);

    /// <summary>
    /// Add product.
    /// </summary>
    /// <param name="product"></param>
    public void AddProduct(Product product);

    /// <summary>
    /// Remove product.
    /// </summary>
    /// <param name="product"></param>
    public void RemoveProduct(int idProduct);

    /// <summary>
    ///  Update product.
    /// </summary>
    /// <param name="product"></param>
    public void UpdateProduct(Product product);

    public IEnumerable<ProductForList?> GetProductAndOrderByName(IEnumerable<ProductForList?> productForLists);
    ProductForList GetProductForList(int id);
}
