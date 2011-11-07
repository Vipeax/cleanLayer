using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanLayer.Library.Combat;

namespace cleanLayer.Brains.Shaman
{
    public static class TotemHelper
    {
        public static void SetTotemSlot(MultiCastSlot slot, eWoWTotem spellID)
        {
            WoWScript.Execute("SetMultiCastSpell(" + (int)slot + ", " + (int)spellID + ")");
        }

        public static bool CallTotems()
        {
            var CotE = WoWSpell.GetSpell("Call of the Elements");
            if (CotE.IsValid && CotE.IsReady)
            {
                Log.WriteLine("Call of the Elements");
                CotE.Cast();
                return true;
            }
            return false;
        }

        public static bool CallTotem(eWoWTotem totem)
        {
            var spell = WoWSpell.GetSpell((int)totem);
            if (spell.IsValid && spell.IsReady)
            {
                Log.WriteLine("Calling {0} totem", totem.ToString());
                spell.Cast();
                return true;
            }
            return false;
        }

        public static bool NeedToRecall
        {
            get { return Manager.LocalPlayer.Totems.Count > 0; }
        }

        public static bool RecallTotems()
        {
            if (NeedToRecall)
            {
                var tr = WoWSpell.GetSpell("Totemic Recall");
                if (tr.IsValid && tr.IsReady)
                {
                    Log.WriteLine("Recalling totems");
                    tr.Cast();
                    return true;
                }
            }
            return false;
        }
    }
}
