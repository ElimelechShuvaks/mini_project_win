using BlApi;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System.Collections.ObjectModel;

namespace PL.Products;

/// <summary>
/// Interaction logic for ProductForListWindow.xaml
/// </summary>
public partial class ProductForListWindow : Window, INotifyPropertyChanged
{
    IBl bl = Factory.get();

    public event PropertyChangedEventHandler? PropertyChanged;

    private ObservableCollection<BO.ProductForList> productForList;
    public ObservableCollection<BO.ProductForList> ProductForList
    {
        get { return productForList; }
        set { productForList = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("ProductForList")); }
    }

    Action action;

    public ProductForListWindow()
    {
        ProductForList = new ObservableCollection<BO.ProductForList>(bl.Product.ProductListRequest()!);

        InitializeComponent();

        for (int i = 0; i < 5; i++) // include the 5 categories into the comboBox.
        {
            categorySelector.Items.Add((BO.Categories)i);
        }
        categorySelector.Items.Add("All products"); //add a basic condition.

    }

    private void categorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (categorySelector.SelectedItem.ToString() == "All products")
        {
            ProductListView.ItemsSource = ProductForList;
        }
        else
        {
            ProductListView.ItemsSource = from item in ProductForList
                                          where item.Category == (BO.Categories)categorySelector.SelectedItem
                                          select item;
        }
    }

    /// <summary>
    /// Adding a product by the manager
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void addProductButton_Click(object sender, RoutedEventArgs e)
    {
        new ProductWindow(id => ProductForList.Add(bl.Product.GetProductForList(id))).ShowDialog();
    }

    private void ProductListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (ProductListView.SelectedItem is BO.ProductForList productForList)
        {
            new ProductWindow(productForList.Id, id => ProductForList[ProductListView.SelectedIndex] = bl.Product.GetProductForList(id)).ShowDialog();   
        }
    }
}