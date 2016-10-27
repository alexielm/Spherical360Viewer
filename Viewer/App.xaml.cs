using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Spherical360Viewer
{
    public partial class App : Application
    {
        public static string CommandLine;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            CommandLine = String.Join(" ", e.Args);
        }
    }
}
