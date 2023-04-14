using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data.OleDb;
using System.Threading;
using System.Diagnostics;
using System.Windows;
using System.Data;

namespace OnlineShopProject
{
    class ViewModel : INotifyPropertyChanged
    {
        #region Fields

        SalesDBManager salesDBManager = new SalesDBManager();
        ClientsDBManager clientsDBManager = new ClientsDBManager();
        public DataTable dt = new DataTable();
        
        public event PropertyChangedEventHandler? PropertyChanged;

        RelayCommand? addClinet;
        RelayCommand? deleteClient;
        RelayCommand? removeClinet;
        RelayCommand? addPurachase;
        RelayCommand? deletePurchase;
        RelayCommand? setClientsPurchaseDT;
        #endregion

        #region Properties

        #region View
        /// <summary>
        /// Ссылка на класс-менеджер базой данных продаж
        /// </summary>
        public SalesDBManager SalesDBManager { get { return salesDBManager; } }
        public ClientsDBManager _ClientsDBManager { get { return clientsDBManager; } }
        public DataTable clientsPurchaseDT = new DataTable();
        public DataTable ClientsPurchaseDT  { get{ return clientsPurchaseDT; } set { clientsPurchaseDT = value; OnPropertyChanged(); } }
        #region ClientsDataFromView
        public DataRowView SelectedElement { get;set; }
        public string ClientName { get; set; }
        public string ClientLastName { get; set; }
        public string ClientMiddleName { get; set; }
        public string ClientPhoneNumber { get; set; }
        public string ClientEMail { get; set; }

        #endregion

        #region SalesDataFromView

        public string ItemCode { get; set; }
        public string ItemDescription { get; set; } 

        #endregion

        #endregion

        #region Commands
        public RelayCommand AddClient
        {
            get
            {
                return addClinet ??
                    (addClinet = new RelayCommand(o =>
                    {
                        DataRow dataRow = clientsDBManager.ClientsDBDataTable.NewRow();
                        dataRow["lastName"] = ClientLastName;
                        dataRow["name"] = ClientName;
                        dataRow["middleName"] = ClientMiddleName;
                        dataRow["phoneNumber"] = ClientPhoneNumber;
                        dataRow["eMail"] = ClientEMail;
                        clientsDBManager.ClientsDBDataTable.Rows.Add(dataRow);
                        _ClientsDBManager.InsertClientCommand();
                    }));
            }
        }
        public RelayCommand DeleteClient
        {
            get
            {
                return deleteClient ?? 
                    (deleteClient = new RelayCommand(o =>
                {
                    SelectedElement.Row.Delete();
                    _ClientsDBManager.DeleteClientCommand();
                }));
            }
        }
        public RelayCommand EndEditClient
        {
            get
            {
                return removeClinet ?? (removeClinet = new RelayCommand(o =>
                {
                    
                    clientsDBManager.UpdateClientCommand();
                }));
            }
        }

        public RelayCommand AddPurachase
        {
            get
            {
                return addPurachase ?? (addPurachase= new RelayCommand(o =>
                {
                    salesDBManager.InsertPurchaseCommand(ClientEMail, 
                        salesDBManager.ItemsIdNamesDict.FirstOrDefault(x => x.Value == ItemDescription).Key.ToString(),
                        ItemCode);
                }));
            }
        }

        public RelayCommand DeleteCommand
        {
            get
            {
                return deletePurchase ?? (deletePurchase =
                    new RelayCommand(o =>
                    {
                        SelectedElement.Row.Delete();
                        salesDBManager.DeletePurchaseCommand();
                    }));
            }
        }

        public RelayCommand SetClientsPurchaseDT
        {
            get
            {
                return setClientsPurchaseDT ??= new RelayCommand(o =>
                {
                    try
                    {
                        Debug.WriteLine($"--- {SelectedElement.Row.ItemArray[5]} ---");
                        ClientsPurchaseDT = salesDBManager.SelectClientsPurchase(SelectedElement.Row.ItemArray[5].ToString());
                        ClientsPurchaseDT.EndInit();
                        
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }

                });
            }
        }
        #endregion

        #endregion

        
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
