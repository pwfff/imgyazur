﻿using System;
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

            ImgyazurIcon icon = ImgyazurIcon.Instance;
            Application.Run();
            icon.notifyIcon.Dispose();
            icon = null;
        }
    }
}
