using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Topiary
{
    public static class Library
    {
        public delegate void OnDialogueDelegate(IntPtr vmPtr, Dialogue dialogue);

        public delegate void OnChoicesDelegate(IntPtr vmPtr, Choice[] choices);

        public delegate void OnChoicesDelegateInternal(IntPtr vmPtr, IntPtr choicePtr, byte length);

        public delegate TopiValue ExternFunctionDelegate(IntPtr argPtr, byte length);

        public delegate void Subscriber(ref TopiValue value);

        /// <summary>
        /// Create a VM instance
        /// </summary>
        /// <param name="source">Compiled source file "topib"</param>
        /// <param name="onDialogue">Callback function to be called when a Dialogue line is reached</param>
        /// <param name="onChoices">Callback function to be called when a Choice is reached</param>
        /// <returns>Pointer to the VM, store this to be used when calling functions from Library</returns>
        public static IntPtr InitVm(byte[] source, OnDialogueDelegate onDialogue, OnChoicesDelegate onChoices)
        {
            var dialoguePtr = Marshal.GetFunctionPointerForDelegate(onDialogue);
            var choicesPtr =
                Marshal.GetFunctionPointerForDelegate(
                    new OnChoicesDelegateInternal(new Choice.Wrapper(onChoices).OnChoices));
            var ptr = createVm(source, source.Length, dialoguePtr, choicesPtr);
            return ptr;
        }

        /// <summary>
        /// Destroy a VM instance to clean up memory
        /// </summary>
        /// <param name="vmPtr">The Pointer returned from InitVm</param>
        public static void DestroyVm(IntPtr vmPtr) => destroyVm(vmPtr);

        /// <summary>
        /// Start the topi Story
        /// </summary>
        /// <param name="vmPtr">The Pointer returned from InitVm</param>
        /// <param name="errLine">If error, the line number it occured on</param>
        /// <param name="errMsg">If error, the message output</param>
        /// <param name="errMsgCapacity">The max length of the errMsg</param>
        public static void Run(IntPtr vmPtr, out int errLine, StringBuilder errMsg, int errMsgCapacity) =>
            run(vmPtr, out errLine, errMsg, errMsgCapacity);

        /// <summary>
        /// Continue the Story.
        /// Should be called in the OnDialogueDelegate callback
        /// </summary>
        /// <param name="vmPtr">The Pointer returned from InitVm</param>
        public static void Continue(IntPtr vmPtr) => selectContinue(vmPtr);

        /// <summary>
        /// Select a Choice.
        /// Should be called in the OnChoicesDelegate callback.
        /// </summary>
        /// <param name="vmPtr">The Pointer returned from InitVm</param>
        /// <param name="index">The index of the choice selected</param>
        public static void SelectChoice(IntPtr vmPtr, int index) => selectChoice(vmPtr, index);

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

        /// <summary>
        /// Retrieve the current value of any Global variable in the story
        /// </summary>
        /// <param name="vmPtr">The Pointer returned from InitVm</param>
        /// <param name="name">The name of the variable</param>
        /// <returns>TopiValue.nil if not found</returns>
        public static TopiValue GetValue(IntPtr vmPtr, string name)
        {
            if (!tryGetValue(vmPtr, name, name.Length, out var value)) Console.WriteLine($"Cannot find value: {name}");
            return value;
        }

        /// <summary>
        /// Destroy a reference value in unmanaged memory
        /// NOTE: This should be removed in future versions
        /// </summary>
        /// <param name="value">The value to be destroyed</param>
        public static void DestroyValue(ref TopiValue value)
        {
            destroyValue(ref value);
        }

        /// <summary>
        /// Subscribe to when a Global variable changes
        /// </summary>
        /// <param name="vmPtr">The Pointer returned from InitVm</param>
        /// <param name="name">The name of the variable</param>
        /// <param name="callback">The callback to be executed on change</param>
        public static void Subscribe(IntPtr vmPtr, string name, Subscriber callback) => subscribe(vmPtr, name,
            name.Length, Marshal.GetFunctionPointerForDelegate(callback));

        /// <summary>
        /// Unsubscribe when a Global variable changes
        /// </summary>
        /// <param name="vmPtr">The Pointer returned from InitVm</param>
        /// <param name="name">The name of the variable</param>
        /// <param name="callback">The callback that was passed into Subscribe</param>
        public static void Unsubscribe(IntPtr vmPtr, string name, Subscriber callback) => unsubscribe(vmPtr, name,
            name.Length, Marshal.GetFunctionPointerForDelegate(callback));

        /// <summary>
        /// Set an Extern variable to a bool value
        /// </summary>
        /// <param name="vmPtr">The Pointer returned from InitVm</param>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value to set</param>
        public static void SetExternBool(IntPtr vmPtr, string name, bool value) =>
            setExternBool(vmPtr, name, name.Length, value);

        /// <summary>
        /// Set an Extern variable to a float value
        /// </summary>
        /// <param name="vmPtr">The Pointer returned from InitVm</param>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value to set</param>
        public static void SetExternNumber(IntPtr vmPtr, string name, float value) =>
            setExternNumber(vmPtr, name, name.Length, value);

        /// <summary>
        /// Set an Extern variable to a float value
        /// </summary>
        /// <param name="vmPtr">The Pointer returned from InitVm</param>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value to set</param>
        public static void SetExternString(IntPtr vmPtr, string name, string value) =>
            setExternString(vmPtr, name, name.Length, value, value.Length);

        /// <summary>
        /// Set an Extern variable to a nil value
        /// </summary>
        /// <param name="vmPtr">The Pointer returned from InitVm</param>
        /// <param name="name">The name of the variable</param>
        public static void SetExternNil(IntPtr vmPtr, string name) => setExternNil(vmPtr, name, name.Length);

        /// <summary>
        /// Set a Global Extern variable to a function value
        /// Note: It is easier to use the TopiAttribute instead with the BindFunctions method
        /// However this is kept in case you need more control 
        /// </summary>
        /// <param name="vmPtr">The Pointer returned from InitVm</param>
        /// <param name="name">The name of the variable</param>
        /// <param name="function">The value to set</param>
        /// <param name="arity">The number of parameters the function accepts</param>
        public static void SetExternFunction(IntPtr vmPtr, string name, ExternFunctionDelegate function, byte arity) =>
            setExternFunc(vmPtr, name, name.Length, Marshal.GetFunctionPointerForDelegate(function), arity);

        /// <summary>
        /// Bind all TopiAttribute functions within the given Assemblies
        /// Functions must be of type "Func&lt;TopiValue[,TopiValue,TopiValue,TopiValue,TopiValue]&gt;"
        /// or "Action[&lt;TopiValue,TopiValue,TopiValue,TopiValue&gt;]"
        /// See <see cref="Function"/>
        /// </summary>
        /// <param name="vmPtr"></param>
        /// <param name="assemblies"></param>
        public static void BindFunctions(IntPtr vmPtr, IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                if (Regex.IsMatch(assembly.FullName, "^(System|Microsoft|mscorlib|netstandard|Windows|JetBrains|test)"))
                    continue;
                foreach (var type in assembly.DefinedTypes)
                {
                    foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static |
                                                           BindingFlags.NonPublic))
                    {
                        if (!(method.GetCustomAttribute(typeof(TopiAttribute), false) is TopiAttribute attr)) continue;
                        var del = Function.CreateFromMethodInfo(method);
                        SetExternFunction(vmPtr, attr.Name, del, (byte) method.GetParameters().Length);
                    }
                }
            }
        }

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
        private static extern void run(IntPtr vmPtr, out int errLine, StringBuilder errMsg, int capacity);

        [DllImport(DllPath)]
        private static extern void selectContinue(IntPtr vmPtr);

        [DllImport(DllPath)]
        private static extern void selectChoice(IntPtr vmPtr, int index);

        [DllImport(DllPath)]
        private static extern bool tryGetValue(IntPtr vmPtr, string name, int nameLength, out TopiValue value);

        // TODO: Need to remove cross code memory management
        [DllImport(DllPath)]
        private static extern bool destroyValue(ref TopiValue value);

        [DllImport(DllPath)]
        private static extern void subscribe(IntPtr vmPtr, string name, int nameLength, IntPtr callbackPtr);

        [DllImport(DllPath)]
        private static extern void unsubscribe(IntPtr vmPtr, string name, int nameLength, IntPtr callbackPtr);

        [DllImport(DllPath)]
        private static extern void setExternNumber(IntPtr vmPtr, string name, int nameLength, float value);

        [DllImport(DllPath)]
        private static extern void setExternString(IntPtr vmPtr, string name, int nameLength, string value,
            int valueLength);

        [DllImport(DllPath)]
        private static extern void setExternBool(IntPtr vmPtr, string name, int nameLength, bool value);

        [DllImport(DllPath)]
        private static extern void setExternNil(IntPtr vmPtr, string name, int nameLength);

        [DllImport(DllPath)]
        private static extern void setExternFunc(IntPtr vmPtr, string name, int nameLength, IntPtr funcPtr, byte arity);
    }
}