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
            Log.WriteLine("Dumping Properties of Object (Type = {0})", t);
            foreach (var p in t.GetProperties())
                Log.WriteLine("    {0} = {1}", p.Name, p.GetValue(o, null));
        }
    }
}
