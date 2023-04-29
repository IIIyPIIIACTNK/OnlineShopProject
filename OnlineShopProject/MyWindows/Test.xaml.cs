using OnlineShopProject.Pages;
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
    /// Логика взаимодействия для Test.xaml
    /// </summary>
    public partial class Test : Window
    {
        ClientPage clientPage = new ClientPage();
        SalesPage salesPage = new SalesPage();
        ViewModel vm = new ViewModel();
        public Test()
        {
            InitializeComponent();
            DataContext = vm;

            clientPage.DataContext = DataContext;
            salesPage.DataContext = DataContext;

            Mainframe.Content = clientPage;
        }


        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenClientPage(object sender, RoutedEventArgs e)
        {
            if (Mainframe.Content != clientPage)
            {
                Mainframe.Content = clientPage;
            }
        }

        private void OpenSalesWindow(object sender, RoutedEventArgs e)
        {
            if (Mainframe.Content != salesPage)
            {
                Mainframe.Content = salesPage;
            }
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }
    }
}
