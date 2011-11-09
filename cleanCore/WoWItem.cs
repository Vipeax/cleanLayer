using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace cleanCore
{

    public class WoWItem : WoWObject
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void UseItemDelegate(IntPtr thisObj, ref ulong guid, int unkZero);
        private static UseItemDelegate _useItem;

        public WoWItem(IntPtr pointer)
            : base(pointer)
        { }

        public List<WoWEnchant> Enchants
        {
            get
            {
                var ret = new List<WoWEnchant>();
                for (var i = 0; i < 12; i++)
                {
                    var id = GetDescriptor<uint>((int)ItemField.ITEM_FIELD_ENCHANTMENT_1_1 + (i * 12));
                    var exp = GetDescriptor<int>(((int)ItemField.ITEM_FIELD_ENCHANTMENT_1_1 + (i * 12)) + 4);
                    var charge = GetDescriptor<int>(((int)ItemField.ITEM_FIELD_ENCHANTMENT_1_1 + (i * 12)) + 8);
                    ret.Add(new WoWEnchant(id, exp, charge));
                }
                return ret;
            }
        }

        public ulong OwnerGuid
        {
            get
            {
                return GetDescriptor<ulong>((int)ItemField.ITEM_FIELD_OWNER);
            }
        }

        public ulong CreatorGuid
        {
            get
            {
                return GetDescriptor<ulong>((int)ItemField.ITEM_FIELD_CREATOR);
            }
        }

        public uint StackCount
        {
            get
            {
                return GetDescriptor<uint>((int)ItemField.ITEM_FIELD_STACK_COUNT);
            }
        }

        public ItemFlags Flags
        {
            get
            {
                return (ItemFlags)GetDescriptor<uint>((int)ItemField.ITEM_FIELD_FLAGS);
            }
        }

        public uint RandomPropertiesId
        {
            get
            {
                return GetDescriptor<uint>((int)ItemField.ITEM_FIELD_RANDOM_PROPERTIES_ID);
            }
        }

        public uint Durability
        {
            get
            {
                return GetDescriptor<uint>((int)ItemField.ITEM_FIELD_DURABILITY);
            }
        }

        public uint MaxDurability
        {
            get
            {
                return GetDescriptor<uint>((int)ItemField.ITEM_FIELD_MAXDURABILITY);
            }
        }

        public uint EnchantId
        {
            get { return GetDescriptor<uint>((int)ItemField.ITEM_FIELD_ENCHANTMENT_1_1); }
        }

        public void Use()
        {
            Use(Manager.LocalPlayer);
        }

        public void Use(WoWObject target)
        {
            var guid = target.Guid;
            if (_useItem == null)
                _useItem = Helper.Magic.RegisterDelegate<UseItemDelegate>(Offsets.UseItem);
            _useItem(Manager.LocalPlayer.Pointer, ref guid, 0);
        }

        public class WoWEnchant
        {
            public WoWEnchant(uint id, int expiration, int chargesleft)
            {
                Id = id;
                Expiration = expiration;
                ChargesLeft = chargesleft;
            }
            public uint Id;
            public int Expiration;
            public int ChargesLeft;
        }

        public static implicit operator IntPtr(WoWItem self)
        {
            return self != null ? self.Pointer : IntPtr.Zero;
        }

        public static new WoWItem Invalid = new WoWItem(IntPtr.Zero);
    }
}