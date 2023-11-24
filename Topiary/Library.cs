using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Topiary
{
    public static class Library
    {
        public delegate void OnDialogueDelegate(IntPtr vmPtr, Dialogue dialogue);
        
        public delegate void OnChoicesDelegate(IntPtr vmPtr, IntPtr choicePtr, byte length);

        public delegate TopiValue ExternFunctionDelegate(IntPtr argPtr, byte length);

        public delegate void Subscriber(ref TopiValue value);

        /// <summary>
        /// Create a VM instance
        /// </summary>
        /// <param name="source">Compiled source file "topib"</param>
        /// <param name="onDialogue">Static callback function to be called when a Dialogue line is reached</param>
        /// <param name="onChoices">Static callback function to be called when a Fork is reached</param>
        /// <returns>Pointer to the VM, store this to be used when calling functions from Library</returns>
        public static IntPtr InitVm(byte[] source, OnDialogueDelegate onDialogue, OnChoicesDelegate onChoices)
        {
            var dialoguePtr = Marshal.GetFunctionPointerForDelegate(onDialogue);
            var choicesPtr = Marshal.GetFunctionPointerForDelegate(onChoices);
            var ptr = createVm(source, source.Length, dialoguePtr, choicesPtr);
            return ptr;
        }
        
        /// <summary>
        /// Compile a ".topi" file into bytes.
        /// Should be saved to a ".topib" file
        /// </summary>
        /// <param name="source">The file contents</param>
        /// <returns>Compiled bytes</returns>
        public static byte[] Compile(string source)
        {
            var bytes = Encoding.UTF8.GetBytes(source);
            var output = new byte[bytes.Length * 10];
            compile(bytes, bytes.Length, output, output.Length);
            return output;
        }
        
#if OS_MAC
        private const string DllPath = "topi.dylib";
#elif OS_WINDOWS
        private const string DllPath = "topi.dll";
#endif

        [DllImport(DllPath)]
        private static extern IntPtr createVm(byte[] source, int sourceLength, IntPtr onDialoguePtr, IntPtr onChoicesPtr);

        [DllImport(DllPath)]
        internal static extern void destroyVm(IntPtr vmPtr);

        [DllImport(DllPath)]
        internal static extern void start(IntPtr vmPtr);

        [DllImport(DllPath)]
        private static extern void compile(byte[] source, int sourceLength, byte[] output, int capacity);

        [DllImport(DllPath)]
        internal static extern void run(IntPtr vmPtr, out int errLine, StringBuilder errMsg, int capacity);

        [DllImport(DllPath)]
        internal static extern void selectContinue(IntPtr vmPtr);

        [DllImport(DllPath)]
        internal static extern bool canContinue(IntPtr vmPtr);

        [DllImport(DllPath)]
        internal static extern bool isWaiting(IntPtr vmPtr);

        [DllImport(DllPath)]
        internal static extern void selectChoice(IntPtr vmPtr, int index);

        [DllImport(DllPath)]
        internal static extern bool tryGetValue(IntPtr vmPtr, string name, int nameLength, out TopiValue value);

        // TODO: Need to remove cross code memory management
        [DllImport(DllPath)]
        internal static extern bool destroyValue(ref TopiValue value);

        [DllImport(DllPath)]
        internal static extern void subscribe(IntPtr vmPtr, string name, int nameLength, IntPtr callbackPtr);

        [DllImport(DllPath)]
        internal static extern void unsubscribe(IntPtr vmPtr, string name, int nameLength, IntPtr callbackPtr);

        [DllImport(DllPath)]
        internal static extern void setExternNumber(IntPtr vmPtr, string name, int nameLength, float value);

        [DllImport(DllPath)]
        internal static extern void setExternString(IntPtr vmPtr, string name, int nameLength, string value,
            int valueLength);

        [DllImport(DllPath)]
        public static extern void setExternBool(IntPtr vmPtr, string name, int nameLength, bool value);

        [DllImport(DllPath)]
        internal static extern void setExternNil(IntPtr vmPtr, string name, int nameLength);

        [DllImport(DllPath)]
        internal static extern void setExternFunc(IntPtr vmPtr, string name, int nameLength, IntPtr funcPtr, byte arity);
    }
}