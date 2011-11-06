// Taken from Apoc's FSM and other contributors (heavily modified)
// http://www.mmowned.com/forums/general/programming/232703-bot-developers-simple-but-effective-fsm-your-bots.html
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using cleanCore;

namespace cleanLayer.Library.FSM
{
    public class Engine
    {
        public Engine(int timeBetweenPulses = 333)
        {
            TimeBetweenPulses = timeBetweenPulses; // Defaults to 333 = 3 pulses every second
            States = new List<State>(); // Instantiate a new list of states
        }

        //public void LoadInternalStates(Type sType)
        //{
        //    Assembly asm = Assembly.GetExecutingAssembly(); // Internal assembly
        //    Type[] types = asm.GetTypes(); // All types
        //    foreach (Type type in types)
        //    {
        //        if (type.IsClass && type.IsSubclassOf(sType))
        //        { // Only pick the ones that are inherited from State
        //            var tempState = Activator.CreateInstance(type) as sType; // Instantiate the class
        //            if (!States.Contains(tempState))
        //            { // Add if we don't already have it!
        //                States.Add(tempState);
        //            }
        //        }
        //    }
        //}

        public void LoadStates(List<State> states)
        {
            States = states;
            States.Sort();
        }

        public void Start()
        {
            IsRunning = true; // Start the FSM
            LastState = null;
        }

        public void Stop()
        {
            IsRunning = false; // Stop the FSM
        }

        public bool IsRunning { get; private set; }
        public int TimeBetweenPulses { get; private set; } // How long to wait between each pulse (milliseconds)

        private State LastState;
        private DateTime LastPulse = DateTime.MinValue;
        private List<State> States { get; set; }

        public string StateText = string.Empty;

        public void DelayNextPulse(int milliseconds)
        {
            LastPulse = ((DateTime.Now + TimeSpan.FromMilliseconds(milliseconds)) - TimeSpan.FromMilliseconds(TimeBetweenPulses));
        }

        public bool Pulse()
        {
            if (!IsRunning)
                return false; // We shouldn't pulse our FSM if we aren't supposed to be running ;)

            if ((LastPulse + TimeSpan.FromMilliseconds(TimeBetweenPulses)) > DateTime.Now)
                return false; // We haven't waited long enough since the last pulse!
            LastPulse = DateTime.Now;

            States.Sort(); // Sort the states by priority
            foreach (State state in States)
            {
                if (state.NeedToRun)
                { // Find the first state that needs to run ...
                    state.Run(); // ... and run it
                    if (LastState != state)
                    {
                        LastState = state;
                        StateText = state.Description;
                        //Log.WriteLine("Switching to state: {0}", state.GetType().Name);
                    }
                    return true; // Break the loop so we don't run more than 1 state at a time
                }
            }

            return false; // It's a boolean returntype, heh
        }
    }
}
