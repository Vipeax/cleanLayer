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
            AddAction(new Shock(this, 10));
            AddAction(new HarmfulSpellAction(this, 9, "Lava Burst", 25));
            AddAction(new Shield(this, 8));
            AddAction(new HarmfulSpellAction(this, 7, "Lightning Bolt", 25));
        }

        public override WoWClass Class
        {
            get { return WoWClass.Shaman; }
        }

        public override string Specialization
        {
            get { return "Elemental"; }
        }

        private List<WoWUnit> Totems = new List<WoWUnit>();

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
            // Refresh our list of totems
            // TODO: Verify that SummonedBy is the right way to check if totems are ours!
            Totems = Manager.Objects.Where(x => x.IsValid && x.IsUnit).Select(x => x as WoWUnit).Where(x => x.SummonedBy == Manager.LocalPlayer.Guid && x.IsTotem).ToList();

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
                var tr = WoWSpell.GetSpell("Totemic Recall");
                if (tr.IsValid && tr.IsReady)
                {
                    Log.WriteLine("Removing totems");
                    tr.Cast();
                    Sleep(Globals.SpellWait);
                }
            }
            
            // Thunderstorm
            if (Manager.LocalPlayer.PowerPercentage < 70)
            {
                var ts = WoWSpell.GetSpell("Thunderstorm");
                if (ts.IsValid && ts.IsReady)
                {
                    Log.WriteLine("Casting Thunderstorm to regain mana");
                    ts.Cast();
                    Sleep(Globals.SpellWait);
                }
            }
        }

        // Decide what shock to use
        protected class Shock : HarmfulSpellAction
        {
            public Shock(Brain brain, int priority)
                : base(brain, priority, "Flame Shock", 25)
            { }

            public override string SpellName
            {
                get
                {  return (WoWSpell.GetSpell("Flame Shock").IsValid ? "Flame Shock" : "Earth Shock"); }
            }
        }

        // Decide what shield to use
        protected class Shield : HarmfulSpellAction
        {
            public Shield(Brain brain, int priority)
                : base(brain, priority, "Lightning Shield", 25)
            { }

            public override string SpellName
            {
                get { return (Manager.LocalPlayer.PowerPercentage < 40 ? "Water Shield" : "Lightning Shield"); }
            }
        }
    }
}
