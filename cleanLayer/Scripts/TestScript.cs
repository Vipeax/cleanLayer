using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanLayer.Library.Scripts;

namespace cleanLayer.Scripts
{
    public class TestScript : Script
    {
        public TestScript()
            : base("Test", "Sample")
        {
            //Print("Constructor");
        }

        public override void OnStart()
        {
            Print("OnStart");
            StartThread(new Action(Thread1));
            StartThread(new Action(Thread2));
        }

        public override void OnTick()
        {
            Print("OnTick");
        }

        private void Thread1()
        {
            Print("Thread1 tick");
            StopThread();
        }

        private void Thread2()
        {
            Print("Thread2 tick");
            Sleep(500);
        }

        public override void OnTerminate()
        {
            Print("OnTerminate");
        }
    }
}
