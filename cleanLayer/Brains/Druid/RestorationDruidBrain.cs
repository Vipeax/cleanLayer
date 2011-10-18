using System.Linq;
using cleanCore;
using cleanLayer.Library.Combat;

namespace cleanLayer.Brains
{
    public class RestorationDruidBrain : Brain
    {
        public RestorationDruidBrain()
        {
            AddAction(new Rejuvenation(this, 1));
            AddAction(new Regrowth(this, 2));
            AddAction(new Nourish(this, 4));
            AddAction(new Swiftmend(this, 5));
            AddAction(new WildGrowth(this, 6));
            AddAction(new HealingTouch(this, 7));
        }

        public override WoWClass Class
        {
            get { return WoWClass.Druid; }
        }

        public override string Specialization
        {
            get { return "Restoration"; }
        }

        protected override HarmfulSpellAction PullSpell
        {
            get { return new HarmfulSpellAction(this, 1, "Moonfire", 30); }
        }

        private WoWPlayer PartyTank = WoWPlayer.Invalid;
        protected override void OnBeforeAction(ActionBase action)
        {
            PartyTank = WoWParty.Members.OrderByDescending(m => m.MaxHealth).First() ?? WoWPlayer.Invalid;
            var ns = WoWSpell.GetSpell("Nature's Swiftness");
            if (action is HealingTouch)
            {
                if (HelpfulTarget.IsValid
                    && HelpfulTarget.Distance < Globals.MaxDistance
                    && HelpfulTarget.HealthPercentage < 20
                    && ns.IsValid
                    && ns.IsReady)
                {
                    Log.WriteLine("Casting {0} for instant Healing Touch", ns.Name);
                    ns.Cast(HelpfulTarget);
                    Sleep(200);
                }
            }
        }

        protected override void OnAfterAction(ActionBase action)
        {
            var innervate = WoWSpell.GetSpell("Innervate");
            if (Manager.LocalPlayer.PowerPercentage < 30
                && innervate.IsValid
                && innervate.IsReady)
            {
                innervate.Cast();
                Sleep(500);
            }
        }

        protected class Lifebloom : HelpfulSpellAction
        {
            public Lifebloom(Brain brain, int priority)
                : base(brain, priority, "Lifebloom")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && (!Brain.HelpfulTarget.Auras[SpellName].IsValid || Brain.HelpfulTarget.Auras[SpellName].StackCount < 3) && Brain.HelpfulTarget.HealthPercentage < 90; }
            }
        }

        protected class Rejuvenation : HelpfulSpellAction
        {
            public Rejuvenation(Brain brain, int priority)
                : base(brain, priority, "Rejuvenation")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && !Brain.HelpfulTarget.Auras[SpellName].IsValid && Brain.HelpfulTarget.HealthPercentage < 95; }
            }
        }

        protected class Swiftmend : HelpfulSpellAction
        {
            public Swiftmend(Brain brain, int priority)
                : base(brain, priority, "Swiftmend")
            { }

            public override bool IsReady
            {
                get { return base.IsReady && Brain.HelpfulTarget.Auras["Rejuvenation"].IsValid; }
            }

            public override bool IsWanted
            {
                get { return base.IsWanted && Brain.HelpfulTarget.HealthPercentage < 80; }
            }
        }

        protected class Regrowth : HelpfulSpellAction
        {
            public Regrowth(Brain brain, int priority)
                : base(brain, priority, "Regrowth")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && !Brain.HelpfulTarget.Auras[SpellName].IsValid && Brain.HelpfulTarget.HealthPercentage < 80; }
            }
        }

        protected class Nourish : HelpfulSpellAction
        {
            public Nourish(Brain brain, int priority)
                : base(brain, priority, "Nourish")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Brain.HelpfulTarget.HealthPercentage < 90; }
            }
        }

        protected class HealingTouch : HelpfulSpellAction
        {
            public HealingTouch(Brain brain, int priority)
                : base(brain, priority, "Healing Touch")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Brain.HelpfulTarget.HealthPercentage < 80; }
            }
        }

        protected class WildGrowth : SpellAction
        {
            public WildGrowth(Brain brain, int priority)
                : base(brain, priority, "Wild Growth")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && WoWParty.Members.Average(m => m.HealthPercentage) < 90; }
            }
        }
    }
}
