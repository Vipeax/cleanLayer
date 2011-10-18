using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using cleanCore;
using cleanLayer.Bots.GBStates;
using cleanLayer.Library;
using cleanLayer.Library.FSM;

namespace cleanLayer.Bots
{
    public class Grindbot : BotBase
    {
        private const string version = "0.1b";

        private List<string> EventSubscriptions = new List<string>
                                                      {
                                                          "PLAYER_LEVEL_UP"
                                                      };

        public Engine FSM;
        public List<Location> Hotspots;

        public HBProfile Profile;
        public SubProfile SubProfile;
        private GrindBotForm _Form;

        public List<ulong> Blacklisted = new List<ulong>(); 

        private string _lastStateText = string.Empty;
        private bool _leveledUp = false;

        public Grindbot()
        {
            FSM = new Engine(333);
        }

        public override string Name
        {
            get { return "GrindBot " + version; }
        }

        public override bool IsRunning
        {
            get { return FSM.IsRunning; }
        }

        public override Form BotForm
        {
            get
            {
                _Form = new GrindBotForm(this);
                return _Form;
            }
        }

        public override bool Start()
        {
            if (!Manager.IsInGame)
            {
                Print("Please login before starting the bot");
                return false;
            }

            if (Profile == null)
            {
                Print("Please load a profile in the bot settings before starting the bot");
                return false;
            }

            //Print("Profile {0} (Level range {1}-{2}) by {3} successfully loaded with {4} subprofiles", Profile.Name, Profile.MinLevel, Profile.MaxLevel, Profile.Creator, Profile.SubProfile.Count());

            if (!SetupProfile())
            {
                Print("There was a problem loading a suitable subprofile.");
                return false;
            }

            foreach (var ev in EventSubscriptions)
                Events.Register(ev, HandleEvents);

            Blacklisted.Clear();
            FSM.Start();
            return true;
        }

        public override void Stop()
        {
            foreach (var ev in EventSubscriptions)
                Events.Remove(ev, HandleEvents);
            Mover.StopMoving();
            FSM.Stop();
        }

        private bool SetupProfile()
        {
            try
            {
                if (Profile == null)
                    return false;

                //SubProfile = ProfileHelper.GetAppropriateSubProfile(Profile);
                var myLevel = Manager.LocalPlayer.Level;
                foreach (var s in Profile.SubProfile)
                {
                    if (s.MinLevel <= myLevel && s.MaxLevel >= myLevel)
                        SubProfile = s;
                }

                // Profile doesnt contain anything for our level?
                if (SubProfile == null)
                    return false;

                if (Hotspots == null)
                    Hotspots = new List<Location>();
                Hotspots.Clear();

                for (int i = 0; i < SubProfile.GrindArea[0].Hotspots.Count(); i++)
                {
                    var hotspot = SubProfile.GrindArea[0].Hotspots[i];
                    Hotspots.Add(hotspot.Location);
                }

                //Print("Loaded subprofile {0} (Level range: {1}-{2}) with {3} hotspots", SubProfile.Name, SubProfile.MinLevel,
                //      SubProfile.MaxLevel, Hotspots.Count);

                var states = new List<State>
                                 {
                                     new GBReleaseCorpse(this),
                                     new GBCorpseRun(this),
                                     new GBRetrieveCorpse(this),
                                     new GBCombat(this),
                                     new GBGoto(this),
                                     new GBLoot(this),
                                     new GBPull(this),
                                     new GBPatrol(this)
                                 };

                FSM.LoadStates(states);

                //Print("Loaded {0} states", states.Count);
            }
            catch (Exception)
            {
                Print("This profile is a mess, fix it or get a new one!");
                return false;
            }
            return true;
        }

        private void HandleEvents(string ev, List<string> args)
        {
            switch (ev)
            {
                case "PLAYER_LEVEL_UP":
                    _leveledUp = true;
                    break;
            }
        }

