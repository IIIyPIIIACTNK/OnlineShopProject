using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopProject.MyViewModels
{
    public class ClientsPurchasesViewModel : INotifyPropertyChanged
    {
        string itemName;
        int itemCode;
        public string ItemName { get { return itemName; } set { itemName = value; OnPropertyChanged(); } }
        public int ItemCode { get { return itemCode; } set { itemCode = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
