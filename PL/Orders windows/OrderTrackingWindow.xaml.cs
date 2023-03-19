using BlApi;
using BO;
using System;
using System.Collections.Generic;
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

namespace PL.Products
{
    /// <summary>
    /// Interaction logic for OrderTrackingWindow.xaml
    /// </summary>
    public partial class OrderTrackingWindow : Window
    {
        public BO.OrderTracking orderTracking { set; get; }

        IBl bl = BlApi.Factory.get();


        public OrderTrackingWindow(int IdOrder)
        {
            orderTracking = bl.Order.OrderTracking(IdOrder);

            InitializeComponent();
        }

        private void ShowOrderButton(object sender, RoutedEventArgs e)
        {
            BO.Order order = bl.Order.GetDetailsOrder(orderTracking.OrderId);
            new OrderWindow(order, false).Show();
        }
    }
}
