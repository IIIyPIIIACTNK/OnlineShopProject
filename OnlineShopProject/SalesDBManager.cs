using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Data.Sql;

namespace OnlineShopProject
{
    internal class SalesDBManager
    {
        #region Fields
        OleDbConnection salesDBconnection;
        OleDbDataAdapter dataAdapter;
        DataTable dt = new DataTable();
        #endregion

        #region Propreties

            #region ConnectionString
                OleDbConnectionStringBuilder connString = new OleDbConnectionStringBuilder()
                {
                    DataSource = "C:\\Users\\gerem\\OneDrive\\Документы\\OnlineShopMSAccessBD.accdb",
                    Provider = "Microsoft.ACE.OLEDB.12.0"
                };

        #endregion

            #region View
            public DataView SalesDBView { get { return dt.DefaultView; } }
            /// <summary>
            /// Состояние подключения к OSGoods базе данных для View
            /// </summary>
            /// <param name="propertyName"></param>
            public string salesDBConState { get { return salesDBconnection.State.ToString(); } }
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

        }
    }
}
