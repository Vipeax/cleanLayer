using System.Linq;
using cleanCore;
using cleanLayer.Library.Combat;

namespace cleanLayer.Brains
{
    public class LowbieShaman : Brain
    {
        public LowbieShaman()
        {
            AddAction(new HarmfulSpellAction(this, 1, "Earth Shock", 24));
            AddAction(new HarmfulSpellAction(this, 1, "Lightning Bolt", 30));
        }

        public override WoWClass Class
        {
            get { return WoWClass.Shaman; }
        }

        public override string Specialization
        {
            get { return "Low level"; }
        }

        protected override HarmfulSpellAction PullSpell
        {
            get { return new HarmfulSpellAction(this, spellName: "Lightning Bolt"); }
        }

        protected override void OnBeforeAction(ActionBase action)
        {
        }

        protected override void OnAfterAction(ActionBase action)
        {
        }
    }
}
