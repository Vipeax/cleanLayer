using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using cleanCore;
using cleanLayer.GUI;
using cleanLayer.Library;
using cleanLayer.Library.Bots;
using cleanLayer.Library.Combat;
using cleanLayer.Library.Scripts;

namespace cleanLayer
{
    public partial class CleanForm : Form, ILog
    {
        public CleanForm()
        {
            InitializeComponent();
        }

        #region Form globals

        private void GUITimer_Tick(object sender, EventArgs e)
        {
            if (!Manager.IsInGame)
                return;

            if (ScriptManager.ScriptPool.Where(s => s.IsRunning).Contains(SelectedScript))
            {
                btnScriptStart.Enabled = false;
                btnScriptStop.Enabled = true;
            }
            else
            {
                btnScriptStart.Enabled = true;
                btnScriptStop.Enabled = false;
            }

            WoWLocalPlayer me = Manager.LocalPlayer;

            pbHealth.Maximum = (int)me.MaxHealth;
            pbHealth.Value = (int)me.Health;

            pbPower.Maximum = (int)me.MaxPower;
            pbPower.Value = (int)me.Power;
        }

        private void SetupBrains()
        {
            WoWBrains.Initialize();
            cbBrains.DataSource = WoWBrains.BrainsForClass(Manager.LocalPlayer.Class);
            Log.WriteLine("Loaded {0} brains.", WoWBrains.BrainPool.Count);
        }

        private void SetupBots()
        {
            WoWBots.Initialize();
            cbBots.DataSource = WoWBots.BotPool;
            Log.WriteLine("Loaded {0} bots.", WoWBots.BotPool.Count);
        }

        private void SetupScripts()
        {
            lstScripts.DataSource = ScriptManager.ScriptPool.ToList();
            Log.WriteLine("Loaded {0} scripts.", ScriptManager.ScriptPool.Count);
        }

        private void CleanForm_Load(object sender, EventArgs e)
        {
            Log.AddReader(this);
            Log.WriteLine("cleanLayer loaded");

            SetupBots();
            SetupBrains();
            SetupScripts();

            lstLocations.DataSource = Locations.Keys.ToList();
        }

        #endregion

        #region Main tab

        private void cbBots_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bot.Initialize((BotBase)cbBots.SelectedItem);
            btnBotSettings.Enabled = Bot.CurrentBot.BotForm != null;
            btnBotStart.Enabled = Bot.CurrentBot != null && Combat.Brain != null;
        }

        private void btnBotSettings_Click(object sender, EventArgs e)
        {
            if (Bot.CurrentBot == null)
            {
                Log.WriteLine("You haven't selected any bots");
                return;
            }

            if (Bot.CurrentBot.BotForm == null)
            {
                Log.WriteLine("This bot doesn't have any settings");
                return;
            }

            Bot.CurrentBot.BotForm.Show();
        }

        private void cbBrains_SelectedIndexChanged(object sender, EventArgs e)
        {
            Combat.Initialize((Brain)cbBrains.SelectedItem);
            btnBotStart.Enabled = Bot.CurrentBot != null && Combat.Brain != null;
        }

        private void btnBotStart_Click(object sender, EventArgs e)
        {
            if (Bot.CurrentBot == null)
                return;

            if (Bot.Start())
            {
                btnBotStart.Enabled = false;
                btnBotStop.Enabled = true;

                cbBots.Enabled = false;
                btnBotSettings.Enabled = false;
                cbBrains.Enabled = false;
            }
        }

        private void btnBotStop_Click(object sender, EventArgs e)
        {
            if (Bot.CurrentBot == null)
                return;
            Bot.Stop();
            btnBotStart.Enabled = true;
            btnBotStop.Enabled = false;
            cbBots.Enabled = true;
            btnBotSettings.Enabled = Bot.CurrentBot.BotForm != null;
            cbBrains.Enabled = true;
        }

        #endregion

        #region Debug tab

        private void btnSpellDump_Click(object sender, EventArgs e)
        {
            List<WoWSpell> spells = WoWSpell.GetAllSpells();
            foreach (WoWSpell spell in spells)
                Log.WriteLine("#{0} = {1}", spell.Id, spell.Name);
        }

