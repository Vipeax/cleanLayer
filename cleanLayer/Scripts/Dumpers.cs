using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanLayer.Library.Scripts;

namespace cleanLayer.Scripts
{
    public class PlayerDumperScript : Script
    {
        public PlayerDumperScript()
            : base("Players", "Dumper")
        { }

        public override void OnStart()
        {
            //Log.WriteLine("Facing: {0}", Manager.LocalPlayer.Facing);
            foreach (var p in Manager.Objects.Where(x => x.IsPlayer).Cast<WoWPlayer>())
            {
                Log.WriteLine("-- {0}", p.Name);
                Log.WriteLine("\tGUID: 0x{0}", p.Guid.ToString("X8"));
                Log.WriteLine("\tPosition: {0}", p.Location);
            }
        }
    }

    public class PartyDumperScript : Script
    {
        public PartyDumperScript()
            : base("Party", "Dumper")
        { }

        public override void OnStart()
        {
            foreach (var p in WoWParty.Members)
            {
                Log.WriteLine("-- {0}", p.Name);
                Log.WriteLine("\tHealth: {0}/{1} ({2}%)", p.Health, p.MaxHealth, p.HealthPercentage);
                Log.WriteLine("\tClass: {0}", p.Class);
                Log.WriteLine("\tLevel: {0}", p.Level);
                Log.WriteLine("\tLocation: {0} ({1} yards)", p.Location, p.Distance);
                Log.WriteLine("\tLoS: {0}", p.InLoS);
            }
        }
    }
}
