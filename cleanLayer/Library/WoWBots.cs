using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using cleanLayer.Library.Combat;
using cleanCore;

namespace cleanLayer.Library
{
    public static class WoWBots
    {
        public static readonly List<BotBase> BotPool = new List<BotBase>();

        public static void Initialize()
        {
            BotPool.Clear();
            Assembly asm = Assembly.GetExecutingAssembly();
            foreach (Type t in asm.GetTypes())
            {
                if (t.IsSubclassOf(typeof(BotBase)))
                {
                    var b = (BotBase)Activator.CreateInstance(t);
                    BotPool.Add(b);
                }
            }            
        }
    }
}
