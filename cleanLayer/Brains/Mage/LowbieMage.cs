using System.Linq;
using cleanCore;
using cleanLayer.Library.Combat;

namespace cleanLayer.Brains
{
    [PluginInfo("Basic Low-level Mage", "1.0")]
    [PluginAuthor("miceiken")]
    [BrainInfo(WoWClass.Mage, "Lowbie")]
    public class LowbieMage : Brain
    {
        public LowbieMage()
        {
            AddAction(new HarmfulSpellAction(this, 1, "Fireball", 30));
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
