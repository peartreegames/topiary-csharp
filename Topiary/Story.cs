using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace PeartreeGames.Topiary
{
    public class Story : IDisposable
    {
        private readonly Library _library;
        private IntPtr _vmPtr;

        private readonly OnDialogueCallback _onDialogue;
        private readonly OnChoicesCallback _onChoices;
        private readonly List<Function> _functions;
        private readonly SortedSet<string> _externs;
        private GCHandle _onDialogueHandle;
        private GCHandle _onChoicesHandle;
        public bool IsValid => _vmPtr != IntPtr.Zero;

        public delegate void OnDialogueCallback(Story story, Dialogue dialogue);

        public delegate void OnChoicesCallback(Story story, Choice[] choices);

        public Library Library => _library;

        public Story(byte[] source, OnDialogueCallback onDialogue, OnChoicesCallback onChoices,
            Library.Severity severity = Library.Severity.Error)
        {
            _library = new Library();
            _library.SetDebugSeverity(severity);

            using var memStream = new MemoryStream(source);
            using var reader = new BinaryReader(memStream);
            _externs = ByteCode.GetExterns(reader);
            _onDialogue = onDialogue;
            _onChoices = onChoices;
            Delegates.OnChoicesDelegate onChoicesDel = OnChoices;
            Delegates.OnDialogueDelegate onDialogueDel = OnDialogue;

            if (Library.IsUnityRuntime)
            {
                _onChoicesHandle = GCHandle.Alloc(onDialogueDel, GCHandleType.Pinned);
                _onDialogueHandle = GCHandle.Alloc(onChoicesDel, GCHandleType.Pinned);
            }

            var dialoguePtr = Marshal.GetFunctionPointerForDelegate(onDialogueDel);
            var choicesPtr = Marshal.GetFunctionPointerForDelegate(onChoicesDel);
            _vmPtr = _library.CreateVm(source, source.Length, dialoguePtr, choicesPtr);
            _functions = new List<Function>();
        }

        public void Dispose()
        {
            if (_vmPtr == IntPtr.Zero) return;
            _library.DestroyVm(_vmPtr);
            _library.Dispose();
            if (_onDialogueHandle.IsAllocated) _onDialogueHandle.Free();
            if (_onChoicesHandle.IsAllocated) _onChoicesHandle.Free();
            foreach (var func in _functions) func.Dispose();
            _vmPtr = IntPtr.Zero;
        }

        /// <summary>
        /// Compile a ".topi" file into bytes.
        /// Should be saved to a ".topib" file
        /// </summary>
        /// <param name="fullPath">The file absolute path</param>
        /// <param name="severity" default="Error">Log severity</param>
        /// <param name="capacity" default="5_242_800">Optional capacity for the bytecode</param>
        /// <returns>Compiled bytes</returns>
        public static byte[] Compile(string fullPath,
            Library.Severity severity = Library.Severity.Error, long capacity = 5_242_800)
        {
            var lib = new Library();
            lib.SetDebugSeverity(severity);
            var output = new byte[capacity];
            var size = lib.Compile(fullPath, fullPath.Length, output, output.Length);
            lib.Dispose();
            return output.Take(size).ToArray();
        }

        private void OnDialogue(IntPtr vmPtr, Dialogue dialogue) => _onDialogue(this, dialogue);

        private void OnChoices(IntPtr vmPtr, IntPtr choicesPtr, byte count) =>
            _onChoices(this, Choice.MarshalPtr(choicesPtr, count));

        /// <summary>
        /// Start the Story
        /// </summary>
        /// <param name="bough">
        /// Optional: The bough path where the conversation will start.
        /// If non provided, first bough in the file will be used
        /// </param>
        public void Start(string bough = "") =>
            _library.Start(_vmPtr, bough, bough.Length);

        /// <summary>
        /// Run the Story until the next Dialogue or Choice
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Run() => _library.Run(_vmPtr);

        /// <summary>
        /// Continue the Story.
        /// </summary>
        public void Continue() => _library.SelectContinue(_vmPtr);

        public bool CanContinue => _library.CanContinue(_vmPtr);
        public bool IsWaiting => _library.IsWaiting(_vmPtr);

        /// <summary>
        /// Select a Choice.
        /// </summary>
        /// <param name="index">The index of the choice selected</param>
        public void SelectChoice(int index) => _library.SelectChoice(_vmPtr, index);

        /// <summary>
        /// Retrieve the current value of any Global variable in the story
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <returns>TopiValue.nil if not found</returns>
        public TopiValue GetValue(string name)
        {
            if (!_library.TryGetValue(_vmPtr, name, name.Length, out var value))
                Console.WriteLine($"Cannot find value: {name}");
            return value;
        }

        /// <summary>
        /// Destroy a reference value in unmanaged memory
        /// NOTE: This should be removed in future versions
        /// </summary>
        /// <param name="value">The value to be destroyed</param>
        public void DestroyValue(ref TopiValue value) => _library.DestroyValue(ref value);

        /// <summary>
        /// Subscribe to when a Global variable changes
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="callback">The callback to be executed on change</param>
        public bool Subscribe(string name, Delegates.Subscriber callback) =>
            _library.Subscribe(_vmPtr, name, name.Length,
                Marshal.GetFunctionPointerForDelegate(callback));

        /// <summary>
        /// Unsubscribe when a Global variable changes
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="callback">The callback that was passed into Subscribe</param>
        public bool Unsubscribe(string name, Delegates.Subscriber callback) =>
            _library.Unsubscribe(_vmPtr, name, name.Length,
                Marshal.GetFunctionPointerForDelegate(callback));

        /// <summary>
        /// Set an Extern variable to a bool value
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value to set</param>
        public void Set(string name, bool value) =>
            _library.SetExternBool(_vmPtr, name, name.Length, value);

        /// <summary>
        /// Set an Extern variable to a float value
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value to set</param>
        public void Set(string name, float value) =>
            _library.SetExternNumber(_vmPtr, name, name.Length, value);

        /// <summary>
        /// Set an Extern variable to a float value
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value to set</param>
        public void Set(string name, string value) =>
            _library.SetExternString(_vmPtr, name, name.Length, value, value.Length);

        /// <summary>
        /// Set a Global Extern variable to a function value
        /// Note: It is easier to use the TopiAttribute instead with the BindFunctions method
        /// However this is kept in case you need more control 
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="function">The value to set</param>
        /// <param name="arity">The number of parameters the function accepts</param>
        public void Set(string name, Delegates.ExternFunctionDelegate function, byte arity)
        {
            var ptr = Marshal.GetFunctionPointerForDelegate(function);
            _library.SetExternFunc(_vmPtr, name, name.Length, ptr, arity);
        }

        /// <summary>
        /// Set an Extern variable to a nil value
        /// </summary>
        /// <param name="name">The name of the variable</param>
        public void Unset(string name) => _library.SetExternNil(_vmPtr, name, name.Length);

        /// <summary>
        /// Bind all TopiAttribute functions within the given Assemblies
        /// Functions must be of type "Func&lt;TopiValue[,TopiValue,TopiValue,TopiValue,TopiValue]&gt;"
        /// or "Action[&lt;TopiValue,TopiValue,TopiValue,TopiValue&gt;]"
        /// See <see cref="Function"/>
        /// </summary>
        /// <param name="assemblies"></param>
        public void BindFunctions(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                if (Regex.IsMatch(assembly.FullName,
                        "^(System|Microsoft|mscorlib|netstandard|Windows|JetBrains|test)"))
                    continue;
                foreach (var type in assembly.DefinedTypes)
                {
                    foreach (var method in type.GetMethods(BindingFlags.Public |
                                                           BindingFlags.Static |
                                                           BindingFlags.NonPublic))
                    {
                        if (!(method.GetCustomAttribute(typeof(TopiAttribute), false) is
                                TopiAttribute attr))
                            continue;
                        if (!_externs.Contains(attr.Name)) continue;
                        var func = Function.Create(method);
                        _functions.Add(func);
                        Set(attr.Name, func.Call,
                            (byte) method.GetParameters().Length);
                    }
                }
            }
        }
    }
}