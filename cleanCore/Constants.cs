using System;
using System.Runtime.InteropServices;

namespace cleanCore
{
    [Flags]
    public enum UnitFlags : uint
    {
        None = 0,
        Sitting = 0x1,
        Influenced = 0x4,
        PlayerControlled = 0x8,
        Totem = 0x10,
        Preparation = 0x20,
        PlusMob = 0x40,
        NotAttackable = 0x100,
        Looting = 0x400,
        PetInCombat = 0x800,
        PvPFlagged = 0x1000,
        Silenced = 0x2000,
        Pacified = 0x20000,
        Stunned = 0x40000,
        CanPerformAction_Mask1 = 0x60000,
        Combat = 0x80000,
        TaxiFlight = 0x100000,
        Disarmed = 0x200000,
        Confused = 0x400000,
        Fleeing = 0x800000,
        Possessed = 0x1000000,
        NotSelectable = 0x2000000,
        Skinnable = 0x4000000,
        Mounted = 0x8000000,
        Dazed = 0x20000000,
        Sheathe = 0x40000000,
    }

    [Flags]
    public enum UnitFlags2
    {
        FeignDeath = 0x1,
        NoModel = 0x2,
        Flag_0x4 = 0x4,
        Flag_0x8 = 0x8,
        Flag_0x10 = 0x10,
        Flag_0x20 = 0x20,
        ForceAutoRunForward = 0x40,

        /// <summary>
        /// Treat as disarmed?
        /// Treat main and off hand weapons as not being equipped?
        /// </summary>
        Flag_0x80 = 0x80,

        /// <summary>
        /// Skip checks on ranged weapon?
        /// Treat it as not being equipped?
        /// </summary>
        Flag_0x400 = 0x400,

        Flag_0x800 = 0x800,
        Flag_0x1000 = 0x1000,
    }

    [Flags]
    public enum UnitDynamicFlags
    {
        None = 0,
        Lootable = 0x1,
        TrackUnit = 0x2,
        TaggedByOther = 0x4,
        TaggedByMe = 0x8,
        SpecialInfo = 0x10,
        Dead = 0x20,
        ReferAFriendLinked = 0x40,
        IsTappedByAllThreatList = 0x80,
    }

    public enum UnitNPCFlags
    {
        UNIT_NPC_FLAG_NONE = 0x00000000,
        UNIT_NPC_FLAG_GOSSIP = 0x00000001, // 100%
        UNIT_NPC_FLAG_QUESTGIVER = 0x00000002, // guessed, probably ok
        UNIT_NPC_FLAG_UNK1 = 0x00000004,
        UNIT_NPC_FLAG_UNK2 = 0x00000008,
        UNIT_NPC_FLAG_TRAINER = 0x00000010, // 100%
        UNIT_NPC_FLAG_TRAINER_CLASS = 0x00000020, // 100%
        UNIT_NPC_FLAG_TRAINER_PROFESSION = 0x00000040, // 100%
        UNIT_NPC_FLAG_VENDOR = 0x00000080, // 100%
        UNIT_NPC_FLAG_VENDOR_AMMO = 0x00000100, // 100%, general goods vendor
        UNIT_NPC_FLAG_VENDOR_FOOD = 0x00000200, // 100%
        UNIT_NPC_FLAG_VENDOR_POISON = 0x00000400, // guessed
        UNIT_NPC_FLAG_VENDOR_REAGENT = 0x00000800, // 100%
        UNIT_NPC_FLAG_REPAIR = 0x00001000, // 100%
        UNIT_NPC_FLAG_FLIGHTMASTER = 0x00002000, // 100%
        UNIT_NPC_FLAG_SPIRITHEALER = 0x00004000, // guessed
        UNIT_NPC_FLAG_SPIRITGUIDE = 0x00008000, // guessed
        UNIT_NPC_FLAG_INNKEEPER = 0x00010000, // 100%
        UNIT_NPC_FLAG_BANKER = 0x00020000, // 100%
        UNIT_NPC_FLAG_PETITIONER = 0x00040000, // 100% 0xC0000 = guild petitions, 0x40000 = arena team petitions
        UNIT_NPC_FLAG_TABARDDESIGNER = 0x00080000, // 100%
        UNIT_NPC_FLAG_BATTLEMASTER = 0x00100000, // 100%
        UNIT_NPC_FLAG_AUCTIONEER = 0x00200000, // 100%
        UNIT_NPC_FLAG_STABLEMASTER = 0x00400000, // 100%
        UNIT_NPC_FLAG_GUILD_BANKER = 0x00800000, // cause client to send 997 opcode
        UNIT_NPC_FLAG_SPELLCLICK = 0x01000000, // cause client to send 1015 opcode (spell click)
        UNIT_NPC_FLAG_GUARD = 0x10000000, // custom flag for guards
    }

