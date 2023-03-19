using DalApi;
using DO;

//--------------main------------------------------------------------

int choice = 0;
IDal? dal = Factory.Get();

while (true)
{
    Console.WriteLine(@" for which struct you want a test?
 press 0 to Exsit.
 press 1 to check a Product.
 press 2 to check an Order.
 press 3 to check an Order Item.
"
);
    choice = IntTryParse(ref choice);
    switch ((Menu)choice)
    {
        case Menu.Product:
            choisesProduct(dal ?? throw new DO.DalConfigException("Error in configuration process"));
            break;

        case Menu.Order:
            choises_Order(dal ?? throw new DO.DalConfigException("Error in configuration process"));
            break;

        case Menu.OrderItem:
            choises_Orderitem(dal ?? throw new DO.DalConfigException("Error in configuration process"));
            break;

        case Menu.Exsit:
            return;

        default:
            Console.WriteLine("Invalid selection");
            break;
    }
}

//------------------functions---------------------------------------------


//the func PrintList,Product and Order and orderItem.
void PrintList<T>(IEnumerable<T?> items)
{
    foreach (var item in items)
    {
        Console.WriteLine(item);
        Console.WriteLine();
    }
}

//the func TryParse for int.
int IntTryParse(ref int cin)
{
    bool succes = false;
    while (!succes)
    {
        succes = int.TryParse(Console.ReadLine(), out cin);
        if (!succes)
            Console.WriteLine("invail Cin, Please try again");
    }
    return cin;
}

double DoubleTryParse(ref double cin)
{
    bool succes = false;
    while (!succes)
    {
        succes = double.TryParse(Console.ReadLine(), out cin);
        if (!succes)
            Console.WriteLine("invail Cin, Please try again");
    }
    return cin;
}

//the func checkTryParse for DateTime.
DateTime DateTimeTryParse(ref DateTime cin)
{
    bool succes = false;
    while (!succes)
    {
        succes = DateTime.TryParse(Console.ReadLine(), out cin);
        if (!succes)
            Console.WriteLine("invail Cin, Please try again");
    }
    return cin;
}

//the func CinProduct for func Add and updata.
Product CinProduct(Product product)
{

    int intNum = 0;
    double doubleNum = 0;

    Console.WriteLine("cin id:");
    product.ProductId = IntTryParse(ref intNum);

    Console.WriteLine("cin name:");
    product.Name = Console.ReadLine();

    Console.WriteLine("cin price:");
    product.Price = DoubleTryParse(ref doubleNum);

    Console.WriteLine("cin category:");
    product.Category = (Categories)IntTryParse(ref intNum);

    Console.WriteLine("cin instock:");
    product.InStock = IntTryParse(ref intNum);

    return product;
}

//the func CinOrder for func Add and updata.
Order CinOrder(Order order)
{
    int intNum = 0;
    DateTime dateTime = new DateTime();

    Console.WriteLine("cin id:");
    order.Id = IntTryParse(ref intNum);

    Console.WriteLine("cin name:");
    order.CustomerName = Console.ReadLine();

    Console.WriteLine("cin Email:");
    order.CustomerEmail = Console.ReadLine();

    Console.WriteLine("cin Adress:");
    order.CustomerAdress = Console.ReadLine();

    Console.WriteLine("cin OrderDate:");
    order.OrderDate = DateTimeTryParse(ref dateTime);

    Console.WriteLine("cin ShipDate:");
    order.ShipDate = DateTimeTryParse(ref dateTime);

    Console.WriteLine("cin DeliveryDate:");
    order.DeliveryDate = DateTimeTryParse(ref dateTime);

    return order;
}

//function that receive an Order Item from the user.
OrderItem CinOrderItem()
{
    OrderItem ret = new OrderItem();
    int intNum = 0;
    double doubleNum = 0;

    Console.WriteLine("type an order item Id");
    ret.ItemId = IntTryParse(ref intNum);

    Console.WriteLine("type a Product Id");
    ret.ProductId = IntTryParse(ref intNum);

    Console.WriteLine("type an Order Id");
    ret.OrderId = IntTryParse(ref intNum);

    Console.WriteLine("type an amount of the product");
    ret.Amount = IntTryParse(ref intNum);

    Console.WriteLine("type the price of a product");
    ret.Price = DoubleTryParse(ref doubleNum);

    return ret;
}

