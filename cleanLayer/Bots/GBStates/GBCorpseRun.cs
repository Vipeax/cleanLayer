using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanLayer.Library;
using cleanLayer.Library.FSM;

namespace cleanLayer.Bots.GBStates
{
    public class GBCorpseRun : State
    {
        private Grindbot _parent;
        public GBCorpseRun(Grindbot parent)
        {
            _parent = parent;
        }

        public override int Priority
        {
            get { return 100; }
        }

        public override bool NeedToRun
        {
            get { return Manager.LocalPlayer.IsGhost && Manager.LocalPlayer.Location.DistanceTo(Manager.LocalPlayer.Corpse) > 8f; }
        }

        public override void Run()
        {
            if (!Mover.IsCorpseRunning)
            {
                Mover.StopMoving();
                Mover.MoveToCorpse();
            }

            _parent.FSM.DelayNextPulse(3000); // Unlikely that the Engine will need a pulse the next 3 seconds...
        }

        public override string Description
        {
            get { return "Returning to corpse"; }
        }
    }
}
