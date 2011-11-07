using System.Linq;
using cleanCore;
using cleanLayer.Library.Combat;

namespace cleanLayer.Brains
{
    public class FeralDruidBrain : Brain
    {
        public FeralDruidBrain()
        {
            AddAction(new Mangle(this, 1));
            AddAction(new Rake(this, 3));
            AddAction(new FerociousBite(this, 4));
            AddAction(new FaerieFire(this, 2));
        }

        public override WoWClass Class
        {
            get { return WoWClass.Druid; }
        }

        public override string Specialization
        {
            get { return "Feral"; }
        }

        protected override HarmfulSpellAction PullSpell
        {
            get { return new HarmfulSpellAction(this, 1, "Mangle", 6); }
        }

        private WoWPlayer Leader = WoWPlayer.Invalid;
        protected override void OnBeforeAction(ActionBase action)
        {
            if (Leader == null || !Leader.IsValid)
                Leader = WoWParty.Members.FirstOrDefault() ?? WoWPlayer.Invalid;

            if (action is CatSpellAction && Manager.LocalPlayer.Shapeshift != ShapeshiftForm.Cat)
            {
                WoWSpell.GetSpell("Cat Form").Cast();
                Sleep(Globals.SpellWait);
            }

            if (action is BearSpellAction && Manager.LocalPlayer.Shapeshift != ShapeshiftForm.Bear)
            {
                WoWSpell.GetSpell("Bear Form").Cast();
                Sleep(Globals.SpellWait);
            }

            var cd = WoWSpell.GetSpell("Tiger's Fury");
            if (action is CatSpellAction && cd.IsValid && cd.IsReady)
            {
                Log.WriteLine("Popping {0}", cd.Name);
                cd.Cast();
                Sleep(Globals.SpellWait);
            }
        }

        protected override void OnAfterAction(ActionBase action)
        {
            var healer = WoWParty.Members.OrderBy(x => x.MaxPower).FirstOrDefault() ?? WoWPlayer.Invalid;
            if (healer.IsValid && healer.PowerPercentage < 40 && WoWSpell.GetSpell("Innervate").IsValid && WoWSpell.GetSpell("Innervate").IsReady)
            {
                WoWSpell.GetSpell("Innervate").Cast(healer);
                Log.WriteLine("Casting Innervate on {0}", healer.Name);
            }
        }
        protected class Mangle : CatSpellAction
        {
            public Mangle(Brain brain, int priority)
                : base(brain, priority, "Mangle")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Manager.LocalPlayer.PowerPercentage > 35; }
            }
        }

        protected class Rake : CatSpellAction
        {
            public Rake(Brain brain, int priority)
                : base(brain, priority, "Rake")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Manager.LocalPlayer.PowerPercentage > 35 && Brain.HarmfulTarget.Auras.Where(x => x.IsValid && x.Name == "Rake" && x.CasterGuid == Manager.LocalPlayer.Guid).Count() == 0; }
            }
        }

        protected class FaerieFire : CatSpellAction
        {
            public FaerieFire(Brain brain, int priority)
                : base(brain, priority, "Faerie Fire (Feral)")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && !Brain.HarmfulTarget.Auras["Faerie Fire"].IsValid && Brain.HarmfulTarget.Auras["Faerie Fire"].StackCount <= 3; }
            }
        }

        protected class FerociousBite : CatSpellAction
        {
            public FerociousBite(Brain brain, int priority)
                : base(brain, priority, "Ferocious Bite")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Manager.LocalPlayer.PowerPercentage > 25 && Manager.LocalPlayer.ComboPoints == 5; }
            }
        }

        #region Customized SpellActions
        protected class CatSpellAction : HarmfulSpellAction
        {
            public CatSpellAction(Brain brain, int priority = 0, string spellName = null, int range = 6)
                : base(brain, priority, spellName, range)
            { }

            public override bool IsReady
            {
                get { return base.IsWanted && WoWSpell.GetSpell("Cat Form").IsValid; }
            }
        }

        protected class BearSpellAction : HarmfulSpellAction
        {
            public BearSpellAction(Brain brain, int priority = 0, string spellName = null, int range = 6)
                : base(brain, priority, spellName, range)
            { }

            public override bool IsReady
            {
                get { return base.IsWanted && WoWSpell.GetSpell("Bear Form").IsValid; }
            }
        }
        #endregion
    }
}
