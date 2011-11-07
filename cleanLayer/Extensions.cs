using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                catch { Log.WriteLine("\t{0} = Unable to dump"); }
            }
        }
    }
}