    public enum UnitReaction
    {
        ExceptionallyHostile,
        VeryHostile,
        Hostile,
        Neutral,
        Friendly,
        VeryFriendly,
        ExceptionallyFriendly,
        Exalted
    }

    public enum WoWClass
    {
        Warrior = 1,
        Paladin,
        Hunter,
        Rogue,
        Priest,
        DeathKnight,
        Shaman,
        Mage,
        Warlock,
        Druid = 11
    }

    public enum WoWRace
    {
        Human = 1,
        Orc,
        Dwarf,
        NightElf,
        Undead,
        Tauren,
        Gnome,
        Troll,
        Goblin,
        BloodElf,
        Draenei,
        FelOrc,
        Naga,
        Broken,
        Skeleton,
        Vrykul,
        Tuskarr,
        ForestTroll,
        Taunka,
        NorthrendSkeleton,
        IceTroll,
        Worgen,
        Gilnean
    }

    [Flags]
    public enum WoWObjectType
    {
        Object = 0x1,
        Item = 0x2,
        Container = 0x4,
        Unit = 0x8,
        Player = 0x10,
        GameObject = 0x20,
        DynamicObject = 0x40,
        Corpse = 0x80,
        AiGroup = 0x100,
        AreaTrigger = 0x200,
    }

    [Flags]
    public enum GameObjectFlags : ushort
    {
        None = 0x00000000,
        InUse = 0x00000001,
        Locked = 0x00000002,
        InteractionCondition = 0x00000004,
        Transport = 0x00000008,
        Unknown1 = 0x00000010,
        NeverDespawn = 0x00000020,
        Triggered = 0x00000040,
        Unknown2 = 0x00000080,
        Unknown3 = 0x00000100,
        Damaged = 0x00000200,
        Destroyed = 0x00000400
    }

    [Flags]
    public enum ItemFlags : uint
    {
        None = 0x00000000,
        Unknown1 = 0x00000001,
        Conjured = 0x00000002,
        Openable = 0x00000004,
        Heroic = 0x00000008,
        Deprecated = 0x00000010,
        Indestructible = 0x00000020,
        Consumable = 0x00000040,
        NoEquipCooldown = 0x00000080,
        Unknown2 = 0x00000100,
        Wrapper = 0x00000200,
        Unknown3 = 0x00000400,
        PartyLoot = 0x00000800,
        Refundable = 0x00001000,
        Charter = 0x00002000,
        Unknown4 = 0x00004000,
        Unknown5 = 0x00008000,
        Unknown6 = 0x00010000,
        Unknown7 = 0x00020000,
        Prospectable = 0x00040000,
        UniqueEquip = 0x00080000,
        Unknown8 = 0x00100000,
        UsableInArena = 0x00200000,
        Throwable = 0x00400000,
        UsableWhenShapeshifted = 0x00800000,
        Unknown9 = 0x01000000,
        SmartLoot = 0x02000000,
        UnusableInArena = 0x04000000,
        BindToAccount = 0x08000000,
        TriggeredCast = 0x10000000,
        Millable = 0x20000000,
        Unknown10 = 0x40000000,
        Unknown11 = 0x80000000,
    }

