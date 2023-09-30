using System;
using System.Runtime.InteropServices;

namespace Topiary
{
    public static class Library
    {
        public delegate void OnDialogueDelegate(IntPtr vmPtr, Dialogue dialogue);

        public delegate void OnChoicesDelegate(IntPtr vmPtr, IntPtr choicesPtr, byte length);

        public delegate void Subscriber(ref TopiValue value);

        public static IntPtr InitVm(string path, OnDialogueDelegate onDialogue, OnChoicesDelegate onChoices)
        {
            var dialoguePtr = Marshal.GetFunctionPointerForDelegate(onDialogue);
            var choicesPtr = Marshal.GetFunctionPointerForDelegate(onChoices);
            var ptr = createVm(path, path.Length, dialoguePtr, choicesPtr);
            return ptr;
        }

        public static void DestroyVm(IntPtr vmPtr) => destroyVm(vmPtr);
        public static void Run(IntPtr vmPtr) => run(vmPtr);
        public static void Continue(IntPtr vmPtr) => selectContinue(vmPtr);
        public static void Compile(string path) => compile(path, path.Length);
        public static void SelectChoice(IntPtr vmPtr, int index) => selectChoice(vmPtr, index);

        public static TopiValue GetVariable(IntPtr vmPtr, string name)
        {
            tryGetVariable(vmPtr, name, name.Length, out var value);
            return value;
        }

        public static void Subscribe(IntPtr vmPtr, string name, Subscriber callback) => subscribe(vmPtr, name,
            name.Length, Marshal.GetFunctionPointerForDelegate(callback));

        public static void Unsubscribe(IntPtr vmPtr, string name, Subscriber callback) => unsubscribe(vmPtr, name,
            name.Length, Marshal.GetFunctionPointerForDelegate(callback));

#if OS_MAC
        [DllImport("topi.dylib")]
#elif OS_WINDOWS
        [DllImport("topi.dll")]
#endif
        private static extern IntPtr createVm(string path, int pathLength, IntPtr onDialoguePtr, IntPtr onChoicesPtr);

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
        private static extern void compile(string path, int pathLength);

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
        private static extern bool tryGetVariable(IntPtr vmPtr, string name, int nameLength, out TopiValue value);

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