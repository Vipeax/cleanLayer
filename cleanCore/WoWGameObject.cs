using System;

namespace cleanCore
{

    public class WoWGameObject : WoWObject
    {
        public WoWGameObject(IntPtr pointer)
            : base(pointer)
        {

        }

        public uint DisplayId
        {
            get
            {
                return GetDescriptor<uint>((int)GameObjectField.GAMEOBJECT_DISPLAYID);
            }
        }

        public uint Flags
        {
            get
            {
                return GetDescriptor<uint>((int)GameObjectField.GAMEOBJECT_FLAGS);
            }
        }

        public uint Level
        {
            get
            {
                return GetDescriptor<uint>((int)GameObjectField.GAMEOBJECT_LEVEL);
            }
        }

        public uint Faction
        {
            get
            {
                return GetDescriptor<uint>((int)GameObjectField.GAMEOBJECT_FACTION);
            }
        }

        public bool Locked
        {
            get
            {
                return (Flags & (uint)GameObjectFlags.Locked) > 0;
            }
        }

        public bool InUse
        {
            get
            {
                return (Flags & (uint)GameObjectFlags.InUse) > 0;
            }
        }

        public bool IsTransport
        {
            get
            {
                return (Flags & (uint)GameObjectFlags.Transport) > 0;
            }
        }

        public ulong CreatedBy
        {
            get
            {
                return GetDescriptor<ulong>((int)GameObjectField.GAMEOBJECT_FIELD_CREATED_BY);
            }
        }

        public bool CreatedByMe
        {
            get
            {
                return CreatedBy == Manager.LocalPlayer.Guid;
                return false;
            }
        }

        public bool Gatherable
        {
            get
            {
                return false;
            }
        }

        public static implicit operator IntPtr(WoWGameObject self)
        {
            return self != null ? self.Pointer : IntPtr.Zero;
        }

        public static new WoWGameObject Invalid = new WoWGameObject(IntPtr.Zero);
    }
}