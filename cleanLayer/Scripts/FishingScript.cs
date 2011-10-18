using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using cleanCore;
using cleanLayer.Library.Scripts;

namespace cleanLayer.Scripts
{
    public class FishingScript : Script
    {
        public FishingScript()
            : base("Fishing", "Bot")
        { }

        private int Fishes;
        private WoWSpell Fishing;
        private DateTime LootTimer;
        private DateTime CombatTimer;

        private List<string> EventSubscription = new List<string>
        {
            "LOOT_OPENED",
            "LOOT_SLOT_CLEARED",
            "LOOT_CLOSED",
            "SKILL_LINES_CHANGED",
            "PLAYER_REGEN_DISABLED",
            "PLAYER_REGEN_ENABLED"
        };

        private FishingState CurrentState
        {
            get;
            set;
        }

        public override void OnStart()
        {
            CurrentState = FishingState.Lure;
            Fishes = 0;
            Fishing = WoWSpell.GetSpell("Fishing");
            if (!Fishing.IsValid)
            {
                Print("You don't know fishing!");
                Stop();
            }

            foreach (var ev in EventSubscription)
                Events.Register(ev, HandleFishingEvents);
        }

        public override void OnTick()
        {
            switch (CurrentState)
            {
                case FishingState.Lure:
                    //if (!HasBait)
                    //{
                    //    // Implement applying lure
                    //}
                    CurrentState = FishingState.Cast;
                    break;

                case FishingState.Cast:
                    Print("Casting Fishing Pole");
                    Fishing.Cast();
                    CurrentState = FishingState.Fishing;
                    Sleep(500); // Force a sleep for the cast
                    break;

                case FishingState.Fishing:
                    if (IsFishing)
                    {
                        if (IsBobbing)
                            CurrentState = FishingState.Loot;
                    }
                    else
                        CurrentState = FishingState.Lure;
                    break;

                case FishingState.Loot:
                    Print("Getting Fishing Bobber");
                    Bobber.Interact();
                    LootTimer = DateTime.Now;
                    CurrentState = FishingState.Looting;
                    break;

                case FishingState.Looting:
                    var span = DateTime.Now - LootTimer;
                    if (span.TotalSeconds > 3)
                    {
                        Print("No loot? Back to fishing then!");
                        CurrentState = FishingState.Lure;
                    }
                    break;

                case FishingState.Combat:
                    if (Manager.LocalPlayer.IsDead)
                        OnTerminate();
                    // Not much we can do but wait :(
                    break;
            }

            Sleep(200);
        }


        private void HandleFishingEvents(string ev, List<string> args)
        {
            //Print("DEBUG - EVENT: {0} ({1})", ev, args);
            TimeSpan span;
            switch (ev)
            {
                case "LOOT_OPENED":
                    Print("We have {0} catches to loot", args[0]);
                    break;

                case "LOOT_SLOT_CLEARED":
                    //var loot = WoWScript.Execute("GetLootSlotInfo(" + args[0] + ")");
                    //Print("Looted [{0}]x{1}", loot[1], loot[2]);
                    break;

                case "LOOT_CLOSED":
                    span = DateTime.Now - LootTimer;
                    Print("Looting took {0} seconds.", Math.Round(span.TotalSeconds, 1));
                    Fishes++;
                    CurrentState = FishingState.Lure;
                    break;

                case "SKILL_LINES_CHANGED":
                    Print("Seems like we leveled up our fishing!");
                    break;

                case "PLAYER_REGEN_DISABLED":
                    Print("Seems like we entered combat!");
                    CombatTimer = DateTime.Now;
                    if (CanHandleCombat)
                        Combat.Pulse();
                    else
                        Print("... too bad you didn't select a combat brain, now we're going to die");
                    CurrentState = FishingState.Combat;
                    break;

                case "PLAYER_REGEN_ENABLED":
                    span = DateTime.Now - CombatTimer;
                    Print("Combat done after {0} seconds", Math.Round(span.TotalSeconds, 1));
                    CurrentState = FishingState.Lure;
                    break;
            }
        }

        public override void OnTerminate()
        {
            foreach (var ev in EventSubscription)
                Events.Remove(ev, HandleFishingEvents);

            CurrentState = FishingState.Lure;
            Fishing = null;
            Print("Fishing season is over! You did however catch {0} fishes.", Fishes);
        }

        #region Properties

        private WoWGameObject Bobber
        {
            get
            {
                return Manager.Objects.Where(b => b.IsValid && b.IsGameObject)
                    .Select(b => b as WoWGameObject)
                    .Where(b => b.CreatedByMe && b.Name == "Fishing Bobber")
                    .FirstOrDefault() ?? WoWGameObject.Invalid;
            }
        }

        private bool IsBobbing
        {
            get { return (Bobber.IsValid ? Helper.Magic.Read<byte>((uint)Bobber.Pointer + (uint)Offsets.IsBobbing) == 1 : false); }
        }

        private bool IsFishing
        {
            get { return Manager.LocalPlayer.ChanneledCastingId == Fishing.Id; }
        }

        private bool HasBait
        {
            get { return WoWScript.Execute<int>("GetWeaponEnchantInfo()", 0) == 1; }
        }

        private bool CanHandleCombat
        {
            get { return Combat.Brain != null; }
        }

        #endregion

        #region FishingState enum

        enum FishingState
        {
            Lure,
            Cast,
            Fishing,
            Loot,
            Looting,
            Combat
        }

        #endregion
    }
}
