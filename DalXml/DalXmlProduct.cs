using DalApi;
using DO;
using System.Runtime.CompilerServices;

namespace Dal;

internal class DalXmlProduct : IProduct
{
    string productPath = @"..\xml\products.xml";

    /// <summary>
    /// add a given produc into the xml database.
    /// </summary>
    /// <returns>
    /// return the product id
    /// </returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(DO.Product newProduct)
    {
        List<Product?> products = XmlTools.LoadListFromXMLSerializer<Product>(productPath);

        if (existProduct(products, newProduct.ProductId) is not null)
        {
            throw new DO.IdExistException($"Product with id: {newProduct.ProductId} already exist in database.");
        }

        products.Add(newProduct);
        XmlTools.SaveListToXMLSerializer(products, productPath);

        return newProduct.ProductId;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        List<Product?> products = XmlTools.LoadListFromXMLSerializer<Product>(productPath);
        Product? product = existProduct(products, id);

        if (product is null)
        {
            throw new DO.IdNotExistException($"Product with id: {id} does not exist in database.");
        }

        products.Remove(product);
        XmlTools.SaveListToXMLSerializer(products, productPath);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public DO.Product Get(Func<DO.Product?, bool>? func)
    {
        List<Product?> products = XmlTools.LoadListFromXMLSerializer<Product>(productPath);
        Product? product = products.FirstOrDefault(func!);

        return product ?? throw new EntityNotExistException("There is no product that meets these conditions in the database");
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<DO.Product?> GetList(Func<DO.Product?, bool>? func = null)
    {
        IEnumerable<Product?> products = XmlTools.LoadListFromXMLSerializer<Product>(productPath);
        bool check = func is null;

        return check ? products.Select(product => product) : products.Where(func!);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(DO.Product newProduct)
    {
        Delete(newProduct.ProductId);
        Add(newProduct);
    }

    private Product? existProduct(List<Product?> products, int id)
    {
        return products.FirstOrDefault(product => product?.ProductId == id);
    }
}
