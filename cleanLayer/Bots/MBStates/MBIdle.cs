using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanLayer.Library;
using cleanLayer.Library.FSM;

namespace cleanLayer.Bots.MBStates
{
    public class MBIdle : State
    {
        public MBIdle()
        { }

        public override int Priority
        {
            get { return int.MinValue; }
        }

        public override bool NeedToRun
        {
            get { return true; }
        }

        public override void Run()
        {
        }

        public override string Description
        {
            get { return "Idling"; }
        }
    }
}
