using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanLayer.Library;
using cleanLayer.Library.FSM;

namespace cleanLayer.Bots.GBStates
{
    public class GBCombat : State
    {
        private Grindbot _parent;
        public GBCombat(Grindbot parent)
        {
            _parent = parent;
        }

        public override int Priority
        {
            get { return 80; }
        }

        public override bool NeedToRun
        {
            get { return Attackers.Count > 0; }
        }

        private WoWUnit CurrentEnemy = WoWUnit.Invalid;
        public override void Run()
        {
            if (Manager.LocalPlayer.IsClickMoving || Mover.Status != MovementStatus.Stopped)
                Mover.StopMoving();
            Combat.Pulse();
        }

        public override string Description
        {
            get { return "Combat"; }
        }

        private List<WoWUnit> Attackers
        {
            get
            {
                return
                    Manager.Objects
                    .Where(x => x.IsValid && x.IsUnit)
                    .Select(x => x as WoWUnit)
                    .Where(x => !x.IsFriendly && x.TargetGuid == Manager.LocalPlayer.Guid)
                    .OrderBy(x => x.HealthPercentage)
                    .ToList();
            }
        }
    }
}
