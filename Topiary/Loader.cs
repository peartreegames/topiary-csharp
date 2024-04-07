using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace PeartreeGames.Topiary
{
    /// <summary>
    /// Represents a loader interface for loading and interacting with libraries.
    /// </summary>
    public interface ILoader
    {
        /// <summary>
        /// Loads the library.
        /// </summary>
        /// <returns>A <see cref="SafeHandle"/> representing the loaded library.</returns>
        /// <exception cref="System.ComponentModel.Win32Exception">Thrown if the library failed to load.</exception>
        SafeHandle Load();

        /// <summary>
        /// Frees the specified library handle.
        /// </summary>
        /// <param name="ptr">The pointer to the library handle.</param>
        /// <returns>
        /// <c>true</c> if the library handle is successfully freed; otherwise, <c>false</c>.
        /// </returns>
        bool Free(IntPtr ptr);

        /// <summary>
        /// Retrieves the address of the specified function from the loaded library.
        /// </summary>
        /// <param name="name">The name of the function to retrieve.</param>
        /// <returns>
        /// The address of the specified function if the function is found, or IntPtr.Zero if the function is not found.
        /// </returns>
        IntPtr GetProc(string name);
    }

    /// <summary>
    /// Represents a loader interface for loading and interacting with libraries.
    /// </summary>
    public static class EmbeddedLoader
    {
        /// <summary>
        /// Creates an embedded resource from the specified DLL name.
        /// </summary>
        /// <param name="dllName">The name of the DLL.</param>
        /// <returns>The temporary file path where the embedded resource is created.</returns>
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

    /// <summary>
    /// Represents a loader interface for loading and interacting with libraries.
    /// </summary>
    public class WindowsLoader : SafeHandleZeroOrMinusOneIsInvalid, ILoader
    {
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary(string path);

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr ptr);

        [DllImport("kernel32", SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr ptr, string name);

        /// <summary>
        /// Loads the library.
        /// </summary>
        /// <returns>The safe handle of the loaded library.</returns>
        /// <exception cref="System.ComponentModel.Win32Exception">Thrown when the library failed to load.</exception>
        public SafeHandle Load()
        {
            var ptr = LoadLibrary(EmbeddedLoader.CreateEmbeddedResource("topi.dll"));
            handle = ptr;
            if (ptr != IntPtr.Zero) return this;
            var errPtr = Marshal.GetLastWin32Error();
            throw new System.ComponentModel.Win32Exception(errPtr);
        }

        /// <summary>
        /// Frees the specified library handle.
        /// </summary>
        /// <param name="ptr">The pointer to the library handle.</param>
        /// <returns>
        /// <c>true</c> if the library handle is successfully freed; otherwise, <c>false</c>.
        /// </returns>
        /// <example>
        /// <code>
        /// IntPtr libraryHandle = LoadLibrary("example.dll");
        /// bool result = Free(libraryHandle);
        /// </code>
        /// </example>
        public bool Free(IntPtr ptr) => FreeLibrary(ptr);

        /// <summary>
        /// Retrieves the address of the specified function from the loaded library.
        /// </summary>
        /// <param name="name">The name of the function to retrieve.</param>
        /// <returns>
        /// The address of the specified function if the function is found, or IntPtr.Zero if the function is not found.
        /// </returns>
        public IntPtr GetProc(string name)
        {
            var ptr = GetProcAddress(handle, name);
            if (ptr != IntPtr.Zero) return ptr;
            var errPtr = Marshal.GetLastWin32Error();
            throw new System.ComponentModel.Win32Exception(errPtr);
        }

        /// <summary>
        /// Represents a loader interface for loading and interacting with libraries.
        /// </summary>
        public WindowsLoader(bool ownsHandle) : base(ownsHandle)
        {
        }

        /// <summary>
        /// Releases the handle of the library.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the handle is successfully released; otherwise, <c>false</c>.
        /// </returns>
        protected override bool ReleaseHandle() => Free(handle);
    }

    /// <summary>
    /// Represents a loader interface for loading and interacting with libraries.
    /// </summary>
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

        /// <summary>
        /// Loads the library by calling the underlying native method dlopen.
        /// </summary>
        /// <returns>A SafeHandle object representing the loaded library.</returns>
        public SafeHandle Load()
        {
            var ptr = dlopen(EmbeddedLoader.CreateEmbeddedResource("libtopi.dylib"), RtldNow);
            handle = ptr;
            if (ptr != IntPtr.Zero) return this;
            var errPtr = dlerror();
            throw new System.ComponentModel.Win32Exception(Library.PtrToUtf8String(errPtr));
        }

        /// <summary>
        /// Frees the specified library handle.
        /// </summary>
        /// <param name="ptr">The pointer to the library handle.</param>
        /// <returns>
        /// <c>true</c> if the library handle is successfully freed; otherwise, <c>false</c>.
        /// </returns>
        /// <example>
        /// <code>
        /// IntPtr libraryHandle = LoadLibrary("example.dll");
        /// bool result = Free(libraryHandle);
        /// </code>
        /// </example>
        public bool Free(IntPtr ptr) => dlclose(ptr);

        /// <summary>
        /// Retrieves the address of the specified function from the loaded library.
        /// </summary>
        /// <param name="name">The name of the function to retrieve.</param>
        /// <returns>
        /// The address of the specified function if the function is found, or IntPtr.Zero if the function is not found.
        /// </returns>
        public IntPtr GetProc(string name) => dlsym(handle, name);

        /// <summary>
        /// Represents a loader interface for loading and interacting with libraries.
        /// </summary>
        public MacLoader(bool ownsHandle) : base(ownsHandle)
        {
        }

        /// <summary>
        /// Releases the handle of the library.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the handle is successfully released; otherwise, <c>false</c>.
        /// </returns>
        protected override bool ReleaseHandle() => Free(handle);
    }
}