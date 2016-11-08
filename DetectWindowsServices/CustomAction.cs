using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;
using System.ServiceProcess;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using static DetectWindowsServices.Helper;
using System.Text.RegularExpressions;

namespace DetectWindowsServices
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult DetectMSMQ(Session session)
        {
            //session.Log("Begin DetectMSMQ");
            //System.Diagnostics.ProcessStartInfo proc = new System.Diagnostics.ProcessStartInfo();
            //proc.FileName = Environment.ExpandEnvironmentVariables("%SystemRoot%") + @"\System32\cmd.exe";
            //proc.Arguments = "/c dism /online /enable-feature /featurename:WCF-HTTP-Activation45";
            //proc.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //System.Diagnostics.Process.Start(proc);

            ServiceController service = ServiceController.GetServices().FirstOrDefault(f => f.ServiceName == "NetMsmqActivator");

            if (service == null)
            {
                MessageBox.Show("NetMsmqActivator is not installed", "Error!");
                return ActionResult.Failure;
            }
            else
            {

                if (service.Status == ServiceControllerStatus.Running)
                {

                    return ActionResult.Success;
                }
                else
                {
                    MessageBox.Show("NetMsmqActivator is not actived", "Error!");
                    return ActionResult.Failure;
                }
            }



        }

        [CustomAction]
        public static ActionResult DetectPipe(Session session)
        {
            //session.Log("Begin DetectMSMQ");
            //System.Diagnostics.ProcessStartInfo proc = new System.Diagnostics.ProcessStartInfo();
            //proc.FileName = Environment.ExpandEnvironmentVariables("%SystemRoot%") + @"\System32\cmd.exe";
            //proc.Arguments = "/c dism /online /enable-feature /featurename:WCF-HTTP-Activation45";
            //proc.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //System.Diagnostics.Process.Start(proc);

            ServiceController service = ServiceController.GetServices().FirstOrDefault(f => f.ServiceName == "NetPipeActivator");

            if (service == null)
            {
                MessageBox.Show("NetPipeActivator is not installed", "Error!");
                return ActionResult.Failure;
            }
            else
            {

                if (service.Status == ServiceControllerStatus.Running)
                {

                    return ActionResult.Success;
                }
                else
                {
                    MessageBox.Show("NetPipeActivator is not actived", "Error!");
                    return ActionResult.Failure;
                }
            }



        }
        

        [CustomAction]
        public static ActionResult DetectSQLServer(Session session)
        {

            string serverName = session["SERVERNAMEPROP"];
            string userName = session["USERNAMEPROP"];
            string password = session["PASSWORDPROP"];

            ActionResult ar1 = DetectServiceByName("MSSQL$SQLEXPRESS");
            ActionResult ar2 = DetectServiceByName("MSSQLSERVER");


            if (ar1 == ActionResult.Success || ar2 == ActionResult.Success)
            {
                return TestSQLServer(serverName, userName, password,session);
            }
            else
            {
                session["PIDACCEPTED"] = "0";            
                session["DATABASESTATUTEXT"] = "Calisan bir veri tabani servisi bulunamadi!.";
                return ActionResult.Success;                              
            }


        }


        public static ActionResult AppConfigValidation(Session session)
        {
            string serverName = session["SERVERNAMEPROP"];
            string dbName = session["DBNAMEPROP"];
            string appName = session["APPNAMEPROP"];

            string localization = session["LOCALIZATION"];

            var ret = CheckServerInstanceName(serverName);




            return ActionResult.Success;
        }


        private static ServerInstanceNameStatu CheckServerInstanceName(string serverName)
        {

            if (!String.IsNullOrEmpty(serverName))
            {
                return ServerInstanceNameStatu.invalidCharacter;
            }
            else if (!Regex.IsMatch(serverName.Substring(0,1), @"^[a-zA-Z_]+$"))
            {
                return ServerInstanceNameStatu.invalidCharacter;
            }
            else if(serverName.Length < 0 || serverName.Length > 16 )
            {
                return ServerInstanceNameStatu.invalidLength;
            }

            return ServerInstanceNameStatu.valid;
        }


        private static ActionResult TestSQLServer(string serverName, string userName, string password,Session session)
        {


            try
            {
                SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder();
                connection.DataSource = serverName;
                connection.UserID = userName;
                connection.Password = password;
                connection.IntegratedSecurity = false;
                String strConn = connection.ToString();
                SqlConnection sqlConn = new SqlConnection(strConn);
                sqlConn.Open();
                sqlConn.Close();

                session["PIDACCEPTED"] = "1";       
                return ActionResult.Success;

            }
            catch (Exception ex)
            {
                session["PIDACCEPTED"] = "0";
             
                session["DATABASESTATUTEXT"] = "Sunucu bilgileri hatali!.";
                return ActionResult.Success;

            }
        }

        private static ActionResult DetectServiceByName(string serviceName)
        {

            ServiceController service = ServiceController.GetServices().FirstOrDefault(f => f.ServiceName == serviceName);

            if (service == null)
            {

                return ActionResult.Failure;
            }
            else
            {

                if (service.Status == ServiceControllerStatus.Running)
                {

                    return ActionResult.Success;
                }
                else
                {

                    return ActionResult.Failure;
                }
            }




        }

     

    }
}
