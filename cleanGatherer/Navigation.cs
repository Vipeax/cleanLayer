// Taken from Apoc's waypoint system, slightly modified
using System.IO;
using System.Linq;
using System.Xml.Linq;
using cleanCore;

namespace cleanGatherer
{
    public class Navigation
    {
        private CircularQueue<Location> _waypoints;

        public Navigation(CircularQueue<Location> waypoints)
        {
            _waypoints = waypoints;
        }

        public void NavigateWaypoints()
        {
            var dest = _waypoints.Peek();

            if (dest.DistanceTo(Manager.LocalPlayer.Location) < 10)
            {
                _waypoints.Dequeue();
                return;
            }

            Manager.LocalPlayer.ClickToMove(dest);
        }

        public void SetDestinationToNearest()
        {
            var pos = Manager.LocalPlayer.Location;
            var target = (from p in _waypoints
                          orderby p.DistanceTo(pos) ascending
                          select p).FirstOrDefault();
            _waypoints.CycleTo(target);
        }

        public static CircularQueue<Location> LoadFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Could not find the specified file!", filePath);

            var ret = new CircularQueue<Location>();
            XElement file = XElement.Load(filePath);
            var points = from p in file.Descendants("Location")
                         select p;
            foreach (XElement point in points)
                ret.Enqueue(new Location(point));

            return ret;
        }
    }
}
