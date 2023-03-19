using System.Windows;
using PL.Products;

namespace PL
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Click for to show product list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void productClick(object sender, RoutedEventArgs e) => new ProductForListWindow().Show();

        /// <summary>
        /// Click for to show order list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void orderClick(object sender, RoutedEventArgs e) => new orderForListWindow().Show();

    }
}