    [Flags]
    public enum FactionMask
    {
        Neutral = 0,                                    // non-aggressive creature
        Player = 1,                                    // any player
        Alliance = 2,                                    // player or creature from alliance team
        Horde = 4,                                    // player or creature from horde team
        Monster = 8                                     // aggressive creature from monster team
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SpellRec
    {
        public int ID;
        public uint Attributes;
        public uint AttributesEx;
        public uint AttributesExB;
        public uint AttributesExC;
        public uint AttributesExD;
        public uint AttributesExE;
        public uint AttributesExF;
        public uint AttributesExG;
        public uint AttributesExH;
        public uint unk0;
        public uint unk_410_1; // 11 4.2.0
        public int CastingTimeIndex;
        public int DurationIndex;
        public int PowerType;
        public int RangeIndex;
        public float Speed;
        public uint SpellVisualID1;
        public uint SpellVisualID2;
        public int SpellIconID;
        public int ActiveIconID;
        IntPtr _Name;
        public uint NameSubText;
        public uint Description;
        public uint AuraDescription;
        public int SchoolMask;
        public int RuneCostID;
        public int SpellMissileID;
        public int SpellDescriptionVariableID;
        public int SpellDifficultyID;
        public float unk1;
        public int SpellScalingID;
        public int SpellAuraOptionsID;
        public int SpellAuraRestrictionsID;
        public int SpellCastingRequirementsID;
        public int SpellCategoriesID;
        public int SpellClassOptionsID;
        public int SpellCooldownsID;
        public int unk2;
        public int SpellEquippedItemsID;
        public int SpellInterruptsId;
        public int SpellLevelsID;
        public int SpellPowerID;
        public int SpellReagentsID;
        public int SpellShapeshiftID;
        public int SpellTargetRestrictionsID;
        public int SpellTotemsID;
        public int ResearchProject;

        public string Name { get { return _Name != IntPtr.Zero ? Helper.Magic.Read<string>(_Name) : string.Empty; } }
    }

