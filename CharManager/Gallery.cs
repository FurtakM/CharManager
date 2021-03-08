using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace CharManager
{
    /*
     *  XitchedDLL.dll (delphi) by Stuart Carey
        function LoadGallery(Filename : AnsiString) : Pointer; cdecl; external DLL name 'LoadGallery';
        function GetPortraitCount(Gallery : Pointer) : Integer; cdecl; external DLL name 'GetPortraitCount';
        function GetPortrait(Gallery : Pointer; ID : Integer) : Pointer; cdecl; external DLL name 'GetPortrait';
        function GetPortraitRGB(Portrait : Pointer) : Pointer; cdecl; external DLL name 'GetPortraitRGB';
        procedure FreePortraitRGB(RGB : Pointer); cdecl; external DLL name 'FreePortraitRGB';
        procedure FreeGallery(Gallery : Pointer); cdecl; external DLL name 'FreeGallery';
    */

    class Gallery
    {
        private string galleryName;
        private int galleryAddr;

        [DllImport("kernel32.dll", EntryPoint = "LoadLibrary")]
        static extern int LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpLibFileName);

        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress")]
        static extern IntPtr GetProcAddress(int hModule, [MarshalAs(UnmanagedType.LPStr)] string lpProcName);

        [DllImport("kernel32.dll", EntryPoint = "FreeLibrary")]
        static extern bool FreeLibrary(int hModule);

        [DllImport("XichtDLL.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LoadGallery", CharSet = CharSet.Ansi)]
        internal static extern int LoadGallery(string Filename);

        [DllImport("XichtDLL.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetPortraitCount", CharSet = CharSet.Ansi)]
        internal static extern int GetPortraitCount(int Gallery);

        [DllImport("XichtDLL.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetPortrait", CharSet = CharSet.Ansi)]
        internal static extern int GetPortrait(int Gallery, int ID);

        [DllImport("XichtDLL.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetPortraitRGB", CharSet = CharSet.Ansi)]
        internal static extern int GetPortraitRGB(int Portrait);

        public Gallery(string name)
        {
            this.galleryAddr = LoadGallery(name);

            if (this.galleryAddr == 0)
                throw new Exception(string.Format("Could not load gallery \"{0}\".", name));

            this.galleryName = name;
        }

        public int GetAvatarCount()
        {
            return GetPortraitCount(this.galleryAddr);
        }

        public Bitmap GetAvatar(int id)
        {
            if (id <= 0)
                throw new Exception(string.Format("Invalid avatar ID: \"{0}\".", id));

            if (id > this.GetAvatarCount())
                throw new Exception(string.Format("Avatar ID out of range: \"{0}\".", id));

            int portraitAddr = GetPortrait(this.galleryAddr, id);

            if (portraitAddr == 0)
                throw new Exception(string.Format("Could not load avatar - ID: \"{0}\".", id));

            IntPtr portraitRGB = (IntPtr)GetPortraitRGB(portraitAddr);

            return SetPicture(portraitRGB);
        }
        public static bool DLLExist()
        {
            int libHandle = LoadLibrary("XichtDLL.dll");

            FreeLibrary(libHandle);

            if (libHandle == 0)
                return false;

            return true;
        }

        private static Bitmap SetPicture(IntPtr RGB)
        {
            return new Bitmap(80, 100, 160, PixelFormat.Format16bppRgb565, RGB);
        }

        public static List<string> ParseGalleries(string path)
        {
            List<string> list = new List<string>();

            if (!Directory.Exists(path))
                return list;

            DirectoryInfo galleryDir = new DirectoryInfo(@"" + path);
            FileInfo[] galleries = galleryDir.GetFiles("*.xgl");

            foreach (FileInfo file in galleries)
            {
                list.Add(Path.GetFileNameWithoutExtension(file.Name));               
            }

            return list;
        }
    }
}
