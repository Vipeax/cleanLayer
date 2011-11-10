using System;
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
                    if (HarmfulTarget.Distance > a.Range)
                    {
                        if (Manager.LocalPlayer.IsClickMoving)
                            Sleep(200);
                        Manager.LocalPlayer.ClickToMove(HarmfulTarget.Location);
                        Sleep(200);
                    }
                    else
                    {
                        HarmfulTarget.Select();
                        HarmfulTarget.Face();
                    }
                }

                else if (action is HelpfulSpellAction && HelpfulTarget.IsValid)
                {
                    var a = action as HelpfulSpellAction;
                    if (HarmfulTarget.Distance > a.Range)
                    {
                        if (Manager.LocalPlayer.IsClickMoving)
                            Sleep(200);
                        Manager.LocalPlayer.ClickToMove(HelpfulTarget.Location);
                        Sleep(200);
                    }
                    else
                    {
                        HelpfulTarget.Select();
                        HelpfulTarget.Face();
                    }
                }

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

        public WoWClass Class
        {
            get { return this.GetType().GetBrainInfo().Class; }
        }

        public string Specialization
        {
            get { return this.GetType().GetBrainInfo().Specialization; }
        }

        public string Name
        {
            get { return this.GetType().GetInfo().Name; }
        }

        public string Version
        {
            get { return this.GetType().GetInfo().Version; }
        }

        public string Author
        {
            get { return this.GetType().GetAuthor(); }
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

    #region Attributes

    public class PluginInfo : Attribute
    {
        public PluginInfo(string name)
        {
            Name = name;
        }

        public PluginInfo(string name, string version)
        {
            Name = name;
            Version = version;
        }

        public string Name { get; private set; }
        public string Version { get; private set; }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name))
                return string.Format("{0} v{1}", Name, Version);
            return Name;
        }
    }

    public class PluginAuthor : Attribute
    {
        public PluginAuthor(string author)
        {
            Author = author;
        }

        public string Author { get; private set; }
    }

    public class BrainInfo : Attribute
    {
        public BrainInfo(WoWClass wowclass, string specialization)
        {
            Class = wowclass;
            Specialization = specialization;
        }

        public WoWClass Class { get; private set; }
        public string Specialization { get; private set; }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", Class, Specialization);
        }
    }

    #endregion
}
