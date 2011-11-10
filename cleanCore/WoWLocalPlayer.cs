using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace cleanCore
{
    public class WoWLocalPlayer : WoWPlayer
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate int ClickToMoveDelegate(
            IntPtr thisPointer, int clickType, ref ulong interactGuid, ref Location clickLocation, float precision);
        public static ClickToMoveDelegate ClickToMoveFunction;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void SetFacingDelegate(IntPtr thisObj, uint time, float facing);
        private static SetFacingDelegate _setFacing;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate bool IsClickMovingDelegate(IntPtr thisObj);
        private static IsClickMovingDelegate _isClickMoving;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void StopCTMDelegate(IntPtr thisObj);
        private static StopCTMDelegate _stopCTM;

        public new static void Initialize()
        {
            WoWObject.Initialize();

            ClickToMoveFunction = Helper.Magic.RegisterDelegate<ClickToMoveDelegate>(Offsets.ClickToMove);
            _setFacing = Helper.Magic.RegisterDelegate<SetFacingDelegate>(Offsets.SetFacing);
            _isClickMoving = Helper.Magic.RegisterDelegate<IsClickMovingDelegate>(Offsets.IsClickMoving);
            _stopCTM = Helper.Magic.RegisterDelegate<StopCTMDelegate>(Offsets.StopCTM);
        }

        public WoWLocalPlayer(IntPtr pointer)
            : base(pointer)
        {

        }

        public void ClickToMove(Location target, ClickToMoveType type = ClickToMoveType.Move, ulong guid = 0ul)
        {
            Helper.ResetHardwareAction();
            ClickToMoveFunction(Pointer, (int)type, ref guid, ref target, 0.1f);
        }

        public void StopCTM()
        {
            _stopCTM(Pointer);
        }

        public void LookAt(Location loc)
        {
            var local = Location;
            var diffVector = new Location(loc.X - local.X, loc.Y - local.Y, loc.Z - local.Z);
            SetFacing(diffVector.Angle);
        }

        public void SetFacing(float angle)
        {
            const float pi2 = (float)(Math.PI * 2);
            if (angle < 0.0f)
                angle += pi2;
            if (angle > pi2)
                angle -= pi2;
            _setFacing(Pointer, Helper.PerformanceCount, angle);
        }

        public bool IsClickMoving
        {
            get { return _isClickMoving(Pointer); }
        }

        public bool IsLooting
        {
            get { return (Flags & UnitFlags.Looting) != 0; }
        }

        public Location Corpse
        {
            get
            {
                return Helper.Magic.ReadStruct<Location>(new IntPtr(Offsets.CorpsePosition));
            }
        }

        public void StartAttack()
        {
            WoWScript.Execute("StartAttack()");
        }

        public int UnusedTalentPoints
        {
            get
            {
                return WoWScript.Execute<int>("UnitCharacterPoints(\"player\")", 0);
            }
        }

        public int ComboPoints
        {
            get { return Helper.Magic.Read<int>(Helper.Rebase(0xA98D25)); }
        }

        public List<WoWTotem> Totems
        {
            get
            {
                return Manager.Objects
                    .Where(x => x.IsValid && x.IsUnit)
                    .Select(x => x as WoWUnit)
                    .Where(x => x.CreatedBy == Guid && x.IsTotem)
                    .Select(x => x as WoWTotem).ToList();
            }
        }

        public List<WoWItem> Items
        {
            get
            {
                return Manager.Objects
                    .Where(x => x.IsValid && x.IsItem)
                    .Select(x => x as WoWItem)
                    .Where(x => x.OwnerGuid == Guid)
                    .ToList();
            }
        }

        public WoWItem GetEquippedItem(EquipSlot slot)
        {
            var entry = GetDescriptor<uint>((int)PlayerField.PLAYER_VISIBLE_ITEM_1_ENTRYID + ((int)slot * 0x8));
            var item = Items.Where(x => x.Entry == entry).FirstOrDefault() ?? WoWItem.Invalid;
            return item;
        }

        #region Movement

        public void Ascend()
        {
            WoWScript.ExecuteNoResults("JumpOrAscendStart()");
        }

        public void Jump()
        {
            WoWScript.ExecuteNoResults("JumpOrAscendStart()");
        }

        public void Descend()
        {
            WoWScript.ExecuteNoResults("SitStandOrDescendStart()");
        }

        public void MoveBackward()
        {
            WoWScript.ExecuteNoResults("MoveBackwardStart()");
        }

        public void MoveForward()
        {
            WoWScript.ExecuteNoResults("MoveForwardStart()");
        }

        public void StopMoving()
        {
            WoWScript.ExecuteNoResults("AscendStop() DescendStop() MoveBackwardStop() MoveForwardStop() StrafeLeftStop() StrafeRightStop()");
        }

        public void StrafeLeft()
        {
            WoWScript.ExecuteNoResults("StrafeLeftStart()");
        }

        public void StrafeRight()
        {
            WoWScript.ExecuteNoResults("StrafeRightStart()");
        }

        public void Dismount()
        {
            WoWScript.ExecuteNoResults("Dismount()");
        }

        #endregion

        public static implicit operator IntPtr(WoWLocalPlayer self)
        {
            return self != null ? self.Pointer : IntPtr.Zero;
        }

        public static new WoWLocalPlayer Invalid = new WoWLocalPlayer(IntPtr.Zero);
    }
}