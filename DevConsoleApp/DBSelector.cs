using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevConsoleApp
{
    public partial class DBSelector : Form
    {
        public DBSelector()
        {
            InitializeComponent();
            SetServerName();
        }

        private void SetServerName()
        {
            string myServer = Environment.MachineName;

            DataTable servers = SqlDataSourceEnumerator.Instance.GetDataSources();
            for (int i = 0; i < servers.Rows.Count; i++)
            {
                if (myServer == servers.Rows[i]["ServerName"].ToString()) ///// used to get the servers in the local machine////
                {

                }
                if ((servers.Rows[i]["InstanceName"] as string) != null)
                    this.ServerName.Items.Add(servers.Rows[i]["ServerName"] + "\\" + servers.Rows[i]["InstanceName"]);
                else
                    this.ServerName.Items.Add(servers.Rows[i]["ServerName"].ToString());
            }


            this.ServerName.SelectedIndex = 0;
        }


        private void SetDbName(string selectedServer)
        {
            try
            {
                SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder();
                connection.DataSource = selectedServer;
                // enter credentials if you want
                connection.UserID = this.UserName.Text;
                connection.Password = this.Password.Text;
                connection.IntegratedSecurity = false;
                connection.ConnectTimeout = 4;
                String strConn = connection.ToString();

                //create connection
                SqlConnection sqlConn = new SqlConnection(strConn);

                //open connection
                sqlConn.Open();

               
             
                //get databases
                DataTable tblDatabases = sqlConn.GetSchema("Databases");

                //close connection
                sqlConn.Close();

                //add to list
                foreach (DataRow row in tblDatabases.Rows)
                {
                    String strDatabaseName = row["database_name"].ToString();

                    this.DatabaseName.Items.Add(strDatabaseName);

                }
                this.DatabaseName.Enabled = true;
                this.DatabaseName.SelectedIndex = 0;
                testStatuLabel.Text = "Success";
            }
            catch (Exception ex)
            {
                testStatuLabel.Text = "Failed";

            }

                
        }   


        private void ServerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var serverName = ServerName.SelectedItem.ToString();

            SetDbName(serverName);

        }
    }
}
