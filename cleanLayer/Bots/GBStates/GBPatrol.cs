using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanLayer.Library;
using cleanLayer.Library.FSM;
using cleanCore;

namespace cleanLayer.Bots.GBStates
{
    public class GBPatrol : State
    {
        private Grindbot _parent;
        private Location _hotspot = Location.Zero;
        private Random _rand = new Random();

        public GBPatrol(Grindbot parent)
        {
            _parent = parent;
        }
        public override int Priority
        {
            get { return 0; }
        }

        public override bool NeedToRun
        {
            get { return true; }
        }

        public override void Run()
        {
            if (_hotspot == Location.Zero)
                _hotspot = _parent.Hotspots[_rand.Next(0, _parent.Hotspots.Count - 1)];

            if (Manager.LocalPlayer.Location.DistanceTo(_hotspot) < 5f)
                _hotspot = _parent.Hotspots[_rand.Next(0, _parent.Hotspots.Count - 1)];

            if (Mover.Destination != _hotspot)
                Mover.PathTo(_hotspot);
        }

        public override string Description
        {
            get { return "Patroling"; }
        }
    }
}
