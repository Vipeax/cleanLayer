using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace cleanCore
{
    public class WoWAuraCollection : IEnumerable<WoWAura>
    {
        private WoWUnit Unit;
        private Dictionary<IntPtr, WoWAura> Auras;

        public WoWAuraCollection(WoWUnit unit)
        {
            this.Unit = unit;
            this.Auras = new Dictionary<IntPtr, WoWAura>();
        }        

        internal void Update()
        {
            if (!Unit.IsValid)
            {
                Auras.Clear();
                return;
            }

            foreach (var oldAura in Auras)
                oldAura.Value.Invalidate();

            for (int i = 0; i < Unit.GetAuraCount; i++)
            {
                var ptr = Unit.GetAuraPointer(i);
                if (Auras.ContainsKey(ptr))
                    Auras[ptr].Validate(ptr);
                else
                    Auras.Add(ptr, new WoWAura(ptr));
            }

            Auras.Where(a => !a.Value.IsValid).ToList().ForEach(a => Auras.Remove(a.Key));
        }

        public WoWAura this[int id]
        {
            get { return Auras.Values.FirstOrDefault(o => o.ID == id) ?? WoWAura.Invalid; }
        }

        public WoWAura this[string name]
        {
            get { return Auras.Values.FirstOrDefault(o => o.Name == name) ?? WoWAura.Invalid; }
        }

        IEnumerator<WoWAura> IEnumerable<WoWAura>.GetEnumerator()
        {
            return Auras.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Auras.Values.GetEnumerator();
        }
    }
}
