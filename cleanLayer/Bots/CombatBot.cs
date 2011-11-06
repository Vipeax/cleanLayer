using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using cleanCore;
using cleanLayer.Library;
using cleanLayer.Library.FSM;

namespace cleanLayer.Bots
{
    public class CombatBot : BotBase
    {
        public override string Name
        {
            get { return "Pure Combat"; }
        }

        public override Form BotForm
        {
            get { return null; }
        }

        private bool _isRunning = false;
        public override bool IsRunning
        {
            get { return _isRunning; }
        }

        public override bool Start()
        {
            if (!Manager.IsInGame)
                return false;
            _isRunning = true;
            return true;
        }

        public override void Stop()
        {
            _isRunning = false;
        }

        public override void Pulse()
        {
            if (!_isRunning)
                return;
            if (!Manager.IsInGame)
                return;
            if (Combat.Brain == null)
                return;
            Combat.Pulse();
        }
    }
}
