

using BO;
using System.Runtime.CompilerServices;

namespace BlImplementation;

internal class Product : BlApi.IProduct
{
    private DalApi.IDal? dal = DalApi.Factory.Get();
    private DO.Product product = new();

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<BO.ProductForList> ProductListRequest(Func<BO.ProductForList?, bool>? func = null)
    {
        IEnumerable<DO.Product?> products = dal?.Product.GetList() ?? throw new BO.DalConfigException("Error in configuration process");

        List<BO.ProductForList> newProductForList = new List<BO.ProductForList>(products.Count());

        bool f = func is null;
        return (from product in products
                select getProductForList(product)).Where(productForList => f ? f : func!(productForList));
    }

    private ProductForList getProductForList(DO.Product? product)
    {
        return new BO.ProductForList()
        {
            Id = (int)product?.ProductId!,
            Name = product?.Name,
            Category = (BO.Categories)product?.Category!,
            Price = (double)product?.Price!,
        };
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Product ProductDetailsManger(int idProduct)
    {
        try
        {
            if (idProduct >= 100000 && idProduct < 1000000)
            {
                product = dal?.Product.Get(productFunc => productFunc?.ProductId == idProduct) ?? throw new BO.DalConfigException("Error in configuration process"); ;

                BO.Product newProduct = new BO.Product
                {
                    Id = product.ProductId,
                    Name = product.Name,
                    Price = product.Price,
                    Category = (BO.Categories)product.Category!,
                    InStock = product.InStock,
                };

                return newProduct;
            }
            else
            {
                throw new BO.IdNotValidException("not valid id for product");
            }
        }
        catch (BO.BlExceptions ex)
        {
            throw ex;
        }
        catch (DO.EntityNotExistException ex)
        {
            throw new BO.EntityNotExistException(ex.Message, ex);
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.ProductItem ProductDetailsClient(BO.Cart newCart, int idProduct)
    {
        BO.ProductItem newProductItem = new();
        try
        {
            if (idProduct >= 100000)
            {
                product = dal?.Product.Get(productFunc => productFunc?.ProductId == idProduct) ?? throw new BO.DalConfigException("Error in configuration process");
                newProductItem.Id = product.ProductId;
                newProductItem.Name = product.Name;
                newProductItem.Price = product.Price;
                newProductItem.Category = (BO.Categories)product.Category!;

                if (product.InStock > 0)
                    newProductItem.InStock = true;
                else
                    newProductItem.InStock = false;

                BO.OrderItem orderItem = newCart.Items!.FirstOrDefault(ProductItem => ProductItem.ProductId == idProduct)!;

                if (orderItem is null)
                    newProductItem.Amount = 0;
                else
                    newProductItem.Amount = orderItem.Amount;
            }
            else // product id is invalid.
            {
                throw new BO.IdNotValidException("not valid id for product");
            }
        }
        catch (BO.BlExceptions ex)
        {
            throw ex;
        }
        catch (DO.EntityNotExistException ex)
        {
            throw new BO.EntityNotExistException(ex.Message, ex);
        }

        return newProductItem;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void AddProduct(BO.Product newProduct)
    {
        try
        {
            if (newProduct.Id < 99999)
                throw new BO.IdNotValidException("The id is too short.");
            if (newProduct.Price <= 0)
                throw new BO.PriceNotValidException("The price is not valid.");
            if (newProduct.Name is null)
                throw new BO.NameNotValidException("The name is empty.");
            if (newProduct.InStock <= 0)
                throw new BO.InStockNotValidException("The amount in stock is not valid.");

            product.ProductId = newProduct.Id;
            product.Name = newProduct.Name;
            product.Price = newProduct.Price;
            product.InStock = newProduct.InStock;
            product.Category = (DO.Categories)newProduct.Category!;

            dal?.Product.Add(product);
        }
        catch (BO.BlExceptions ex)
        {
            throw ex;
        }
        catch (DO.IdExistException ex)
        {
            throw new BO.IdExistException(ex.Message, ex);
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void RemoveProduct(int idProduct)
    {
        try
        {
            if ((dal?.OrderItem.GetList(orderItem => orderItem?.ProductId == idProduct) ?? throw new BO.DalConfigException("Error in configuration process")).Any())
                throw new BO.CanNotRemoveProductException("can't remove the product becouse he is found in exsist orders.");

            else
                dal.Product.Delete(idProduct);
        }
        catch (BO.BlExceptions ex)
        {
            throw ex;
        }
        catch (DO.EntityNotExistException ex)
        {
            throw new BO.EntityNotExistException(ex.Message, ex);
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void UpdateProduct(BO.Product newProduct)
    {
        try
        {
            if (newProduct.Id < 100000)
                throw new BO.IdNotValidException("The id is too short.");
            if (newProduct.Price <= 0)
                throw new BO.PriceNotValidException("The price is not valid.");
            if (newProduct.Name is null)
                throw new BO.NameNotValidException("The name is empty.");
            if (newProduct.InStock <= 0)
                throw new BO.InStockNotValidException("The amount in stock is not valid.");

            product.ProductId = newProduct.Id;
            product.Name = newProduct.Name;
            product.Price = newProduct.Price;
            product.InStock = newProduct.InStock;
            product.Category = (DO.Categories)newProduct.Category!;

            dal?.Product.Update(product);
        }
        catch (BO.BlExceptions ex)
        {
            throw ex;
        }
        catch (DO.EntityNotExistException ex)
        {
            throw new BO.EntityNotExistException(ex.Message, ex);
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<ProductForList?> GetProductAndOrderByName(IEnumerable<ProductForList?> productForLists)
    {
        return from item in productForLists
               orderby item.Category , item.Price
               select item;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public ProductForList GetProductForList(int id)
        => getProductForList(dal!.Product.Get(p => p?.ProductId == id));
}

