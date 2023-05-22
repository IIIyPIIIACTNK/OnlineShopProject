using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using OnlineShopProject.Models;
using OnlineShopProject.MyViewModels;

namespace OnlineShopProject
{
    class ViewModel : INotifyPropertyChanged
    {
        public OsClientsContext ShopDBContext { get; set; }
        public ViewModel()
        {
            ShopDBContext = new OsClientsContext();
            SIToView = new ObservableCollection<SoldItemViewModel>(SoldItemsToView());
        }

        #region Fields
        
        public event PropertyChangedEventHandler? PropertyChanged;

        RelayCommand? addClinet;
        RelayCommand? deleteClient;
        RelayCommand? removeClinet;
        RelayCommand? addPurachase;
        RelayCommand? deletePurchase;
        RelayCommand? addItem;
        RelayCommand? setClientsPurchaseDT;
        #endregion

        #region Properties

        #region View
        // -----------EF core object binding

        public ObservableCollection<SoldItemViewModel> SIToView { get; set; }
        private List<SoldItemViewModel> SoldItemsToView()
        {
            List<SoldItemViewModel> result = new List<SoldItemViewModel>();
            var query = from si in ShopDBContext.SoldItems
                        join i in ShopDBContext.Items on si.ItemId equals i.Id into gj
                        from sub in gj.DefaultIfEmpty()
                        select new
                        {
                            Id = si.Id,
                            Name = sub.Name,
                            ItemCode = si.ItemCode,
                            CustomerEmail = si.CustomerEmail,
                            ItemId = si.ItemId,
                        };
            result = query.Select(x => new SoldItemViewModel
            {
                Id = x.Id,
                Name = x.Name,
                ItemCode = x.ItemCode,
                CustomerEmail = x.CustomerEmail,
                itemId= x.ItemId,
            }).ToList();

            Debug.WriteLine("-------------Данные--------------");
            foreach(var item in result)
            {
                Debug.WriteLine($"*--- {item.Id} {item.Name} {item.ItemCode} {item.CustomerEmail}");
            }
            Debug.WriteLine("---------------------------------");

            return result;
        }

        //public List<ClientsPurchasesViewModel> CPToView { get { return ClientPurchasesToView(); } set { CPToView = value; OnPropertyChanged(); } }

        //public ObservableCollection<ClientsPurchasesViewModel> cPToView;
        public ObservableCollection<ClientsPurchasesViewModel> CPToView { get; set; }
        public List<ClientsPurchasesViewModel> testtesttest = new List<ClientsPurchasesViewModel>() { new ClientsPurchasesViewModel() { ItemName = "test"} };
        public List<ClientsPurchasesViewModel> ClientPurchasesToView()
        {
            var selectedClient = (Client)SelectedElement;
            List<ClientsPurchasesViewModel> result;
            var query = from si in ShopDBContext.SoldItems
                        join c in ShopDBContext.Clients on si.CustomerEmail equals selectedClient.EMail into gj
                        from sub in gj.DefaultIfEmpty()
                        join i in ShopDBContext.Items on si.ItemId equals i.Id into gj2
                        from sub2 in gj2.DefaultIfEmpty()
                        select new
                        {
                            ItemName = sub2.Name,
                            ItemCode = si.ItemCode,
                        };
            result = query.Select(x => new ClientsPurchasesViewModel()
            {
                ItemName= x.ItemName,
                ItemCode = x.ItemCode,
            }).ToList();
            Debug.WriteLine($"------ {result.Count}");

            return result;
        }

        // -----------

        #region ClientsDataFromView
        //NB! Научится использовать мульти привязку в XAML для исключения множества свойств привязки

        /// <summary>
        /// Выбранный элемент из таблицы (Как для таблицы клиентов так и для таблицы продаж)
        /// </summary>
        public object SelectedElement { get;set; }
        public string ClientName { get; set; }
        public string ClientLastName { get; set; }
        public string ClientMiddleName { get; set; }
        public string ClientPhoneNumber { get; set; }
        public string ClientEMail { get; set; }

        #endregion

        #region SalesDataFromView
        //NB! Научится использовать мульти привязку в XAML для исключения множества свойств привязки

        public string ItemCode { get; set; }
        public Item ComboBoxItemModelBinding { get; set; } 
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
                            ShopDBContext.Clients.Add(new Client()
                            {
                                Name = ClientName,
                                LastName = ClientLastName,
                                MiddleName = ClientMiddleName,
                                PhoneNumber = ClientPhoneNumber,
                                EMail = ClientEMail,
                            });
                            ShopDBContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"--- Ошибка при вызове метода AddClient: {ex.Message} ---");
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
                            ShopDBContext.Clients.Remove((Client)SelectedElement);
                            ShopDBContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"--- Ошибка при вызове метода DeleteClient: {ex.Message} ---");
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
                        ShopDBContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"--- Ошибка при вызове метода EndEditClient: {ex.Message} ---");
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
                        ShopDBContext.SoldItems.Add(new SoldItem()
                        {
                            CustomerEmail = ClientEMail,
                            ItemId = ComboBoxItemModelBinding.Id,
                            ItemCode = Convert.ToInt32(ItemCode)
                        });
                        ShopDBContext.SaveChanges();
                        Debug.WriteLine("----- Обьект добавлен-----");
                        Debug.WriteLine($"{ComboBoxItemModelBinding.Id} {ComboBoxItemModelBinding.Name} {ItemCode} {ClientEMail}");
                        Debug.WriteLine("----- Обьект добавлен-----");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"--- Ошибка: {ex.Message} ---");
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
                            var elementToDelete = (SoldItemViewModel)SelectedElement;
                            ShopDBContext.SoldItems.Remove(elementToDelete.ToSoldItemDBModel());
                            ShopDBContext.SaveChanges(true);
                            Debug.WriteLine("----Удалено----");
                            Debug.WriteLine($"{elementToDelete.Id} {elementToDelete.Name} {elementToDelete.ItemCode} {elementToDelete.CustomerEmail}");
                            Debug.WriteLine("---------------");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"--- Ошибка при вызове метода DeleteCommand: {ex.Message} ---");
                        }
                    }));
            }
        }
        public RelayCommand AddItem
        {
            get
            {
                return addItem ??= new RelayCommand(o =>
                {
                    try
                    {
                        ShopDBContext.Items.Add(new Item
                        {
                            Name = ItemDescription
                        });
                        ShopDBContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"--- {ex.Message}");
                    }
                });
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
                        CPToView = new ObservableCollection<ClientsPurchasesViewModel>(ClientPurchasesToView());
                        Debug.WriteLine($"------ CPToView.Count = {CPToView.Count}");
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
