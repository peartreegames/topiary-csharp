using System;
using System.Runtime.InteropServices;

namespace Topiary
{
    public interface ILoader
    {
        string DefaultDllPath { get; }
        IntPtr Load(string dllPath);
        void Free(IntPtr ptr);
        IntPtr GetProc(IntPtr ptr, string name);
    }

    public class WindowsLoader : ILoader
    {
        public string DefaultDllPath => "topi.dll";

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string path);

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern void FreeLibrary(IntPtr ptr);

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr ptr, string name);

        public IntPtr Load(string dllPath)
        {
            var ptr = LoadLibrary(dllPath);
            if (ptr != IntPtr.Zero) return ptr;
            var errPtr = Marshal.GetLastWin32Error();
            throw new System.ComponentModel.Win32Exception(errPtr);
        }

        public void Free(IntPtr ptr) => FreeLibrary(ptr);
        public IntPtr GetProc(IntPtr libPtr, string name)
        {
            var ptr = GetProcAddress(libPtr, name);
            if (ptr != IntPtr.Zero) return ptr;
            var errPtr = Marshal.GetLastWin32Error();
            throw new System.ComponentModel.Win32Exception(errPtr);
        }
    }

    public class MacLoader : ILoader
    {
        public string DefaultDllPath => "libtopi.dylib";
        private const int RtldNow = 2;

        [DllImport("libdl.dylib")]
        private static extern IntPtr dlopen(string fileName, int flags);

        [DllImport("libdl.dylib")]
        private static extern IntPtr dlsym(IntPtr handle, string symbol);

        [DllImport("libdl.dylib")]
        private static extern bool dlclose(IntPtr handle);

        [DllImport("libdl.dylib")]
        private static extern IntPtr dlerror();

        public IntPtr Load(string dllPath)
        {
            var ptr = dlopen(dllPath, RtldNow);
            if (ptr != IntPtr.Zero) return ptr;
            var errPtr = dlerror();
            throw new System.ComponentModel.Win32Exception(Marshal.PtrToStringAnsi(errPtr));
        }

        public void Free(IntPtr ptr) => dlclose(ptr);

        public IntPtr GetProc(IntPtr ptr, string name) => dlsym(ptr, name);
    }
}