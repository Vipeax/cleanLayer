using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanLayer.Library.Scripts;

namespace cleanLayer.Scripts
{
    public class AutoEquipScript : Script
    {
        public AutoEquipScript()
            : base("AutoEquip", "Plugin")
        { }

        public List<string> ItemEvents = new List<string>()
        {
            "ACTIVE_TALENT_GROUP_CHANGED",
            "CHARACTER_POINTS_CHANGED",
            "PLAYER_ENTERING_WORLD",
            "START_LOOT_ROLL",
            "CONFIRM_DISENCHANT_ROLL",
            "CONFIRM_LOOT_ROLL",
            "ITEM_PUSH",
            "USE_BIND_CONFIRM",
            "LOOT_BIND_CONFIRM",
            "EQUIP_BIND_CONFIRM",
            "AUTOEQUIP_BIND_CONFIRM",
        };

        public override void OnStart()
        {
            foreach (var ev in ItemEvents)
                Events.Register(ev, HandleEvent);
        }

        public override void OnTerminate()
        {
            foreach (var ev in ItemEvents)
                Events.Remove(ev, HandleEvent);
        }

        public override void OnTick()
        {
        }

        private void HandleEvent(string ev, List<string> args)
        {
        }
    }
}
