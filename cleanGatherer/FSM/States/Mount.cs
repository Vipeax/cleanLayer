using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;

namespace cleanGatherer.FSM.States
{
    public class Mount : State
    {
        public override int Priority
        {
            get { return 2; }
        }

        public override bool NeedToRun
        {
            get
            {
                return ((!Gatherer.HarvestTarget.IsValid || Gatherer.HarvestTarget.Distance > 10) && !Manager.LocalPlayer.HasAura(Globals.MountId)) /*!WoWScript.Execute<bool>("IsMounted()", 0))*/;
            }
        }

        public override void Run()
        {
            Manager.LocalPlayer.StopCTM();
            WoWScript.ExecuteNoResults("CastSpellByID(" + Globals.MountId + ")");
            Engine.DelayNextPulse(Globals.SleepTime); // Mounting takes 1,5 seconds so let's delay our next pulse
        }
    }
}
