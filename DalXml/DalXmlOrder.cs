using DalApi;
using DO;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Xml.Linq;

namespace Dal;

internal class DalXmlOrder : IOrder
{
    string orderPath = @"..\xml\orders.xml";
    string configPath = @"..\xml\config.xml";

    /// <summary>
    /// help function that take an element and return an appropriate order.
    /// </summary>
    static DO.Order? createOrderfromXElement(XElement element)
    {
        Order order = new Order
        {
            Id = Convert.ToInt32(element.Element("Id")!.Value),
            CustomerName = element.Element("CustomerName")!.Value,
            CustomerEmail = element.Element("CustomerEmail")!.Value,
            CustomerAdress = element.Element("CustomerAdress")!.Value,
        };

        var date = element.Element("OrderDate")!.Value;
        order.OrderDate = string.IsNullOrEmpty(date) ? null : Convert.ToDateTime(date);

        date = element.Element("ShipDate")!.Value;
        order.ShipDate = string.IsNullOrEmpty(date) ? null : Convert.ToDateTime(date);

        date = element.Element("DeliveryDate")!.Value;
        order.DeliveryDate = string.IsNullOrEmpty(date) ? null : Convert.ToDateTime(date);

        return order;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(DO.Order order)
    {
        int configRunNum = XmlTools.GetConfigNumber(configPath, "num_runOrder");
        XElement root = XmlTools.LoadListFromXElement(orderPath);

        root.Add(new XElement("Order",
                   new XElement("Id", configRunNum),
                   new XElement("CustomerName", order.CustomerName),
                   new XElement("CustomerEmail", order.CustomerEmail),
                   new XElement("CustomerAdress", order.CustomerAdress),
                   new XElement("OrderDate", order.OrderDate),
                   new XElement("ShipDate", order.ShipDate),
                   new XElement("DeliveryDate", order.DeliveryDate)));

        XmlTools.SaveXElementToXelFile(root, orderPath);

        return configRunNum;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        XElement root = XmlTools.LoadListFromXElement(orderPath);

        XElement? delElement = (from element in root.Elements()
                               where Convert.ToInt32(element.Element("Id")!.Value) == id
                               select element).FirstOrDefault() ?? throw new DO.IdNotExistException("No order exists with such an ID");
        delElement.Remove();

        XmlTools.SaveXElementToXelFile(root, orderPath);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public DO.Order Get(Func<DO.Order?, bool>? func)
    {
        try
        {
            return (from element in XmlTools.LoadListFromXElement(orderPath).Elements()
                    let order = createOrderfromXElement(element)
                    where func!(order)
                    select order).FirstOrDefault()!.Value;
        }
        catch (InvalidOperationException)
        {
            throw new EntityNotExistException("There is no order that meets these conditions in the database.");
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<DO.Order?> GetList(Func<DO.Order?, bool>? func = null)
    {
        if (func == null)
        {
            return from element in XmlTools.LoadListFromXElement(orderPath).Elements()
                   select createOrderfromXElement(element);
        }
        else
        {
            return from element in XmlTools.LoadListFromXElement(orderPath).Elements()
                   let order = createOrderfromXElement(element)
                   where func(order)
                   select order;
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(DO.Order order)
    {
        Delete(order.Id);

        XElement root = XmlTools.LoadListFromXElement(orderPath);
     
        root.Add(new XElement("Order",
                    new XElement("Id", order.Id),
                    new XElement("CustomerName", order.CustomerName),
                    new XElement("CustomerEmail", order.CustomerEmail),
                    new XElement("CustomerAdress", order.CustomerAdress),
                    new XElement("OrderDate", order.OrderDate),
                    new XElement("ShipDate", order.ShipDate),
                    new XElement("DeliveryDate", order.DeliveryDate)));

        XmlTools.SaveXElementToXelFile(root, orderPath);
    }
}
