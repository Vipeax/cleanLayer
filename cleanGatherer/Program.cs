using System;
using System.Windows.Forms;
using cleanCore;
using cleanCore.D3D;
using cleanGatherer.FSM;

namespace cleanGatherer
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Offsets.Initialize(); // Initialize offset scanning
            Pulse.OnFrame += OnFrame; // Register OnFrame event

            Application.EnableVisualStyles();
            Application.Run(new Gatherer()); // Create our GUI
        }

        static void OnFrame(object sender, EventArgs e)
        {
            Engine.Pulse(); // Pulse the FSM engine
        }
    }
}
