using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ATM
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //arg: participantID
            String arg="";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CitiBankForm(arg,"EN"));
        }
    }
}
