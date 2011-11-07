using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cleanCore
{
    public static class WoWMounts
    {
        private static List<WoWMount> CachedMounts = new List<WoWMount>();
        public static List<WoWMount> GetAllMounts()
        {
            if (CachedMounts.Count > 0)
                return CachedMounts;

            var ret = new List<WoWMount>();
            var numMounts = WoWScript.Execute<int>("GetNumCompanions(\"MOUNT\")", 0);
            for (int i = 1; i <= numMounts; i++)
            {
                var mountInfo = WoWScript.Execute("GetCompanionInfo(\"MOUNT\", " + i + ")");
                if (mountInfo.Count > 5) // GetCompanionInfo should return 6 items
                {
                    try
                    {
                        ret.Add(new WoWMount(i, mountInfo[1], int.Parse(mountInfo[2]), int.Parse(mountInfo[5])));
                    }
                    catch { }
                }
            }
            CachedMounts = ret;
            return ret;
        }

        public static string RandomMount()
        {
            var r = new Random();
            var mounts = GetAllMounts();
            if (mounts.Count > 0)
            {
                var mount = mounts[r.Next(0, mounts.Count)];
                mount.Mount();
                return mount.Name;
            }
            else
            {
                if (Manager.LocalPlayer.Class == WoWClass.Druid && WoWSpell.GetSpell("Travel Form").IsValid)
                {
                    WoWSpell.GetSpell("Travel Form").Cast();
                    return "Travel Form";
                }
                if (Manager.LocalPlayer.Class == WoWClass.Shaman && WoWSpell.GetSpell("Ghost Wolf").IsValid)
                {
                    WoWSpell.GetSpell("Ghost Wolf").Cast();
                    return "Ghost Wolf";
                }
            }
            return string.Empty;
        }

        public static void ForceUpdate()
        {
            CachedMounts.Clear();
        }
    }
}
