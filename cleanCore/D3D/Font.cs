using System.Diagnostics;
using System.Drawing;
using SlimDX;
using SlimDX.Direct3D9;

namespace cleanCore.D3D
{    
    public class Font : IResource
    {
        private SlimDX.Direct3D9.Font _font;

        public Font(string font, int size)
        {
            //_font = new SlimDX.Direct3D9.Font(Rendering.Device, height, width, FontWeight.Normal, 1, false,
            //                                  CharacterSet.Default, Precision.Default, FontQuality.Antialiased,
            //                                  PitchAndFamily.Default, font);

            _font = new SlimDX.Direct3D9.Font(Rendering.Device, new System.Drawing.Font(font, size));

            Rendering.RegisterResource(this);
        }

        public void OnLostDevice()
        {
            if (_font.OnLostDevice() != ResultCode.Success)
                Debugger.Break();
        }

        public void OnResetDevice()
        {
            if (_font.OnResetDevice() != ResultCode.Success)
                Debugger.Break();
        }

        public void Release()
        {
            _font.Dispose();
            _font = null;
        }

        public void Print(int x, int y, string text, Color color)
        {
            if (_font != null)
                _font.DrawString(null, text, x, y, color);
        }

        public void Draw()
        {
            
        }
    }
}