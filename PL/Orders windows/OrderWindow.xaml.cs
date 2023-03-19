using BlApi;
using BO;
using DO;
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

namespace PL.Products
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        bool DontShow;
        private BlApi.IBl? bl = BlApi.Factory.get();

        private BO.Order order;
        public BO.Order Order
        {
            get => order;
            set { order = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Order")); }
        }
        Action<BO.OrderStatus?> statusAction;
        Action<BO.Order> orderUpdateAction;

        public OrderWindow(BO.Order senderOrder, bool Show, Action<BO.OrderStatus?> senderActionStatus = null, Action<BO.Order> senderActionUpdate = null)
        {
            Order = senderOrder;
            DontShow = Show;
            statusAction = senderActionStatus;
            orderUpdateAction = senderActionUpdate;

            InitializeComponent();

            if (DontShow == true)
            {

                if (Order.Status == OrderStatus.Deliveried)
                {
                    ShipDateButton.Visibility = Visibility.Hidden;
                    DeliveryDateButton.Visibility = Visibility.Hidden;
                }
                else if (Order.Status == OrderStatus.Shipied)
                    ShipDateButton.Visibility = Visibility.Hidden;

                else
                    DeliveryDateButton.Visibility = Visibility.Hidden;

            }
            else
            {
                ShipDateButton.Visibility = Visibility.Hidden;
                DeliveryDateButton.Visibility = Visibility.Hidden;
            }
        }

        private void ShipDateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Order = bl!.Order.OrderShippingUpdate(Order.Id);
                ShipDateButton.Visibility = Visibility.Hidden;
                DeliveryDateButton.Visibility = Visibility.Visible;
                statusAction(Order.Status);
            }
            catch (BlExceptions ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeliveryDateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Order = bl!.Order.OrderDeliveryUpdate(Order.Id);
                DeliveryDateButton.Visibility = Visibility.Hidden;
                statusAction(Order.Status);
            }
            catch (BlExceptions ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateItem(object sender, MouseButtonEventArgs e)
        {
            if (DontShow)
            {
                if (OrderItemListView.SelectedItem is BO.OrderItem item)
                {
                    Action<BO.Order> action = (order) => { Order = order; Order.Items = Order.Items.Select(item => item).ToList(); };
                    new OrderItemWindow(item, action).ShowDialog();
                    orderUpdateAction(Order);
                }
            }
        }
    }
}
