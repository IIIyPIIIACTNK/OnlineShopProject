using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace OnlineShopProject
{
    public class ClientsDBManager
    {

        #region Fields

        SqlConnection clientsDBconnection;
        SqlDataAdapter dataAdapter;
        DataTable dt = new DataTable();
        SqlConnectionStringBuilder connString = new SqlConnectionStringBuilder() 
        {
            DataSource = "(localdb)\\MSSQLLocalDB",
            InitialCatalog = "OSClients",
            IntegratedSecurity = true
        };
        #endregion

        #region Properties

            #region View
                public DataTable ClientsDBDataTable { get { return dt; } set { dt = value; } }

                public string ClientsDBConState { get { return clientsDBconnection.State.ToString(); } }
        #endregion

        #endregion

        public ClientsDBManager()
        {
            clientsDBconnection = new SqlConnection(connString.ConnectionString);

            SetStartDataTableView();
        }

        private void SetStartDataTableView()
        {
//            var request = @"SELECT 
//Clients.id,
//Clients.lastName,
//Clients.[name],
//Clients.middleName,
//Clients.phoneNumber,
//Clients.eMail
//FROM Clients
//ORDER BY Clients.id";
            var request = @"SELECT 
*
FROM Clients
ORDER BY Clients.id";
            dataAdapter = new SqlDataAdapter();

            try
            {
                dataAdapter.SelectCommand = new SqlCommand(request, clientsDBconnection);
                dataAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }

            #region delete
            #endregion

            #region insert

            request = @"INSERT INTO Clients (lastName,[name],middleName,phoneNumber,eMail) 
VALUES (@lastName, @name,@middleName,@phoneNumber,@eMail);
SET @id = @@IDENTITY;";

            dataAdapter = new SqlDataAdapter();

            dataAdapter.InsertCommand = new SqlCommand(request, clientsDBconnection);

            dataAdapter.InsertCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id").Direction = ParameterDirection.Output;
            dataAdapter.InsertCommand.Parameters.Add("@lastName", SqlDbType.NVarChar, 50, "lastName");
            dataAdapter.InsertCommand.Parameters.Add("@name", SqlDbType.NVarChar, 50, "name");
            dataAdapter.InsertCommand.Parameters.Add("@middleName", SqlDbType.NVarChar, 50, "middleName");
            dataAdapter.InsertCommand.Parameters.Add("@phoneNumber", SqlDbType.NVarChar, 50, "phoneNumber");
            dataAdapter.InsertCommand.Parameters.Add("@eMail", SqlDbType.NVarChar, 50, "eMail");

            #endregion
            request = @"DELETE FROM Clients WHERE id = @id";

            dataAdapter.DeleteCommand = new SqlCommand(request, clientsDBconnection);
            dataAdapter.DeleteCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id");

            dataAdapter.Update(dt);
            MessageBox.Show(dataAdapter.DeleteCommand.CommandText);
        }

        public void UpdateView()
        {
            dataAdapter.Update(dt);
        }
    }
}
