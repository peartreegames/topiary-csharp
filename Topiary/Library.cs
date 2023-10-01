using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Topiary
{
    public static class Library
    {
        public delegate void OnDialogueDelegate(IntPtr vmPtr, Dialogue dialogue);

        public delegate void OnChoicesDelegate(IntPtr vmPtr, IntPtr choicesPtr, byte length);

        public delegate void Subscriber(ref TopiValue value);

        public static IntPtr InitVm(byte[] source, OnDialogueDelegate onDialogue, OnChoicesDelegate onChoices)
        {
            var dialoguePtr = Marshal.GetFunctionPointerForDelegate(onDialogue);
            var choicesPtr = Marshal.GetFunctionPointerForDelegate(onChoices);
            var ptr = createVm(source, source.Length, dialoguePtr, choicesPtr);
            return ptr;
        }

        public static void DestroyVm(IntPtr vmPtr) => destroyVm(vmPtr);
        public static void Run(IntPtr vmPtr) => run(vmPtr);
        public static void Continue(IntPtr vmPtr) => selectContinue(vmPtr);
        public static byte[] Compile(string source)
        {
            var bytes = Encoding.UTF8.GetBytes(source);
            var output = new byte[bytes.Length * 4];
            compile(bytes, bytes.Length, output, output.Length);
            return output;
        }

        public static void SelectChoice(IntPtr vmPtr, int index) => selectChoice(vmPtr, index);

        public static TopiValue GetValue(IntPtr vmPtr, string name)
        {
            tryGetValue(vmPtr, name, name.Length, out var value);
            return value;
        }

        // Mixing our memory management here is a bad idea.
        // Will need to come up with a better solution.
        public static void DestroyValue(ref TopiValue value) => destroyValue(ref value);

        public static void Subscribe(IntPtr vmPtr, string name, Subscriber callback) => subscribe(vmPtr, name,
            name.Length, Marshal.GetFunctionPointerForDelegate(callback));

        public static void Unsubscribe(IntPtr vmPtr, string name, Subscriber callback) => unsubscribe(vmPtr, name,
            name.Length, Marshal.GetFunctionPointerForDelegate(callback));

#if OS_MAC
        [DllImport("topi.dylib")]
#elif OS_WINDOWS
        [DllImport("topi.dll")]
#endif
        private static extern IntPtr createVm(byte[] source, int sourceLength, IntPtr onDialoguePtr, IntPtr onChoicesPtr);

#if OS_MAC
        [DllImport("topi.dylib")]
#elif OS_WINDOWS
        [DllImport("topi.dll")]
#endif
        private static extern void destroyVm(IntPtr vmPtr);

#if OS_MAC
        [DllImport("topi.dylib")]
#elif OS_WINDOWS
        [DllImport("topi.dll")]
#endif
        private static extern void compile(byte[] source, int sourceLength, byte[] output, int capacity);

#if OS_MAC
        [DllImport("topi.dylib")]
#elif OS_WINDOWS
        [DllImport("topi.dll")]
#endif
        private static extern void run(IntPtr vmPtr);
#if OS_MAC
        [DllImport("topi.dylib")]
#elif OS_WINDOWS
        [DllImport("topi.dll")]
#endif
        private static extern void selectContinue(IntPtr vmPtr);

#if OS_MAC
        [DllImport("topi.dylib")]
#elif OS_WINDOWS
        [DllImport("topi.dll")]
#endif
        private static extern void selectChoice(IntPtr vmPtr, int index);

#if OS_MAC
        [DllImport("topi.dylib")]
#elif OS_WINDOWS
        [DllImport("topi.dll")]
#endif
        private static extern bool tryGetValue(IntPtr vmPtr, string name, int nameLength, out TopiValue value);

#if OS_MAC
        [DllImport("topi.dylib")]
#elif OS_WINDOWS
        [DllImport("topi.dll")]
#endif
        private static extern void destroyValue(ref TopiValue value);

#if OS_MAC
        [DllImport("topi.dylib")]
#elif OS_WINDOWS
        [DllImport("topi.dll")]
#endif
        private static extern void subscribe(IntPtr vmPtr, string name, int nameLength, IntPtr callbackPtr);

#if OS_MAC
        [DllImport("topi.dylib")]
#elif OS_WINDOWS
        [DllImport("topi.dll")]
#endif
        private static extern void unsubscribe(IntPtr vmPtr, string name, int nameLength, IntPtr callbackPtr);
    }
}