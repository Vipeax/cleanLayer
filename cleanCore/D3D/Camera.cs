using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using SlimDX;

namespace cleanCore.D3D
{
    public struct CameraInfo
    {
        uint unk0;
        uint unk1;
        public Vector3 Position;
        public Matrix3 Matrix;
        public float FieldOfView;
        float unk2;
        int unk3;
        public float NearZ;
        public float FarZ;
        public float Aspect;
    }

    public static unsafe class Camera
    {
        public static void Initialize()
        {
            _GetFov = Helper.Magic.RegisterDelegate<GetFovDelegate>(Helper.Magic.GetObjectVtableFunction(Pointer, 0));
            _Forward = Helper.Magic.RegisterDelegate<ForwardDelegate>(Helper.Magic.GetObjectVtableFunction(Pointer, 1));
            _Right = Helper.Magic.RegisterDelegate<RightDelegate>(Helper.Magic.GetObjectVtableFunction(Pointer, 2));
            _Up = Helper.Magic.RegisterDelegate<UpDelegate>(Helper.Magic.GetObjectVtableFunction(Pointer, 3));
        }

        private static IntPtr Pointer
        {
            get { return Helper.Magic.Read<IntPtr>(Helper.Magic.Read<uint>(Offsets.WorldFrame) + Offsets.ActiveCamera); }
        }

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate float GetFovDelegate(IntPtr ptr);
        private static GetFovDelegate _GetFov;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate Vector3* ForwardDelegate(IntPtr ptr, Vector3* vecOut);
        private static ForwardDelegate _Forward;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate Vector3* RightDelegate(IntPtr ptr, Vector3* vecOut);
        private static RightDelegate _Right;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate Vector3* UpDelegate(IntPtr ptr, Vector3* vecOut);
        private static UpDelegate _Up;

        public static float FieldOfView
        {
            get
            {
                return _GetFov(Pointer);
            }
        }

        public static Vector3 Forward
        {
            get
            {
                var res = new Vector3();
                _Forward(Pointer, &res);
                return res;
            }
        }

        public static Vector3 Right
        {
            get
            {
                var res = new Vector3();
                _Right(Pointer, &res);
                return res;
            }
        }

        public static Vector3 Up
        {
            get
            {
                var res = new Vector3();
                _Up(Pointer, &res);
                return res;
            }
        }

        public static Matrix Projection
        {
            get
            {
                var cam = GetCamera();
                return Matrix.PerspectiveFovRH(FieldOfView * 0.6f, Aspect, cam.NearZ, cam.FarZ);
            }
        }

        public static Matrix View
        {
            get
            {
                var cam = GetCamera();
                var eye = cam.Position;
                var at = eye + Camera.Forward;
                return Matrix.LookAtRH(eye, at, new Vector3(0, 0, 1));
            }
        }

        public static float Aspect
        {
            get { return Helper.Magic.Read<float>(Offsets.AspectRatio); }
        }

        public static CameraInfo GetCamera()
        {
            return Helper.Magic.ReadStruct<CameraInfo>(new IntPtr(Offsets.ActiveCamera));
        }
    }
}
