using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Products;

/// <summary>
/// Interaction logic for orderForListWindow.xaml
/// </summary>
public partial class orderForListWindow : Window, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private IEnumerable<OrderForList> orderForLists;
    public IEnumerable<BO.OrderForList> OrderForLists
    {
        get => orderForLists;
        set { orderForLists = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("OrderForLists")); }
    }

    private IEnumerable<BO.OrderStatistics> orderStatistics;
    public IEnumerable<BO.OrderStatistics> OrderStatistics
    {
        get => orderStatistics;
        set { orderStatistics = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("OrderStatistics")); }
    }

    IBl bl = BlApi.Factory.get();

    public orderForListWindow()
    {
        OrderForLists = bl!.Order.GetOrderAndOrderByName(bl.Order.OrderForListRequest()!.ToList()!)!;

        OrderStatistics = bl?.Order.GetOrderStatiscs(OrderForLists)!;

        InitializeComponent();

        for (int i = 0; i < 3; i++) // include the 3 OrderStatus into the comboBox
        {
            Selector.Items.Add((BO.OrderStatus)i);
        }
        Selector.Items.Add("All orders"); // add a basic condition.
    }

    private void Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (Selector.SelectedItem.ToString() == "All orders")
            OrderForLists = bl!.Order.GetOrderAndOrderByName(bl.Order.OrderForListRequest()!.ToList()!)!;
        else
            OrderForLists = bl!.Order.GetOrderAndOrderByName(bl.Order.OrderForListRequest()!.ToList()!)!.Where(orderStatus => orderStatus!.Status == (OrderStatus)Selector.SelectedItem)!.ToList()!;
    }

    private void OrderListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (OrderListView.SelectedItem is BO.OrderForList orderForList)
        {
            BO.Order order = bl.Order.GetDetailsOrder(orderForList.OrderId);

            Action<BO.OrderStatus?> statusAction = (BO.OrderStatus? status) =>
            {
                orderForList.Status = status;
                OrderForLists = OrderForLists.Select(item => item).ToList();
            };

            Action<BO.Order> orderUpdateAction = (BO.Order order) =>
            {
                orderForList.TotalPrice = order.TotalPrice;
                orderForList.AmountOfItems = order.Items.Count();

                OrderForLists = OrderForLists.Select(item => item).ToList();
            };
            new OrderWindow(order, true, statusAction, orderUpdateAction).ShowDialog();

            OrderStatistics = bl?.Order.GetOrderStatiscs(OrderForLists)!;
        }
    }

}