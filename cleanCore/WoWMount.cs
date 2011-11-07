using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cleanCore
{
    public class WoWMount
    {
        public WoWMount(int companionId, string name, int spellId, int flags)
        {
            CompanionId = companionId;
            Name = name;
            Id = spellId;
            Flags = flags;
        }

        private int CompanionId { get; set; }

        public string Name { get; private set; }
        public int Id { get; private set; }
        public int Flags { get; private set; }

        public bool IsGround { get { return (Flags & 0x01) != 0; } }
        public bool IsFlying { get { return (Flags & 0x02) != 0; } }

        public void Mount()
        {
            WoWScript.ExecuteNoResults("CallCompanion(\"MOUNT\", " + CompanionId + ")");
        }
    }
}
