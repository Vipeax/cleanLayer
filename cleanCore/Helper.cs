using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using WhiteMagic;

namespace cleanCore
{
    public static class Helper
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint PerformanceCounterDelegate();
        private static PerformanceCounterDelegate _performanceCounter;

        public static Magic Magic = new Magic();
        public static bool InCombat { get; private set; }

        public static void Initialize()
        {
            _performanceCounter = Magic.RegisterDelegate<PerformanceCounterDelegate>(Offsets.PerformanceCounter);

            Events.Register("PLAYER_REGEN_DISABLED", SetInCombat);
            Events.Register("PLAYER_REGEN_ENABLED", UnsetInCombat);
        }

        private static void SetInCombat(string ev, List<string> args)
        {
            InCombat = true;
        }

        private static void UnsetInCombat(string ev, List<string> args)
        {
            InCombat = false;
        }

        public static void ResetHardwareAction()
        {
            Magic.Write(Offsets.LastHardwareAction, PerformanceCount);
        }

        public static uint PerformanceCount
        {
            get
            {
                return _performanceCounter();
            }
        }

        public static uint Rebase(uint offset)
        {
            return ((uint)Process.GetCurrentProcess().MainModule.BaseAddress + offset);
        }

        public static IntPtr Rebase(IntPtr offset)
        {
            return (IntPtr)((uint)Process.GetCurrentProcess().MainModule.BaseAddress + (uint)offset);
        }

        public static T Deserialize<T>(string path)
        {
            T obj;
            var fs = new FileStream(path, FileMode.Open);
            var serializer = new XmlSerializer(typeof(T));
            obj = (T)serializer.Deserialize(fs);
            return obj;
        }
    }
}