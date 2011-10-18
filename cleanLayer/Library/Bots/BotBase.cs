using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace cleanLayer.Library
{
    public abstract class BotBase
    {
        public abstract string Name { get; }

        public abstract bool Start();
        public abstract void Stop();

        public abstract bool IsRunning { get; }

        public abstract void Pulse();

        public abstract Form BotForm { get; }

        public void Print(string text, params object[] args)
        {
            Log.WriteLine(string.Format("[{0}] {1}", Name, string.Format(text, args)));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
