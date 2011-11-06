using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanLayer.Brains;
using cleanCore;

namespace cleanLayer.Library.Combat
{
    public class HelpfulSpellAction : SpellAction
    {
        public HelpfulSpellAction(Brain brain, int priority = 0, string spellName = null, int range = 30)
            : base(brain, priority, spellName, range)
        { }

        public override void Execute()
        {
            Log.WriteLine("Casting {0} on {1}", SpellName, Brain.HelpfulTarget.Name);
            WoWSpell.GetSpell(SpellName).Cast(Brain.HelpfulTarget);
            Sleep(Globals.SpellWait);
        }

        public override bool IsWanted
        {
            get
            {
                return base.IsWanted
                    && Brain.HelpfulTarget.IsValid
                    && Brain.HelpfulTarget.InLoS;
            }
        }
    }
}
