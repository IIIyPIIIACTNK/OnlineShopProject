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

        SqlConnection connectionToOSClients = new SqlConnection();
        SalesDBManager salesDBManager = new SalesDBManager();
        ClientsDBManager clientsDBManager = new ClientsDBManager();
        public DataTable dt = new DataTable();

        public event PropertyChangedEventHandler? PropertyChanged;

        RelayCommand addClinet;
        RelayCommand deleteClient;
        #endregion

        #region Properties

        #region View
        /// <summary>
        /// Состояние подключения к OSClients базе данных для View
        /// </summary>
        public string OSClientsConState { get { return connectionToOSClients.State.ToString(); } }
        /// <summary>
        /// Ссылка на класс-менеджер базой данных продаж
        /// </summary>
        public SalesDBManager SalesDBManager { get { return salesDBManager; } }
        public ClientsDBManager ClientsDBManager { get { return clientsDBManager; } }

        public DataRowView SelectedClient { get;set; }
        public string ClientName { get; set; }
        public string ClientLastName { get; set; }
        public string ClientMiddleName { get; set; }
        public string ClientPhoneNumber { get; set; }
        public string ClientEMail { get; set; }


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
                        ClientsDBManager.UpdateView();
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
                    string g = "";
                    foreach(var item in SelectedClient.Row.ItemArray)
                    {
                        g += item as string;
                    }
                    MessageBox.Show(g);
                    SelectedClient.Row.Delete();
                    ClientsDBManager.UpdateView();
                }));
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
