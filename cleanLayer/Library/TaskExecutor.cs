using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cleanLayer.Library
{
    public abstract class TaskExecutor
    {
        public TaskExecutor(params Task[] args)
        {
            
        }

        public List<Task> Tasks { get; private set; }
        public abstract void Execute();
    }
}
