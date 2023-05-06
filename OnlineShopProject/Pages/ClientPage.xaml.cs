using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OnlineShopProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        public ClientPage()
        {
            InitializeComponent();
        }

        private void MainGridSelectionChaged(object sender, SelectionChangedEventArgs e)
        {
            if (SalesGrid != null)
            {
                SalesGrid.Visibility = Visibility.Collapsed;
            }
            Debug.WriteLine($"--- {ClientsDataGrid.ItemsSource}");
        }

        private void AddMenuClick(object sender, RoutedEventArgs e)
        {
            AddClientWindow addClientWindow = new AddClientWindow();
            addClientWindow.DataContext = DataContext;
            //addClientWindow.Owner = ;
            addClientWindow.Show();
        }

        private void SalesGridShow(object sender, RoutedEventArgs e)
        {
            SalesGrid.Visibility = Visibility.Visible;
        }

        private void SalesGridCloseClick(object sender, RoutedEventArgs e)
        {
            SalesGrid.Visibility = Visibility.Collapsed;
        }
    }
}
