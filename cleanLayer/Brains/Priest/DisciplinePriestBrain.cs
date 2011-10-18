using System.Linq;
using cleanCore;
using cleanLayer.Library.Combat;

namespace cleanLayer.Brains
{
    public class DisciplinePriestBrain : Brain
    {
        public DisciplinePriestBrain()
        {
            AddAction(new PWShield(this, 6));
            AddAction(new Renew(this, 1, 5));            
            AddAction(new PrayerOfHealing(this, 4));
            AddAction(new FlashHeal(this, 3));
            AddAction(new Penance(this, 2));     
            AddAction(new GreaterHeal(this, 1));
        }

        public override WoWClass Class
        {
            get { return WoWClass.Priest; }
        }

        public override string Specialization
        {
            get { return "Discipline"; }
        }

        protected override HarmfulSpellAction PullSpell
        {
            get { return new HarmfulSpellAction(this, spellName: "Smite"); }
        }

        protected override void OnBeforeAction(ActionBase action)
        {
            if (!Manager.LocalPlayer.IsInCombat)
            {
                if (WoWSpell.GetSpell("Power Infusion").IsReady)
                {
                    Log.WriteLine("Casting Power Infusion on myself");
                    WoWSpell.GetSpell("Power Infusion").Cast();
                    Sleep(Globals.SpellWait);
                }
            }
            else if (WoWParty.NumPartyMembers > 0 && HelpfulTarget.IsValid && HelpfulTarget.HealthPercentage > 60)
            {
                var tank = WoWParty.Members.OrderByDescending(m => m.MaxHealth).First() ?? WoWPlayer.Invalid;
                if (tank.IsValid && WoWSpell.GetSpell("Pain Suppresion").IsReady)
                {
                    Log.WriteLine("Casting Pain Suppression on ", tank.Name);
                    WoWSpell.GetSpell("Pain Suppresion").Cast(tank);
                    Sleep(Globals.SpellWait);
                }
            }
        }

        protected override void OnAfterAction(ActionBase action)
        {
            if (Manager.LocalPlayer.Power < Manager.LocalPlayer.MaxPower * 0.4 && WoWSpell.GetSpell("Shadowfiend").IsReady && HarmfulTarget.IsValid)
            {
                Log.WriteLine("Casting Shadowfiend on {0} to regenerate some mana", HarmfulTarget.Name);
                WoWSpell.GetSpell("Shadowfiend").Cast();
                Sleep(Globals.SpellWait);
            }
        }

        protected class PWShield : HelpfulSpellAction
        {
            public PWShield(Brain brain, int priority)
                : base(brain, priority, "Power Word: Shield")
            { }

            public override bool IsReady
            {
                get { return base.IsReady && !Brain.HelpfulTarget.Auras["Weakened Soul"].IsValid && Brain.HelpfulTarget.Auras[SpellName].Remaining < 3; }
            }

            public override bool IsWanted
            {
                get { return base.IsWanted && Brain.HelpfulTarget.HealthPercentage < 95; }
            }
        }

        protected class GreaterHeal : HelpfulSpellAction
        {
            public GreaterHeal(Brain brain, int priority)
                : base(brain, priority, "Greater Heal")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Brain.HelpfulTarget.HealthPercentage < 40; }
            }
        }

        protected class PrayerOfHealing : HelpfulSpellAction
        {
            public PrayerOfHealing(Brain brain, int priority)
                : base(brain, priority, "Prayer of Healing")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && WoWParty.Members.Average(m => m.HealthPercentage) < 80; }
            }
        }

        protected class Penance : HelpfulSpellAction
        {
            public Penance(Brain brain, int priority)
                : base(brain, priority, "Penance")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Brain.HelpfulTarget.Health < Brain.HelpfulTarget.MaxHealth * 0.85; }
            }
        }

        protected class FlashHeal : HelpfulSpellAction
        {
            public FlashHeal(Brain brain, int priority)
                : base(brain, priority, "Flash Heal")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Brain.HelpfulTarget.HealthPercentage < 75; }
            }
        }

        protected class Renew : HelpfulSpellAction
        {
            public Renew(Brain brain, int minpriority, int maxpriority)
                : base(brain, minpriority, "Renew")
            { this.MaxPriority = maxpriority; }

            public override bool IsReady
            {
                get { return base.IsReady && Brain.HelpfulTarget.Auras[SpellName].Remaining < 3; }
            }

            public override bool IsWanted
            {
                get { return base.IsWanted && Brain.HelpfulTarget.HealthPercentage < 95; }
            }

            public override int Priority
            {
                get { return WoWParty.Members.Average(m => m.HealthPercentage) > 80 ? MaxPriority : base.Priority; }
            }

            public int MaxPriority
            {
                get;
                private set;
            }
        }
    }
}
