using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;
using SlimDX;
using SlimDX.Direct3D9;

namespace cleanCore.D3D
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PositionColored
    {
        public static readonly VertexFormat FVF = VertexFormat.Position | VertexFormat.Diffuse;
        public static readonly int Stride = Vector3.SizeInBytes + sizeof(int);

        public Vector3 Position;
        public int Color;

        public PositionColored(Vector3 pos, int col)
        {
            Position = pos;
            Color = col;
        }
    }

    public static class Rendering
    {
        private static readonly List<IResource> _resources = new List<IResource>();
        private static IntPtr _usedDevicePointer = IntPtr.Zero;

        public static Device Device { get; private set; }

        public static void Initialize(IntPtr devicePointer)
        {
            if (_usedDevicePointer != devicePointer)
            {
                Debug.WriteLine("Rendering: Device initialized on " + devicePointer);
                Device = Device.FromPointer(devicePointer);
                _usedDevicePointer = devicePointer;
            }

            Camera.Initialize();
        }

        public static void RegisterResource(IResource source)
        {
            _resources.Add(source);
        }

        private static void InternalRender(Vector3 target)
        {
            if (!Rendering.IsInitialized)
                return;

            Device.SetTransform(TransformState.World, Matrix.Translation(target));
            Device.SetTransform(TransformState.View, Camera.View);
            Device.SetTransform(TransformState.Projection, Camera.Projection);

            Device.VertexShader = null;
            Device.PixelShader = null;
            Device.SetRenderState(RenderState.AlphaBlendEnable, true);
            Device.SetRenderState(RenderState.BlendOperation, BlendOperation.Add);
            Device.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);
            Device.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
            Device.SetRenderState(RenderState.Lighting, 0);
            Device.SetTexture(0, null);
            Device.SetRenderState(RenderState.CullMode, Cull.None);
        }

        public static void DrawLine(Location from, Location to, Color color)
        {
            var vertices = new PositionColored[2];
            vertices[0] = new PositionColored(Vector3.Zero, color.ToArgb());
            vertices[1] = new PositionColored(to.ToVector3() - from.ToVector3(), color.ToArgb());

            InternalRender(from.ToVector3());

            Device.DrawUserPrimitives(PrimitiveType.LineList, vertices.Length / 2, vertices);
        }

        public static unsafe void DrawCircle(Location center, float radius, Color innerColor, Color outerColor, int complexity = 24, bool isFilled = true)
        {
            var vertices = new List<PositionColored>();

            if (isFilled)
                vertices.Add(new PositionColored(Vector3.Zero, innerColor.ToArgb()));

            double stepAngle = (Math.PI * 2) / complexity;
            for (int i = 0; i <= complexity; i++)
            {
                double angle = (Math.PI * 2) - (i * stepAngle);
                float x = (float)(radius * Math.Cos(angle));
                float y = (float)(-radius * Math.Sin(angle));
                vertices.Add(new PositionColored(new Vector3(x, y, 0), outerColor.ToArgb()));
            }

            var buffer = vertices.ToArray();

            InternalRender(center.ToVector3() + new Vector3(0, 0, 0.3f));

            if (isFilled)
                Device.DrawUserPrimitives(PrimitiveType.TriangleFan, buffer.Length - 2, buffer);
            else
                Device.DrawUserPrimitives(PrimitiveType.LineStrip, buffer.Length - 1, buffer);
        }

        public static void OnLostDevice()
        {            
            foreach (var resource in _resources)
                resource.OnLostDevice();
        }

        public static void OnResetDevice()
        {
            foreach (var resource in _resources)
                resource.OnResetDevice();
        }

        public static void Pulse()
        {
            if (!IsInitialized)
                return;
        }

        public static bool IsInitialized
        {
            get { return Device != null; }
        }
    }
}