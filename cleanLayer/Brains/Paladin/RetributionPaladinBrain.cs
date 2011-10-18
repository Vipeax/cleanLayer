using System.Linq;
using cleanCore;
using cleanLayer.Library.Combat;

namespace cleanLayer.Brains
{
    public class RetributionPaladinBrain : Brain
    {
        public RetributionPaladinBrain()
        {
            AddAction(new DivineShield(this, 10));
            AddAction(new HammerOfWrath(this, 9));            
            AddAction(new TemplarsVerdict(this, 8));
            AddAction(new HarmfulSpellAction(this, 7, "Crusader Strike"));
            AddAction(new HarmfulSpellAction(this, 6, "Judgement"));
            AddAction(new Exorcism(this, 1)); // 5
            AddAction(new HolyWrath(this, 4));
            AddAction(new Consecration(this, 3));
            AddAction(new Rebuke(this, 2));
        }

        public override WoWClass Class
        {
            get { return WoWClass.Paladin; }
        }

        public override string Specialization
        {
            get { return "Retribution"; }
        }

        protected override HarmfulSpellAction PullSpell
        {
            get { return new HarmfulSpellAction(this, spellName: "Judgement"); }
        }

        protected override void OnBeforeAction(ActionBase action)
        {
            var cd = WoWSpell.GetSpell("Avenging Wrath");
            if (cd.IsValid && cd.IsReady)
            {
                Log.WriteLine("Popping {0}", cd.Name);
                cd.Cast();
                Sleep(Globals.SpellWait);
            }
        }

        protected override void OnAfterAction(ActionBase action)
        {
            var seal = WoWSpell.GetSpell("Seal of Truth");
            var sealBuff = Manager.LocalPlayer.Auras[seal.Name];
            if (seal.IsValid && (!sealBuff.IsValid || sealBuff.Remaining < 60))
            {
                Log.WriteLine("Buffing {0}", seal.Name);
                seal.Cast();
                Sleep(Globals.SpellWait);
            }
        }

        protected class DivineShield : SpellAction
        {
            public DivineShield(Brain brain, int priority)
                : base(brain, priority, "Divine Shield")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Manager.LocalPlayer.HealthPercentage < 30; }
            }

            public override bool IsReady
            {
                get { return base.IsReady && !Manager.LocalPlayer.Auras["Forbearance"].IsValid; }
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

        protected class TemplarsVerdict : HarmfulSpellAction
        {
            public TemplarsVerdict(Brain brain, int priority)
                : base(brain, priority, "Templar's Verdict")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Manager.LocalPlayer.HolyPower > 1; }
            }
        }

        protected class Exorcism : HarmfulSpellAction
        {
            public Exorcism(Brain brain, int priority)
                : base(brain, priority, "Exorcism")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Manager.LocalPlayer.Auras.Count(a => a.Name == "The Art of War") > 1; }
            }
        }

        protected class Rebuke : HarmfulSpellAction
        {
            public Rebuke(Brain brain, int priority)
                : base(brain, priority, "Rebuke")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Brain.HarmfulTarget.IsCasting; }
            }
        }
    }
}
