﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using cleanCore;
using cleanLayer.Library;
using cleanLayer.Library.Scripts;

namespace cleanLayer.Library.Combat
{
    public abstract class Brain
    {
        public Brain()
        {
            BrainActions = new List<ActionBase>();
            Events.Register("PLAYER_REGEN_DISABLED", HandleCombatEvents);
            Events.Register("PLAYER_REGEN_ENABLED", HandleCombatEvents);
        }

        private bool MovingToTarget = false;
        private List<ActionBase> BrainActions;

        private void HandleCombatEvents(string ev, List<string> args)
        {
            switch (ev)
            {
                case "PLAYER_REGEN_DISABLED":
                    OnEnterCombat();
                    break;
                case "PLAYER_REGEN_ENABLED":
                    OnExitCombat();
                    break;
            }
        }

        public void SelectTargets()
        {
            HarmfulTargets = from u in Manager.Objects.Where(o => o.IsValid && (o.IsUnit || o.IsPlayer)).Select(o => o as WoWUnit)
                             where u.IsValid &&
                             u.Distance < Globals.MaxDistance &&
                             !u.IsFriendly &&
                             !u.IsDead &&
                             u.IsInCombat
                             orderby u.Distance ascending
                             select u;

            HarmfulTarget = HarmfulTargets.FirstOrDefault() ?? WoWUnit.Invalid;

            HelpfulTargets = from u in WoWParty.Members
                             where u.IsValid
                             && u.Distance < Globals.MaxDistance
                             && !u.IsDead
                             && u.IsFriendly
                             orderby u.HealthPercentage ascending
                             select u;

            HelpfulTarget = HelpfulTargets.FirstOrDefault() ?? WoWPlayer.Invalid;
            AlternativeHelpfulTarget = HelpfulTargets.ElementAtOrDefault(1) ?? WoWPlayer.Invalid;
        }

        public void Pull(WoWUnit unit)
        {
            if (unit == null || !unit.IsValid)
                return;
            try
            {
                Mover.StopMoving();
                unit.Face();
                unit.Select();
                Manager.LocalPlayer.StartAttack();
                PullSpell.ExecuteEx(unit);
            }
            catch (Exception) { }
        }

        public void Pulse()
        {
            if (!Manager.IsInGame)
                return;

            if (Manager.LocalPlayer.IsDead)
                return;

            if (SleepTime >= DateTime.Now)
                return;

            try
            {
                SelectTargets();
                
                var action = (from a in BrainActions
                              where a.IsWanted && a.IsReady
                              orderby a.Priority descending
                              select a).FirstOrDefault();

                if (action is HarmfulSpellAction && HarmfulTarget.IsValid)
                {
                    var a = action as HarmfulSpellAction;
                    if (Manager.LocalPlayer.Location.DistanceTo(HarmfulTarget.Location) > a.Range)
                    {
                        if (Manager.LocalPlayer.IsClickMoving && MovingToTarget)
                            Sleep(200);
                        MovingToTarget = true;
                        Manager.LocalPlayer.ClickToMove(HarmfulTarget.Location);
                        Sleep(200);
                    }
                }

                else if (action is HelpfulSpellAction && HelpfulTarget.IsValid)
                {
                    var a = action as HelpfulSpellAction;
                    if (Manager.LocalPlayer.Location.DistanceTo(HelpfulTarget.Location) > a.Range)
                    {
                        if (Manager.LocalPlayer.IsClickMoving && MovingToTarget)
                            Sleep(200);
                        MovingToTarget = true;
                        Manager.LocalPlayer.ClickToMove(HelpfulTarget.Location);
                        Sleep(200);
                    }
                }

                MovingToTarget = false;

                if (Manager.LocalPlayer.IsClickMoving)
                    Manager.LocalPlayer.StopCTM();                

                OnBeforeAction(action);
                if (action != null)
                    action.Execute();
                OnAfterAction(action);
            }
            catch (SleepException ex)
            {
                SleepTime = DateTime.Now + ex.Time;
            }
            catch (Exception ex)
            {
                Log.WriteLine("Exception in Brain: {0}", ex.Message);
            }
        }

        public abstract WoWClass Class
        {
            get;
        }

        public abstract string Specialization
        {
            get;
        }

        private DateTime SleepTime
        {
            get;
            set;
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", Class, Specialization);
        }

        protected void AddAction(ActionBase action)
        {
            BrainActions.Add(action);
        }

        public void Sleep(int ms = 0)
        {
            throw new SleepException(ms);
        }

        protected abstract void OnBeforeAction(ActionBase action);

        protected abstract void OnAfterAction(ActionBase action);

        protected abstract HarmfulSpellAction PullSpell { get; }

        public virtual bool NeedRest { get { return false; } }

        public virtual void Rest() { }

        public virtual void OnEnterCombat() { }

        public virtual void OnExitCombat() { }

        public IEnumerable<WoWUnit> HarmfulTargets
        {
            get;
            private set;
        }

        public WoWUnit HarmfulTarget
        {
            get;
            private set;
        }

        public IEnumerable<WoWUnit> HelpfulTargets
        {
            get;
            private set;
        }

        public WoWUnit HelpfulTarget
        {
            get;
            private set;
        }

        public WoWUnit AlternativeHelpfulTarget
        {
            get;
            private set;
        }
    }
}
