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
            foreach (var oldAura in Auras)
                oldAura.Value.Invalidate();

            if (Unit.IsValid)
            {
                var unitPointer = (uint)Unit.Pointer;

                var auraCount = Helper.Magic.Read<int>(unitPointer + Offsets.Aura.AuraCount);
                var auraTable = unitPointer + Offsets.Aura.AuraTable;
                if (auraCount == -1)
                {
                    auraCount = Helper.Magic.Read<int>(unitPointer + Offsets.Aura.AuraCountEx);
                    auraTable = Helper.Magic.Read<uint>(unitPointer + Offsets.Aura.AuraTableEx);
                }

                for (int i = 0; i < auraCount; i++) // Update internal
                {
                    var auraPointer = (IntPtr)(auraTable + i * Offsets.Aura.AuraSize);
                    if (Auras.ContainsKey(auraPointer))
                        Auras[auraPointer].Validate(auraPointer);
                    else
                        Auras.Add(auraPointer, new WoWAura(auraPointer));
                }
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
