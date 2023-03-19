using DO;
using System.Xml.Linq;

namespace Dal;

internal class Program
{
    public static readonly Random random = new Random(DateTime.Now.Millisecond);

    /// <summary>
    /// _products array in Data source.
    /// </summary>
    internal static List<Product?> _products { set; get; } = new List<Product?>(50);

    /// <summary>
    /// _orders array in Data source.
    /// </summary>
    internal static List<Order?> _orders { set; get; } = new List<Order?>(100);

    /// <summary>
    /// Order Items array in Data source.
    /// </summary>
    internal static List<OrderItem?> _orderItems { set; get; } = new List<OrderItem?>(100);

    internal static XElement _element = new XElement("config", new XElement("num_runOrder", 0), new XElement("num_runOrderitem", 0));

    static void Main(string[] args)
    {


        _element.Save(@"..\xml\config.xml");

        s_Initialize();

        Console.WriteLine("success build and feel the files");
    }


    private static void s_Initialize()
    {
        s_Initialize_product();
        XmlTools.SaveListToXMLSerializer(_products, @"..\xml\products.xml");

        s_Initialize_order();
        XmlTools.SaveListToXMLSerializer(_orders, @"..\xml\orders.xml");

        s_Initialize_orderitem();
        XmlTools.SaveListToXMLSerializer(_orderItems, @"..\xml\orderItems.xml");
    }


    public static void s_Initialize_product()
    {
        Product product = new Product();
        int[] IDarray = new int[10];

        string[][] productNames = new string[5][] {
            new string[] { "Toyota Corolla","Toyota Yaris","Hyundai i40" ,"Hyundai Ioniq","Mazda 3","SEAT Ibiza","SEAT Leon","Skoda Rapid","Skoda Octavia","Ford Focus"},
            new string[] { "Geely Geometric","MG ZS EV", "Hyundai buys","Tesla Model 3" ,"Tesla Model y","Tesla Model s","Tesla Model x","Aiways U5","Kia Niro Plus","Hyundai Ioniq 5"},
            new string[] { "Toyota Rav 4","Toyota Highlander","Mercedes GLB Class","Jeep Compass","Jeep Wrangler" ,"Cadillac Escalade","Volvo XC90","Volvo XC60","Porsche Macan","Porsche Cayenne"},
            new string[] { "Chevrolet Camaro", "Ford Mustang", "Audi A3" ,"Audi TT","Volkswagen - Polo","Nissan GT-R","Alfa Romeo Giulia","mini cooper","Cupra Leon","Maserati MC20"},
            new string[] { "Audi A8", "Volvo S90", "Genesis G80", "BMW 5 Series", "Bentley Flying Spur", "Cadillac CT5", "BYD here", "BMW 7 Series", "Mercedes S", "Audi A7" }
        };

        int numberOfCategories = productNames.Count();

        int[,] ProductPrice = new int[5, 10]{
        { 155138,114527, 150000, 162800, 148500, 103900, 134900, 110000, 178990,144900 },
        { 142900, 144981, 152000, 299469, 371946, 825418, 848590, 162500, 135000, 199900 },
        { 220000, 289900, 349900, 189900, 289000, 899990, 770900, 540000, 577900,715000},
        { 430000, 320000, 368000, 602900,189000, 880000, 699900, 195900, 239000,1880000 },
        { 925000, 469900, 349000,588000, 1745000,359900,299000, 895000, 1450000, 634400 }
        };

        int[] productInStock = { 0, 0, 0, 125, 9, 178, 150, 209, 248, 39, 88, 63, 91, 63, 49, 76, 18 };

        for (int i = 0; i < numberOfCategories; i++)
        {
            product.Category = (Categories)i;
            foreach (var productName in productNames[i])
            {
                int num = random.Next(5);
                int tempID = random.Next(100000, 999999);
                if (IDarray.Contains(tempID))
                    continue;
                IDarray[i] = tempID;
                product.ProductId = tempID;
                int temprandom = random.Next(10);
                product.Name = productName;
                product.Price = ProductPrice[num, temprandom];
                product.InStock = productInStock[random.Next(productInStock.Length)];

                _products.Add(product);
            }
        }
    }

