// Taken from Apoc's FSM
// http://www.mmowned.com/forums/general/programming/232703-bot-developers-simple-but-effective-fsm-your-bots.html
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cleanGatherer.FSM
{
    public abstract class State : IComparable<State>, IComparer<State>
    {
        public abstract int Priority { get; }

        public abstract bool NeedToRun { get; }

        public abstract void Run();

        public int CompareTo(State other)
        {
            return -Priority.CompareTo(other.Priority);
        }

        public int Compare(State x, State y)
        {
            return -x.Priority.CompareTo(y.Priority);
        }
    }
}
