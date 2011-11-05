using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using cleanLayer.Library.Combat;
using cleanCore;

namespace cleanLayer.Library
{
    public static class WoWBrains
    {
        public static readonly List<Brain> BrainPool = new List<Brain>();

        public static void Initialize()
        {
            BrainPool.Clear();
            Assembly asm = Assembly.GetExecutingAssembly();
            foreach (Type t in asm.GetTypes())
            {
                if (t.IsSubclassOf(typeof(Brain)))
                {
                    var b = (Brain)Activator.CreateInstance(t);
                    BrainPool.Add(b);
                }
            }
            Log.WriteLine("Loaded {0} brains.", BrainPool.Count);
        }

        public static List<Brain> BrainsForClass(WoWClass wowclass)
        {
            if (!Manager.IsInGame)
                return BrainPool;
            return BrainPool.Where(b => b.Class == Manager.LocalPlayer.Class).ToList();
        }
    }
}
