using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanLayer.Brains;
using cleanCore;

namespace cleanLayer.Library.Combat
{
    public abstract class SpellAction : ActionBase
    {
        public SpellAction(Brain brain, int priority = 0, string spellName = null, int range = 5)
            : base(brain, priority)
        {
            SpellName = spellName;
            Range = range;
        }

        public override void Execute()
        {
            Log.WriteLine("Casting {0}", SpellName);
            WoWSpell.GetSpell(SpellName).Cast();
            Sleep(Globals.SpellWait);
        }

        #region Properties

        public override bool IsWanted
        {
            get { return base.IsWanted && Manager.IsInGame && !Manager.LocalPlayer.IsCasting; }
        }

        public override bool IsReady
        {
            get { return base.IsReady && WoWSpell.GetSpell(SpellName).IsValid && WoWSpell.GetSpell(SpellName).IsReady; }
        }

        public virtual string SpellName
        {
            get;
            private set;
        }

        public virtual int Range
        {
            get;
            private set;
        }

        #endregion
    }
}
