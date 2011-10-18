using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;

namespace cleanGatherer.FSM.States
{
    public class Harvest : State
    {
        public override int Priority
        {
            get { return 4; }
        }

        public override bool NeedToRun
        {
            get
            {
                return (Gatherer.HarvestTarget.IsValid && Gatherer.HarvestTarget.Distance < 3);
            }
        }

        public override void Run()
        {
            WoWScript.ExecuteNoResults("Dismount()");
            Gatherer.HarvestTarget.Interact();
            Engine.DelayNextPulse(Globals.SleepTime); // Let's give ourself about 1,5 seconds to harvest this node
            Globals.Harvests++;
        }
    }
}
