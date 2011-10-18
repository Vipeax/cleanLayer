using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cleanGatherer.FSM.States
{
    public class Patrol : State
    {
        public Patrol()
        {
            Navigator = new Navigation(Engine.Waypoints);
            Navigator.SetDestinationToNearest();
        }

        private Navigation Navigator;

        public override int Priority
        {
            get { return 1; }
        }

        public override bool NeedToRun
        {
            get { return true; }
        }

        public override void Run()
        {
            Navigator.NavigateWaypoints();
        }
    }
}
