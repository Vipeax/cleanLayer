using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;

namespace cleanCore
{
    public class WoWUnit : WoWObject
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate bool HasAuraDelegate(IntPtr thisObj, int spellId);
        private static HasAuraDelegate _hasAura;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int UnitReactionDelegate(IntPtr thisObj, IntPtr unitToCompare);
        private static UnitReactionDelegate _unitReaction;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int UnitThreatInfoDelegate(IntPtr pThis, IntPtr guid, ref IntPtr threatStatus, ref IntPtr threatPct, ref IntPtr rawPct, ref int threatValue);
        private static UnitThreatInfoDelegate _unitThreatInfo;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int CreatureTypeDelegate(IntPtr thisObj);
        private static CreatureTypeDelegate _creatureType;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int GetShapeshiftFormIdDelegate(IntPtr thisObj);
        private static GetShapeshiftFormIdDelegate _getShapeshiftFormId;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int GetAuraCountDelegate(IntPtr thisObj);
        private static GetAuraCountDelegate _getAuraCount;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr GetAuraDelegate(IntPtr thisObj, int index);
        private static GetAuraDelegate _getAura;

        public WoWUnit(IntPtr pointer)
            : base(pointer)
        {
            _auras = new WoWAuraCollection(this);
        }

        public UnitReaction Reaction
        {
            get
            {
                if (_unitReaction == null)
                    _unitReaction = Helper.Magic.RegisterDelegate<UnitReactionDelegate>(Offsets.UnitReaction);
                return (UnitReaction)_unitReaction(Pointer, Manager.LocalPlayer.Pointer);
            }
        }

        public bool IsFriendly
        {
            get { return (int)Reaction > (int)UnitReaction.Neutral; }
        }

        public bool IsNeutral
        {
            get { return Reaction == UnitReaction.Neutral; }
        }

        public bool IsHostile
        {
            get { return (int)Reaction < (int)UnitReaction.Neutral; }
        }

        public ulong TargetGuid
        {
            get
            {
                return GetDescriptor<ulong>((int)UnitField.UNIT_FIELD_TARGET);
            }
        }

        public WoWObject Target
        {
            get
            {
                return Manager.GetObjectByGuid(TargetGuid);
            }
        }

        public bool IsDead
        {
            get { return Health <= 0 || (DynamicFlags & UnitDynamicFlags.Dead) != 0; }
        }

        public WoWRace Race
        {
            get { return (WoWRace)GetDescriptor<byte>((int)UnitField.UNIT_FIELD_BYTES_0); }
        }

        public WoWClass Class
        {
            get { return (WoWClass)GetDescriptor<byte>((int)UnitField.UNIT_FIELD_BYTES_0 + 1); }
        }

        public bool IsLootable
        {
            get { return (DynamicFlags & UnitDynamicFlags.Lootable) != 0; }
        }

        public bool IsTapped
        {
            get { return (DynamicFlags & UnitDynamicFlags.TaggedByOther) != 0; }
        }

        public bool IsTappedByMe
        {
            get { return (DynamicFlags & UnitDynamicFlags.TaggedByMe) != 0; }
        }

        public bool IsInCombat
        {
            get { return (Flags & UnitFlags.Combat) != 0; }
        }