//if you choose Product.
void choisesProduct(IDal dal)
{
    int choiseProduct = 0;

    while (true)
    {
        Console.WriteLine(@" what function you want to test?
 press 0 to Exsit.
 press 1 to Add a Product.
 press 2 to Get a specific Product.
 press 3 to Get an Array of Product Item.
 press 4 to ealete an Product Item.
 press 5 to Updata a Product Item.
"
    );
        choiseProduct = IntTryParse(ref choiseProduct);
        Product product = new Product(); // for add and update functions.
        int intNum = 0; // for add, get and delete functions.

        try
        {
            switch ((Functions)choiseProduct)
            {
                case Functions.ToAdd:

                    intNum = dal.Product.Add(CinProduct(product));
                    Console.WriteLine($"Prints the id :{intNum} of the new product");

                    break;

                case Functions.ToGet:

                    Console.WriteLine("cin the id thet you want to Get:");
                    IntTryParse(ref intNum);
                    Console.WriteLine(dal.Product.Get(product => product?.ProductId == intNum));

                    break;

                case Functions.TogetArray:

                    PrintList(dal.Product.GetList());

                    break;

                case Functions.ToDel:

                    Console.WriteLine("cin id for product thet you want to Delete:");
                    dal.Product.Delete(IntTryParse(ref intNum));

                    break;

                case Functions.ToUpdata:

                    dal.Product.Update(CinProduct(product));

                    break;

                case Functions.Exsit:
                    return;

                default:
                    Console.WriteLine("Invalid selection");
                    break;
            }
        }
        catch (IdExistException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (EntityNotExistException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (IdNotExistException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

//if you choose Order.
void choises_Order(IDal dal)
{
    int chice_Order = 0;

    while (true)
    {
        Console.WriteLine(@" what function you want to test?
 press 0 to Exsit.
 press 1 to Add an Order.
 press 2 to Get a specific Order.
 press 3 to Get an Array of _orders .
 press 4 to Delete an Order .
 press 5 to Updata an Order.
"
    );
        chice_Order = IntTryParse(ref chice_Order);
        Order order = new Order();// for add and update functions.
        int intNum = 0; // for add, get and delete functions.
        try
        {
            switch ((Functions)chice_Order)
            {
                case Functions.ToAdd:

                    intNum = dal.Order.Add(CinOrder(order));
                    Console.WriteLine("Prints the id of the new order");
                    Console.WriteLine(intNum);

                    break;

                case Functions.ToGet:

                    Console.WriteLine("cin the id order thet you want to Get:");
                    IntTryParse(ref intNum);
                    Console.WriteLine(dal.Order.Get(order => order?.Id == intNum));

                    break;

                case Functions.TogetArray:

                    IEnumerable<Order?> newList = dal.Order.GetList();
                    PrintList(newList);

                    break;

                case Functions.ToDel:

                    Console.WriteLine("cin id for order thet you want to Delete:");
                    dal.Order.Delete(IntTryParse(ref intNum));

                    break;

                case Functions.ToUpdata:

                    dal.Order.Update(CinOrder(order));

                    break;

                case Functions.Exsit:
                    return;

                default:
                    Console.WriteLine("Invalid selection");
                    break;
            }
        }
        catch (IdExistException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (EntityNotExistException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (IdNotExistException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

void choises_Orderitem(IDal dal)
{
    int orderChice = 0;
    int intNum = 0;
    int intNum2 = 0;
    while (true)
    {
        Console.WriteLine(@" what function you want to test?
 press 0 to Exsit.
 press 1 to Add an Order Item.
 press 2 to Get a specific Order Item.
 press 3 to Get a specific Order Item by Product id and order id.
 press 4 to Get an Array of a specific Order.
 press 5 to Get an Array of all Order Items.
 press 6 to Delete an Order Item.
 press 7 to Updata an Order Item.
"
);

        try
        {
            switch ((OrderItemFunctions)IntTryParse(ref orderChice))
            {
                case OrderItemFunctions.Add:

                    Console.WriteLine(dal.OrderItem.Add(CinOrderItem()));

                    break;

                case OrderItemFunctions.Get:

                    Console.WriteLine("type an Order Item id");
                    IntTryParse(ref intNum);
                    Console.WriteLine(dal.OrderItem.Get(orderItem => orderItem?.ItemId == intNum));

                    break;

                case OrderItemFunctions.GetBy_2Id:
                    Console.WriteLine("type an Order id");
                    IntTryParse(ref intNum);

                    Console.WriteLine("type a Product id");
                    IntTryParse(ref intNum2);

                    Console.WriteLine(dal.OrderItem.Get(o => o?.ProductId == intNum2 && o?.OrderId == intNum));

                    break;

                case OrderItemFunctions.GetItemArray:

                    Console.WriteLine("type an order id of Order item");
                    IntTryParse(ref intNum);
                    PrintList(dal.OrderItem.GetList(o => o?.OrderId == intNum));

                    break;

                case OrderItemFunctions.GetArray:

                    PrintList(dal.OrderItem.GetList());

                    break;

                case OrderItemFunctions.Del:
                    Console.WriteLine("type an Order Item id to Delete");
                    dal.OrderItem.Delete(IntTryParse(ref intNum));

                    break;

                case OrderItemFunctions.Updata:
                    dal.OrderItem.Update(CinOrderItem());

                    break;

                case OrderItemFunctions.Exsit:
                    return;

                default:
                    Console.WriteLine("Invalid selection");
                    break;
            }
        }
        catch (IdExistException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (EntityNotExistException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (IdNotExistException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

//-------------enum--------------------------i 
enum Menu
{
    Exsit,
    Product,
    Order,
    OrderItem
}

enum Functions
{
    Exsit,
    ToAdd,
    ToGet,
    TogetArray,
    ToDel,
    ToUpdata
}

enum OrderItemFunctions
{
    Exsit,
    Add,
    Get,
    GetBy_2Id,
    GetItemArray,
    GetArray,
    Del,
    Updata
}