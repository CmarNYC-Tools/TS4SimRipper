using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Windows.Forms;
#if  NETCORE
[assembly:System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
namespace TS4SimRipper
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            #if NETCORE
            // Application.SetDefaultFont(new Font(new FontFamily("Microsoft Sans Serif"), 12f));
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Application.SetHighDpiMode(HighDpiMode.DpiUnaware);
            #endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
