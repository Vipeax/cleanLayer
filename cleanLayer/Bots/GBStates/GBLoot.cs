using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanLayer.Library;
using cleanLayer.Library.FSM;

namespace cleanLayer.Bots.GBStates
{
    public class GBLoot : State
    {
        private Grindbot _parent;
        public GBLoot(Grindbot parent)
        {
            _parent = parent;
        }

        public override int Priority
        {
            get { return 60; }
        }

        public override bool NeedToRun
        {
            get { return Lootables.Count > 0; }
        }

        private WoWUnit CurrentLootable = WoWUnit.Invalid;
        public override void Run()
        {
            CurrentLootable = Lootables.FirstOrDefault() ?? WoWUnit.Invalid;

            if (CurrentLootable != null && CurrentLootable.IsValid)
            {
                if (CurrentLootable.Distance > 4f)
                {
                    if (Mover.Status != MovementStatus.Moving || Mover.Destination != CurrentLootable.Location)
                    {
                        _parent.Print("Moving to loot {0}", CurrentLootable.Name);
                        if (!Mover.PathTo(CurrentLootable.Location))
                            _parent.Blacklisted.Add(CurrentLootable.Guid);

                    }
                    _parent.FSM.DelayNextPulse(500);
                }
                else
                {
                    CurrentLootable.Interact();
                    if (Manager.LocalPlayer.IsLooting)
                    {
                        WoWScript.ExecuteNoResults(
                            "local res = GetCVar(\"AutoLootDefault\") if res == \"0\" then for i = GetNumLootItems(), 1, -1 do LootSlot(i) end end CloseLoot()");
                    }
                    _parent.FSM.DelayNextPulse(2000);
                }
            }
        }

        public override string Description
        {
            get { return "Ressurecting"; }
        }

        private List<WoWUnit> Lootables
        {
            get
            {
                return
                    Manager.Objects
                    .Where(x => x.IsValid && x.IsUnit)
                    .Select(x => x as WoWUnit)
                    .Where(x => !_parent.Blacklisted.Contains(x.Guid) && x.IsLootable)
                    .OrderBy(x => x.Distance)
                    .ToList();
            }
        }
    }
}
