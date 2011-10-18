using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;

namespace cleanGatherer.FSM.States
{
    public class Approach : State
    {
        public override int Priority
        {
            get { return 3; }
        }

        public override bool NeedToRun
        {
            get
            {
                return (Gatherer.HarvestTarget.IsValid && Gatherer.HarvestTarget.Distance > 10);
            }
        }

        public override void Run()
        {
            Manager.LocalPlayer.ClickToMove(Gatherer.HarvestTarget.Location);
            Engine.DelayNextPulse(Globals.SleepTime);
        }
    }
}
