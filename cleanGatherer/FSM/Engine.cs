// Taken from Apoc's FSM and other contributors (heavily modified)
// http://www.mmowned.com/forums/general/programming/232703-bot-developers-simple-but-effective-fsm-your-bots.html
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using cleanGatherer.FSM.States;
using cleanCore;

namespace cleanGatherer.FSM
{
    public static class Engine
    {
        public static void Initialize(CircularQueue<Location> waypoints, int timeBetweenPulses = 333)
        {
            TimeBetweenPulses = timeBetweenPulses; // Defaults to 333 = 3 pulses every second
            Waypoints = waypoints;

            States = new List<State>(); // Instantiate a new list of states
            LoadInternalStates(); // Load the States from the internal assembly (subclasses of State)
            States.Sort(); // Sort the list by priority
        }

        private static void LoadInternalStates()
        {
            Assembly asm = Assembly.GetExecutingAssembly(); // Internal assembly
            Type[] types = asm.GetTypes(); // All types
            foreach (Type type in types)
            {
                if (type.IsClass && type.IsSubclassOf(typeof(State)))
                { // Only pick the ones that are inherited from State
                    var tempState = (State)Activator.CreateInstance(type); // Instantiate the class
                    if (!States.Contains(tempState))
                    { // Add if we don't already have it!
                        States.Add(tempState);
                    }
                }
            }
        }

        public static void Start()
        {
            IsRunning = true; // Start the FSM
            LastState = null;
        }

        public static void Stop()
        {
            IsRunning = false; // Stop the FSM
        }

        public static bool HasWaypoints { get { return Waypoints != null; } } // Indicates whether waypoints are loaded or not
        public static CircularQueue<Location> Waypoints { get; private set; } // Collection of waypoints
        public static bool IsRunning { get; private set; }
        public static int TimeBetweenPulses { get; private set; } // How long to wait between each pulse (milliseconds)

        private static State LastState;
        private static DateTime LastPulse = DateTime.Now;
        private static List<State> States { get; set; }

        public static void DelayNextPulse(int milliseconds)
        {
            LastPulse = ((DateTime.Now + TimeSpan.FromMilliseconds(milliseconds)) - TimeSpan.FromMilliseconds(TimeBetweenPulses));
        }

        public static bool Pulse()
        {
            if (!IsRunning)
                return false; // We shouldn't pulse our FSM if we aren't supposed to be running ;)

            if (!HasWaypoints)
                return false; // We can't run unless we have waypoints!

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
                        Log.WriteLine("Switching to state: {0}", state.GetType().Name);
                    }
                    return true; // Break the loop so we don't run more than 1 state at a time
                }
            }

            return false; // It's a boolean returntype, heh
        }
    }
}
