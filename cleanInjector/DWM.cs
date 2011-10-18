/*
Copyright 2009 scorpion

This file is part of N2.

N2 is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

N2 is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with N2.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Runtime.InteropServices;

namespace cleanInjector
{
    public static class DWM
    {
        [DllImport("user32.dll", EntryPoint = "GetWindowPlacement")]
        private static extern bool InternalGetWindowPlacement(IntPtr hWnd, ref WindowPlacement lpwndpl);
        public static bool GetWindowPlacement(IntPtr hWnd, out WindowPlacement placement)
        {
            placement = new WindowPlacement();
            placement.Length = Marshal.SizeOf(typeof(WindowPlacement));
            return InternalGetWindowPlacement(hWnd, ref placement);
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, GetWindowCmd uCmd);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string classname, string title);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

        [DllImport("dwmapi.dll")]
        public static extern int DwmRegisterThumbnail(IntPtr dest, IntPtr source, out IntPtr hthumbnail);

        [DllImport("dwmapi.dll")]
        public static extern int DwmUnregisterThumbnail(IntPtr HThumbnail);

        [DllImport("dwmapi.dll")]
        public static extern int DwmUpdateThumbnailProperties(IntPtr HThumbnail, ref ThumbnailProperties props);

        [DllImport("dwmapi.dll")]
        public static extern int DwmQueryThumbnailSourceSize(IntPtr HThumbnail, out Size size);

        public struct Point
        {
            public int x;
            public int y;
        }

        public struct Size
        {
            public int Width, Height;
        }

        public struct WindowPlacement
        {
            public int Length;
            public int Flags;
            public int ShowCmd;
            public Point MinPosition;
            public Point MaxPosition;
            public Rect NormalPosition;
        }

        public struct ThumbnailProperties
        {
            public ThumbnailFlags Flags;
            public Rect Destination;
            public Rect Source;
            public Byte Opacity;
            public bool Visible;
            public bool SourceClientAreaOnly;
        }

        public struct Rect
        {
            public Rect(int x, int y, int x1, int y1)
            {
                this.Left = x;
                this.Top = y;
                this.Right = x1;
                this.Bottom = y1;
            }

            public int Left, Top, Right, Bottom;
        }

        [Flags]
        public enum ThumbnailFlags : int
        {
            RectDetination = 1,
            RectSource = 2,
            Opacity = 4,
            Visible = 8,
            SourceClientAreaOnly = 16
        }

        public enum GetWindowCmd : uint
        {
            First = 0,
            Last = 1,
            Next = 2,
            Prev = 3,
            Owner = 4,
            Child = 5,
            EnabledPopup = 6
        }
    }
}