    public static void s_Initialize_order()
    {
        string configPath = @"..\xml\config.xml";
        string elementRunNumOrder = "num_runOrder";

        string[] customer_Name =
        {
            "Augustine Fiddeman","Richardo McPeice","Stanton Bilton","Joela Prandoni",
            "Michal Nodin","Caty Rannigan","Gaylor Drewes","Guillemette Purtell",
            "Analiese Prosser","Nestor Oakeby","Farlay Kaas","Evvie Hartell",
            "Evvie Hartell","Margarette Parfrey","Spenser Erdis","Jaymie Wilds",
            "Percival Hardes","Maria Tommis","Andrea Brooksbank","Kellyann Peckett",
            "Wynnie Broadbent","Nadia Leibold","Averell Aizlewood"
        };
        string[] customer_Email =
        {
            "gpurtell2w@pcworld.com","aprosser2x@wisc.edu","jprandoni2s@sciencedaily.com","mnodin2t@usgs.gov",
            "crannigan2u@forbes.com","gdrewes2v@walmart.com","gpurtell2w@pcworld.com","gpurtell2w@pcworld.com",
            "fkaas2z@mtv.comr","ehartell30@dion.ne.jp","mparfrey31@seesaa.net","serdis32@flickr.com",
            "jwilds33@meetup.com","phardes34@narod.ru","mtommis35@dyndns.org","abrooksbank36@de.vu",
            "kpeckett37@csmonitor.com","wbroadbent38@tiny.cc","holagen39@yellowbook.com","kchastneyqq@seattletimes.com",
            "jprandoni2s@sciencedaily.com","aashe2m@com.com","mdonalsonqt@arstechnica.com"
        };
        string[] customer_Adress =
        {
            "8 Elgar Road","925 Kedzie Court","5614 Derek Lane","0476 Gateway Avenue",
            "05 Blue Bill Park Park","51426 Sutteridge Lane","9186 Schiller Drive","864 Colorado Circle",
            "51426 Sutteridge Lane","3534 Carey Street","8735 Cambridge Circle","1 Thackeray Circlem",
            "199 Manitowish Place","5614 Derek Lane","954 5th Hill","2949 Oak Valley Pass",
            "4906 Service Hill","2756 Main Point","783 Butterfield Circle","5 International Alley",
            "13191 Kim Drive","6282 Calypso Place","7736 Jenna Center"
        };

        TimeSpan timeSpan;
        int tempRandom;

        for (int i = 0; i < 12; i++) // orders with ship date and delivery date
        {
            timeSpan = new TimeSpan(random.Next(30, 90), random.Next(24), random.Next(60));
            tempRandom = random.Next(customer_Name.Length);

            Order order = new Order
            {
                CustomerName = customer_Name[tempRandom],
                CustomerEmail = customer_Email[tempRandom],
                CustomerAdress = customer_Adress[tempRandom],
                OrderDate = DateTime.Now.AddDays(random.Next(-30, -5)) - timeSpan,
            };

            order.Id = XmlTools.GetConfigNumber(configPath, elementRunNumOrder);

            timeSpan = new TimeSpan(random.Next(60), random.Next(24), random.Next(60));
            order.ShipDate = order.OrderDate + timeSpan;

            timeSpan = new TimeSpan(random.Next(30, 280), random.Next(24), random.Next(60));
            order.DeliveryDate = order.ShipDate + timeSpan;

            _orders.Add(order);
        }

        for (int i = 0; i < 4; i++) // orders with ship date and without delivery date
        {
            timeSpan = new TimeSpan(random.Next(30, 90), random.Next(24), random.Next(60));
            tempRandom = random.Next(customer_Name.Length);

            Order order = new Order
            {
                CustomerName = customer_Name[tempRandom],
                CustomerEmail = customer_Email[tempRandom],
                CustomerAdress = customer_Adress[tempRandom],
                OrderDate = DateTime.Now.AddDays(random.Next(-30, -5)) - timeSpan,
                DeliveryDate = null,
            };

            order.Id = XmlTools.GetConfigNumber(configPath, elementRunNumOrder);

            timeSpan = new TimeSpan(random.Next(60), random.Next(24), random.Next(60));
            order.ShipDate = order.OrderDate + timeSpan;

            _orders.Add(order);
        }

        for (int i = 0; i < 4; i++) // orders without ship date and without delivery date
        {
            timeSpan = new TimeSpan(random.Next(30, 90), random.Next(24), random.Next(60));
            tempRandom = random.Next(customer_Name.Length);

            Order order = new Order
            {
                CustomerName = customer_Name[tempRandom],
                CustomerEmail = customer_Email[tempRandom],
                CustomerAdress = customer_Adress[tempRandom],
                OrderDate = DateTime.Now.AddDays(random.Next(-30, -5)) - timeSpan,
                ShipDate = null,
                DeliveryDate = null,
            };

            order.Id = XmlTools.GetConfigNumber(configPath, elementRunNumOrder);

            _orders.Add(order);
        }
    }

    public static void s_Initialize_orderitem()
    {
        int indx;
        string configPath = @"..\xml\config.xml";
        string elementRunNumOrderItem = "num_runOrderitem";

        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < random.Next(1, 5); j++)
            {
                indx = random.Next(10);
                if (_products[indx] is Product product && _orders[i] is Order order)
                {
                    OrderItem item = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = product.ProductId,
                        Price = product.Price,
                        Amount = random.Next(1, 10)
                    };

                    item.ItemId = XmlTools.GetConfigNumber(configPath, elementRunNumOrderItem);

                    _orderItems.Add(item);
                }
            }
        }
    }
}
