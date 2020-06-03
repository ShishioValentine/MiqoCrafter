using System;
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

            Application.Run(new MainForm());
        }
    }
}
