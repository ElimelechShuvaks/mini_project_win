using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BO;
using System.ComponentModel;

namespace PL;

/// <summary>
/// Interaction logic for CartWindow.xaml
/// </summary>
public partial class CartWindow : Window, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    IBl bl = Factory.get();

    Action action;
    Action closeAction;

    private Cart cart;
    public BO.Cart Cart
    {
        get => cart;
        set { cart = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Cart")); }
    }

    public CartWindow(BO.Cart senderCart, Action senderAction, Action senderCloseAction)
    {
        action = senderAction;
        closeAction = senderCloseAction;
        Cart = senderCart;

        InitializeComponent();
    }

    private void cmdDown_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Button button = (sender as Button)!;
            if (button is not null)
            {
                BO.OrderItem orderItem = (button.DataContext as BO.OrderItem)!;
                if (orderItem is not null)
                {
                    Cart = bl.Cart.ProductUpdateCart(Cart, orderItem.ProductId, orderItem.Amount - 1);
                    Cart.Items = Cart.Items.Select(item => item).ToList();
                    action();
                }
            }
        }
        catch (BlExceptions ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void cmdUp_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Button button = (sender as Button)!;
            if (button is not null)
            {
                BO.OrderItem orderItem = (button.DataContext as BO.OrderItem)!;
                if (orderItem is not null)
                {
                    Cart = bl.Cart.ProductUpdateCart(Cart, orderItem.ProductId, orderItem.Amount + 1);
                    Cart.Items = Cart.Items.Select(item => item).ToList();
                    action();
                }
            }
        }
        catch (BlExceptions ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void del_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Button button = (sender as Button)!;
            if (button is not null)
            {
                BO.OrderItem orderItem = (button.DataContext as BO.OrderItem)!;
                if (orderItem is not null)
                {
                    Cart = bl.Cart.ProductUpdateCart(Cart, orderItem.ProductId, 0);
                    Cart.Items = Cart.Items.Select(item => item).ToList();
                    action();
                }
            }
        }
        catch (BlExceptions ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("Are you sure?\r\nThe information cannot be recovered after this operation.", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning)
            == MessageBoxResult.OK)
        {
            bl.Cart.ResetCart(Cart);
            action();
            Close();
        }
    }

    private void ConfirmButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Cart.CustomerName is not null && Cart.CustomerEmail is not null && Cart.CustomerAdress is not null)
            {
                bl.Cart.ConfirmationOrderToCart(Cart);
                MessageBox.Show("Your order has been successfully received", "Order Confirmation", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                closeAction();
                Close();
            }
            else
                MessageBox.Show("It is not possible to confirm the order without the name, email and address", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        }
        catch (BO.BlExceptions ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
