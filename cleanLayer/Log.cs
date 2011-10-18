using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;

namespace cleanLayer
{
    public static class Log
    {
        private static LinkedList<ILog> LogReaders = new LinkedList<ILog>();
        private static LinkedList<string> LogContent = new LinkedList<string>();

        public static void WriteLine(string text, params object[] args)
        {
            var entry = string.Format("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss"), string.Format(text, args));
            LogContent.AddLast(entry);

            foreach (var LogReader in LogReaders)
                LogReader.WriteLine(entry);
        }

        public static void AddReader(ILog LogReader)
        {
            LogReaders.AddLast(LogReader);
            foreach (var LogLines in LogContent)
                LogReader.WriteLine(LogLines);
        }

        public static void RemoveReader(ILog LogReader)
        {
            LogReaders.Remove(LogReader);
        }
    }

    public interface ILog
    {
        void WriteLine(string line);
    }
}
