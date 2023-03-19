using BlApi;
using BO;
using PL.Products;
using System;
using System.ComponentModel;
using System.Windows;


namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    //public event PropertyChangedEventHandler? PropertyChanged;

    public int OrderId { get; set; }

    IBl? bl = Factory.get();

    public MainWindow()
    {
        InitializeComponent();
    }

    private void AdminButton_Click(object sender, RoutedEventArgs e) => new AdminWindow().Show();

    /// <summary>
    /// Order tracking.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ShowOrderTracking(object sender, RoutedEventArgs e)
    {
        try
        {
            if(bl!.Order.GetDetailsOrder(OrderId) is BO.Order)
            {
                new OrderTrackingWindow(OrderId).Show();
            }
        }
        catch (BlExceptions ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    private void NewOrderButton_Click(object sender, RoutedEventArgs e) => new ProductCatalogWindow().ShowDialog();

    private void Simulator(object sender, RoutedEventArgs e)
    {
      new SimulatorWindow.SimulatorWindow().Show();
    }
}