    public struct FactionTemplateRec // sizeof(0x38)
    {
        public uint m_ID;           // 0
        public uint m_faction;      // 1
        public uint m_flags;        // 2
        public uint m_factionGroup; // 3
        public uint m_friendGroup;  // 4
        public uint m_enemyGroup;   // 5
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] m_enemies;    // 6-9
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] m_friend;     // 10-13
    }

    public struct AreaTableRec
    {
        public uint m_ID;                           // 0
        public uint m_ContinentID;                  // 1
        public uint m_ParentAreaID;                 // 2
        public uint m_AreaBit;                      // 3
        public uint m_flags;                        // 4
        public uint m_SoundProviderPref;            // 5
        public uint m_SoundProviderPrefUnderwater;  // 6
        public uint m_AmbienceID;                   // 7
        public uint m_ZoneMusic;                    // 8
        public uint m_IntroSound;                   // 9
        public uint m_ExplorationLevel;             // 10
        public uint m_AreaName_lang;                // 11
        public uint m_factionGroupMask;             // 12
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] m_liquidTypeID;               // 13-16
        public float m_minElevation;                // 17
        public float m_ambient_multiplier;          // 18
        public uint m_lightid;                      // 19
        public uint m_field20;                      // 20
        public uint m_field21;                      // 21
        public uint m_field22;                      // 22
        public uint m_field23;                      // 23
        public uint m_field24;                      // 24

        public string AreaName { get { return Helper.Magic.Read<string>(m_AreaName_lang); } }
    }

    public enum LockKeyType
    {
        LOCK_KEY_NONE = 0,
        LOCK_KEY_ITEM = 1,
        LOCK_KEY_SKILL = 2
    };

    public enum LockType
    {
        LOCKTYPE_PICKLOCK = 1,
        LOCKTYPE_HERBALISM = 2,
        LOCKTYPE_MINING = 3,
        LOCKTYPE_DISARM_TRAP = 4,
        LOCKTYPE_OPEN = 5,
        LOCKTYPE_TREASURE = 6,
        LOCKTYPE_CALCIFIED_ELVEN_GEMS = 7,
        LOCKTYPE_CLOSE = 8,
        LOCKTYPE_ARM_TRAP = 9,
        LOCKTYPE_QUICK_OPEN = 10,
        LOCKTYPE_QUICK_CLOSE = 11,
        LOCKTYPE_OPEN_TINKERING = 12,
        LOCKTYPE_OPEN_KNEELING = 13,
        LOCKTYPE_OPEN_ATTACKING = 14,
        LOCKTYPE_GAHZRIDIAN = 15,
        LOCKTYPE_BLASTING = 16,
        LOCKTYPE_SLOW_OPEN = 17,
        LOCKTYPE_SLOW_CLOSE = 18,
        LOCKTYPE_FISHING = 19,
        LOCKTYPE_INSCRIPTION = 20,
        LOCKTYPE_OPEN_FROM_VEHICLE = 21
    };

    public struct LockEntry
    {
        public uint ID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public uint[] Type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public uint[] Index;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public uint[] Skill;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        //public uint Action[];
    }

    public enum ClickToMoveType
    {
        LeftClick = 0x1,
        Face = 0x2,
        Stop_ThrowsException = 0x3, // Throws a Lua error whenever used. Caused by event state mismatch!
        Move = 0x4,
        NpcInteract = 0x5,
        Loot = 0x6,
        ObjInteract = 0x7,
        Skin = 0x9,
        AttackPosition = 0xA,
        AttackGuid = 0xB,

        ConstantFace = 0xC,

        // This is actually unknown. Usually referenced with Face though.
        FaceOther = 0x8,

        None = 0xD
    }

    public enum ItemQuality : int
    {
        Poor = 0,   // gray
        Common,     // white
        Uncommon,   // green
        Rare,       // blue
        Epic,       // purple
        Legendary,  // orange
        Artifact,   // golden yellow
        Heirloom,   // light yellow
    }

    public enum PowerType : int
    {
        Mana = 0,
        Rage,
        Focus,
        Energy,
        Happiness,
        Runes,
        RunicPower,
        SoulShards,
        Eclipse,
        HolyPower,
    }

    public enum InventorySlots : int
    {
        Ammo = 0,
        Head,
        Neck,
        Shoulder,
        Body,
        Chest,
        Waist,
        Legs,
        Feet,
        Wrist,
        Hand,
        Finger1,
        Finger2,
        Trinket1,
        Trinket2,
        Back,
        MainHand,
        OffHand,
        Ranged,
        Tabard,

        FIRST_EQUIPPED = InventorySlots.Head,
        LAST_EQUIPPED = InventorySlots.Tabard,
    }

    public enum ShapeshiftForm
    {
        Normal = 0,
        Cat = 1,
        TreeOfLife = 2,
        Travel = 3,
        Aqua = 4,
        Bear = 5,
        Ambient = 6,
        Ghoul = 7,
        DireBear = 8,
        CreatureBear = 14,
        CreatureCat = 15,
        GhostWolf = 16,
        BattleStance = 17,
        DefensiveStance = 18,
        BerserkerStance = 19,
        EpicFlightForm = 27,
        Shadow = 28,
        Stealth = 30,
        Moonkin = 31,
        SpiritOfRedemption = 32
    }

    public enum CreatureType
    {
        Unknown = 0,
        Beast,
        Dragon,
        Demon,
        Elemental,
        Giant,
        Undead,
        Humanoid,
        Critter,
        Mechanical,
        NotSpecified,
        Totem,
        NonCombatPet,
        GasCloud,
    }

    public enum MultiCastSlot
    {
        ElementsFire = 133,
        ElementsEarth,
        ElementsWater,
        ElementsAir,

        AncestorsFire,
        AncestorsEarth,
        AncestorsWater,
        AncestorsAir,

        SpiritsFire,
        SpiritsEarth,
        SpiritsWater,
        SpiritsAir
    }

    public enum Totem : int
    {
        // Earth Totems
        EarthElemental = 2062,
        Earthbind = 2484,
        Stoneclaw = 5730,
        Stoneskin = 8071,
        StrengthOfEarth = 8075,
        Tremor = 8143,

        // Fire Totems
        FireElemental = 2894,
        Searing = 3599,
        Magma = 8190,
        Flametongue = 8227,

        // Water Totems
        HealingStream = 5394,
        ManaSpring = 5675,
        ManaTide = 16190,
        TranquilMind = 87718,

        // Air Totems
        WrathOfAir = 3738,
        Grounding = 8177,
        ElementalResistance = 8184,
        Windfury = 8512,
    }

    public enum WeaponEnchantments : int
    {
        // Shaman
        Flametongue = 1800,

    }
}