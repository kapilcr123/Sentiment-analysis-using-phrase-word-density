using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Twitter_Event_Detection;

namespace Akzz_Twitter
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
            Application.Run(new twitter_main());
        }
    }
}
