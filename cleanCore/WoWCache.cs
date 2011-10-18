using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace cleanCore
{
    public static class WoWCache
    {
        public static List<QuestCache> GetQuests()
        {
            var cRet = new List<QuestCache>();
            var cBase = Helper.Magic.ReadStruct<WowCache>((IntPtr)Helper.Rebase(0x8DDAC8));
            var cPtr = cBase.First;
            while (true)
            {
                var cList = Helper.Magic.ReadStruct<TSExplicitList>(cPtr);
                if (cList.Next == cBase.First)
                    break;

                var cQuest = Helper.Magic.ReadStruct<QuestCache>(cPtr + 0x0C);
                cRet.Add(cQuest);

                cPtr = cList.Next;
            }
            return cRet;
        }

        #region Structures

        [StructLayout(LayoutKind.Sequential)]
        public struct TSExplicitList
        {
            public IntPtr Next;
            public IntPtr Prev;
            public uint Unk1; // == 0x03 for last node and 0x00 for others

            public bool IsLast { get { return (Unk1 & 0x01) != 0; } } // works too...
        }

        public struct WowCache
        {
            uint vTable;        // pointer to virtual methods table
            uint blizzUsing;    // pointer to string "Blizzard::Using::GlobalUse"
            uint field08;       // ? pointer so something
            uint field0C;       // ?
            public IntPtr First;  // pointer to first cached node
            public IntPtr Last;   // pointer to last cached node
            uint field18;       // ?
            uint field1C;       // ?
            uint field20;       // ?
            uint field24;       // ? pointer to something
            uint field28;       // ?
            uint field2C;       // ?
            uint field30;       // ?
            uint signature;     // WDB file signature, like 'WQST' etc...
            uint fileName;      // pointer to WDB file name
            uint field3C;       // ?
            uint opcode;        // opcode id used for cache requests
            uint field44;       // ?
            byte HasQueryGuid;  // does it add guid to query packet or not
            byte saveToDisk;    // does it save *.wdb file to disk or not
            byte field4A;       // ?
            byte field4B;       // ?
            uint field4C;       // ?
            uint RequestCounter; // counter for cache requests from server
            // add other fields below... there's a lot of unknown ones...
        }

        public struct QuestCache
        {
            public uint Id;
            public uint Tag;
            public int Level;
            public uint MinLevel;
            public int ZoneOrSort;
            public uint Type;
            public uint SuggestedPlayers;
            public uint RepObjectiveFaction;
            public uint RepObjectiveValue;
            public uint OppositeRepFaction;
            public uint OppositeRepValue;
            public uint FollowupQuestId;
            public uint RewXPId;
            public uint RewOrReqMoney;
            public uint RewMoneyMaxLevel;
            public uint RewSpell;
            public uint RewSpellCast;
            public uint RewHonorAddition;
            public float RewHonorMultiplier;
            public uint SrcItemId;
            public uint QuestFlags;
            public uint QuestFlags2;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] RewItem;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] RewItemCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public uint[] ReqChoiseItem;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public uint[] ReqChoiseItemCount;
            public uint PointMapId;
            public float PointX;
            public float PointY;
            public uint PointOpt;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
            byte[] _Title;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3000)]
            byte[] _Objectives;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3000)]
            byte[] _Details;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
            byte[] _EndText;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] ReqCreatureOrGOId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] ReqCreatureOrGOIdCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public uint[] CollectItemId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public uint[] CollectItemCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] IntermediateItemId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] IntermediateItemCount;
            public uint ReqLearnedSpell;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            byte[] _ObjectiveText1;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            byte[] _ObjectiveText2;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            byte[] _ObjectiveText3;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            byte[] _ObjectiveText4;
            public uint CharTitleId;
            public uint PlayersSlain;
            public uint BonusTalents;
            public uint BonusArenaPoints;
            public uint RewSkill;
            public uint RewSkillValue;
            public uint QuestPortrait;
            public uint QuestPortraitTurnIn;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
            byte[] _QuestPortraitText;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            byte[] _QuestPortraitName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
            byte[] _QuestPortraitTurnInText;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            byte[] _QuestPortraitTurnInName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2048)]
            byte[] _CompletedText;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public uint[] RewRepFaction;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public uint[] RewRepValueId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public uint[] RewRepValue;
            public uint RewRepShowMask;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] RewCurrencyId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] RewCurrencyCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] ReqCurrencyId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] ReqCurrencyCount;
            public uint AcceptSoundId;
            public uint TurnInSoundKitId;

            public string Title { get { return Encoding.UTF8.GetString(_Title.TakeWhile(b => b != 0).ToArray()); } }
            public string Objectives { get { return Encoding.UTF8.GetString(_Objectives.TakeWhile(b => b != 0).ToArray()); } }
            public string Details { get { return Encoding.UTF8.GetString(_Details.TakeWhile(b => b != 0).ToArray()); } }
            public string EndText { get { return Encoding.UTF8.GetString(_EndText.TakeWhile(b => b != 0).ToArray()); } }
        };
        #endregion
    }
}
