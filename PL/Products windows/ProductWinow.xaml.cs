using BlApi;
using BO;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace PL.Products;

/// <summary>
/// Interaction logic for product.xaml
/// </summary>
public partial class ProductWindow : Window
{

    private BlApi.IBl? bl = BlApi.Factory.get();

    Action<int> action;
    /// <summary>
    /// ctor with 1 parameter for add product.
    /// </summary>
    public BO.Product product { get; set; } = new Product();
    public ProductWindow(Action<int> senderAction)
    {

        action = senderAction;
        InitializeComponent();
        UpDate.Visibility = Visibility.Hidden;
        categoryComboBox.ItemsSource = Enum.GetValues(typeof(BO.Categories));
    }

    /// <summary>
    /// ctor with int parameter for update an exist product.
    /// </summary>
    /// <param name="bl"></param>
    /// <param name="ProductId"></param>
    public ProductWindow(int ProductId, Action<int> senderAction)
    {
        action = senderAction;
        product = bl?.Product.ProductDetailsManger(ProductId)!;
        InitializeComponent();
        Add.Visibility = Visibility.Hidden;
        categoryComboBox.ItemsSource = Enum.GetValues(typeof(BO.Categories));
        idTextBox.IsEnabled = false;
    }

    /// <summary>
    /// Button Clic for add.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void addProductButton(object sender, RoutedEventArgs e)
    {
        try
        {
            bl?.Product.AddProduct(product);
            action(product.Id);
            Close();
        }
        catch (BlExceptions ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    /// <summary>
    /// Button Clic for upDate.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void updateProductButton(object sender, RoutedEventArgs e)
    {
        try
        {
            bl?.Product.UpdateProduct(product);
            action(product.Id);
            Close();
        }
        catch (BlExceptions ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// private helpe function to prevent the user to type non number digits in text box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void IntegerValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]");
        e.Handled = regex.IsMatch(e.Text);
    }

    /// <summary>
    /// private helpe function to prevent the user to type non numbers or decimal numbers in text box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DoubleValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9.]");
        e.Handled = regex.IsMatch(e.Text);
    }
}
