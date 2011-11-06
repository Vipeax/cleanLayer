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
using cleanLayer.Bots.MBStates;


namespace cleanLayer.Bots
{
    public class Multiboxer : BotBase
    {
        public Multiboxer()
        {
            FSM = new Engine();
            FSM.LoadStates(new List<State>()
            {
                new MBStopFollow(this),
                new MBCombat(this),
                new MBLoot(this),
                new MBFollow(this),
                new MBIdle(),
            });
        }

        public const string VERSION = "1.0";

        public Engine FSM;
        public WoWPlayer Leader = WoWPlayer.Invalid;
        public bool FollowingLeader = false;
        private string _lastStateText = string.Empty;

        public override string Name
        {
            get { return "Multiboxer v" + VERSION; }
        }

        public override bool Start()
        {
            if (!Manager.IsInGame)
                return false;

            if (Combat.Brain == null)
                return false;

            Leader = WoWParty.Members.FirstOrDefault() ?? WoWPlayer.Invalid;
            if (!Leader.IsValid)
                return false;

            FSM.Start();

            return true;
        }

        public override void Stop()
        {
            FSM.Stop();
            Leader = WoWPlayer.Invalid;
            FollowingLeader = false;
        }

        public override bool IsRunning
        {
            get { return FSM.IsRunning; }
        }

        public override void Pulse()
        {
            if (!FSM.IsRunning)
                return;

            if (!Manager.IsInGame)
                return;

            if (!Leader.IsValid)
                return;

            // Pulse the damn engine
            FSM.Pulse();

            if (_lastStateText == FSM.StateText)
                return;
            _lastStateText = FSM.StateText;
            Print(_lastStateText);
        }

        public override Form BotForm
        {
            get { return null; }
        }
    }
}
