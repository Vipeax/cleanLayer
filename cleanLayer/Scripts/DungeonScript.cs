using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanLayer.Library.Scripts;
using cleanLayer.Library.LUA;

namespace cleanLayer.Scripts
{
    public class DungeonScript : Script
    {
        public DungeonScript()
            : base("Dungeon", "Handler")
        { }

        public override void OnStart()
        {
            Print("Make sure you have the LFG tool set up properly before starting!");
        }

        public override void OnTick()
        {
            if (!Dungeon.InDungeon())
            {
                if (!Dungeon.InQueue())
                {
                    if (!Dungeon.IsDeserter())
                    {
                        //Dungeon.SetRoles();
                        //Dungeon.SetToRandom();
                        Dungeon.JoinQueue();
                        Print("Joining queue");
                    }
                }

                if (Dungeon.IsProposal() && !Dungeon.HasAcceptedProposal())
                {
                    Dungeon.AcceptDungeon();
                    Print("Accepting dungeon proposal");
                }
            }
            Sleep(2000);
        }

        public override void OnTerminate()
        {
            Print("Dungeon finder terminated");
        }
    }
}
