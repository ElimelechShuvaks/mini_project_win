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
    /// Interaction logic for OrderItemWindow.xaml
    /// </summary>
    public partial class OrderItemWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private BlApi.IBl? bl = BlApi.Factory.get();

        public BO.OrderItem Item { set; get; }

        public Action<BO.Order> Action { set; get; }

        private int newAmount;
        public int NewAmount
        {
            get => newAmount;
            set { newAmount = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("NewAmount")); }
        }

        public OrderItemWindow(BO.OrderItem orderItem, Action<BO.Order> action)
        {
            Item = orderItem;
            Action = action;
            NewAmount = Item.Amount;

            InitializeComponent();
        }

        private void OrderItemWindowButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Order order = bl!.Order.OrderUpdate(Item.OrderId, Item.ProductId, newAmount);
                Action(order);
                Close();
            }
            catch (BlExceptions ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            if (NewAmount > 0)
            {
                NewAmount -= 1;
            }
        }

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            NewAmount += 1;
        }
    }
}
