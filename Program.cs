using MiqoCraftCore;
using System;
using System.IO;
using System.Windows.Forms;
using VPL.Application.Data;

namespace MiqoCraft
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            VPApplication.ApplicationName = "MiqoCrafter";

            //Clean cache

            string logName = Path.Combine(Service_Misc.GetExecutionPath(), "GeneralDataBase.log");
            if(File.Exists(logName))
            {
                try
                {
                    File.Delete(logName);
                }
                catch
                {

                }
            }

            Application.Run(new MainForm());
        }
    }
}
