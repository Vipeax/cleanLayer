using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanLayer.Library;
using cleanLayer.Library.FSM;

namespace cleanLayer.Bots.MBStates
{
    public class MBFollow : State
    {
        private Multiboxer _parent;
        public MBFollow(Multiboxer parent)
        {
            _parent = parent;
        }

        public override int Priority
        {
            get { return 60; }
        }

        public override bool NeedToRun
        {
            get { return (!_parent.Leader.IsInCombat && !Helper.InCombat) && !_parent.FollowingLeader; }
        }

        public override void Run()
        {
            if (_parent.Leader.Distance > 8)
            {
                if (Manager.LocalPlayer.IsClickMoving)
                    return;
                Manager.LocalPlayer.ClickToMove(_parent.Leader.Location);
            }
            _parent.Leader.Select();
            WoWScript.ExecuteNoResults("FollowUnit(\"target\")");
            _parent.FollowingLeader = true;
        }

        public override string Description
        {
            get { return "Following leader"; }
        }
    }
}
