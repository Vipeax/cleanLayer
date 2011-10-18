using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using cleanCore;

namespace cleanLayer.Library
{
    public static class ProfileHelper
    {
        public static HBProfile GetProfile(string path)
        {
            if (!File.Exists(path))
                return null;
            var profile = Helper.Deserialize<HBProfile>(path);
            return profile;
        }

        public static SubProfile GetAppropriateSubProfile(HBProfile profile)
        {
            SubProfile sub = null;

            var myLevel = Manager.LocalPlayer.Level;
            foreach (var s in profile.SubProfile)
            {
                if (s.MinLevel <= myLevel && s.MaxLevel >= myLevel)
                    sub = s;
            }

            return sub;
        }
    }
}