        public override void Pulse()
        {
            // We aren't meant to do anything...
            if (!FSM.IsRunning)
                return;

            // No point in doing anything if we aren't in game!
            if (!Manager.IsInGame)
                return;

            if (_leveledUp)
            {
                Stop();
                Print("Leveled up!");
                SetupProfile();
                Start();

                _leveledUp = false;
            }

            FSM.Pulse();

            if (_lastStateText == FSM.StateText)
                return;
            _lastStateText = FSM.StateText;
            Print(_lastStateText);
        }

        #region Form

        public class GrindBotForm : Form
        {
            private readonly Grindbot _Parent;

            /// <summary>
            /// Required designer variable.
            /// </summary>
            private readonly IContainer components;

            private Button btnLoadProfile;

            private TabControl tabControl1;
            private TabPage tabPage1;


            public GrindBotForm(Grindbot bot)
            {
                _Parent = bot;
                InitializeComponent();
            }

            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
            protected override void Dispose(bool disposing)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }

            private void btnLoadProfile_Click(object sender, EventArgs e)
            {
                // We set up a task for loading profile since
                // starting the OpenFileDialog would most likely 
                // cause the rendering to stop as we don't have our own thread :)
                var task = new Task<HBProfile>(() => SelectFile());
                task.RunSynchronously();
                HBProfile result = task.Result;
                if (result == null)
                    return; // SelectFile() failed

                _Parent.Profile = result;
            }

            private HBProfile SelectFile()
            {
                var fileDlg = new OpenFileDialog();
                fileDlg.Filter = "Profiles|*.xml";
                if (fileDlg.ShowDialog() != DialogResult.OK)
                    return null; // User most likely canceled

                if (string.IsNullOrEmpty(fileDlg.FileName))
                    return null; // User did not select a valid item

                HBProfile profile;
                if ((profile = Library.ProfileHelper.GetProfile(fileDlg.FileName)) == null)
                    return null; // Unable to load the path as a profile

                return profile;
            }

            #region Windows Form Designer generated code

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent()
            {
                this.tabControl1 = new System.Windows.Forms.TabControl();
                this.tabPage1 = new System.Windows.Forms.TabPage();
                this.btnLoadProfile = new System.Windows.Forms.Button();
                this.tabControl1.SuspendLayout();
                this.tabPage1.SuspendLayout();
                this.SuspendLayout();
                // 
                // tabControl1
                // 
                this.tabControl1.Controls.Add(this.tabPage1);
                this.tabControl1.Location = new System.Drawing.Point(12, 12);
                this.tabControl1.Name = "tabControl1";
                this.tabControl1.SelectedIndex = 0;
                this.tabControl1.Size = new System.Drawing.Size(426, 236);
                this.tabControl1.TabIndex = 0;
                // 
                // tabPage1
                // 
                this.tabPage1.Controls.Add(this.btnLoadProfile);
                this.tabPage1.Location = new System.Drawing.Point(4, 22);
                this.tabPage1.Name = "tabPage1";
                this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
                this.tabPage1.Size = new System.Drawing.Size(418, 210);
                this.tabPage1.TabIndex = 0;
                this.tabPage1.Text = "Settings";
                this.tabPage1.UseVisualStyleBackColor = true;
                // 
                // btnLoadProfile
                // 
                this.btnLoadProfile.Location = new System.Drawing.Point(337, 181);
                this.btnLoadProfile.Name = "btnLoadProfile";
                this.btnLoadProfile.Size = new System.Drawing.Size(75, 23);
                this.btnLoadProfile.TabIndex = 0;
                this.btnLoadProfile.Text = "Load Profile";
                this.btnLoadProfile.UseVisualStyleBackColor = true;
                this.btnLoadProfile.Click += new System.EventHandler(this.btnLoadProfile_Click);
                // 
                // GrindBotForm
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(450, 260);
                this.Controls.Add(this.tabControl1);
                this.Name = "GrindBotForm";
                this.Text = "Grindbot Settings";
                this.tabControl1.ResumeLayout(false);
                this.tabPage1.ResumeLayout(false);
                this.ResumeLayout(false);
            }

            #endregion
        }

        #endregion
    }
}