using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using cleanCore;

namespace cleanLayer.Library
{
    [XmlRoot("HBProfile", Namespace = "", IsNullable = false)]
    public class HBProfile
    {
        [XmlAttribute("creator")]
        public string Creator;
        [XmlAttribute("version")]
        public string Version;

        public string Name;
        public int MinLevel;
        public int MaxLevel;

        public float MinDurability;
        public int MinFreeBagSlots;

        public bool MailGrey;
        public bool MailWhite;
        public bool MailGreen;
        public bool MailBlue;
        public bool MailPurple;

        public bool SellGrey;
        public bool SellWhite;
        public bool SellGreen;
        public bool SellBlue;
        public bool SellPurple;

        public bool TargetElites;

        //[XmlArrayItem("SubProfile", typeof(SubProfile))]
        //public List<SubProfile> SubProfiles = new List<SubProfile>();
        [XmlElement("SubProfile")]
        public SubProfile[] SubProfile;
    }

    public class SubProfile
    {
        public string Name;
        public int MinLevel;
        public int MaxLevel;

        [XmlArray("Vendors"), XmlArrayItem("Vendor", typeof(Vendor))]
        public List<Vendor> Vendors = new List<Vendor>();

        [XmlArray("Mailboxes"), XmlArrayItem("Mailbox", typeof(Location))]
        public List<Location> Mailboxes = new List<Location>();

        [XmlElement("GrindArea")]
        public GrindArea[] GrindArea;

        //[XmlIgnore]
        //public List<GrindArea> GrindAreas { get { return _GrindArea.ToList(); } }

        [XmlArray("AvoidMobs"), XmlArrayItem("Mob", typeof(Mob))]
        public List<Mob> AvoidMobs = new List<Mob>();
    }

    public class Vendor
    {
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public int Entry;
        [XmlAttribute]
        public string Type;
        [XmlAttribute]
        public string TrainClass;
        [XmlAttribute]
        private float X;
        [XmlAttribute]
        private float Y;
        [XmlAttribute]
        private float Z;

        [XmlIgnore]
        public Location Location
        {
            get { return new Location(X, Y, Z); }
        }
    }

    public class GrindArea
    {
        public int TargetMinLevel;
        public int TargetMaxLevel;

        public string Factions;

        [XmlIgnore]
        public List<int> FactionList { get { return Factions.Split(' ').Select(i => int.Parse(i)).ToList(); } }

        [XmlArray("Hotspots"), XmlArrayItem("Hotspot", typeof(Hotspot))]
        public List<Hotspot> Hotspots = new List<Hotspot>();
    }

    public class Mob
    {
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public int Entry;
    }

    public class Hotspot
    {
        [XmlAttribute]
        public float X;
        [XmlAttribute]
        public float Y;
        [XmlAttribute]
        public float Z;

        [XmlIgnore]
        public Location Location { get { return new Location(X, Y, Z); } }
    }
}
