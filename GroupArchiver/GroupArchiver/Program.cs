using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GroupArchiver
{
    static class Program
    {
         //public static bool ongoingAuth = false;
         //public static bool authenticationNeeded = false;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new App());
        }
    }
}
