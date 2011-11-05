using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cleanLayer.Library
{
    public class SequentialTaskExecutor : TaskExecutor
    {
        public SequentialTaskExecutor(params Task[] args)
            : base(args)
        { }

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
