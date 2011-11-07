using System.Linq;
using cleanCore;
using cleanLayer.Library.Combat;

namespace cleanLayer.Brains.Shaman
{
    public class RestorationShamanBrain : Brain
    {
        public RestorationShamanBrain()
        {
            AddAction(new HealingSurge(this, 2));
            AddAction(new HealingWave(this, 1));
        }

        public override WoWClass Class
        {
            get { return WoWClass.Shaman; }
        }

        public override string Specialization
        {
            get { return "Restoration"; }
        }

        protected override HarmfulSpellAction PullSpell
        {
            get { return new HarmfulSpellAction(this, spellName: "Earth Shock"); }
        }

        protected override void OnBeforeAction(ActionBase action)
        {
            if (WoWParty.NumPartyMembers > 0 && HelpfulTarget.IsValid && HelpfulTarget.HealthPercentage > 60)
            {
                var tank = WoWParty.Members.OrderByDescending(m => m.MaxHealth).First() ?? WoWPlayer.Invalid;
                if (tank.IsValid && !tank.IsDead && tank.Distance < 40)
                {
                    var es = WoWSpell.GetSpell("Earth Shield");
                    if (es.IsValid
                        && es.IsReady
                        && !tank.Auras[es.Name].IsValid)
                    {
                        Log.WriteLine("Casting {0} on {1}", es.Name, tank.Name);
                        es.Cast(tank);
                        Sleep(Globals.SpellWait);
                    }
                }
            }
        }

        protected override void OnAfterAction(ActionBase action)
        {
            var ws = WoWSpell.GetSpell("Water Shield");
            if (ws.IsValid
                && ws.IsReady
                && !Manager.LocalPlayer.Auras[ws.Name].IsValid)
            {
                Log.WriteLine("Refreshing {0}", ws.Name);
                ws.Cast();
                Sleep(Globals.SpellWait);
            }
        }

        protected class HealingWave : HelpfulSpellAction
        {
            public HealingWave(Brain brain, int priority)
                : base(brain, priority, "Healing Wave")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Brain.HelpfulTarget.HealthPercentage < 80; }
            }
        }

        protected class HealingSurge : HelpfulSpellAction
        {
            public HealingSurge(Brain brain, int priority)
                : base(brain, priority, "Healing Surge")
            { }

            public override bool IsWanted
            {
                get { return base.IsWanted && Brain.HelpfulTarget.HealthPercentage < 60; }
            }
        }
    }
}
