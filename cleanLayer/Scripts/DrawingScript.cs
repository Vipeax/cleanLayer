using System.Drawing;
using System.Linq;
using cleanCore;
using cleanCore.D3D;
using cleanLayer.Library.Scripts;

namespace cleanLayer.Scripts
{
    public class DrawingScript : Script
    {
        public DrawingScript()
            : base("Drawing", "Test")
        { }

        private cleanCore.D3D.Font _font;

        public override void OnStart()
        {
            _font = new cleanCore.D3D.Font("Consolas", 9);
        }

        public override void OnTick()
        {
            //var target = Manager.LocalPlayer.Target as WoWUnit ?? WoWUnit.Invalid;
            //if (target.IsValid)
            //{
            //    var color = Color.Blue;
            //    if (target.IsDead)
            //        color = Color.Red;
            //    else if (target.InLoS)
            //        color = Color.Green;

            //    _font.Print(10, 200, target.Name, Color.Red);
            //    Rendering.DrawCircle(target.Location, 2f, Color.FromArgb(0xAF, Color.ForestGreen), Color.FromArgb(0xAF, Color.ForestGreen));
            //    Rendering.DrawLine(Manager.LocalPlayer.Location, target.Location, color);
            //}
            var players = from p in Manager.Objects where p.IsValid && p.IsPlayer select p as WoWPlayer;
            foreach (var ply in players)
            {
                var color = Color.Blue;
                if (ply.IsDead)
                    color = Color.Red;
                else if (ply.InLoS)
                    color = Color.Green;

                Rendering.DrawLine(Manager.LocalPlayer.Location, ply.Location, color);
                Rendering.DrawCircle(ply.Location, 2f, Color.FromArgb(0xAF, Color.LightYellow), Color.FromArgb(0xAF, Color.LightYellow));
            }
        }

        public override void OnTerminate()
        {
            _font.Release();
        }
    }
}
