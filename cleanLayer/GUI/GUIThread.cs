using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Windows;

namespace cleanLayer.GUI
{
    public static class GUIThread
    {
        private static Application App;
        private static MainWindow Window;
        private static Thread _Worker;
        private static bool _IsRunning = false;

        public static void Initialize()
        {
            _Worker = new Thread(new ThreadStart(Work))
            {
                IsBackground = true,
                ApartmentState = ApartmentState.STA,
                CurrentCulture = CultureInfo.InvariantCulture,
                CurrentUICulture = CultureInfo.InvariantCulture
            };
            _Worker.Start();
        }

        private static void Work()
        {
            try
            {
                App = new Application();
                App.Startup += new StartupEventHandler(App_Startup);
                try
                {
                    Window = new MainWindow();
                    App.Run(Window);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            finally
            {
                if (Window != null)
                    Window.Close();
            }
        }

        private static void App_Startup(object sender, StartupEventArgs e)
        {
            _IsRunning = true;
        }
    }
}
