using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Azure.Core;
using Microsoft.Data.SqlClient;

namespace OnlineShopProject
{
    public class ClientsDBManager : INotifyPropertyChanged
    {

        #region Fields

        SqlConnection clientsDBconnection;
        SqlDataAdapter? dataAdapter;
        DataTable dt = new DataTable();
        SqlConnectionStringBuilder connString = new SqlConnectionStringBuilder() 
        {
            DataSource = "(localdb)\\MSSQLLocalDB",
            InitialCatalog = "OSClients",
            IntegratedSecurity = true
        };

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        #region Properties

        #region View
            public DataTable ClientsDBDataTable { get { return dt; } set { dt = value; } }

                public string ClientsDBConState { get { return clientsDBconnection.State.ToString(); } set { OnPropertyChanged(); } }
        #endregion

        #endregion

        public ClientsDBManager()
        {
            clientsDBconnection = new SqlConnection(connString.ConnectionString);

            SetStartDataTableView();
        }

        private void SetStartDataTableView()
        {
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

        }

        public void DeleteClientCommand()
        {
            var request = @"DELETE FROM Clients WHERE id = @id";

            dataAdapter.DeleteCommand = new SqlCommand(request, clientsDBconnection);
            dataAdapter.DeleteCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id");

            dataAdapter.Update(dt);
        }

        public void InsertClientCommand() 
        {
            var request = @"INSERT INTO Clients (lastName,[name],middleName,phoneNumber,eMail) 
VALUES (@lastName, @name,@middleName,@phoneNumber,@eMail);
SET @id = @@IDENTITY;";


            dataAdapter.InsertCommand = new SqlCommand(request, clientsDBconnection);

            dataAdapter.InsertCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id").Direction = ParameterDirection.Output;
            dataAdapter.InsertCommand.Parameters.Add("@lastName", SqlDbType.NVarChar, 50, "lastName");
            dataAdapter.InsertCommand.Parameters.Add("@name", SqlDbType.NVarChar, 50, "name");
            dataAdapter.InsertCommand.Parameters.Add("@middleName", SqlDbType.NVarChar, 50, "middleName");
            dataAdapter.InsertCommand.Parameters.Add("@phoneNumber", SqlDbType.NVarChar, 50, "phoneNumber");
            dataAdapter.InsertCommand.Parameters.Add("@eMail", SqlDbType.NVarChar, 50, "eMail");

            dataAdapter.Update(dt);
        }

        public void UpdateClientCommand()
        {
            var request = @"UPDATE Clients 
SET lastName = @lastName,
    name = @name,
    middleName = @middleName,
    phoneNumber = @phoneNumber,
    eMail = @eMail
WHERE id = @id";

            dataAdapter.UpdateCommand = new SqlCommand(request, clientsDBconnection);
            dataAdapter.UpdateCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id");
            dataAdapter.UpdateCommand.Parameters.Add("@lastName", SqlDbType.NVarChar, 50, "lastName");
            dataAdapter.UpdateCommand.Parameters.Add("@name", SqlDbType.NVarChar, 50, "name");
            dataAdapter.UpdateCommand.Parameters.Add("@middleName", SqlDbType.NVarChar, 50, "middleName");
            dataAdapter.UpdateCommand.Parameters.Add("@phoneNumber", SqlDbType.NVarChar, 50, "phoneNumber");
            dataAdapter.UpdateCommand.Parameters.Add("@eMail", SqlDbType.NVarChar, 50, "eMail");

            dataAdapter.Update(dt);
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
