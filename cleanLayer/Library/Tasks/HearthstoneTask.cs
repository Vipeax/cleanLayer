using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cleanLayer.Library.Tasks
{
    public class HearthstoneTask : UseItemTask
    {
        public HearthstoneTask(int priority = int.MinValue, bool repeat = true)
            : base("Hearthstone", priority, repeat)
        { }
    }
}
