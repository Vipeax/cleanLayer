using System;
using System.Collections.Generic;
using cleanCore;
using cleanLayer.Brains;
using cleanLayer.Library.Combat;

namespace cleanLayer
{
    public static class Combat
    {
        static Combat()
        {
            IsRunning = false;
        }        

        public static bool IsRunning
        {
            get;
            private set;
        }

        public static void Initialize(Brain brain)
        {
            Brain = brain;
        }

        public static Brain Brain
        {
            get;
            private set;
        }

        public static bool Pulse()
        {
            if (Brain == null)
                return false;

            if (!Manager.IsInGame)
                return false;

            Brain.Pulse();

            return false;
        }
    }
}
