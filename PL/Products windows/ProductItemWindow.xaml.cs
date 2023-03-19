using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
/// Interaction logic for ProductItemWindow.xaml
/// </summary>
public partial class ProductItemWindow : Window, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    IBl bl = Factory.get();

    int newAmount;
    public int NewAmount
    {
        get => newAmount;
        set { newAmount = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("NewAmount")); }

    }

    public ProductItem ProductItem { get; set; }
    public Cart Cart { get; set; }
    Action Action { get; set; }

    public ProductItemWindow(ProductItem senderItem, Cart senderCart, Action senderActio)
    {
        ProductItem = senderItem;
        Cart = senderCart;
        Action = senderActio;

        NewAmount = ProductItem.Amount;

        InitializeComponent();
    }

    private void AddCartButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            bl.Cart.AddToCart(Cart, ProductItem.Id);
            Action();
            Close();
        }
        catch (BlExceptions ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void UpdateCartButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            bl.Cart.ProductUpdateCart(Cart, ProductItem.Id, NewAmount);
            Action();
            Close();
        }
        catch (BlExceptions ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void cmdUp_Click(object sender, RoutedEventArgs e)
    {
        NewAmount += 1;
    }

    private void cmdDown_Click(object sender, RoutedEventArgs e)
    {
        if (NewAmount > 0)
            NewAmount -= 1;
    }
}
