using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using Azure.Core;
using Microsoft.Data.Sql;

namespace OnlineShopProject
{
    internal class SalesDBManager : INotifyPropertyChanged
    {
        #region Fields
        OleDbConnection salesDBconnection;
        OleDbDataAdapter dataAdapter;
        DataTable dt = new DataTable();
        Dictionary<int, string> itemsIdNamesDict = new Dictionary<int,string>();
        #endregion

        #region Propreties

            #region ConnectionString
                OleDbConnectionStringBuilder connString = new OleDbConnectionStringBuilder()
                {
                    DataSource = @"..\OnlineShopMSAccessBD.accdb",
                    Provider = "Microsoft.ACE.OLEDB.12.0"
                };

        #endregion

        #region View
            public DataTable SalesDBDataTable { get { return dt; } }
            /// <summary>
            /// Состояние подключения к OSGoods базе данных для View
            /// </summary>
            /// <param name="propertyName"></param>
            public string salesDBConState { get { return salesDBconnection.State.ToString(); } set { OnPropertyChanged(); } }

            public Dictionary<int, string> ItemsIdNamesDict { get { return itemsIdNamesDict;} }

            public string ItemCode { get; set; }
            public string ItemDescription { get; set; }

        RelayCommand? addItem;

        public event PropertyChangedEventHandler? PropertyChanged;

        public RelayCommand AddItem
        {
            get
            {
                return addItem ??= new RelayCommand(o =>
                {
                    InsertNewItem();
                });
            } 
        }

        #endregion

        #endregion

        public SalesDBManager()
        {
            salesDBconnection = new OleDbConnection(connString.ConnectionString);
            SetStartDataTableView();
        }

        private void SetStartDataTableView()
        {
            var request = @"SELECT SoldGoods.id, SoldGoods.eMail, SoldGoods.uniqueItemCode, Goods.itemDescription
                            FROM     (Goods INNER JOIN
                            SoldGoods ON Goods.itemId = SoldGoods.itemCode)
                            ORDER BY SoldGoods.id";
            dataAdapter = new OleDbDataAdapter();
            try
            {
                dataAdapter.SelectCommand = new OleDbCommand(request, salesDBconnection);
                dataAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }

            Task t = new Task(() => SetItemNames());
            t.Start();
        }

        public void InsertPurchaseCommand(string eMail, string itemCode, string uniqueItemCode)
        {
            var request = @"INSERT INTO SoldGoods (eMail, itemCode,uniqueItemCode)
VALUES (@eMail,@itemCode,@uniqueItemCode);
";
            using (salesDBconnection = new OleDbConnection(connString.ConnectionString))
            {
                salesDBconnection.Open();
                OleDbParameter eMailPar = new OleDbParameter("@eMail", eMail);
                OleDbParameter itemCodePar = new OleDbParameter("@itemCode", itemCode);
                OleDbParameter uniqueItemCodePar = new OleDbParameter("@uniqueItemCode", uniqueItemCode);
                OleDbCommand insertComand = new OleDbCommand(request, salesDBconnection);
                insertComand.Parameters.Add(eMailPar);
                insertComand.Parameters.Add(itemCodePar);
                insertComand.Parameters.Add(uniqueItemCodePar);

                insertComand.ExecuteNonQuery();
                dataAdapter.Update(dt);
            }
        }

        public void DeletePurchaseCommand()
        {
            var request = @"DELETE FROM SoldGoods WHERE id = @id";

            using (salesDBconnection = new OleDbConnection(connString.ConnectionString))
            {
                salesDBconnection.Open();
                dataAdapter.DeleteCommand = new OleDbCommand(request,salesDBconnection);
                dataAdapter.DeleteCommand.Parameters.Add("@id", OleDbType.Integer, 4, "id");
                dataAdapter.Update(dt);
            }

               
        }

        public void InsertNewItem()
        {
            var request = @"INSERT INTO Goods (itemId,itemDescription)
VALUES (@itemId,@itemDescription);";

            using (salesDBconnection = new OleDbConnection(connString.ConnectionString))
            {
                if(ItemCode == null || ItemDescription == null)
                {
                    MessageBox.Show("Введите данные");
                    return;
                }
                if(itemsIdNamesDict.ContainsKey(Convert.ToInt32(ItemCode)) || itemsIdNamesDict.Values.Contains(ItemDescription))
                {
                    MessageBox.Show("Такой товар уже существует");
                    return;
                }
                salesDBconnection.Open();

                OleDbParameter itemIdPar = new OleDbParameter("@itemId",ItemCode);
                OleDbParameter itemDesriptionPar = new OleDbParameter("@itemDescription", ItemDescription);

                OleDbCommand insertCommand = new OleDbCommand(request,salesDBconnection);
                insertCommand.Parameters.Add(itemIdPar);
                insertCommand.Parameters.Add(itemDesriptionPar);

                insertCommand.ExecuteNonQuery();

                itemsIdNamesDict.Add(Convert.ToInt32(ItemCode), ItemDescription);
            }
        }

        public DataTable SelectClientsPurchase(string clientsEmail)
        {
            StringBuilder sb = new StringBuilder(clientsEmail);
            sb.Replace("@", "");
            sb.Replace(".","");
            string cleanClientsEmail = sb.ToString();
                Debug.WriteLine($"--- Clean clients email {cleanClientsEmail} ---");
            DataTable salesDT = new DataTable();
            var request = @"SELECT Goods.itemDescription, SoldGoods.uniqueItemCode
FROM     (Goods INNER JOIN
                  SoldGoods ON Goods.itemId = SoldGoods.itemCode)
WHERE  (REPLACE(REPLACE(SoldGoods.eMail, '@', ''), '.', '') LIKE @eMail)";

            using (salesDBconnection = new OleDbConnection(connString.ConnectionString))
            {
                dataAdapter.SelectCommand= new OleDbCommand(request, salesDBconnection);
                dataAdapter.SelectCommand.Parameters.Add(new OleDbParameter("@eMail", cleanClientsEmail));

                dataAdapter.Fill(salesDT);
                //
                Debug.WriteLine(salesDT.DefaultView.Count);
            }

            return salesDT;
        }

        /// <summary>
        /// Установка наименований для предметов
        /// </summary>
        private void SetItemNames()
        {
            
            using (salesDBconnection )
            {
                salesDBconnection.Open();
                var request = @"SELECT itemId, itemDescription
                            FROM     Goods";

                OleDbCommand fillItemNamesCommand = new OleDbCommand(request, salesDBconnection);
                OleDbDataReader reader = fillItemNamesCommand.ExecuteReader();

                while (reader.Read())
                {
                    try
                    {
                        itemsIdNamesDict.Add(reader.GetInt32("itemId"), reader.GetString("itemDescription"));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex}");
                        break;
                    }
                }

            }
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
