using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StuckList = System.Collections.Generic.List<System.Action<cleanCore.Location>>;
using cleanCore;

namespace cleanLayer.Library
{
    public class AutoStuckHandler
    {
        private static StuckList
            FlyingUnstucks,
            GroundUnstucks;

        private StuckHandler
            GroundUnstucker,
            FlyingUnstucker;

        private StuckHandler CurrentUnstucker { get { return GroundUnstucker; } } // TODO: Add mount check for flying/ground

        public AutoStuckHandler(Func<Location, bool> Done, Location Target)
        {
            FlyingUnstucks = FlyingUnstucks ?? new StuckList
            {
                (dest) => { Manager.LocalPlayer.Ascend(); /*Thread.Sleep(1500);*/ },
                (dest) => { Manager.LocalPlayer.Descend(); /*Thread.Sleep(1500);*/ },
                (dest) => { Manager.LocalPlayer.StrafeLeft(); /*Thread.Sleep(1500);*/ },
                (dest) => { Manager.LocalPlayer.StrafeRight(); /*Thread.Sleep(1500);*/ },
            };

            GroundUnstucks = GroundUnstucks ?? new StuckList {
                (dest) =>
                    {
                        Manager.LocalPlayer.MoveForward();
                        //Thread.Sleep(500);
                        Manager.LocalPlayer.Jump();
                        //Thread.Sleep(100);
                        Manager.LocalPlayer.Jump();
                        //Thread.Sleep(100);
                        Manager.LocalPlayer.Jump();
                        //Thread.Sleep(250);
                    },
    
                (dest) => 
                    {
                        Manager.LocalPlayer.Descend();
                        Manager.LocalPlayer.Dismount();
                        Manager.LocalPlayer.MoveBackward();
                        //Thread.Sleep(250);
                    },

                (dest) =>
                    {
                        int i = new Random().Next(0,1);
                        if (i==0)
                        {
                            Manager.LocalPlayer.MoveForward();
                            Manager.LocalPlayer.StrafeLeft();
                        }
                        else
                        {
                            Manager.LocalPlayer.MoveForward();
                            Manager.LocalPlayer.StrafeRight();
                        }
                        //Thread.Sleep(500);
                        Manager.LocalPlayer.MoveBackward();
                        //Thread.Sleep(500);
                        Manager.LocalPlayer.Jump();
                        //Thread.Sleep(250);
                    },

                (dest) =>
                    {
                        Manager.LocalPlayer.MoveBackward();
                        Manager.LocalPlayer.Jump();
                        //Thread.Sleep(500);
                        Manager.LocalPlayer.Jump();
                        Manager.LocalPlayer.StrafeLeft();
                        Manager.LocalPlayer.Jump();
                        //Thread.Sleep(500);
                        Manager.LocalPlayer.StopMoving();
                        Manager.LocalPlayer.MoveBackward();
                        Manager.LocalPlayer.Jump();
                        Manager.LocalPlayer.StrafeRight();
                        //Thread.Sleep(500);
                    },
            };

            GroundUnstucker = new StuckHandler(GroundUnstucks, Done, Target);
            FlyingUnstucker = new StuckHandler(FlyingUnstucks, Done, Target);
        }

        public bool Next()
        {
            return CurrentUnstucker.Next();
        }

        public int Remaining
        {
            get { return CurrentUnstucker.Remaining; }
        }

        public int Total
        {
            get { return CurrentUnstucker.Total; }
        }
    }
}
