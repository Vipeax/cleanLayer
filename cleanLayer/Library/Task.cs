using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cleanLayer.Library
{
    public abstract class Task : IComparable<Task>, IComparer<Task>
    {
        public Task(int priority = int.MinValue, bool repeat = true)
        {
            this.Priority = priority;
            this.Repeat = repeat;
            this.Ready = false;
        }

        public abstract void Execute();
        public virtual int Priority { get; private set; }
        public virtual bool Repeat { get; private set; }
        public virtual bool Ready { get; private set; }

        public int CompareTo(Task other)
        {
            return -Priority.CompareTo(other.Priority);
        }

        public int Compare(Task x, Task y)
        {
            return -x.Priority.CompareTo(y.Priority);
        }
    }
}
