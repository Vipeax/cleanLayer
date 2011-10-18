using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;

namespace cleanLayer.Library.LUA
{
    public static class Dungeon
    {
        private static string GetLFDMode()
        {
            var ret = WoWScript.Execute<string>("GetLFGMode()");
            return (!string.IsNullOrEmpty(ret) ? ret : "none");
        }

        public static bool InDungeon()
        {
            return WoWScript.Execute<string>("IsInInstance()", 1) == "party";
        }

        public static bool InQueue()
        {
            return GetLFDMode() == "queued" || IsProposal();
        }

        public static bool IsProposal()
        {
            return GetLFDMode() == "proposal";
        }

        public static bool HasAcceptedProposal()
        {
            return WoWScript.Execute<int>("GetLFGProposal()", 6) == 1;
        }

        public static bool IsRolecheck()
        {
            return GetLFDMode() == "rolecheck";
        }

        public static bool IsDeserter()
        {
            return WoWScript.Execute<bool>("UnitHasLFGDeserter(\"player\")");
        }

        public static void AcceptDungeon()
        {
            WoWScript.ExecuteNoResults("AcceptProposal()");
        }

        public static void RejectDungeon()
        {
            WoWScript.ExecuteNoResults("RejectProposal()");
        }

        public static void JoinQueue()
        {
            WoWScript.ExecuteNoResults("LFDQueueFrame_Join()");
        }

        public static void LeaveQueue()
        {
            WoWScript.ExecuteNoResults("LeaveLFG()");
        }

        //public static void SetToRandom()
        //{
        //    WoWScript.ExecuteNoResults(
        //        @"LFDQueueFrame_SetTypeRandomDungeon()" +
        //          "LFDQueueFrameRandom_UpdateFrame()"
        //    );
        //}

        //public static void SetRoles(bool leader = false, bool tank = false, bool healer = false, bool damage = true)
        //{
        //    WoWScript.ExecuteNoResults(string.Format("SetLFGRoles({0}, {1}, {2}, {3})", leader, tank, healer, damage));
        //}
    }
}
