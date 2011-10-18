using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using cleanCore;

namespace cleanGatherer
{
    public static class Settings
    {
        private static IniFile File;
        private static Dictionary<string, string> Defaults;

        public static void Load()
        {
            var name = (Manager.IsInGame ? Manager.LocalPlayer.Name : "Unknown");
            var path = Assembly.GetExecutingAssembly().Location;
            path = path.Substring(0, path.LastIndexOf('\\') + 1);
            var filename = string.Format("{1}{0}.Settings.ini", path, name);
            Load(filename);
        }

        public static void Load(string path)
        {
            Defaults = new Dictionary<string, string>()
                {
                    {"Mines", "True"},
                    {"Herbs", "True"},
                    {"Mount", ""}
                };
            File = new IniFile(path);
        }

        public static void Set(string key, string value)
        {
            File.Write("Settings", key, value);
        }

        public static string Get(string key)
        {
            var ret = File.Read("Settings", key);
            if (string.IsNullOrEmpty(ret))
                ret = (Defaults.ContainsKey(key) ? Defaults[key] : string.Empty);
            return ret;
        }

        public static T Get<T>(string key)
        {
            var value = Get(key);
            object ret;
            switch (Type.GetTypeCode((typeof(T))))
            {
                case TypeCode.Int32:
                    ret = int.Parse(value);
                    break;
                case TypeCode.Boolean:
                    ret = bool.Parse(value);
                    break;
                case TypeCode.String:
                default:
                    ret = value;
                    break;
            }
            return (T)ret;
        }
    }
}
