using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Topiary
{
    public static class Library
    {
        public delegate void OnDialogueDelegate(IntPtr vmPtr, Dialogue dialogue);

        public delegate void OnChoicesDelegate(IntPtr vmPtr, IntPtr choicesPtr, byte length);
        
        public delegate TopiValue ExternFunctionDelegate(IntPtr argPtr, byte length);

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
            if (!tryGetValue(vmPtr, name, name.Length, out var value)) Console.WriteLine($"Cannot find value: {name}");
            return value;
        }

        public static void DestroyValue(ref TopiValue value)
        {
            destroyValue(ref value);
        }

        public static void Subscribe(IntPtr vmPtr, string name, Subscriber callback) => subscribe(vmPtr, name,
            name.Length, Marshal.GetFunctionPointerForDelegate(callback));

        public static void Unsubscribe(IntPtr vmPtr, string name, Subscriber callback) => unsubscribe(vmPtr, name,
            name.Length, Marshal.GetFunctionPointerForDelegate(callback));

        public static void SetExternBool(IntPtr vmPtr, string name, bool value) =>
            setExternBool(vmPtr, name, name.Length, value);

        public static void SetExternNumber(IntPtr vmPtr, string name, float value) =>
            setExternNumber(vmPtr, name, name.Length, value);

        public static void SetExternNil(IntPtr vmPtr, string name) => setExternNil(vmPtr, name, name.Length);

        public static void SetExternFunction(IntPtr vmPtr, string name, ExternFunctionDelegate function) =>
            setExternFunc(vmPtr, name, name.Length, Marshal.GetFunctionPointerForDelegate(function), 1);

#if OS_MAC
        private const string DllPath = "topi.dylib";
#elif OS_WINDOWS
        private const string DllPath = "topi.dll";
#endif

        [DllImport(DllPath)]
        private static extern IntPtr createVm(byte[] source, int sourceLength, IntPtr onDialoguePtr,
            IntPtr onChoicesPtr);

        [DllImport(DllPath)]
        private static extern void destroyVm(IntPtr vmPtr);

        [DllImport(DllPath)]
        private static extern void compile(byte[] source, int sourceLength, byte[] output, int capacity);

        [DllImport(DllPath)]
        private static extern void run(IntPtr vmPtr);

        [DllImport(DllPath)]
        private static extern void selectContinue(IntPtr vmPtr);

        [DllImport(DllPath)]
        private static extern void selectChoice(IntPtr vmPtr, int index);

        [DllImport(DllPath)]
        private static extern bool tryGetValue(IntPtr vmPtr, string name, int nameLength, out TopiValue value);

        [DllImport(DllPath)]
        private static extern bool destroyValue(ref TopiValue value);

        [DllImport(DllPath)]
        private static extern void subscribe(IntPtr vmPtr, string name, int nameLength, IntPtr callbackPtr);

        [DllImport(DllPath)]
        private static extern void unsubscribe(IntPtr vmPtr, string name, int nameLength, IntPtr callbackPtr);

        [DllImport(DllPath)]
        private static extern void setExternNumber(IntPtr vmPtr, string name, int nameLength, float value);

        [DllImport(DllPath)]
        private static extern void setExternBool(IntPtr vmPtr, string name, int nameLength, bool value);

        [DllImport(DllPath)]
        private static extern void setExternNil(IntPtr vmPtr, string name, int nameLength);

        [DllImport(DllPath)]
        private static extern void setExternFunc(IntPtr vmPtr, string name, int nameLength, IntPtr funcPtr, byte arity);
    }
}