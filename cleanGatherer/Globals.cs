using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cleanGatherer
{
    public static class Globals
    {
        public static int SleepTime = 2500; // ms
        public static int MountId = 59569; // Swift Purple Wind Rider

        public static List<string> HarvestNames = new List<string>()
        {
            "Twilight Jasmine",
            "Cinderbloom",
            "Elementium Vein",
            "Rich Elementium Vein",
            "Pyrite Vein"
        };

        /* Don't touch! */
        public static int Harvests = 0;
        public static int Kills = 0;
        public static int Deaths = 0;
    }
}
