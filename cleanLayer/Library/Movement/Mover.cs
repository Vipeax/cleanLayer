using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using meshPather;
using RecastLayer;
using DetourLayer;
using cleanCore;

namespace cleanLayer.Library
{
    public enum MovementStatus { Moving, Stopped, Stuck, Error }
    public static class Mover
    {
        private static Pather _Pather;
        private static Queue<Location> _GeneratedPath = new Queue<Location>();

        public static Location Destination
        {
            get;
            private set;
        }

        public static MovementStatus Status
        {
            get;
            private set;
        }

        private static DateTime SleepTime
        {
            get;
            set;
        }

        public static bool IsCorpseRunning
        {
            get;
            private set;
        }

        public static void StopMoving()
        {
            Destination = Location.Zero;
            Status = MovementStatus.Stopped;
            if (_GeneratedPath != null)
                _GeneratedPath.Clear();
            Manager.LocalPlayer.StopCTM();
            Manager.LocalPlayer.StopMoving();
        }

        public static void MoveTo(Location to)
        {
            Manager.LocalPlayer.ClickToMove(to);
        }

        public static void MoveToCorpse()
        {
            if (!Manager.LocalPlayer.IsGhost)
                return;
            IsCorpseRunning = true;
            PathTo(Manager.LocalPlayer.Corpse);
        }

        public static bool PathTo(Location to)
        {
            return PathTo(Manager.LocalPlayer.Location, to);
        }

        private static AutoStuckHandler Unstuck;
        private static DateTime stuckCheckTimer;
        public static bool PathTo(Location from, Location to, bool preferRoads = true)
        {
            try
            {
                // Reinstate the pather with our current continent/dungeon
                _Pather = new Pather(WoWWorld.CurrentMap);
                if (_Pather == null)
                {
                    Log.WriteLine("Unable to instantiate the pather on map {0} (#{1})",
                                  WoWWorld.CurrentMap,
                                  WoWWorld.CurrentMapId);
                    return false;
                }

                if (preferRoads)
                {
                    // Use only water if necessary
                    _Pather.Filter.SetAreaCost((int) PolyArea.Water, 10);
                    // Roads and terrain keeps their priority
                    _Pather.Filter.SetAreaCost((int) PolyArea.Road, 1);
                    _Pather.Filter.SetAreaCost((int) PolyArea.Terrain, 1);

                    // Exclude flightmasters as they arent properly implemented (yet)
                    // Remember that they are implemented in the mesh, just not into the pather!
                    _Pather.Filter.ExcludeFlags = (int) PolyFlag.FlightMaster; // Eventually add (int)PolyFlag.Swim
                }

                // Convert our locations to XNA and request a path
                var hops = _Pather.FindPath(from.ToXNA(), to.ToXNA());

                if (hops == null)
                {
                    Log.WriteLine("Unable to generate path to {0}", to);
                    return false;
                }

                // Since we now know that we're ready to move, we can let the rest of the both know that we have a destination
                Destination = to;
                _lastLocation = from;

                stuckCheckTimer = DateTime.Now + TimeSpan.FromMilliseconds(5000);

                if (_GeneratedPath == null)
                    _GeneratedPath = new Queue<Location>();
                _GeneratedPath.Clear();
                foreach (var hop in hops)
                    _GeneratedPath.Enqueue(new Location(hop.Location.X, hop.Location.Y, hop.Location.Z));
            }
            catch (NavMeshException ex)
            {
                Log.WriteLine("Exception in NavMesh (Status: {0}):", ex.Status);
                Log.WriteLine(ex.Message);
                Exception inner;
                while ((inner = ex.InnerException) != null)
                {
                    Log.WriteLine(inner.Message);
                }

                Status = MovementStatus.Error;

                return false;
            }
            catch(Exception ex)
            {
                return false;
            }

            return true;
        }

        private static Location _lastLocation = Location.Zero;
        public static void Pulse()
        {
            if (_GeneratedPath == null)
                return;

            if (_GeneratedPath.Count == 0)
            {
                IsCorpseRunning = false;
                Destination = Location.Zero;
                _GeneratedPath = null;
                stuckCheckTimer = DateTime.MinValue;
                return;
            }

            if (SleepTime >= DateTime.Now)
                return;

            SleepTime = DateTime.Now + TimeSpan.FromMilliseconds(100);

            var nextLocation = _GeneratedPath.Peek();
            if (nextLocation != null && nextLocation != default(Location))
            {
                if (Unstuck == null)
                    Unstuck = new AutoStuckHandler(loc => Manager.LocalPlayer.Location.DistanceTo(nextLocation) <= 3f, nextLocation);

                MoveTo(nextLocation);

                if (Manager.LocalPlayer.Location.DistanceTo(nextLocation) < 3f)
                {
                    // We have a new location, dequeue
                    // and reset the unstucker
                    nextLocation = _GeneratedPath.Dequeue(); 
                    Unstuck = new AutoStuckHandler((loc) => Manager.LocalPlayer.Location.DistanceTo(nextLocation) <= 3f, nextLocation);
                }
            }

            if (Manager.LocalPlayer.Location.Distance2D(_lastLocation) >= .20) //.19???
            {
                Status = MovementStatus.Moving;
                _lastLocation = Manager.LocalPlayer.Location;
                stuckCheckTimer = DateTime.Now + TimeSpan.FromMilliseconds(1500);
                Manager.LocalPlayer.StopMoving(); // TODO: Implement MovementFlags (SetControlBits and that shit)
                return;
            }
            else
            {
                if (stuckCheckTimer <= DateTime.Now)
                {
                    Status = MovementStatus.Stuck;
                    if (Unstuck != null && Unstuck.Remaining > 0)
                    {
                        Log.WriteLine("Trying to unstuck (#{0}).", ((Unstuck.Total - Unstuck.Remaining)+1));
                        StopMoving();
                        Unstuck.Next();
                        stuckCheckTimer = DateTime.Now + TimeSpan.FromMilliseconds(1500);
                    }
                    else
                    {
                        Manager.LocalPlayer.StopCTM();
                        StopMoving();
                        Log.WriteLine("Unable to unstuck, stopping all movement!");
                    }
                }
            }
            _lastLocation = Manager.LocalPlayer.Location;
        }
    }
}
