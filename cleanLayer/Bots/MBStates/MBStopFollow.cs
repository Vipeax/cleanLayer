using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanLayer.Library;
using cleanLayer.Library.FSM;

namespace cleanLayer.Bots.MBStates
{
    public class MBStopFollow : State
    {
        private Multiboxer _parent;
        public MBStopFollow(Multiboxer parent)
        {
            _parent = parent;
        }

        public override int Priority
        {
            get { return 90; }
        }

        public override bool NeedToRun
        {
            get { return (_parent.Leader.IsInCombat || Helper.InCombat) && _parent.FollowingLeader; }
        }

        public override void Run()
        {
            Mover.MoveTo(_parent.Leader.Location);
            _parent.FollowingLeader = false;
        }

        public override string Description
        {
            get { return "Stopped following"; }
        }
    }
}
