using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace cleanCore
{
    public class WoWAura
    {
        public WoWAura(IntPtr pointer)
        {
            if (pointer == IntPtr.Zero)
                Invalidate();
            else
                Validate(pointer);
        }

        private IntPtr Pointer
        {
            get;
            set;
        }

        internal void Validate(IntPtr pointer)
        {
            Pointer = pointer;
            Entry = Helper.Magic.ReadStruct<AuraStruct>(Pointer);
            ID = Entry.AuraId;
            var SpellRow = WoWDB.GetTable(ClientDB.Spell).GetRow(Entry.AuraId);
            if (SpellRow != null)
                Name = SpellRow.GetStruct<SpellRec>().Name;
            else
                Name = "unknown";
        }

        internal void Invalidate()
        {
            Pointer = IntPtr.Zero;
            ID = 0;
            Name = "unknown";
        }

        private AuraStruct Entry
        {
            get;
            set;
        }

        public int ID
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public bool IsValid
        {
            get { return ID != 0 && Remaining >= 0; }
        }

        public WoWUnit Caster
        {
            get
            {
                if (!IsValid)
                    return WoWUnit.Invalid;
                return Manager.GetObjectByGuid(CasterGuid) as WoWUnit ?? WoWUnit.Invalid;
            }
        }

        public ulong CasterGuid
        {
            get
            {
                if (!IsValid)
                    return 0ul;
                return Entry.CreatorGuid;
            }
        }

        public bool IsMine
        {
            get
            {
                return CasterGuid == Manager.LocalPlayer.Guid;
            }
        }

        public byte Flags
        {
            get
            {
                if (!IsValid)
                    return (byte)0;
                return Entry.Flags;
            }
        }

        public int Level
        {
            get
            {
                if (!IsValid)
                    return 0;
                return Entry.Level;
            }
        }

        public ushort StackCount
        {
            get
            {
                if (!IsValid)
                    return (ushort)0;
                return Entry.StackCount;
            }
        }

        public int Duration
        {
            get
            {
                if (!IsValid)
                    return 0;
                return (int)(Entry.Duration / 1000);
            }
        }

        public int Remaining
        {
            get
            {
                uint endTime = Entry.EndTime;
                return (int)((endTime == 0 ? 0 : endTime - Helper.PerformanceCount) / 1000);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct AuraStruct
        {
            public ulong CreatorGuid;
            public int AuraId;
            public byte Flags;
            public byte Level;
            public ushort StackCount;
            public uint Duration;
            public uint EndTime;
        }

        public enum AuraFlags
        {
            Active = 0x80,
            Passive = 0x10, // Check if !Active
            Harmful = 0x20
        }

        public static WoWAura Invalid = new WoWAura(IntPtr.Zero);
    }
}
