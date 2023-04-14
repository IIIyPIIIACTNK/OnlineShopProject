﻿using System;
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
    /// Логика взаимодействия для SalesWindow.xaml
    /// </summary>
    public partial class SalesWindow : Window
    {
        ViewModel vm = new ViewModel();
        public SalesWindow()
        {
            InitializeComponent();
            DataContext= vm;
        }
        

        private void OpenAddPurchaseWindow(object sender, RoutedEventArgs e)
        {
            AddPurchaseWindow addPurchaseWindow = new AddPurchaseWindow();
            addPurchaseWindow.Owner= this;
            addPurchaseWindow.DataContext = DataContext;
            addPurchaseWindow.Show();
        }
    }
}
