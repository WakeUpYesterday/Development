using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Windows.Forms;
using Microsoft.Deployment.WindowsInstaller;

namespace RegistrationInfoCustomAction
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult SaveUserInfo(Session session)
        {
            string name = session["FULLNAMEProperty"];
            string email = session["EMAILProperty"];

            string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (!Directory.Exists(appdataPath + "\\Win App"))
                Directory.CreateDirectory(appdataPath + "\\Win App");

            File.WriteAllText(appdataPath + "\\Win App\\registrationInfo.txt", name + "," + email);

            return ActionResult.Success;
        }
    }
}
