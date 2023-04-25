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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OnlineShopProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для SalesPage.xaml
    /// </summary>
    public partial class SalesPage : Page
    {
        public SalesPage()
        {
            InitializeComponent();
        }

        private void OpenAddPurchaseWindow(object sender, RoutedEventArgs e)
        {
            AddPurchaseWindow addPurchaseWindow = new AddPurchaseWindow();
            addPurchaseWindow.DataContext = DataContext;
            addPurchaseWindow.Show();
        }
    }
}
