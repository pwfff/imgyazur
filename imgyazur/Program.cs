using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace imgyazur
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

            if (String.IsNullOrEmpty(Properties.Settings.Default.key))
            {
                Properties.Settings.Default.key = Microsoft.VisualBasic.Interaction.InputBox("Please insert your imgur API key:", "Missing Imgur API Key");
                Properties.Settings.Default.Save();
            }

            if (String.IsNullOrEmpty(Properties.Settings.Default.key))
                MessageBox.Show("An imgur API key is required for this program to run.", "Unable To Run");
            else
                Application.Run(new formImgyazur());
        }
    }
}
