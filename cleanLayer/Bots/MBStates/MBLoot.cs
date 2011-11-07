using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanLayer.Library;
using cleanLayer.Library.FSM;

namespace cleanLayer.Bots.MBStates
{
    public class MBLoot : State
    {
        private Multiboxer _parent;
        private bool Moving = false;
        public MBLoot(Multiboxer parent)
        {
            _parent = parent;
        }

        public override int Priority
        {
            get { return 70; }
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
                    if (!Moving || !Manager.LocalPlayer.IsClickMoving)
                    {
                        Moving = true;
                        _parent.Print("Moving to loot {0}", CurrentLootable.Name);
                        Mover.MoveTo(CurrentLootable.Location);
                    }
                    _parent.FSM.DelayNextPulse(500);
                }
                else
                {
                    Moving = false;
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
            get { return "Looting"; }
        }

        private List<WoWUnit> Lootables
        {
            get
            {
                return
                    Manager.Objects
                    .Where(x => x.IsValid && x.IsUnit)
                    .Select(x => x as WoWUnit)
                    .Where(x => x.IsLootable && x.Distance < Globals.MaxDistance)
                    .OrderBy(x => x.Distance)
                    .ToList();
            }
        }
    }
}
