using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
/// Interaction logic for ProductCatalogWindow.xaml
/// </summary>
public partial class ProductCatalogWindow : Window, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    IBl bl = Factory.get();

    private IEnumerable<ProductItem?> productItems;
    public IEnumerable<ProductItem?> ProductItems
    {
        get => productItems;
        set { productItems = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("ProductItems")); }
    }

    private Cart cart;
    public Cart Cart { get => cart; set { cart = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Cart")); } }

    Action action;

    public ProductCatalogWindow()
    {
        try
        {
            Cart = new Cart { Items = new() };
            ProductItems = bl.Product.GetProductAndOrderByName(bl.Product.ProductListRequest()).Select(item => bl.Product.ProductDetailsClient(Cart, item!.Id));

            InitializeComponent();

            CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.Categories));
        }
        catch (BlExceptions ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ProductItemsListView.ItemsSource = from item in ProductItems
                                           where item.Category == (BO.Categories)CategorySelector.SelectedItem
                                           select item;
        refreshButton.Visibility = Visibility.Visible;
    }

    private void refreshButton_Click(object sender, RoutedEventArgs e)
    {
        ProductItemsListView.ItemsSource = ProductItems;
        refreshButton.Visibility = Visibility.Hidden;
    }

    private void ProductDetails_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (ProductItemsListView.SelectedItem is ProductItem item)
        {
            action = () =>
            {
                if (PropertyChanged != null)
                {
                    ProductItems = ProductItems.Select(item => item);
                    PropertyChanged(this, new PropertyChangedEventArgs("Cart"));
                }
            };

            new ProductItemWindow(item!, Cart, action).ShowDialog(); 
        }
    }

    private void cartButton_Click(object sender, RoutedEventArgs e)
    {
        action = () =>
        {
            if (PropertyChanged != null)
            {
                ProductItems = ProductItems.Select(item => item);
                Cart = Cart;
            }
        };

        Action closeAction = () => { this.Close(); };

        new CartWindow(Cart, action, closeAction).ShowDialog();
    }
}