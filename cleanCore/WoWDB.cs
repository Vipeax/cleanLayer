using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace cleanCore
{
    public static class WoWDB
    {
        private static readonly Dictionary<ClientDB, DBTable> Tables = new Dictionary<ClientDB, DBTable>();

        static WoWDB()
        {
            foreach (object TableType in Enum.GetValues(typeof(ClientDB)))
                Tables.Add((ClientDB)Enum.ToObject(typeof(ClientDB), TableType), new DBTable((uint)TableType));
        }

        public static DBTable GetTable(ClientDB table)
        {
            if (Tables.ContainsKey(table))
                return Tables[table];
            return null;
        }

        public class DBTable
        {
            private IntPtr Pointer;
            private GetRowDelegate _getRow;

            public DBTable(uint pointer)
            {
                Pointer = (IntPtr)Helper.Rebase(pointer);
                var header = (DBHeader)Marshal.PtrToStructure(Pointer, typeof(DBHeader));
                MaxIndex = header.maxIndex;
                MinIndex = header.minIndex;
            }

            public int MaxIndex
            {
                get;
                private set;
            }

            public int MinIndex
            {
                get;
                private set;
            }

            public DBRow GetRow(int index)
            {
                if (_getRow == null)
                    _getRow = Helper.Magic.RegisterDelegate<GetRowDelegate>(Offsets.ClientDB_GetRow);
                var row = _getRow(Pointer, index);
                return (row == IntPtr.Zero ? null : new DBRow(row));
            }

            #region GetRow

            [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
            private delegate IntPtr GetRowDelegate(IntPtr pointer, int index);

            #endregion

            #region DBHeader

            [StructLayout(LayoutKind.Sequential)]
            private struct DBHeader
            {
                public uint funcTable;
                public int numRows;
                public int maxIndex;
                public int minIndex;
                public int stringTable;
                public uint firstRow;
                public uint Rows;
            }

            #endregion

            #region DBRow

            public class DBRow
            {
                private IntPtr Pointer;

                public DBRow(IntPtr pointer)
                {
                    Pointer = pointer;
                }

                public T GetField<T>(uint index)
                {
                    try
                    {
                        if (typeof(T) == typeof(string))
                        {
                            object s = Marshal.PtrToStringAnsi(Helper.Magic.Read<IntPtr>((uint)Pointer + (index * 4)));
                            return (T)s;
                        }

                        return Helper.Magic.Read<T>((uint)Pointer + (index * 4));
                    }
                    catch
                    {
                        return default(T);
                    }
                }

                public T GetStruct<T>() where T : struct
                {
                    try { return (T)Marshal.PtrToStructure(Pointer, typeof(T)); }
                    catch { return default(T); }
                }
            }

            #endregion
        }
    }

    #region ClientDB

    public enum ClientDB : uint
    {
        AnimKitBoneSetAlias = 0x0095A82C,
        AnimKitBoneSet = 0x0095A810,
        AnimKit = 0x0095A7F4,
        AnimKitConfig = 0x0095A848,
        AnimKitConfigBoneSet = 0x0095A864,
        AnimKitPriority = 0x0095A880,
        AnimKitSegment = 0x0095A89C,
        AnimReplacement = 0x0095A8B8,
        AnimReplacementSet = 0x0095A8D4,
        BannedAddOns = 0x0095A9D0,
        Cfg_Categories = 0x0095AA78,
        Cfg_Configs = 0x0095AA94,
        CharBaseInfo = 0x0095AAB0,
        CharHairGeosets = 0x0095AACC,
        CharSections = 0x0095AAE8,
        CharStartOutfit = 0x0095AB04,
        CharacterFacialHairStyles = 0x0095AB3C,
        ChatProfanity = 0x0095AB74,
        ChrClasses = 0x0095AB90,
        ChrRaces = 0x0095ABC8,
        CreatureDisplayInfo = 0x0095AC38,
        CreatureDisplayInfoExtra = 0x0095AC1C,
        CreatureFamily = 0x0095AC54,
        CreatureModelData = 0x0095AC8C,
        FactionGroup = 0x0095AF10,
        FactionTemplate = 0x0095AF48,
        FileData = 0x0095AF64,
        GameTips = 0x0095AFF0,
        GlueScreenEmote = 0x0095B028,
        GuildColorBackground = 0x0095B290,
        GuildColorBorder = 0x0095B2AC,
        GuildColorEmblem = 0x0095B2C8,
        HelmetGeosetVisData = 0x0095B300,
        ItemClass = 0x0095B3E0,
        ItemDisplayInfo = 0x0095B4F8,
        ItemSubClass = 0x0095B634,
        ItemVisuals = 0x0095B66C,
        ItemVisualEffects = 0x0095B650,
        LoadingScreens = 0x0095B7A0,
        Movie = 0x0095B89C,
        MovieFileData = 0x0095B8B8,
        MovieVariation = 0x0095B8D4,
        NameGen = 0x0095B8F0,
        NamesProfanity = 0x0095B944,
        NamesReserved = 0x0095B960,
        ObjectEffect = 0x0095C680,
        ObjectEffectGroup = 0x0095C69C,
        ObjectEffectModifier = 0x0095C6B8,
        ObjectEffectPackage = 0x0095C6D4,
        ObjectEffectPackageElem = 0x0095C6F0,
        ParticleColor = 0x0095B9EC,
        Resistances = 0x0095BB58,
        SoundFilter = 0x0095C70C,
        SoundFilterElem = 0x0095C728,
        SpamMessages = 0x0095BDA4,
        SpellEffect = 0x0095BF48,
        SpellVisualEffectName = 0x0095C220,
        SpellVisualKit = 0x0095C23C,
        SoundProviderPreferences = 0x0095BD88,
        AnimationData = 0x0093978C,
        AreaTable = 0x009397A8,
        LightIntBand = 0x00939834,
        LightParams = 0x0093986C,
        Map = 0x009398F8,
        SoundEntriesAdvanced = 0x00939914,
        SoundEntries = 0x00939930,
        Achievement = 0x0095A7A0,
        Achievement_Criteria = 0x0095A7BC,
        Achievement_Category = 0x0095A7D8,
        AreaGroup = 0x0095A8F0,
        AreaPOI = 0x0095A90C,
        AreaPOISortedWorldState = 0x0095A928,
        AreaAssignment = 0x0095A944,
        AreaTrigger = 0x0095A960,
        ArmorLocation = 0x0095A97C,
        AuctionHouse = 0x0095A998,
        BankBagSlotPrices = 0x0095A9B4,
        BarberShopStyle = 0x0095A9EC,
        BattlemasterList = 0x0095AA08,
        CameraMode = 0x0095AA24,
        CameraShakes = 0x0095AA40,
        CastableRaidBuffs = 0x0095AA5C,
        CharTitles = 0x0095AB20,
        ChatChannels = 0x0095AB58,
        ChrClassesXPowerTypes = 0x0095ABAC,
        CinematicCamera = 0x0095ABE4,
        CinematicSequences = 0x0095AC00,
        CreatureImmunities = 0x0095AC70,
        CreatureMovementInfo = 0x0095ACA8,
        CreatureSoundData = 0x0095ACC4,
        CreatureSpellData = 0x0095ACE0,
        CreatureType = 0x0095ACFC,
        CurrencyTypes = 0x0095AD18,
        CurrencyCategory = 0x0095AD34,
        DanceMoves = 0x0095AD50,
        DeathThudLookups = 0x0095AD6C,
        DestructibleModelData = 0x0095ADC0,
        DungeonEncounter = 0x0095ADDC,
        DungeonMap = 0x0095ADF8,
        DungeonMapChunk = 0x0095AE14,
        DurabilityCosts = 0x0095AE30,
        DurabilityQuality = 0x0095AE4C,
        Emotes = 0x0095AE68,
        EmotesTextData = 0x0095AE84,
        EmotesTextSound = 0x0095AEA0,
        EmotesText = 0x0095AEBC,
        EnvironmentalDamage = 0x0095AED8,
        Exhaustion = 0x0095AEF4,
        Faction = 0x0095AF2C,
        FootstepTerrainLookup = 0x0095AF80,
        GameObjectArtKit = 0x0095AF9C,
        GameObjectDisplayInfo = 0x0095AFB8,
        GameTables = 0x0095AFD4,
        GemProperties = 0x0095B00C,
        GlyphProperties = 0x0095B044,
        GlyphSlot = 0x0095B060,
        GMSurveyAnswers = 0x0095B07C,
        GMSurveyCurrentSurvey = 0x0095B098,
        GMSurveyQuestions = 0x0095B0B4,
        GMSurveySurveys = 0x0095B0D0,
        GMTicketCategory = 0x0095B0EC,
        gtBarberShopCostBase = 0x0095B108,
        gtCombatRatings = 0x0095B124,
        gtChanceToMeleeCrit = 0x0095B140,
        gtChanceToMeleeCritBase = 0x0095B15C,
        gtChanceToSpellCrit = 0x0095B178,
        gtChanceToSpellCritBase = 0x0095B194,
        gtNPCManaCostScaler = 0x0095B1B0,
        gtOCTBaseHPByClass = 0x0095B1CC,
        gtOCTBaseMPByClass = 0x0095B1E8,
        gtOCTClassCombatRatingScalar = 0x0095B204,
        gtOCTHpPerStamina = 0x0095B220,
        gtOCTRegenMP = 0x0095B23C,
        gtRegenMPPerSpt = 0x0095B258,
        gtSpellScaling = 0x0095B274,
        GuildPerkSpells = 0x0095B2E4,
        HolidayDescriptions = 0x0095B31C,
        HolidayNames = 0x0095B338,
        Holidays = 0x0095B354,
        ItemArmorQuality = 0x0095B38C,
        ItemArmorTotal = 0x0095B370,
        ItemArmorShield = 0x0095B3A8,
        ItemBagFamily = 0x0095B3C4,
        ItemDamageAmmo = 0x0095B3FC,
        ItemDamageOneHand = 0x0095B418,
        ItemDamageOneHandCaster = 0x0095B434,
        ItemDamageRanged = 0x0095B450,
        ItemDamageThrown = 0x0095B46C,
        ItemDamageTwoHand = 0x0095B488,
        ItemDamageTwoHandCaster = 0x0095B4A4,
        ItemDamageWand = 0x0095B4C0,
        ItemDisenchantLoot = 0x0095B4DC,
        ItemGroupSounds = 0x0095B51C,
        ItemLimitCategory = 0x0095B538,
        ItemNameDescription = 0x0095B554,
        ItemPetFood = 0x0095B570,
        ItemPurchaseGroup = 0x0095B58C,
        ItemRandomProperties = 0x0095B5A8,
        ItemRandomSuffix = 0x0095B5C4,
        ItemReforge = 0x0095B5E0,
        ItemSet = 0x0095B5FC,
        ItemSubClassMask = 0x0095B618,
        JournalEncounterCreature = 0x0095B688,
        JournalEncounterItem = 0x0095B6A4,
        JournalEncounter = 0x0095B6C0,
        JournalEncounterSection = 0x0095B6DC,
        JournalInstance = 0x0095B6F8,
        LanguageWords = 0x0095B714,
        Languages = 0x0095B730,
        LfgDungeonExpansion = 0x0095B74C,
        LfgDungeonGroup = 0x0095B768,
        LfgDungeons = 0x0095B784,
        LoadingScreenTaxiSplines = 0x0095B7BC,
        Lock = 0x0095B7D8,
        LockType = 0x0095B7F4,
        MailTemplate = 0x0095B810,
        MapDifficulty = 0x0095B82C,
        Material = 0x0095B848,
        MountCapability = 0x0095B864,
        MountType = 0x0095B880,
        NPCSounds = 0x0095B90C,
        NumTalentsAtLevel = 0x0095B928,
        OverrideSpellData = 0x0095B97C,
        Package = 0x0095B998,
        PageTextMaterial = 0x0095B9B4,
        PaperDollItemFrame = 0x0095B9D0,
        Phase = 0x0095BA08,
        PhaseXPhaseGroup = 0x0095BA40,
        PlayerCondition = 0x0095BA5C,
        PowerDisplay = 0x0095BA78,
        PvpDifficulty = 0x0095BA94,
        QuestFactionReward = 0x0095BAB0,
        QuestInfo = 0x0095BACC,
        QuestPOIBlob = 0x0095BAE8,
        QuestPOIPoint = 0x0095BB04,
        QuestSort = 0x0095BB20,
        QuestXP = 0x0095BB3C,
        ResearchBranch = 0x0095BBAC,
        ResearchField = 0x0095BB90,
        ResearchProject = 0x0095BBC8,
        ResearchSite = 0x0095BBE4,
        RandPropPoints = 0x0095BB74,
        ScalingStatDistribution = 0x0095BC00,
        ScalingStatValues = 0x0095BC1C,
        ScreenEffect = 0x0095BC38,
        ScreenLocation = 0x0095BC54,
        ServerMessages = 0x0095BC70,
        SkillLineAbility = 0x0095BC8C,
        SkillLineAbilitySortedSpell = 0x0095BCA8,
        SkillLineCategory = 0x0095BCC4,
        SkillLine = 0x0095BCE0,
        SkillRaceClassInfo = 0x0095BCFC,
        SkillTiers = 0x0095BD18,
        SoundAmbience = 0x0095BD34,
        SoundAmbienceFlavor = 0x0095BD50,
        SoundEmitters = 0x0095BD6C,
        SpellActivationOverlay = 0x0095BDC0,
        SpellAuraOptions = 0x0095BDDC,
        SpellAuraRestrictions = 0x0095BDF8,
        SpellCastingRequirements = 0x0095BE14,
        SpellCastTimes = 0x0095BE30,
        SpellCategories = 0x0095BE4C,
        SpellCategory = 0x0095BE68,
        SpellChainEffects = 0x0095BE84,
        SpellClassOptions = 0x0095BEA0,
        SpellCooldowns = 0x0095BEBC,
        Spell = 0x0095C15C,
        SpellDescriptionVariables = 0x0095BED8,
        SpellDifficulty = 0x0095BEF4,
        SpellDispelType = 0x0095BF10,
        SpellDuration = 0x0095BF2C,
        SpellEffectCameraShakes = 0x0095BF64,
        SpellEquippedItems = 0x0095BF80,
        SpellFlyout = 0x0095BF9C,
        SpellFlyoutItem = 0x0095BFB8,
        SpellFocusObject = 0x0095BFD4,
        SpellIcon = 0x0095BFF0,
        SpellInterrupts = 0x0095C00C,
        SpellItemEnchantment = 0x0095C028,
        SpellItemEnchantmentCondition = 0x0095C044,
        SpellLevels = 0x0095C060,
        SpellMechanic = 0x0095C07C,
        SpellMissile = 0x0095C098,
        SpellMissileMotion = 0x0095C0B4,
        SpellRadius = 0x0095C0EC,
        SpellRange = 0x0095C108,
        SpellPower = 0x0095C0D0,
        SpellReagents = 0x0095C140,
        SpellRuneCost = 0x0095C124,
        SpellScaling = 0x0095C178,
        SpellShapeshift = 0x0095C194,
        SpellShapeshiftForm = 0x0095C1B0,
        SpellSpecialUnitEffect = 0x0095C1CC,
        SpellTargetRestrictions = 0x0095C1E8,
        SpellTotems = 0x0095C204,
        SpellVisual = 0x0095C290,
        SpellVisualKitAreaModel = 0x0095C258,
        SpellVisualKitModelAttach = 0x0095C274,
        Stationery = 0x0095C2AC,
        StringLookups = 0x0095C2C8,
        SummonProperties = 0x0095C2E4,
        Talent = 0x0095C300,
        TalentTab = 0x0095C31C,
        TalentTreePrimarySpells = 0x0095C338,
        TaxiNodes = 0x0095C354,
        TaxiPathNode = 0x0095C370,
        TaxiPath = 0x0095C38C,
        TerrainTypeSounds = 0x0095C3A8,
        TotemCategory = 0x0095C3C4,
        TransportAnimation = 0x0095C3E0,
        TransportPhysics = 0x0095C3FC,
        TransportRotation = 0x0095C418,
        UnitBloodLevels = 0x0095C434,
        UnitBlood = 0x0095C450,
        UnitPowerBar = 0x0095C46C,
        Vehicle = 0x0095C488,
        VehicleSeat = 0x0095C4A4,
        VehicleUIIndicator = 0x0095C4C0,
        VehicleUIIndSeat = 0x0095C4DC,
        VocalUISounds = 0x0095C4F8,
        World_PVP_Area = 0x0095C514,
        WeaponImpactSounds = 0x0095C530,
        WeaponSwingSounds2 = 0x0095C54C,
        WorldMapArea = 0x0095C568,
        WorldMapContinent = 0x0095C584,
        WorldMapOverlay = 0x0095C5A0,
        WorldMapTransforms = 0x0095C5BC,
        WorldSafeLocs = 0x0095C5D8,
        WorldStateUI = 0x0095C5F4,
        ZoneIntroMusicTable = 0x0095C610,
        ZoneMusic = 0x0095C62C,
        WorldStateZoneSounds = 0x0095C648,
        WorldChunkSounds = 0x0095C664,
        PhaseShiftZoneSounds = 0x0095BA24,
        FootprintTextures = 0x009397C4,
        GroundEffectDoodad = 0x009397E0,
        GroundEffectTexture = 0x009397FC,
        Light = 0x00939818,
        LightFloatBand = 0x00939850,
        LightSkybox = 0x00939888,
        LiquidMaterial = 0x009398A4,
        LiquidObject = 0x009398C0,
        LiquidType = 0x009398DC,
        SoundEntriesFallbacks = 0x0093994C,
        TerrainMaterial = 0x00939968,
        TerrainType = 0x00939984,
        Weather = 0x009399BC,
        WMOAreaTable = 0x009399D8,
        ZoneLight = 0x009399F4,
        ZoneLightPoint = 0x00939A10,
    }

    #endregion

}
