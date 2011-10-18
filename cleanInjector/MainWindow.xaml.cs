#region Legal
/*
Copyright 2009 scorpion

This file is part of N2.

N2 is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

N2 is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with N2.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion Legal
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace cleanInjector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer WoWFinder;

        private IEnumerable<int> WoWs = new List<int>();
        private ObservableCollection<WoWAttachVisual> ProcMap = new ObservableCollection<WoWAttachVisual>();
        private bool v_series = false;

        public MainWindow()
        {
            InitializeComponent();

            WoWFinder = new Timer(new TimerCallback(PaintWoWs), null, 0, 1000);

            var os = Environment.OSVersion;
            if (os.Platform == PlatformID.Win32NT && os.Version.Major == 6)
            {
                v_series = true;
                Sidebar.DataContext = ProcMap;
                SelectedWoW.DataContext = ProcMap;
            }
        }

        private void PaintWoWs(object dummy)
        {
            if (this.Visibility.Equals(Visibility.Hidden)) if (!this.IsVisible) this.Dispatcher.Invoke(new Action(this.Show)); else return;

            if (v_series)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    var wowids = from p in Process.GetProcessesByName("WoW") select p.Id;

                    if (wowids == null) return;

                    if (wowids.SequenceEqual(WoWs)) return; else WoWs = wowids;

                    ProcMap.Clear();
                    IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;
                    foreach (var pid in wowids)
                    {
                        var MWH = Process.GetProcessById(pid).MainWindowHandle;
                        ProcMap.Add(new WoWAttachVisual(MWH, pid));
                    }
                    Sidebar.UpdateLayout();
                }));
            }
            else
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    Sidebar.Items.Clear();
                    Sidebar.ItemTemplate = default(DataTemplate);
                    foreach (var wow in Process.GetProcessesByName("WoW"))
                    {
                        var lab = new TextBlock();
                        lab.Text = string.Format("{0} #{1}", wow.MainWindowTitle, wow.Id);
                        lab.Margin = new Thickness(4);
                        lab.Cursor = Cursors.Hand;
                        lab.Tag = wow.Id;
                        lab.MouseDown += new MouseButtonEventHandler(lab_MouseDown);
                        Sidebar.Items.Add(lab);
                    }
                    Sidebar.UpdateLayout();
                }));
            }
        }

        private void Attach(int pid)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo("Loader_IA32.exe", "--pid \"" + pid + "\" --module \"CLRHost.dll\"");
                startInfo.UseShellExecute = false;
                Process process = Process.Start(startInfo);
                process.WaitForExit();
                App.Current.Shutdown();
            }
            catch (AccessViolationException)
            {
                App.Current.Shutdown();
                MessageBox.Show("An error occurred while attaching to that WoW process.\r\nPlease ensure you are logged into a character.");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (!v_series || SelectedWoW.Source == IntPtr.Zero) return;
            Point mouse = e.GetPosition(SelectedWoW);
            if (mouse.X >= 0 && mouse.Y >= 0) e.MouseDevice.SetCursor(Cursors.Hand); else e.MouseDevice.UpdateCursor();
        }
        private void lab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int PID = -1;
            if (!v_series)
            {
                var send = (sender as TextBlock);
                if (send == null) return;
                PID = (int)send.Tag;
            }
            else
            {
                Point mouse = e.GetPosition(SelectedWoW);
                if (mouse.X >= 0 && mouse.Y >= 0)
                    PID = (int)SelectedWoW.Tag;
            }
            if (PID <= 0) return;
            Attach(PID);
        }

        private void Sidebar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sidebar = (ListBox)sender;
            var WAV = (WoWAttachVisual)sidebar.SelectedItem;
            SelectedWoW.Source = WAV != null ? WAV.DWM : IntPtr.Zero;
            SelectedWoW.Tag = WAV != null ? WAV.PID : -1;
        }
    }
    public class WoWAttachVisual
    {
        public IntPtr DWM { get; set; }
        public int PID { get; set; }
        public WoWAttachVisual(IntPtr DWM, int PID)
        {
            this.DWM = DWM;
            this.PID = PID;
        }
    }
}
