using System.Linq;
using cleanCore;
using cleanLayer.Library.Combat;

namespace cleanLayer.Brains
{
    [PluginInfo("Basic Holy Paladin", "1.0")]
    [PluginAuthor("miceiken")]
    [BrainInfo(WoWClass.Paladin, "Holy")]
    public class HolyPaladinBrain : Brain
    {
        public HolyPaladinBrain()
        {
            AddAction(new LayOnHands(this, 9));
            AddAction(new DivineShield(this, 8));
            AddAction(new HolyRadiance(this, 7));
            AddAction(new WordOfGlory(this, 6));
            AddAction(new DivineLight(this, 5));
            AddAction(new HolyLight(this, 4));            
            AddAction(new HolyShock(this, 3));
            AddAction(new FlashOfLight(this, 2));
            AddAction(new Judgement(this, 1, 8));
        }

        protected override HarmfulSpellAction PullSpell
        {
            get { return new HarmfulSpellAction(this, spellName: "Judgement"); }
        }

        protected override void OnBeforeAction(ActionBase action)
        {
            if (WoWParty.NumPartyMembers > 0 && HelpfulTarget.IsValid && HelpfulTarget.HealthPercentage > 60)
            {
                var tank = HelpfulTargets.OrderByDescending(m => m.MaxHealth).First() ?? WoWPlayer.Invalid;
                if (tank.IsValid && !tank.IsDead && tank.Distance < Globals.MaxDistance)
                {
                    var beacon = WoWSpell.GetSpell("Beacon of Light");
                    if (beacon.IsValid
                        && beacon.IsReady
                        && !tank.Auras["Beacon of Light"].IsValid)
                    {
                        Log.WriteLine("Casting {0} on {1}", beacon.Name, tank.Name);
                        beacon.Cast(tank);
                        Sleep(Globals.SpellWait);
                    }
                }
            }
            if (action != null)
            {
                var aw = WoWSpell.GetSpell("Avenging Wrath");
                if (aw.IsValid && aw.IsReady)
                {
                    Log.WriteLine("Popping {0}", aw.Name);
                    aw.Cast();
                    Sleep(Globals.SpellWait);
                }
            }
        }

        protected override void OnAfterAction(ActionBase action)
        {
            var dp = WoWSpell.GetSpell("Divine Plea");
            var soi = WoWSpell.GetSpell("Seal of Insight");
            if (dp.IsValid
                && dp.IsReady
                && !Manager.LocalPlayer.IsInCombat
                && Manager.LocalPlayer.PowerPercentage < 80)
            {
                Log.WriteLine("Popping {0}", dp.Name);
                dp.Cast();
                Sleep(Globals.SpellWait);
            }
            else if (soi.IsValid
                && soi.IsReady
                && !Manager.LocalPlayer.Auras["Seal of Insight"].IsValid)
            {
                Log.WriteLine("Buffing {0}", soi.Name);
                soi.Cast();
                Sleep(Globals.SpellWait);
            }
        }

        protected class LayOnHands : HelpfulSpellAction
        {
            public LayOnHands(Brain brain, int priority)
                : base(brain, priority, "Lay on Hands")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Brain.HelpfulTarget.HealthPercentage < 20; }
            }

            public override bool IsReady
            {
                get { return base.IsReady && !Brain.HelpfulTarget.Auras["Forbearance"].IsValid; }
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

        protected class HolyRadiance : SpellAction
        {
            public HolyRadiance(Brain brain, int priority)
                : base(brain, priority, "Holy Radiance")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && WoWParty.Members.Average(m => m.HealthPercentage) < 80; }
            }
        }

        protected class DivineLight : BeaconSpellAction
        {
            public DivineLight(Brain brain, int priority)
                : base(brain, priority, "Divine Light")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Brain.HelpfulTarget.HealthPercentage < 70; }
            }
        }

        protected class HolyLight : BeaconSpellAction
        {
            public HolyLight(Brain brain, int priority)
                : base(brain, priority, "Holy Light")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Brain.HelpfulTarget.HealthPercentage < 80; }
            }
        }

        protected class WordOfGlory : BeaconSpellAction
        {
            public WordOfGlory(Brain brain, int priority)
                : base(brain, priority, "Word of Glory")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Brain.HelpfulTarget.HealthPercentage < 90; }
            }

            public override bool IsReady
            {
                get { return base.IsReady && Manager.LocalPlayer.HolyPower > 1; }
            }
        }

        protected class HolyShock : BeaconSpellAction
        {
            public HolyShock(Brain brain, int priority)
                : base(brain, priority, "Holy Shock")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Brain.HelpfulTarget.HealthPercentage < 90; }
            }
        }

        protected class FlashOfLight : BeaconSpellAction
        {
            public FlashOfLight(Brain brain, int priority)
                : base(brain, priority, "Flash of Light")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Brain.HelpfulTarget.HealthPercentage < 60; }
            }
        }

        protected class Judgement : HarmfulSpellAction
        {
            public Judgement(Brain brain, int priority, int maxPriority)
                : base(brain, priority, "Judgement")
            {
                MaxPriority = maxPriority;
            }

            public override int Priority
            {
                get { return (Manager.LocalPlayer.PowerPercentage < 30 ? MaxPriority : base.Priority); }
            }

            public int MaxPriority
            {
                get;
                private set;
            }
        }

        public class BeaconSpellAction : HelpfulSpellAction
        {
            public BeaconSpellAction(Brain brain, int priority = 0, string spellName = null, int range = 40)
                : base(brain, priority, spellName, range)
            { }

            public override void Execute()
            {
                var target = Brain.HelpfulTarget;
                var altTarget = Brain.AlternativeHelpfulTarget;
                if (altTarget.IsValid && target.Auras["Beacon of Light"].IsValid && altTarget.HealthPercentage < target.HealthPercentage)
                {
                    Log.WriteLine("Casting {0} on {1} (beacon {2})", SpellName, altTarget.Name, target.Name);
                    WoWSpell.GetSpell(SpellName).Cast(altTarget);
                    Sleep(Globals.SpellWait);
                }
                else
                {
                    Log.WriteLine("Casting {0} on {1}", SpellName, target.Name);
                    WoWSpell.GetSpell(SpellName).Cast(target);
                    Sleep(Globals.SpellWait);
                }
            }
        }
    }
}
