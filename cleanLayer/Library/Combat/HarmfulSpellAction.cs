using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;

namespace cleanLayer.Library.Combat
{
    public class HarmfulSpellAction : SpellAction
    {
        public HarmfulSpellAction(Brain brain, int priority = 0, string spellName = null, int range = 5)
            : base(brain, priority, spellName, range)
        { }

        public override void Execute()
        {
            ExecuteEx(Brain.HarmfulTarget);
        }

        public void ExecuteEx(WoWUnit unit)
        {
            Log.WriteLine("Casting {0} on {1}", SpellName, unit.Name);
            WoWSpell.GetSpell(SpellName).Cast(unit);
            Sleep(Globals.SpellWait);
        }

        public override bool IsWanted
        {
            get
            {
                return base.IsWanted
                    && Brain.HarmfulTarget.IsValid
                    && Brain.HarmfulTarget.Distance < Range
                    && Brain.HarmfulTarget.InLoS;
            }
        }
    }
}
