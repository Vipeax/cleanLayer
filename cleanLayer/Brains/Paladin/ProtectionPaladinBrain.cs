using System.Linq;
using cleanCore;
using cleanLayer.Library.Combat;

namespace cleanLayer.Brains
{
    public class ProtectionPaladinBrain : Brain
    {
        public ProtectionPaladinBrain()
        {
            AddAction(new TankSpellAction(this, 8, "Hand of Reckoning"));

            AddAction(new HammerOfWrath(this, 7));

            AddAction(new HarmfulSpellAction(this, 6, "Avenger's Shield"));
            AddAction(new HarmfulSpellAction(this, 5, "Judgement"));
            AddAction(new HarmfulSpellAction(this, 4, "Hammer of the Righteous"));
            AddAction(new ShieldOfTheRighteous(this, 3));
            AddAction(new HolyWrath(this, 2));
            AddAction(new Consecration(this, 1));
        }

        public override WoWClass Class
        {
            get { return WoWClass.Paladin; }
        }

        public override string Specialization
        {
            get { return "Protection"; }
        }

        protected override HarmfulSpellAction PullSpell
        {
            get { return new HarmfulSpellAction(this, spellName: "Judgement"); }
        }

        protected override void OnBeforeAction(ActionBase action)
        {
        }

        protected override void OnAfterAction(ActionBase action)
        {
        }

        protected class ShieldOfTheRighteous : HarmfulSpellAction
        {
            public ShieldOfTheRighteous(Brain brain, int priority)
                : base(brain, priority, "Shield of the Righteous")
            { }

            public override bool IsReady
            {
                get { return base.IsReady && Manager.LocalPlayer.HolyPower > 1; }
            }
        }

        protected class HammerOfWrath : HarmfulSpellAction
        {
            public HammerOfWrath(Brain brain, int priority)
                : base(brain, priority, "Hammer of Wrath")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Brain.HarmfulTarget.HealthPercentage < 20; }
            }
        }

        protected class Consecration : SpellAction
        {
            public Consecration(Brain brain, int priority)
                : base(brain, priority, "Consecration")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Brain.HarmfulTargets.Count(h => h.Distance < 8) > 2; }
            }
        }

        protected class HolyWrath : SpellAction
        {
            public HolyWrath(Brain brain, int priority)
                : base(brain, priority, "Holy Wrath")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Brain.HarmfulTargets.Count(h => h.Distance < 10) > 2; }
            }
        }

        protected class TankSpellAction : HarmfulSpellAction
        {
            public TankSpellAction(Brain brain, int priority, string spellname, int range = 5)
                : base(brain, priority, spellname, range)
            { }

            public override void Execute()
            {
                var unit = GetLowestThreatUnit();
                if (unit.IsValid)
                {
                    Log.WriteLine("Casting {0} on {1} to get aggro", unit.Name, SpellName);
                    WoWSpell.GetSpell(SpellName).Cast(unit);
                    Sleep(Globals.SpellWait);
                }
                else
                {
                    base.Execute();
                }
            }

            public override bool IsWanted
            {
                get
                {
                    var unit = GetLowestThreatUnit();
                    return Manager.IsInGame && !Manager.LocalPlayer.IsCasting
                        && unit.IsValid && unit.Distance < Range && unit.InLoS && unit.CalculateThreat < 3;
                }
            }

            private WoWUnit GetLowestThreatUnit()
            {
                return Brain.HarmfulTargets.OrderBy(o => o.CalculateThreat).First() ?? WoWUnit.Invalid;
            }
        }        
    }
}
