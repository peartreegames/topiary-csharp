using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace PeartreeGames.Topiary
{
    /// <summary>
    /// Represents a dialogue instance.
    /// </summary>
    public class Dialogue : IDisposable
    {
        private readonly Library _library;
        private IntPtr _vmPtr;

        private readonly OnLineCallback _onLine;
        private readonly OnChoicesCallback _onChoices;
        private static readonly List<Function> Functions = new List<Function>();
        private readonly SortedSet<string> _externs;
        private GCHandle _onLineHandle;
        private GCHandle _onChoicesHandle;

        /// <summary>
        /// Gets a value indicating whether the Dialogue instance is valid.
        /// </summary>
        /// <remarks>
        /// This property returns <c>true</c> if the internal pointer <c>_vmPtr</c> is not zero;
        /// otherwise, it returns <c>false</c>.
        /// </remarks>
        public bool IsValid => _vmPtr != IntPtr.Zero;

        /// <summary>
        /// Represents a callback method that processes each line of dialogue.
        /// </summary>
        /// <param name="dialogue">The Dialogue object that invoked the callback.</param>
        /// <param name="line">The line of dialogue being processed.</param>
        public delegate void OnLineCallback(Dialogue dialogue, Line line);

        /// <summary>
        /// Represents a callback function for handling choices in a dialogue.
        /// </summary>
        /// <param name="dialogue">The dialogue in which the choice is being made.</param>
        /// <param name="choices">The array of choices available to the player.</param>
        public delegate void OnChoicesCallback(Dialogue dialogue, Choice[] choices);

        /// <summary>
        /// Represents a library that provides functionality for creating and managing dialogues.
        /// </summary>
        public Library Library => _library;

        /// <summary>
        /// Represents a dialogue.
        /// </summary>
        public Dialogue(byte[] source, OnLineCallback onLine, OnChoicesCallback onChoices,
            Library.Severity severity = Library.Severity.Error)
        {
            _library = new Library();
            _library.SetDebugSeverity(severity);

            using var memStream = new MemoryStream(source);
            using var reader = new BinaryReader(memStream);
            _externs = ByteCode.GetExterns(reader);
            _onLine = onLine;
            _onChoices = onChoices;
            Delegates.OnChoicesDelegate onChoicesDel = OnChoices;
            Delegates.OnLineDelegate onLineDel = OnLine;

            if (Library.IsUnityRuntime)
            {
                _onChoicesHandle = GCHandle.Alloc(onLineDel, GCHandleType.Pinned);
                _onLineHandle = GCHandle.Alloc(onChoicesDel, GCHandleType.Pinned);
            }

            var linePtr = Marshal.GetFunctionPointerForDelegate(onLineDel);
            var choicesPtr = Marshal.GetFunctionPointerForDelegate(onChoicesDel);
            _vmPtr = _library.CreateVm(source, source.Length, linePtr, choicesPtr);
        }

        /// <summary>
        /// Releases all resources used by the Dialogue object.
        /// </summary>
        public void Dispose()
        {
            if (_vmPtr == IntPtr.Zero) return;
            _library.DestroyVm(_vmPtr);
            _library.Dispose();
            if (_onLineHandle.IsAllocated) _onLineHandle.Free();
            if (_onChoicesHandle.IsAllocated) _onChoicesHandle.Free();
            foreach (var func in Functions) func.Dispose();
            Functions.Clear();
            _vmPtr = IntPtr.Zero;
        }

        /// <summary>
        /// Compile a ".topi" file into bytes.
        /// Should be saved to a ".topib" file
        /// Will precalculate the required capacity
        /// </summary>
        /// <param name="fullPath">The file absolute path</param>
        /// <param name="severity" default="Error">Log severity</param>
        /// <returns>Compiled bytes</returns>
        public static byte[] Compile(string fullPath, Library.Severity severity = Library.Severity.Error)
        {
            var lib = new Library();
            lib.SetDebugSeverity(severity);
            var capacity = lib.CalculateCompileSize(fullPath, fullPath.Length);
            var output = new byte[capacity];
            _ = lib.Compile(fullPath, fullPath.Length, output, output.Length);
            lib.Dispose();
            return output;
        }
        /// <summary>
        /// Compile a ".topi" file into bytes.
        /// Should be saved to a ".topib" file
        /// </summary>
        /// <param name="fullPath">The file absolute path</param>
        /// <param name="capacity">Capacity for the bytecode output</param>
        /// <param name="severity" default="Error">Log severity</param>
        /// <returns>Compiled bytes</returns>
        public static byte[] Compile(string fullPath, long capacity, Library.Severity severity = Library.Severity.Error)
        {
            var lib = new Library();
            lib.SetDebugSeverity(severity);
            var output = new byte[capacity];
            var size = lib.Compile(fullPath, fullPath.Length, output, output.Length);
            lib.Dispose();
            return output.Take(size).ToArray();
        }

        private void OnLine(IntPtr vmPtr, Line line) => _onLine(this, line);

        private void OnChoices(IntPtr vmPtr, IntPtr choicesPtr, byte count) =>
            _onChoices(this, Choice.MarshalPtr(choicesPtr, count));

        /// <summary>
        /// Start the Dialogue 
        /// </summary>
        /// <param name="bough">
        /// Optional: The bough path where the conversation will start.
        /// If non provided, first bough in the file will be used
        /// </param>
        public void Start(string bough = "") =>
            _library.Start(_vmPtr, bough, bough.Length);

        /// <summary>
        /// Run the Dialogue until the next Dialogue or Choice
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Run() => _library.Run(_vmPtr);

        /// <summary>
        /// Continue the Dialogue.
        /// </summary>
        public void Continue() => _library.SelectContinue(_vmPtr);

        /// <summary>
        /// Gets a value indicating whether the dialogue can continue.
        /// </summary>
        /// <value>
        /// <c>true</c> if the dialogue can continue; otherwise, <c>false</c>.
        /// </value>
        public bool CanContinue => _library.CanContinue(_vmPtr);

        /// <summary>
        /// Gets a value indicating whether the dialogue is waiting for user input.
        /// </summary>
        public bool IsWaiting => _library.IsWaiting(_vmPtr);

        /// <summary>
        /// Select a Choice.
        /// </summary>
        /// <param name="index">The index of the choice selected</param>
        public void SelectChoice(int index) => _library.SelectChoice(_vmPtr, index);

        /// <summary>
        /// Save current Dialogue State to JSON
        /// Will precalculate the necessary size
        /// Should merge the resulting JSON with the Game Root JSON State
        /// </summary>
        public string SaveState()
        {
            var capacity = _library.CalculateStateSize(_vmPtr);
            var output = new byte[capacity];
            _ = _library.SaveState(_vmPtr, output, output.Length);
            return System.Text.Encoding.UTF8.GetString(output);
        }
        
        /// <summary>
        /// Save current Dialogue State to JSON
        /// Should merge the resulting JSON with the Game Root JSON State
        /// </summary>
        /// <param name="capacity">The maximum size of bytes to allocate</param>
        public string SaveState(long capacity)
        {
            var output = new byte[capacity];
            var size = _library.SaveState(_vmPtr, output, output.Length);
            var segment = new ArraySegment<byte>(output, 0, size);
            return System.Text.Encoding.UTF8.GetString(segment.Array!, segment.Offset, segment.Count);
        }

        /// <summary>
        /// Load a JSON state into the Dialogue
        /// </summary>
        /// <param name="json">JSON string</param>
        public void LoadState(string json) => _library.LoadState(_vmPtr, json, json.Length);


        /// <summary>
        /// Retrieve the current value of any Global variable in the dialogue
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
        public void Set(string name, Function function, byte arity)
        {
            _library.SetExternFunc(_vmPtr, name, name.Length, function.GetCallIntPtr(), arity);
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
                        Functions.Add(func);
                        Set(attr.Name, func,
                            (byte) method.GetParameters().Length);
                    }
                }
            }
        }
    }
}