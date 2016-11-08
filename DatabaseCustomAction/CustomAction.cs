using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;
using System.Data;
using System.Data.SqlClient;
using System.Security.Principal;
using Microsoft.SqlServer.Management.Smo;
using View = Microsoft.Deployment.WindowsInstaller.View;
using System.Data.Sql;
using System.Windows.Forms;
using DatabaseCustomAction.Model;
namespace DatabaseCustomAction
{
    public static class CustomActions
    {
        [CustomAction]
        public static ActionResult EnumerateSqlServers(Session session)
        {
         
            if (null == session)
            {
                throw new ArgumentNullException("session");
            }

            session.Log("EnumerateSQLServers: Begin");

            // Check if running with admin rights and if not, log a message to
            // let them know why it's failing.
            if (false == HasAdminRights())
            {
                session.Log("EnumerateSQLServers: " + "ATTEMPTING TO RUN WITHOUT ADMIN RIGHTS");
                return ActionResult.Failure;
            }
            ActionResult result;

            DataTable dt = SmoApplication.EnumAvailableSqlServers(false);
            DataRow[] rows = dt.Select(string.Empty, "IsLocal desc, Name asc");
            result = EnumSqlServersIntoComboBox(session, rows);
         
            session.Log("EnumerateSQLServers: End");
            return result;
        }

        [CustomAction]
        public static ActionResult VerifySqlConnection(Session session)
        {
            try
            {
                //Debugger.Break();

                session.Log("VerifySqlConnection: Begin");

                var builder = new SqlConnectionStringBuilder
                {
                    DataSource = session["DATABASE_SERVER"],
                    InitialCatalog = "Device",
                    ConnectTimeout = 5
                };

                if (session["DATABASE_LOGON_TYPE"] != "DatabaseIntegratedAuth")
                {
                    builder.UserID = session["DATABASE_USERNAME"];
                    builder.Password = session["DATABASE_PASSWORD"];
                }
                else
                {
                    builder.IntegratedSecurity = true;
                }

                using (var connection = new SqlConnection(builder.ConnectionString))
                {
                    if (connection.CheckConnection(session))
                    {
                        session["ODBC_CONNECTION_ESTABLISHED"] = "1";
                        session["CONNECTION_STRING"] = connection.ConnectionString;

                        ApplicationReference appref = new ApplicationReference();
                        appref.ApplicationInitPage = "Home/Index";
                        appref.ApplicationOrDeviceType = "http://localhost/Moltu";
                        appref.ApplicationRootName = "TasmusNetasMoltu";
                        SetApplicationRef(connection, appref);
                    }
                    else
                    {
                        session["ODBC_CONNECTION_ESTABLISHED"] = string.Empty;
                    }
                }

                session.Log("VerifySqlConnection: End");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                session.Log("VerifySqlConnection: exception: {0}", ex.Message);
                throw;
            }

            return ActionResult.Success;
        }


        private static void SetApplicationRef(SqlConnection conn,ApplicationReference appRef)
        {

            try
            {
                string selectSql = "SELECT * FROM ApplicationRef  WHERE ApplicationOrDeviceType='" + appRef.ApplicationOrDeviceType+"';";
                string deleteSql = string.Empty;
                string insertSql = string.Empty;

                SqlCommand command = new SqlCommand(selectSql, conn);
                command.Connection.Open();

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    deleteSql = "DELETE  ApplicationRef WHERE ApplicationOrDeviceType='" + appRef.ApplicationOrDeviceType + "';";
                    command.Connection.Close();
                    command.CommandText = deleteSql;
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }

                insertSql = "INSERT INTO ApplicationRef (ApplicationOrDeviceType, ApplicationRootName,ApplicationInitPage) VALUES ('" + appRef.ApplicationOrDeviceType + "','" + appRef.ApplicationRootName + "','" + appRef.ApplicationInitPage + "')";
                command.Connection.Close();
                command.CommandText = insertSql;
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static ActionResult SetApplicationReference(Session session)
        {

            try
            {
          
                session.Log("SetApplicationReference: Begin");

                var builder = new SqlConnectionStringBuilder(session["CONNECTION_STRING"]);        


                using (var connection = new SqlConnection(builder.ConnectionString))
                {
                    ApplicationReference appref = new ApplicationReference();
                    appref.ApplicationInitPage = "Home/Index";
                    appref.ApplicationOrDeviceType = "http://localhost/Moltu";
                    appref.ApplicationRootName = "TasmusNetasMoltu";
                    SetApplicationRef(connection,appref);            
                }

                session.Log("SetApplicationReference: End");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                session.Log("SetApplicationReference: exception: {0}", ex.Message);
                throw;
            }


            return ActionResult.Success;
        }
  
        private static ActionResult EnumSqlServersIntoComboBox(Session session, IEnumerable<DataRow> rows)
        {
            try
            {
                //Debugger.Break();

                session.Log("EnumSQLServers: Begin");

                View view = session.Database.OpenView("DELETE FROM ComboBox WHERE ComboBox.Property='DATABASE_SERVER'");
                view.Execute();

                view = session.Database.OpenView("SELECT * FROM ComboBox");
                view.Execute();

                Int32 index = 1;
                session.Log("EnumSQLServers: Enumerating SQL servers");
                foreach (DataRow row in rows)
                {
                    String serverName = row["Name"].ToString();

                    // Create a record for this web site. All I care about is
                    // the name so use it for fields three and four.
                    session.Log("EnumSQLServers: Processing SQL server: {0}", serverName);

                    Record record = session.Database.CreateRecord(4);
                    record.SetString(1, "DATABASE_SERVER");
                    record.SetInteger(2, index);
                    record.SetString(3, serverName);
                    record.SetString(4, serverName);

                    session.Log("EnumSQLServers: Adding record");
                    view.Modify(ViewModifyMode.InsertTemporary, record);
                    index++;
                }

                view.Close();

                session.Log("EnumSQLServers: End");
            }
            catch (Exception ex)
            {
                session.Log("EnumSQLServers: exception: {0}", ex.Message);
                throw;
            }

            return ActionResult.Success;
        }

        private static bool HasAdminRights()
        {
            return true;
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private static bool CheckConnection(this SqlConnection connection, Session session)
        {
            try
            {
                if (connection == null)
                {
                    return false;
                }

                connection.Open();
                var canOpen = connection.State == ConnectionState.Open;
                connection.Close();

                return canOpen;
            }
            catch (SqlException ex)
            {
                session["ODBC_ERROR"] = ex.Message;
                return false;
            }
        }

    }
}
