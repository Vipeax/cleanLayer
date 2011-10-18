using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanLayer.Library;
using cleanLayer.Library.FSM;

namespace cleanLayer.Bots.GBStates
{
    public class GBPull : State
    {
        private Grindbot _parent;

        private int MinLevel;
        private int MaxLevel;
        private List<int> Factions;
        private List<string> AvoidMobs;

        private WoWUnit CurrentEnemy = WoWUnit.Invalid;

        public GBPull(Grindbot parent)
        {
            _parent = parent;

            MinLevel = _parent.SubProfile.GrindArea[0].TargetMinLevel;
            MaxLevel = _parent.SubProfile.GrindArea[0].TargetMaxLevel;
            Factions = _parent.SubProfile.GrindArea[0].FactionList;
            AvoidMobs = _parent.SubProfile.AvoidMobs.Select(m => m.Name).ToList();
        }

        public override int Priority
        {
            get { return 50; }
        }

        public override bool NeedToRun
        {
            get { return Enemies.Count > 0; }
        }

        public override void Run()
        {
            CurrentEnemy = Enemies.FirstOrDefault() ?? WoWUnit.Invalid;

            if (CurrentEnemy != null && CurrentEnemy.IsValid)
            {
                if (CurrentEnemy.Distance > 20f) //tmp distance
                {
                    if (Mover.Status != MovementStatus.Moving || CurrentEnemy.Location.DistanceTo(Mover.Destination) > 15f)
                    {
                        _parent.Print("Moving to {0}", CurrentEnemy.Name);
                        if (!Mover.PathTo(CurrentEnemy.Location))
                            _parent.Blacklisted.Add(CurrentEnemy.Guid);
                    }
                    _parent.FSM.DelayNextPulse(500);
                }
                else
                {
                    if (!Manager.LocalPlayer.IsCasting)
                    {
                        Combat.Brain.Pull(CurrentEnemy);
                    }
                    _parent.FSM.DelayNextPulse(1000);
                }
            }
        }

        private List<WoWUnit> Enemies
        {
            get
            {
                return
                    Manager.Objects
                    .Where(x => x.IsValid && x.IsUnit) // Make sure to only get valid units
                    .Select(x => x as WoWUnit) // Cast as unit
                    .Where(x => !_parent.Blacklisted.Contains(x.Guid)
                        && !x.IsDead // Not dead
                        && !x.IsFriendly // Is he attackable? 
                        && Factions.Contains((int)x.Faction) // Is it a part of the factions we want to pull?
                        && !AvoidMobs.Contains(x.Name)
                        && x.Level <= MaxLevel // Check if it's below or equal to MaxLevel
                        && x.Level >= MinLevel // Check if it's above or equal to MinLevel
                        && x.InLoS) // Check for Line of Sight
                    .OrderBy(x => x.Distance) // Order by distance
                    .ToList();
            }
        }

        public override string Description
        {
            get { return "Pulling..."; }
        }
    }
}
