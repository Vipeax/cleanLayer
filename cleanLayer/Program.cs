using System;
using System.Windows.Forms;
using cleanCore;
using cleanCore.D3D;
using cleanLayer.Library;
using cleanLayer.Library.Scripts;
using cleanLayer.Library.Bots;

namespace cleanLayer
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Offsets.Initialize();
            Pulse.OnFrame += OnFrame;
            ScriptManager.Initialize();

            Application.EnableVisualStyles();
            Application.Run(new CleanForm());
        }

        public delegate void OnFrameDelegate();
        public static event OnFrameDelegate OnFrameOnce;

        static void OnFrame(object sender, EventArgs e)
        {
            WoWSpell.Pulse();
            ScriptManager.Pulse();
            Mover.Pulse();
            Bot.Pulse();

            if (OnFrameOnce != null)
            {
                OnFrameOnce();
                OnFrameOnce = null;
            }
        }

        public static string Directory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }
    }
}
