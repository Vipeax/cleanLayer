using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace cleanCore
{
    public class WoWSpell
    {
        #region Static members

        public static void Initialize()
        {
            castSpell = Helper.Magic.RegisterDelegate<CastSpellDelegate>(Offsets.CastSpell);
            getSpellCooldown = Helper.Magic.RegisterDelegate<GetSpellCooldownDelegate>(Offsets.GetSpellCooldown);
        }

        public static bool ForceUpdate = true;
        public static bool Pulse()
        {
            if (Manager.LocalPlayer == null)
                return false;

            if (!ForceUpdate)
                return false;

            ForceUpdate = false;

            var spellCount = Helper.Magic.Read<int>(Offsets.SpellCount);
            var spellBook = Helper.Magic.Read<uint>(Offsets.SpellBook);
            for (int i = 0; i < spellCount; i++)
            {
                var spellStruct = Helper.Magic.ReadStruct<uint>((IntPtr)(spellBook + (i * 4)));
                var spellId = Helper.Magic.Read<int>(spellStruct + 0x4);
                if (!PlayerSpells.ContainsKey(spellId))
                    PlayerSpells.Add(spellId, new WoWSpell(spellId));
            }
            return true;
        }

        private static Dictionary<int, WoWSpell> PlayerSpells = new Dictionary<int, WoWSpell>();

        public static WoWSpell GetSpell(int id)
        {
            return PlayerSpells.Values.FirstOrDefault(o => o.Id == id) ?? WoWSpell.Invalid;
        }

        public static WoWSpell GetSpell(string name)
        {
            return PlayerSpells.Values.FirstOrDefault(o => o.Name == name) ?? WoWSpell.Invalid;
        }

        public static List<WoWSpell> GetAllSpells()
        {
            return PlayerSpells.Values.ToList();
        }

        #endregion

        public WoWSpell(int id)
        {
            try
            {
                SpellRecord = WoWDB.GetTable(ClientDB.Spell).GetRow(id).GetStruct<SpellRec>();
            }
            catch
            {
                SpellRecord = default(SpellRec);
            }
        }

        private SpellRec SpellRecord
        {
            get;
            set;
        }

        public bool IsValid
        {
            get { return Id != 0; }
        }

        public int Id
        {
            get { return SpellRecord.ID; }
        }

        public string Name
        {
            get { return SpellRecord.Name; }
        }

        public int Category
        {
            get { return SpellRecord.SpellCategoriesID; } // TODO: verify
        }

        public void Cast()
        {
            Cast(Manager.LocalPlayer);
        }

        public void Cast(WoWUnit target)
        {
            if (!IsValid)
                return;

            if (target == null || !target.IsValid)
                return;

            //target.Select();
            //WoWScript.ExecuteNoResults("CastSpellByID(" + Id + ")");
            castSpell(Id, guid: target.Guid);
        }

        public float Cooldown
        {
            get
            {
                if (!IsValid)
                    return float.MaxValue;

                int start = 0;
                int duration = 0;
                bool isReady = false;
                int unk0 = 0;

                getSpellCooldown(Id, false, ref duration, ref start, ref isReady, ref unk0);

                int result = start + duration - (int)Helper.PerformanceCount;
                return isReady ? (result > 0 ? result / 1000f : 0f) : float.MaxValue;
            }
        }

        public bool IsReady
        {
            get { return Cooldown <= 0f; }
        }

        public static WoWSpell Invalid = new WoWSpell(0);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int CastSpellDelegate(int spellId, int itemId = 0, ulong guid = 0ul, int isTrade = 0, int a6 = 0, int a7 = 0, int a8 = 0);
        private static CastSpellDelegate castSpell;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool GetSpellCooldownDelegate(int spellId, bool isPet, ref int duration, ref int start, ref bool isEnabled, ref int unk0);
        private static GetSpellCooldownDelegate getSpellCooldown;
    }
}
