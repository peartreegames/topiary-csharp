using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace PeartreeGames.Topiary
{
    public interface ILoader
    {
        SafeHandle Load();
        bool Free(IntPtr ptr);
        IntPtr GetProc(string name);
    }

    public static class EmbeddedLoader
    {
        public static string CreateEmbeddedResource(string dllName)
        {
            var asm = Assembly.GetExecutingAssembly();
            var resName = "PeartreeGames.Topiary." + dllName;

            using var stream = asm.GetManifestResourceStream(resName);
            if (stream == null) throw new DllNotFoundException($"Could not find {resName}");
            var bytes = new byte[stream.Length];
            _ = stream.Read(bytes, 0, bytes.Length);

            var tempFilePath = Path.GetTempFileName();
            File.WriteAllBytes(tempFilePath, bytes);
            return tempFilePath;
        }
    }

    public class WindowsLoader : SafeHandleZeroOrMinusOneIsInvalid, ILoader
    {
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary(string path);

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr ptr);

        [DllImport("kernel32", SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr ptr, string name);

        public SafeHandle Load()
        {
            var ptr = LoadLibrary(EmbeddedLoader.CreateEmbeddedResource("topi.dll"));
            handle = ptr;
            if (ptr != IntPtr.Zero) return this;
            var errPtr = Marshal.GetLastWin32Error();
            throw new System.ComponentModel.Win32Exception(errPtr);
        }

        public bool Free(IntPtr ptr) => FreeLibrary(ptr);

        public IntPtr GetProc(string name)
        {
            var ptr = GetProcAddress(handle, name);
            if (ptr != IntPtr.Zero) return ptr;
            var errPtr = Marshal.GetLastWin32Error();
            throw new System.ComponentModel.Win32Exception(errPtr);
        }

        public WindowsLoader(bool ownsHandle) : base(ownsHandle)
        {
        }

        protected override bool ReleaseHandle() => Free(handle);
    }

    public class MacLoader : SafeHandleZeroOrMinusOneIsInvalid, ILoader
    {
        private const int RtldNow = 2;

        [DllImport("libdl.dylib")]
        private static extern IntPtr dlopen(string fileName, int flags);

        [DllImport("libdl.dylib")]
        private static extern IntPtr dlsym(IntPtr handle, string symbol);

        [DllImport("libdl.dylib")]
        private static extern bool dlclose(IntPtr handle);

        [DllImport("libdl.dylib")]
        private static extern IntPtr dlerror();

        public SafeHandle Load()
        {
            var ptr = dlopen(EmbeddedLoader.CreateEmbeddedResource("libtopi.dylib"), RtldNow);
            handle = ptr;
            if (ptr != IntPtr.Zero) return this;
            var errPtr = dlerror();
            throw new System.ComponentModel.Win32Exception(Library.PtrToUtf8String(errPtr));
        }

        public bool Free(IntPtr ptr) => dlclose(ptr);

        public IntPtr GetProc(string name) => dlsym(handle, name);

        public MacLoader(bool ownsHandle) : base(ownsHandle)
        {
        }

        protected override bool ReleaseHandle() => Free(handle);
    }
}