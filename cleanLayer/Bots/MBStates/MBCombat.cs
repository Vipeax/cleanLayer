using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanLayer.Library;
using cleanLayer.Library.FSM;

namespace cleanLayer.Bots.MBStates
{
    public class MBCombat : State
    {
        private Multiboxer _parent;
        public MBCombat(Multiboxer parent)
        {
            _parent = parent;
        }

        public override int Priority
        {
            get { return 80; }
        }

        public override bool NeedToRun
        {
            get { return Helper.InCombat || _parent.Leader.IsInCombat; }
        }

        public override void Run()
        {
            Combat.Pulse();
        }

        public override string Description
        {
            get { return "Combat!"; }
        }
    }
}
