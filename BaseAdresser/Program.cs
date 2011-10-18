using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BaseAdresser
{
    class Program
    {
        static void Main(string[] args)
        {
            var proc = Process.GetProcessesByName("WoW");
            foreach (var p in proc)
            {
                Console.WriteLine("{0} -> 0x{1}", p.Id, p.MainModule.BaseAddress.ToString("X"));
            }

            string offset = string.Empty;
            while ((offset = Console.ReadLine()) != null)
            {
                var off = uint.Parse(offset);
                Console.WriteLine("{0} -> 0x{1:X}", offset, ((uint)proc[0].MainModule.BaseAddress + off).ToString("X"));
            }

            Console.ReadKey();
        }
    }
}
