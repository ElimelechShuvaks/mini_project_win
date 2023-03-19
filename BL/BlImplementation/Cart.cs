using BO;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace BlImplementation;

internal class Cart : BlApi.ICart
{
    DalApi.IDal? dal = DalApi.Factory.Get();

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Cart AddToCart(BO.Cart cart, int idProduct)
    {
        try
        {
            BO.OrderItem? orderItem = cart.Items!.FirstOrDefault(orderItems => orderItems.ProductId == idProduct);

            DO.Product product = dal?.Product.Get(productFunc => productFunc?.ProductId == idProduct) ?? throw new BO.DalConfigException("Error in configuration process");

            if (orderItem is null) //Not exsist in cart.
            {
                if (product.InStock > 0) // the product exsit in stock.
                {
                    orderItem = new BO.OrderItem
                    {
                        ProductId = idProduct,
                        Name = product.Name,
                        Amount = 1,
                        Price = product.Price,
                        TotalPrice = product.Price,
                    };

                    cart.Items!.Add(orderItem);
                }
                else
                {
                    throw new BO.NotExsitInStockException("the product is out of stock");
                }
            }
            else //if exsist in cart.
            {
                if (product.InStock > 0)
                {
                    orderItem.TotalPrice += orderItem.Price;
                    orderItem.Amount += 1;
                }
                else
                {
                    throw new BO.NotExsitInStockException("the product is out of stock");
                }
            }
            cart.TotalPrice += orderItem.Price;
        }
        catch (BO.BlExceptions ex)
        {
            throw ex;
        }
        catch (DO.EntityNotExistException ex)
        {
            throw new BO.EntityNotExistException(ex.Message, ex);
        }

        return cart;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Cart ProductUpdateCart(BO.Cart cart, int idProduct, int newQuantity)
    {
        if (newQuantity < 0)
            throw new BO.NegativeAmountExeption("Negative amount");

        BO.OrderItem? orderItem = cart.Items!.FirstOrDefault(orderItems => orderItems.ProductId == idProduct);

        try
        {
            if (orderItem is not null) // order item exsit in cart
            {
                int temp = (int)dal?.Product.Get(p => p?.ProductId == idProduct).InStock!;

                if (newQuantity > temp)
                    throw new BO.NotExsitInStockException("the product is out of stock");

                if (newQuantity > 0 && newQuantity != orderItem!.Amount && dal.Product.Get(product => product?.ProductId == idProduct).InStock >= newQuantity)
                {
                    cart.TotalPrice = cart.TotalPrice - orderItem!.TotalPrice + orderItem.Price * newQuantity;
                    orderItem.Amount = newQuantity;
                    orderItem.TotalPrice = newQuantity * orderItem.Price;

                    return cart;
                }
                if (newQuantity == 0)
                {
                    cart.TotalPrice -= orderItem!.TotalPrice;
                    cart.Items!.Remove(orderItem);
                    return cart;
                }
            }
            else // order item douse not exsit in cart
            {
                throw new BO.EntityNotExistException("There is no order with this product id");
            }
        }
        catch (BO.BlExceptions ex)
        {
            throw ex;
        }

        return cart;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void ConfirmationOrderToCart(BO.Cart cart)
    {
        try
        {
            var items = cart.Items!;

            if (items.Count() == 0)
                throw new BO.EmptyCartException("can't confirm an empty cart.");

            if (cart.CustomerName is null)
                throw new BO.NameNotValidException("Name required");

            if (cart.CustomerAdress is null)
                throw new BO.EmailNotValidException("An address is required");

            if (! new EmailAddressAttribute().IsValid(cart.CustomerEmail))
                throw new BO.EmailNotValidException("Email is not valid");

            List<DO.Product> products = (from item in items
                                         let product = dal?.Product.Get(productFunc => productFunc?.ProductId == item.ProductId) ?? throw new BO.DalConfigException("Error in configuration process")
                                         select item!.Amount > product.InStock || product.InStock < 0 ?
                                         throw new BO.NotExsitInStockException("the product is out of stock") : product).ToList();

            DO.Order order = new DO.Order
            {
                CustomerName = cart.CustomerName,
                CustomerAdress = cart.CustomerAdress,
                CustomerEmail = cart.CustomerEmail,
                OrderDate = DateTime.Now,
                ShipDate = null,
                DeliveryDate = null,
            };

            int orderNumber = dal?.Order.Add(order) ?? throw new BO.DalConfigException("Error in configuration process");

            var orderItemAndProducts = items.Zip(products!).ToList();

            orderItemAndProducts.ForEach(orderItemAndProduct =>
            {
                var orderItem = orderItemAndProduct.First;
                var product = orderItemAndProduct.Second;

                dal.OrderItem.Add(new DO.OrderItem
                {
                    OrderId = orderNumber,
                    ProductId = orderItem.ProductId,
                    Amount = orderItem.Amount,
                    Price = orderItem.Price,
                });

                product.InStock -= orderItem.Amount;

                dal.Product.Update(product);
            });

            ResetCart(cart); // reset the cart.

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
    public void ResetCart(BO.Cart cart)
    {
        //reset the cart
        cart.Items!.Clear();
        cart.TotalPrice = 0;
    }
}