using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanLayer.Library.Combat;

namespace cleanLayer
{
    public static class Extensions
    {
        public static void DumpProperties(this object o)
        {
            var t = o.GetType();
            Log.WriteLine("Dumping Properties of {0} (Type = {1})", t.Name, t);
            foreach (var p in t.GetProperties())
            {
                try
                {
                    Log.WriteLine("\t{0} = {1}", p.Name, p.GetValue(o, null));
                }
                catch { Log.WriteLine("\t{0} = null?", p.Name); }
            }
        }

        public static PluginInfo GetInfo(this Type t)
        {
            PluginInfo info = null;
            var attr = t.GetCustomAttributes(typeof(PluginInfo), true);
            foreach (PluginInfo c in attr)
                info = c;
            return info;
        }

        public static string GetAuthor(this Type t)
        {
            var author = "Unknown";
            var attr = t.GetCustomAttributes(typeof(PluginAuthor), true);
            foreach (PluginAuthor c in attr)
                author = c.Author;
            return author;
        }

        public static BrainInfo GetBrainInfo(this Type b)
        {
            BrainInfo brain = null;
            var attr = b.GetCustomAttributes(typeof(BrainInfo), true);
            foreach (BrainInfo c in attr)
                brain = c;
            return brain;
        }
    }
}
