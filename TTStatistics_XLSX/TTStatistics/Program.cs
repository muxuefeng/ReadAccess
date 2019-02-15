using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TTStatistics
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ConfigurationManager.LoadConfiguration(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\TTStatistics.cfg");
            ConfigurationManager.SaveConfiguration();
            Application.Run(new TTStatistics());
        }
    }
}