        private void btnExecuteLua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbLua.Text))
                return;

            Program.OnFrameOnce += delegate
            {
                Log.WriteLine(tbLua.Text);
                List<string> ret = WoWScript.Execute(tbLua.Text);
                for (int i = 0; i < ret.Count; i++)
                {
                    Log.WriteLine("\t[{0}] = \"{1}\"", i, ret[i]);
                }
            };
        }

        private void btnTotems_Click(object sender, EventArgs e)
        {
            var allObjects = Manager.Objects;
            var allTotems = allObjects.Where(x => x.IsValid && x.IsUnit).Select(x => x as WoWUnit).Where(x => x.IsTotem).ToList();
            var totemsSummoned = allTotems.Where(x => x.SummonedBy == Manager.LocalPlayer.Guid);
            var totemsCreated = allTotems.Where(x => x.CreatedBy == Manager.LocalPlayer.Guid);
            Log.WriteLine("Totally {0} totems:", allTotems.Count);
            foreach (var t in allTotems)
            {
                Log.WriteLine("N: {0} - S: {1} - C: {2}", t.Name, t.SummonedBy, t.CreatedBy);
            }
            Log.WriteLine("Summoned by me: {0}", totemsSummoned.Count());
            Log.WriteLine("Created by me: {0}", totemsCreated.Count());
        }

        private void btnMounts_Click(object sender, EventArgs e)
        {
            Program.OnFrameOnce += delegate
            {
                var mounts = WoWMounts.GetAllMounts();
                Log.WriteLine("Dumping {0} mounts", mounts.Count);
                foreach (var m in mounts)
                    m.DumpProperties();
            };
        }

        private void btnEnchants_Click(object sender, EventArgs e)
        {
            var items = Manager.LocalPlayer.Items;
            foreach (var i in items)
            {
                if (i.Enchants.Count > 0)
                {
                    Log.WriteLine("{0}:", i.Name);
                    foreach (var en in i.Enchants)
                    {
                        Log.WriteLine("\tID: {0}", en);
                    }
                }
            }
        }

        #endregion

        #region Scripts tab

        private Script SelectedScript;

        private void btnScriptStart_Click(object sender, EventArgs e)
        {
            if (SelectedScript == null)
                return;

            SelectedScript.Start();
        }

        private void btnScriptStop_Click(object sender, EventArgs e)
        {
            if (SelectedScript == null)
                return;

            SelectedScript.Stop();
        }

        private void lstScripts_SelectedIndexChanged(object sender, EventArgs e)
        {
            Script script = ScriptManager.ScriptPool.Where(s => s == lstScripts.SelectedItem).First();
            if (script == null)
                return;

            SelectedScript = script;
        }

        #endregion

        #region Locations tab

        private readonly Dictionary<string, Location> Locations = new Dictionary<string, Location>
        {
            {
                "Orgrimmar",
                new Location(1397.2f, -4367.8f, 25.3f)
                },
            {
                "Stormwind",
                new Location(-8957.4f, 517.3f, 96.3f)
                },
            {
                "Crossroads",
                new Location(-448.6f, -2641.4f, 95.5f)
                },
        };

        private void btnPathTo_Click(object sender, EventArgs e)
        {
            Location target;
            Locations.TryGetValue((string)lstLocations.SelectedItem, out target);
            if (target == null)
                return;

            Mover.PathTo(target);
        }

        private void btnMountUp_Click(object sender, EventArgs e)
        {
            Program.OnFrameOnce += delegate
            {
                var m = WoWMounts.RandomMount();
                Log.WriteLine("Mounted {0}", m);
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GUIThread.Initialize();
        }

        private void btnShapeshift_Click(object sender, EventArgs e)
        {
            Log.WriteLine("Shapeshift Form ID: #{0} - {1}", (int)Manager.LocalPlayer.Shapeshift, Manager.LocalPlayer.Shapeshift.ToString());
        }

        private void btnDumpMe_Click(object sender, EventArgs e)
        {
            Manager.LocalPlayer.DumpProperties();
        }

        #endregion

        #region ILog interface

        public void WriteLine(string entry)
        {
            rbLogBox.Invoke((Action)(() =>
                                          {
                                              rbLogBox.AppendText(entry + Environment.NewLine);
                                              rbLogBox.ScrollToCaret();
                                          }));
        }
        #endregion

        private void btnAuraDump_Click(object sender, EventArgs e)
        {
            Program.OnFrameOnce += delegate
            {
                var aura = Manager.LocalPlayer.Auras;
                foreach (var a in aura)
                {
                    a.DumpProperties();
                    //Log.WriteLine("#{0}: {1}", a.ID, a.Name);
                }
            };
        }
    }
}