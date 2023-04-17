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
        /// <summary>
        /// Таблица покупок отдельного клиенты
        /// </summary>
        public DataTable clientsPurchaseDT = new DataTable();
        public DataTable ClientsPurchaseDT  { get{ return clientsPurchaseDT; } set { clientsPurchaseDT = value; OnPropertyChanged(); } }
       
        #region ClientsDataFromView
        //NB! Научится использовать мульти привязку в XAML для исключения множества свойств привязки

        /// <summary>
        /// Выбранный элемент из таблицы (Как для таблицы клиентов так и для таблицы продаж)
        /// </summary>
        public DataRowView SelectedElement { get;set; }
        public string ClientName { get; set; }
        public string ClientLastName { get; set; }
        public string ClientMiddleName { get; set; }
        public string ClientPhoneNumber { get; set; }
        public string ClientEMail { get; set; }

        #endregion

        #region SalesDataFromView
        //NB! Научится использовать мульти привязку в XAML для исключения множества свойств привязки

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
                        try
                        {
                            DataRow dataRow = clientsDBManager.ClientsDBDataTable.NewRow();
                            dataRow["lastName"] = ClientLastName;
                            dataRow["name"] = ClientName;
                            dataRow["middleName"] = ClientMiddleName;
                            dataRow["phoneNumber"] = ClientPhoneNumber;
                            dataRow["eMail"] = ClientEMail;
                            clientsDBManager.ClientsDBDataTable.Rows.Add(dataRow);
                            _ClientsDBManager.InsertClientCommand();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"--- Ошибка : {ex.Message} ---");
                        }
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
                    try
                    {
                        SelectedElement.Row.Delete();
                        _ClientsDBManager.DeleteClientCommand();
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine($"--- Ошибка: {ex.Message} ---")
                    }
                }));
            }
        }
        public RelayCommand EndEditClient
        {
            get
            {
                return removeClinet ?? (removeClinet = new RelayCommand(o =>
                {
                    try
                    {
                        clientsDBManager.UpdateClientCommand();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"--- Ошибка: {ex.Message} ---")
                    }
                }));
            }
        }
        public RelayCommand AddPurachase
        {
            get
            {
                return addPurachase ?? (addPurachase= new RelayCommand(o =>
                {
                    try
                    {
                        salesDBManager.InsertPurchaseCommand(ClientEMail,
                         salesDBManager.ItemsIdNamesDict.FirstOrDefault(x => x.Value == ItemDescription).Key.ToString(),
                         ItemCode);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"--- Ошибка: {ex.Message} ---")
                    }
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
                        try
                        {
                            SelectedElement.Row.Delete();
                            salesDBManager.DeletePurchaseCommand();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"--- Ошибка: {ex.Message} ---")
                        }
                    }));
            }
        }
        /// <summary>
        /// Вызов комманды для формирования таблицы покупок определенного клента по eMail. 
        /// Клиент берется из SelectedElement, eMail из 5 индекса массива Rows.ItemArrays
        /// </summary>
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
