using BlApi;
using BlImplementation;
using BO;
using DalApi;

namespace BlTest;

internal class Program
{
    static IBl? bl = BlApi.Factory.get();

    static BO.Cart cart = new BO.Cart
    {
        Items = new List<BO.OrderItem>(),
        TotalPrice = 0
    }; // creat an empty cart to ran of him the all program.

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine(@"Please select an entity to check.
press 1 to check a product entity,
press 2 to check an order entity,
press 3 to check a cart entity.
press 0 to exit.
");

            switch ((MainMenu)IntTryParse())
            {
                case MainMenu.ProductCheck:

                    ProductChecking();

                    break;

                case MainMenu.OrderCheck:

                    OrderChecking();

                    break;

                case MainMenu.CartCheck:

                    CartChecking();

                    break;

                case MainMenu.Exit:
                    return;

                default:
                    Console.WriteLine("invail input, Please try again");
                    break;
            }
        }
    } // End the main.

    //------------------------------- entity checking ---------------------------

    /// <summary>
    /// func to check the product entity functions.
    /// </summary>
    static void ProductChecking()
    {
        while (true)
        {
            Console.WriteLine(@"
Please select a function to check.
press 1 to get a product (manager).
press 2 to get a product item (client).
press 3 to get a list of products (productForList).
press 4 to add a product.
press 5 to remove a product.
press 6 to update a product.
press 0 to exsit.
");
            try
            {
                switch ((ProductMenu)IntTryParse())
                {
                    case ProductMenu.GetManager:
                        Console.WriteLine("please enter a product id.");
                        Console.WriteLine(bl?.Product.ProductDetailsManger(IntTryParse()));

                        break;

                    case ProductMenu.GetClient:
                        Console.WriteLine("please enter a product id.");
                        Console.WriteLine(bl?.Product.ProductDetailsClient(cart, IntTryParse()));

                        break;

                    case ProductMenu.GetList:
                        foreach (var product in bl?.Product.ProductListRequest()!)
                        {
                            Console.WriteLine(product);
                        }

                        break;

                    case ProductMenu.Add:
                        bl?.Product.AddProduct(GetProduct());

                        break;

                    case ProductMenu.Remove:
                        Console.WriteLine("please enter a product id.");
                        bl?.Product.RemoveProduct(IntTryParse());

                        break;

                    case ProductMenu.Update:
                        bl?.Product.UpdateProduct(GetProduct());

                        break;

                    case ProductMenu.Exit:
                        return;

                    default:
                        Console.WriteLine("invail input, Please try again");

                        break;
                }
            }
            catch (BO.BlExceptions ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    /// <summary>
    /// func to check the order entity functions.
    /// </summary>
    static void OrderChecking()
    {
        while (true)
        {
            Console.WriteLine(@"
Please select a function to check.
press 1 to get an order by order id.
press 2 to update the shiping date.
press 3 to update the delivery date.
press 4 to check the order tracking.
press 5 to get a list of orders.
press 6 to change the order (only manager).
press 0 to exit.
");
            try
            {
                switch ((OrderMenu)IntTryParse())
                {

                    case OrderMenu.Get:
                        Console.WriteLine("please enter an order id.");
                        Console.WriteLine(bl?.Order.GetDetailsOrder(IntTryParse()));

                        break;

                    case OrderMenu.ShipingUpdate:
                        Console.WriteLine("please enter an order id.");
                        Console.WriteLine(bl?.Order.OrderShippingUpdate(IntTryParse()));

                        break;

                    case OrderMenu.DeliveryUpdate:
                        Console.WriteLine("please enter an order id.");
                        Console.WriteLine(bl?.Order.OrderDeliveryUpdate(IntTryParse()));

                        break;

                    case OrderMenu.OrderTracking:
                        Console.WriteLine("please enter an order id.");
                        Console.WriteLine(bl?.Order.OrderTracking(IntTryParse()));

                        break;

                    case OrderMenu.GetList:
                        foreach (var item in bl?.Order.OrderForListRequest()!)
                        {
                            Console.WriteLine(item);
                        }

                        break;

                    case OrderMenu.OrderUpdate:
                        Console.WriteLine("please enter an order id, product id and the new amount.");
                        bl?.Order.OrderUpdate(IntTryParse(), IntTryParse(), IntTryParse());

                        break;

                    case OrderMenu.Exit:
                        return;

                    default:
                        Console.WriteLine("invail input, Please try again");

                        break;
                }
            }
            catch (BO.BlExceptions ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    /// <summary>
    /// func to check the cart entity functions.
    /// </summary>
    static void CartChecking()
    {
        while (true)
        {
            Console.WriteLine(@"
Please select a function to check.
press 1 to add a product in cart.
press 2 to do change in cart.
press 3 to confirm the cart.
press 4 to reset the cart.
press 0 to exsit.
");
            try
            {
                switch ((CartMenu)IntTryParse())
                {
                    case CartMenu.Add:

                        Console.WriteLine("please enter a product id.");
                        Console.WriteLine(bl?.Cart.AddToCart(cart, IntTryParse()));

                        break;

                    case CartMenu.Update:

                        Console.WriteLine("please enter a product id and the amount that you want.");
                        Console.WriteLine(bl?.Cart.ProductUpdateCart(cart, IntTryParse(), IntTryParse()));

                        break;

                    case CartMenu.Confirm:

                        Console.WriteLine("please enter a customer name, mail and adress.");
                        cart.CustomerName= Console.ReadLine();
                        cart.CustomerEmail= Console.ReadLine();
                        cart.CustomerAdress = Console.ReadLine(); 
                        bl?.Cart.ConfirmationOrderToCart(cart);

                        break;

                    case CartMenu.Reset:

                        bl?.Cart.ResetCart(cart);

                        break;

                    case CartMenu.Exsit:

                        return;

                    default:
                        Console.WriteLine("invail input, Please try again");

                        break;
                }
            }
            catch (BO.BlExceptions ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    //------------------------- help functions for TryParse ------------------------

    /// <summary>
    /// the func TryParse for int.
    /// </summary>
    /// <returns></returns>
    static int IntTryParse()
    {
        int result = 0;
        bool succes = false;
        while (!succes)
        {
            succes = int.TryParse(Console.ReadLine(), out result);
            if (!succes)
                Console.WriteLine("invail input, Please try again");
        }
        return result;
    }

    /// <summary>
    /// the func TryParse for double.
    /// </summary>
    /// <returns></returns>
    static double DoubleTryParse()
    {
        double result = 0;
        bool succes = false;
        while (!succes)
        {
            succes = double.TryParse(Console.ReadLine(), out result);
            if (!succes)
                Console.WriteLine("invail input, Please try again");
        }
        return result;
    }

    /// <summary>
    /// the func TryParse for DateTime.
    /// </summary>
    /// <returns></returns>
    static DateTime DateTimeTryParse()
    {
        DateTime result = DateTime.MinValue;
        bool succes = false;
        while (!succes)
        {
            succes = DateTime.TryParse(Console.ReadLine(), out result);
            if (!succes)
                Console.WriteLine("invail input, Please try again");
        }
        return result;
    }

    /// <summary>
    /// the func TryParse for categories.
    /// </summary>
    /// <returns></returns>
    static BO.Categories CategoriesTryParse()
    {
        Console.WriteLine(@"choose the category
0 for simple
1 for electric
2 for SVU
3 for sport
4 for luxury");
        int result = IntTryParse();
        while (result > 4 || result < 0)
        {
            Console.WriteLine("invail input, Please try again");
            result = IntTryParse();
        }
        return (BO.Categories)result;
    }

    //-----------------------------------

    /// <summary>
    /// the function takes data for a product from the user, and return it.
    /// </summary>
    /// <returns></returns>
    static BO.Product GetProduct()
    {
        BO.Product product = new BO.Product();

        Console.WriteLine("please enter the product id.");
        product.Id = IntTryParse();

        Console.WriteLine("please enter the product name");
        product.Name = Console.ReadLine();

        Console.WriteLine("please enter the product price.");
        product.Price = DoubleTryParse();

        product.Category = CategoriesTryParse();

        Console.WriteLine("please enter the amount of the product in stock.");
        product.InStock = IntTryParse();

        return product;
    }

    //---------------------- Enums -------------------------------

    enum MainMenu
    {
        Exit,
        ProductCheck,
        OrderCheck,
        CartCheck,
    }

    enum ProductMenu
    {
        Exit,
        GetManager,
        GetClient,
        GetList,
        Add,
        Remove,
        Update
    }

    enum OrderMenu
    {
        Exit,
        Get,
        ShipingUpdate,
        DeliveryUpdate,
        OrderTracking,
        GetList,
        OrderUpdate,
    }

    enum CartMenu
    {
        Exsit,
        Add,
        Update,
        Confirm,
        Reset
    }

}