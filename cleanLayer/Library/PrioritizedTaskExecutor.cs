using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cleanLayer.Library
{
    public class PrioritizedTaskExecutor : TaskExecutor
    {
        public PrioritizedTaskExecutor(params Task[] args)
            :base(args)
        {
            Tasks.Sort();
        }

        public override void Execute()
        {
            var tasks = Tasks;
            foreach (var t in tasks)
            {
                if (t.Ready)
                {
                    t.Execute();
                    if (!t.Repeat)
                        Tasks.Remove(t);
                    break; // Break because we're in endscene!
                }
            }
        }
    }
}
