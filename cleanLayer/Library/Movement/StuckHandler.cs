using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using UnstuckAction = System.Action<cleanCore.Location>;

namespace cleanLayer.Library
{
    public class StuckHandler
    {
        private Queue<UnstuckAction> Unstucks;
        public virtual int Remaining { get { return Unstucks.Count; } }
        public virtual int Total { get; private set; }

        private Func<Location, bool> Done;
        private Location Target;

        private static StuckHandler _empty = null;
        public static StuckHandler Empty
        {
            get
            {
                if (_empty == null)
                    _empty = new StuckHandler(new List<UnstuckAction>(), (loc) => false, Location.Zero);

                return _empty;
            }
        }

        public StuckHandler(List<UnstuckAction> Unstucks, Func<Location, bool> Done, Location Target)
        {
            this.Target = Target;
            this.Done = Done;
            this.Unstucks = new Queue<UnstuckAction>(Unstucks);

            Total = Unstucks.Count;
        }

        public virtual bool Next()
        {
            var act = Unstucks.Dequeue();
            var res = act.BeginInvoke(Target, null, null);
            while (!res.IsCompleted && !Done(Target))
            { }// Helper.Wait(100);

            if (Done(Target))
            {
                act.EndInvoke(res);
                return true;
            }

            return false;
        }
    }
}