        public uint Health
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_HEALTH);
            }
        }

        public uint MaxHealth
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MAXHEALTH);
            }
        }

        public double HealthPercentage
        {
            get
            {
                return (Health / (double)MaxHealth) * 100;
            }
        }

        public double PowerPercentage
        {
            get
            {
                return (Power / (double)MaxPower) * 100;
            }
        }

        public uint Level
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_LEVEL);
            }
        }

        public UnitFlags Flags
        {
            get
            {
                return (UnitFlags)GetDescriptor<uint>((int)UnitField.UNIT_FIELD_FLAGS);
            }
        }

        public UnitFlags2 Flags2
        {
            get
            {
                return (UnitFlags2)GetDescriptor<uint>((int)UnitField.UNIT_FIELD_FLAGS_2);
            }
        }

        public UnitDynamicFlags DynamicFlags
        {
            get { return (UnitDynamicFlags)GetDescriptor<uint>((int)UnitField.UNIT_DYNAMIC_FLAGS); }
        }

        public uint NpcFlags
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_NPC_FLAGS);
            }
        }

        public uint Faction
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_FACTIONTEMPLATE);
            }
        }

        public uint BaseAttackTime
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_BASEATTACKTIME);
            }
        }

        public uint RangedAttackTime
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_RANGEDATTACKTIME);
            }
        }

        public float BoundingRadius
        {
            get
            {
                return GetDescriptor<float>((int)UnitField.UNIT_FIELD_BOUNDINGRADIUS);
            }
        }

        public float CombatReach
        {
            get
            {
                return GetDescriptor<float>((int)UnitField.UNIT_FIELD_COMBATREACH);
            }
        }

        public uint DisplayId
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_DISPLAYID);
            }
        }

        public uint MountDisplayId
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MOUNTDISPLAYID);
            }
        }

        public uint NativeDisplayId
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_NATIVEDISPLAYID);
            }
        }

        public uint MinDamage
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MINDAMAGE);
            }
        }

        public uint MaxDamage
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MAXDAMAGE);
            }
        }

        public uint MinOffhandDamage
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MINOFFHANDDAMAGE);
            }
        }

        public uint MaxOffhandDamage
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MAXOFFHANDDAMAGE);
            }
        }

        public uint PetExperience
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_PETEXPERIENCE);
            }
        }

        public uint PetNextLevelExperience
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_PETNEXTLEVELEXP);
            }
        }

        public uint BaseMana
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_BASE_MANA);
            }
        }

        public uint BaseHealth
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_BASE_HEALTH);
            }
        }

        public uint AttackPower
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_ATTACK_POWER);
            }
        }

        public uint RangedAttackPower
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_RANGED_ATTACK_POWER);
            }
        }

        public uint MinRangedDamage
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MINRANGEDDAMAGE);
            }
        }

        public uint MaxRangedDamage
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MAXRANGEDDAMAGE);
            }
        }

        public uint MaxItemLevel
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MAXITEMLEVEL);
            }
        }

        // 4.1: Contains mana, energy, rage and runic power. Thanks to JuJu.
        public uint Power
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_POWER1);
            }
        }

        public uint MaxPower
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MAXPOWER1);
            }
        }

        // Seems like both soul shard and holy power are in the same descriptor now - adding a property for both to avoid confusion.
        public uint SoulShard
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_POWER2);
            }
        }

        public uint MaxSoulShard
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MAXPOWER2);
            }
        }

        public uint HolyPower
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_POWER2);
            }
        }

        public uint MaxHolyPower
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MAXPOWER2);
            }
        }

        public uint Eclipse
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_POWER4);
            }
        }

        public uint MaxEclipse
        {
            get
            {
                return GetDescriptor<uint>((int)UnitField.UNIT_FIELD_MAXPOWER4);
            }
        }

        public int ChanneledCastingId
        {
            get { return Helper.Magic.Read<int>((uint)Pointer + Offsets.ChanneledCastingId); }
        }

        public int CastingId
        {
            get { return Helper.Magic.Read<int>((uint)Pointer + Offsets.CastingId); }
        }

        public bool IsCasting
        {
            get { return (ChanneledCastingId != 0 || CastingId != 0); }
        }

        public bool HasAura(int spellId)
        {
            if (_hasAura == null)
                _hasAura = Helper.Magic.RegisterDelegate<HasAuraDelegate>(Offsets.HasAuraBySpellId);
            return _hasAura(Pointer, spellId);
        }

        public IntPtr GetAuraPointer(int index)
        {
            if (_getAura == null)
                _getAura = Helper.Magic.RegisterDelegate<GetAuraDelegate>(Helper.Rebase(Offsets.GetAura));
            return _getAura(Pointer, index);
        }

        public int GetAuraCount
        {
            get
            {
                if (_getAuraCount == null)
                    _getAuraCount = Helper.Magic.RegisterDelegate<GetAuraCountDelegate>(Helper.Rebase(Offsets.GetAuraCount));
                return _getAuraCount(Pointer);
            }
        }

        private WoWAuraCollection _auras;
        public WoWAuraCollection Auras
        {
            get
            {
                _auras.Update();
                return _auras;
            }
        }

        public CreatureType CreatureType
        {
            get
            {
                if (_creatureType == null)
                    _creatureType = Helper.Magic.RegisterDelegate<CreatureTypeDelegate>(Helper.Rebase(Offsets.CreatureType));
                return (CreatureType)_creatureType(Pointer);
            }
        }

        public ShapeshiftForm Shapeshift
        {
            get
            {
                if (_getShapeshiftFormId == null)
                    _getShapeshiftFormId = Helper.Magic.RegisterDelegate<GetShapeshiftFormIdDelegate>(Helper.Rebase(Offsets.GetShapeshiftFormId));
                return (ShapeshiftForm)_getShapeshiftFormId(Pointer);
            }
        }

        public bool IsTotem
        {
            get { return CreatureType == CreatureType.Totem; }
        }

        public ulong SummonedBy
        {
            get { return GetDescriptor<ulong>((int)UnitField.UNIT_FIELD_SUMMONEDBY); }
        }

        public ulong CreatedBy
        {
            get { return GetDescriptor<ulong>((int)UnitField.UNIT_FIELD_CREATEDBY); }
        }

        public int CalculateThreat
        {
            get
            {
                if (_unitThreatInfo == null)
                {
                    _unitThreatInfo = Helper.Magic.RegisterDelegate<UnitThreatInfoDelegate>(Offsets.CalculateThreat);
                }

                IntPtr threatStatus = new IntPtr();
                IntPtr threatPct = new IntPtr();
                IntPtr threatRawPct = new IntPtr();
                int threatValue = 0;
                var storageField = Helper.Magic.Read<IntPtr>(Manager.LocalPlayer.Pointer + 0x08);
                _unitThreatInfo(Pointer, storageField, ref threatStatus, ref threatPct, ref threatRawPct, ref threatValue);

                return (int)threatStatus;
            }
        }

        public override string ToString()
        {
            return "[\"" + Name + "\", Distance = " + (int)Distance + ", Type = " + Type + ", React = " + Reaction + "]";
        }


        public static implicit operator IntPtr(WoWUnit self)
        {
            return self != null ? self.Pointer : IntPtr.Zero;
        }

        public static new WoWUnit Invalid = new WoWUnit(IntPtr.Zero);
    }
}