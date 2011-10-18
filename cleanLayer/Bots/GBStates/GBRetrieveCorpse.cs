using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanLayer.Library;
using cleanLayer.Library.FSM;

namespace cleanLayer.Bots.GBStates
{
    public class GBRetrieveCorpse : State
    {
        private Grindbot _parent;
        public GBRetrieveCorpse(Grindbot parent)
        {
            _parent = parent;
        }

        public override int Priority
        {
            get { return 90; }
        }

        public override bool NeedToRun
        {
            get { return Manager.LocalPlayer.IsGhost && Manager.LocalPlayer.Location.DistanceTo(Manager.LocalPlayer.Corpse) < 8f; }
        }

        public override void Run()
        {
            Mover.StopMoving();
            WoWScript.ExecuteNoResults("RetrieveCorpse()");
        }

        public override string Description
        {
            get { return "Ressurecting"; }
        }
    }
}
