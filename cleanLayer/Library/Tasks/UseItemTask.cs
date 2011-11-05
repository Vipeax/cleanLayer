using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;

namespace cleanLayer.Library.Tasks
{
    public class UseItemTask : Task
    {
        public UseItemTask(string item, int priority = int.MinValue, bool repeat = true)
            : base(priority, repeat)
        {
            itemName = item;
        }

        public override void Execute()
        {
            if (itemObject.IsValid)
                itemObject.Use();
        }

        public override bool Ready
        {
            get
            {
                return itemObject.IsValid;
            }
        }

        private WoWItem itemObject
        {
            get
            {
                return Manager.Objects.Where(o => o.IsValid && o.IsItem && o.Name == itemName).Select(o => o as WoWItem).FirstOrDefault() ?? WoWItem.Invalid;
            }
        }

        private string itemName;
    }
}
