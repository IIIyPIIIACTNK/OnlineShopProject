using System;
using System.Collections.Generic;
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

namespace OnlineShopProject
{
    /// <summary>
    /// Логика взаимодействия для StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        ViewModel vm = new ViewModel();
        public StartWindow()
        {
            InitializeComponent();
        }

        private void CloseAppClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenClientsClick(object sender, RoutedEventArgs e)
        {
            ClientsWindow cw = new ClientsWindow();
            cw.DataContext= vm;
            cw.Show();
            Close();
        }

        private void OpenSalesClick(object sender, RoutedEventArgs e)
        {
            SalesWindow sw = new SalesWindow();
            sw.DataContext= vm;
            sw.Show();
            Close();
        }

        private void OpenStatusWindow(object sender, RoutedEventArgs e)
        {
            StatusWindow sw = new StatusWindow();   
            sw.DataContext= vm;
            sw.Show();
        }
    }
}
