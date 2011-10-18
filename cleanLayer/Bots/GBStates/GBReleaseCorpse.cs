using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanLayer.Library;
using cleanLayer.Library.FSM;

namespace cleanLayer.Bots.GBStates
{
    public class GBReleaseCorpse : State
    {
        private Grindbot _parent;

        public GBReleaseCorpse(Grindbot parent)
        {
            _parent = parent;
        }

        public override int Priority
        {
            get { return 110; }
        }

        public override bool NeedToRun
        {
            get { return Manager.LocalPlayer.IsDead && !Manager.LocalPlayer.IsGhost; }
        }

        public override void Run()
        {
            Mover.StopMoving();
            WoWScript.ExecuteNoResults("RepopMe()");
            _parent.FSM.DelayNextPulse(1000);
        }

        public override string Description
        {
            get { return "Releasing corpse"; }
        }
    }
}
