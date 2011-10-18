//
// miceiken's Gatherer v1.0
//
// I want to express great gratitude to caytchen
// for cleanCore and all the help and support he has given me
// along this journey
//
// Credits goes to:
// caytchen     -- as mentioned above
// Apoc         -- Navigation system and Circular Queue
// Kryso        -- PyMode (mainly the Log part, in here)
// amadmonk     -- introducing me to tasks
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using cleanGatherer.FSM;
using cleanCore;
using System.Threading.Tasks;

namespace cleanGatherer
{
    public partial class Gatherer : Form, ILog
    {
        public Gatherer()
        {
            InitializeComponent();
        }

        private void btnToggle_Click(object sender, EventArgs e)
        {
            if (Engine.IsRunning)
            {
                Engine.Stop(); // Stop the FSM eninge
                btnToggle.Text = "Start"; // Change the text of the button
                btnLoadProfile.Enabled = true; // Allow user to change profile
                Log.WriteLine("Stopping bot");
            }
            else
            {
                if (!Engine.HasWaypoints)
                    return; // The engine has no profile

                Engine.Start(); // Start the FSM engine
                btnToggle.Text = "Stop"; // Change the text of the button
                btnLoadProfile.Enabled = false; // Disallow the user to change profile while running
                Log.WriteLine("Starting bot");
            }
        }

        private void btnLoadProfile_Click(object sender, EventArgs e)
        {
            // We set up a task for loading profile since
            // starting the OpenFileDialog would most likely 
            // cause the rendering to stop as we don't have our own thread :)
            var task = new Task<CircularQueue<Location>>(() => SelectFile());
            task.RunSynchronously();
            var result = task.Result;
            if (result == null)
                return; // SelectFile() failed

            Log.WriteLine("Profile loaded ({0} waypoints)", result.Count);
            Engine.Initialize(result); // Reinstantiate the FSM engine with the new profile (as string path) and amount of frames between each pulse
            btnToggle.Enabled = true;
        }

        private CircularQueue<Location> SelectFile()
        {
            var fileDlg = new OpenFileDialog();
            fileDlg.Filter = "Profiles|*.xml";
            if (fileDlg.ShowDialog() != DialogResult.OK)
                return null; // User most likely canceled

            if (string.IsNullOrEmpty(fileDlg.FileName))
                return null; // User did not select a valid item

            CircularQueue<Location> waypoints;
            if ((waypoints = Navigation.LoadFile(fileDlg.FileName)) == null)
                return null; // Unable to load the path as a profile

            return waypoints;
        }

        private void Gatherer_Load(object sender, EventArgs e)
        {
            Log.AddReader(this); // Add this object as a reader for the log (ILog interface, Log class)
            //Settings.Load();
            btnToggle.Enabled = false; // The user must choose profile before starting the bot

            if (!Manager.IsInGame)
                return;

            FlyingMounts = GetMounts();
            foreach (var m in FlyingMounts)
                Log.WriteLine("{0} = {1}", m.Key, m.Value);
            //foreach (var m in FlyingMounts)
            //    cbMount.Items.Add(m.Key);
        }

        public static WoWGameObject HarvestTarget
        {
            get
            {
                return (from go in Manager.Objects // Get all objects
                        where go.IsGameObject // Select only GameObjects
                        && Globals.HarvestNames.Contains(go.Name) // Whitelisted gameobject names (Check Globals.cs)
                        orderby go.Distance ascending // Sort by distance (closest first)
                        select go as WoWGameObject).FirstOrDefault() ?? WoWGameObject.Invalid; // Return results as WoWGameObjects, if null WoWGameObject.Invalid
            }
        }

        public void WriteLine(string entry)
        { // For the ILog interface
            tbLogBox.Invoke((Action)(() =>
            {
                tbLogBox.AppendText(entry + Environment.NewLine);
                tbLogBox.ScrollToCaret();
            }));
        }

        private void timer1_Tick(object sender, EventArgs e)
        { // Update GUI labels
            lblHarvests.Text = Globals.Harvests.ToString();
            lblKills.Text = Globals.Kills.ToString();
            lblDeaths.Text = Globals.Deaths.ToString();
        }

        private Dictionary<string, int> FlyingMounts;
        private Dictionary<string, int> GetMounts()
        {
            var mountDictionary = new Dictionary<string, int>();
            var mountCount = 0;
            var NumCompanions = WoWScript.Execute("GetNumCompanions(\"mount\")");
            if (NumCompanions != null && NumCompanions.Count > 0)
                mountCount = int.Parse(NumCompanions[0]);

            if (mountCount > 0)
            {
                for (int i = 0; i < mountCount; i++)
                {
                    var mountInfo = WoWScript.Execute("GetCompanionInfo(\"mount\", " + i + ")");
                    if (mountInfo != null && mountInfo.Count > 0)
                    {
                        var mountFlags = int.Parse(mountInfo[5]);
                        if ((mountFlags & 0x02) != 0) // It's a flying mount
                            mountDictionary.Add(mountInfo[1], i);
                    }
                }
            }
            return mountDictionary;
        }

        private void cbMount_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedMount = cbMount.SelectedText;
            if (FlyingMounts.ContainsKey(selectedMount))
                Globals.MountId = FlyingMounts[selectedMount];
        }
    }
}
