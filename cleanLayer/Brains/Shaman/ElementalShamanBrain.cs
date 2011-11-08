using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanLayer.Library.Combat;

namespace cleanLayer.Brains.Shaman
{
    public class ElementalShamanBrain : Brain
    {
        // TODO: Add checks for whether our weapon has a Shaman enchant (they're enchants, so you can't check by aura)

        public ElementalShamanBrain()
        {
            AddAction(new Thunderstorm(this, 11));
            AddAction(new EarthShock(this, 10));
            AddAction(new FlameShock(this, 9));
            AddAction(new HarmfulSpellAction(this, 8, "Lava Burst", 25));
            AddAction(new Shield(this, 7));
            AddAction(new HarmfulSpellAction(this, 8, "Lightning Bolt", 25));
        }

        public override WoWClass Class
        {
            get { return WoWClass.Shaman; }
        }

        public override string Specialization
        {
            get { return "Elemental"; }
        }

        private bool _totemsSet = false;

        protected void SetTotems()
        {
            TotemHelper.SetTotemSlot(MultiCastSlot.ElementsEarth, Totem.Stoneskin);
            TotemHelper.SetTotemSlot(MultiCastSlot.ElementsFire, Totem.Searing);
            TotemHelper.SetTotemSlot(MultiCastSlot.ElementsWater, Totem.HealingStream);
            TotemHelper.SetTotemSlot(MultiCastSlot.ElementsAir, Totem.WrathOfAir);
        }

        protected override HarmfulSpellAction PullSpell
        {
            get
            {
                if (WoWSpell.GetSpell("Flame Shock").IsValid) // Make sure we have Flame Shock
                    return new HarmfulSpellAction(this, 0, "Flame Shock", 25);
                return new HarmfulSpellAction(this, 0, "Earth Shock", 25); // Shamans start with Earth Shock
            }
        }

        protected override void OnBeforeAction(ActionBase action)
        {
            if (!_totemsSet)
            {
                SetTotems();
                _totemsSet = true;
            }

            if (Helper.InCombat && Manager.LocalPlayer.Totems.Count == 0)
            {
                if (TotemHelper.CallTotems())
                    Sleep(Globals.SpellWait);
            }

            // Instant Lava Burst
            if (action is HarmfulSpellAction)
            {
                var haction = action as HarmfulSpellAction;
                if (haction.SpellName == "Lava Burst")
                {
                    var cd = WoWSpell.GetSpell("Elemental Mastery");
                    if (cd.IsValid && cd.IsReady)
                    {
                        Log.WriteLine("Popping Elemental Mastery for instant Lava Burst");
                        cd.Cast();
                        Sleep(Globals.SpellWait);
                    }
                }
            }
        }

        protected override void OnAfterAction(ActionBase action)
        {
            // Remove Totems
            if (!Helper.InCombat)
            {
                if (TotemHelper.RecallTotems())
                    Sleep(Globals.SpellWait);
            }
        }

        protected class Thunderstorm : HarmfulSpellAction
        {
            public Thunderstorm(Brain brain, int priority)
                : base(brain, priority, "Thunderstorm")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && (Manager.LocalPlayer.PowerPercentage < 70 || Brain.HarmfulTargets.Count(x => x.Distance < 8) > 2); }
            }
        }

        // Proc it!
        protected class EarthShock : HarmfulSpellAction
        {
            public EarthShock(Brain brain, int priority)
                : base(brain, priority, "Earth Shock", 25)
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Manager.LocalPlayer.Auras["Elemental Overload"].IsValid; }
            }
        }

        protected class FlameShock : HarmfulSpellAction
        {
            public FlameShock(Brain brain, int priority)
                : base(brain, priority, "Flame Shock", 25)
            { }

            // TODO: Fix the aura issue where CasterGuid gives "Arithmetic operation resulted in an overflow."
            public override bool IsWanted
            {
                get { return base.IsWanted && !Brain.HarmfulTarget.Auras["Flame Shock"].IsValid; }
                /*get { return base.IsWanted && Brain.HarmfulTarget.Auras.Where(x => x.IsValid && x.Name == "Flame Shock" && x.CasterGuid == Manager.LocalPlayer.Guid).Count() == 0; }*/
            }
        }

        // Decide what shield to use
        protected class Shield : SpellAction
        {
            public Shield(Brain brain, int priority)
                : base(brain, priority, "Lightning Shield", 6)
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && !Manager.LocalPlayer.Auras[SpellName].IsValid; }
            }

            public override string SpellName
            {
                get { return (Manager.LocalPlayer.PowerPercentage < 40 ? "Water Shield" : "Lightning Shield"); }
            }
        }
    }
}
