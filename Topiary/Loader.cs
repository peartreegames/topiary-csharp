using System;
using System.Runtime.InteropServices;

namespace Topiary
{
    internal interface ILoader
    {
        string DllPath { get; }
        IntPtr Load();
        void Free(IntPtr ptr);
        IntPtr GetProc(IntPtr ptr, string name);
    }

    public class WindowLoader : ILoader
    {
        public string DllPath => "topi.dll";

        [DllImport("Kernel32.dll")]
        private static extern IntPtr LoadLibrary(string path);

        [DllImport("Kernel32.dll")]
        private static extern void FreeLibrary(IntPtr ptr);

        [DllImport("Kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr ptr, string name);

        public IntPtr Load() => LoadLibrary(DllPath);
        public void Free(IntPtr ptr) => FreeLibrary(ptr);
        public IntPtr GetProc(IntPtr ptr, string name) => GetProcAddress(ptr, name);
    }

    public class MacLoader : ILoader
    {
        public string DllPath => "libtopi.dylib";
        private const int RTLDNow = 2;
        private const int RTLDLazy = 3;

        [DllImport("libdl.dylib")]
        private static extern IntPtr dlopen(string fileName, int flags);

        [DllImport("libdl.dylib")]
        private static extern IntPtr dlsym(IntPtr handle, string symbol);

        [DllImport("libdl.dylib")]
        private static extern bool dlclose(IntPtr handle);

        [DllImport("libdl.dylib")]
        private static extern IntPtr dlerror();

        public IntPtr Load() => dlopen(DllPath, RTLDNow);

        public void Free(IntPtr ptr) => dlclose(ptr);

        public IntPtr GetProc(IntPtr ptr, string name) => dlsym(ptr, name);
    }
}