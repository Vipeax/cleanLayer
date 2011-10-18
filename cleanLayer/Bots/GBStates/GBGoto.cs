using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanLayer.Library;
using cleanLayer.Library.FSM;

namespace cleanLayer.Bots.GBStates
{
    public class GBGoto : State
    {
        private Grindbot _parent;
        private Location _hotspot = Location.Zero;

        public GBGoto(Grindbot parent)
        {
            _parent = parent;
        }
        public override int Priority
        {
            get { return 70; }
        }

        public override bool NeedToRun
        {
            get { return ClosestHotspot.DistanceTo(Manager.LocalPlayer.Location) > 75f; }
        }

        public override void Run()
        {
            if (_hotspot == Location.Zero)
                _hotspot = ClosestHotspot;

            if (Mover.Destination != _hotspot)
                Mover.PathTo(_hotspot);
        }

        public override string Description
        {
            get { return "Moving to hotspot"; }
        }

        private Location ClosestHotspot
        {
            get { return _parent.Hotspots.OrderBy(x => x.DistanceTo(Manager.LocalPlayer.Location)).FirstOrDefault(); }
        }
    }
}
