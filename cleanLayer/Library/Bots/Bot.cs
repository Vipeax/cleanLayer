using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cleanLayer.Library.Bots
{
    public static class Bot
    {
        public static void Initialize(BotBase bot)
        {
            CurrentBot = bot;
        }

        public static bool Start()
        {
            if (CurrentBot == null)
                return false;
            return CurrentBot.Start();
        }

        public static void Stop()
        {
            if (CurrentBot == null)
                return;
            CurrentBot.Stop();
        }

        public static BotBase CurrentBot
        {
            get;
            private set;
        }

        public static void Pulse()
        {
            if (CurrentBot == null)
                return;

            if (!CurrentBot.IsRunning)
                return;

            CurrentBot.Pulse();
        }
    }
}
