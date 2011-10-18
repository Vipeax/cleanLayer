using cleanCore;
using cleanLayer.Library.Scripts;

namespace cleanLayer.Library.Combat
{
    public abstract class ActionBase
    {
        public ActionBase(Brain brain, int priority = 0)
        {
            Brain = brain;
            Priority = priority;
        }

        public abstract void Execute();

        public void Sleep(int ms)
        {
            throw new SleepException(ms);
        }

        public void Print(string text, params object[] args)
        {
            Log.WriteLine(text, args);
        }

        public virtual bool IsWanted
        {
            get { return true; }
        }

        public virtual bool IsReady
        {
            get { return true; }
        }

        public virtual int Priority
        {
            get;
            private set;
        }

        public Brain Brain
        {
            get;
            private set;
        }
    }
}
