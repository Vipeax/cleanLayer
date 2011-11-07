using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cleanCore
{
    public class WoWTotem : WoWUnit
    {
        public WoWTotem(IntPtr pointer)
            : base(pointer)
        { }

        public bool IsMine
        {
            get { return CreatedBy == Manager.LocalPlayer.CreatedBy; }
        }

        public static implicit operator IntPtr(WoWTotem self)
        {
            return self != null ? self.Pointer : IntPtr.Zero;
        }

        public static new WoWTotem Invalid = new WoWTotem(IntPtr.Zero);
    }
}
