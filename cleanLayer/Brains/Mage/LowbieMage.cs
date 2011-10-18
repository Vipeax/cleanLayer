using System.Linq;
using cleanCore;
using cleanLayer.Library.Combat;

namespace cleanLayer.Brains
{
    public class LowbieMage : Brain
    {
        public LowbieMage()
        {
            AddAction(new HarmfulSpellAction(this, 1, "Fireball", 30));
        }

        public override WoWClass Class
        {
            get { return WoWClass.Mage; }
        }

        public override string Specialization
        {
            get { return "Low level"; }
        }

        protected override HarmfulSpellAction PullSpell
        {
            get { return new HarmfulSpellAction(this, spellName: "Fireball"); }
        }

        protected override void OnBeforeAction(ActionBase action)
        {
        }

        protected override void OnAfterAction(ActionBase action)
        {
        }
    }
}
