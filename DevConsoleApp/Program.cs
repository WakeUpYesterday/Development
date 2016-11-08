using System;
using System.Diagnostics;
using System.Data;
using Microsoft.SqlServer.Management.Smo;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace DevConsoleApp
{
    
    class Program
    {        
        static void ExecuteCommand(string command)
        {
            int exitCode;
            ProcessStartInfo processInfo;
            Process process;

            processInfo = new ProcessStartInfo(@"C:\Windows\SysWOW64\cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            // *** Redirect the output ***
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            process = Process.Start(processInfo);
            process.WaitForExit();


            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            exitCode = process.ExitCode;

            Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
            Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
            Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
            process.Close();
        }

        [STAThread]
        static void Main()
        {

            string a = "00";

            string b = a.TrimStart('0');
            int ca = b.Length;
            try
            {
                var ApplicationOrDeviceType = "TasmusNetasMoltu";
                var ApplicationRootName = @"http://localhost/" + "Moltu"+ "/Moltu/";
                var ApplicationInitPage = "Home/Index";

                SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder();
                connection.DataSource = @"ICTSIMOS\ICT";
                connection.UserID = "sa";
                connection.Password = "12345678.ict";
                connection.InitialCatalog = "Siskon";
                connection.IntegratedSecurity = false;

                String strConn = connection.ToString();


                SqlConnection sqlConn = new SqlConnection(strConn);

                String sql = @"INSERT INTO ApplicationRef (ApplicationOrDeviceType,ApplicationRootName,ApplicationInitPage) VALUES('" + ApplicationOrDeviceType + "','" + ApplicationRootName + "','" + ApplicationInitPage + "')";
                sqlConn.Open();
                SqlCommand cmd = new SqlCommand(sql, sqlConn);
                cmd.ExecuteNonQuery();

                sqlConn.Close();
                
            }
            catch (Exception ex)
            {
               
            }

            //ExecuteCommand("dism /online /enable-feature /featurename:WCF-HTTP-Activation45");

            //var serviceList = ServiceController.GetServices().ToList().OrderBy(o => o.ServiceName);

            //for (int i = 0; i < serviceList.Count(); i++)
            //{
            //    Console.WriteLine(i.ToString() + "- ServiceName : " + serviceList.ElementAt(i).ServiceName + " ~~ " + "DisplayName : " + serviceList.ElementAt(i).DisplayName + " ~~ " + "MachineName : " + serviceList.ElementAt(i).MachineName);
            //}

            //Console.ReadLine();

            //ServiceController service = ServiceController.GetServices().FirstOrDefault(f => f.ServiceName == "NetMsmqActivator");

            //   string a = GetBatOutput();

            DataTable dataTable = SmoApplication.EnumAvailableSqlServers(true);
            DBSelector dBSelector = new DBSelector();
            Application.EnableVisualStyles();
            dBSelector.Visible = true;
            Application.Run(dBSelector);

        }


        private static bool CheckServerInstanceName(string serverName)
        {

            if (String.IsNullOrEmpty(serverName))
            {
                return false;
            }
            else if (!Regex.IsMatch(serverName.Substring(0, 1), @"^[a-zA-Z_]+$"))
            {
                return false;
            }
            else if (serverName.Length < 0 || serverName.Length > 16)
            {
                return false;
            }

            return true;
        }
        static string GetBatOutput()
        {

            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = @"c:\Windows\Syswow64\cmd.exe";
            // p.StartInfo.Arguments = "/C dism /online /get-featureInfo /featurename:WCF-HTTP-Activation45";
            p.StartInfo.Arguments = @"/C c:\windows\System32\dism.exe /online /get-featureInfo /featurename:WCF-HTTP-Activation45";
            p.Start();
       
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            return output;
        }


      
    }
}
