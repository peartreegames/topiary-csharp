using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace PeartreeGames.Topiary
{
    /// <summary>
    /// Represents a dialogue instance.
    /// </summary>
    public class Dialogue : IDisposable
    {
        private readonly Library _library;
        private IntPtr _vmPtr;

        public SortedSet<string> Externs { get; }

        public static readonly Dictionary<IntPtr, Dialogue> Dialogues =
            new Dictionary<IntPtr, Dialogue>();

        public IntPtr VmPtr => _vmPtr;

        /// <summary>
        /// Gets a value indicating whether the Dialogue instance is valid.
        /// </summary>
        /// <remarks>
        /// This property returns <c>true</c> if the internal pointer <c>_vmPtr</c> is not zero;
        /// otherwise, it returns <c>false</c>.
        /// </remarks>
        public bool IsValid => _vmPtr != IntPtr.Zero;

        /// <summary>
        /// Represents a library that provides functionality for creating and managing dialogues.
        /// </summary>
        public Library Library => _library;

        /// <summary>
        /// Represents a dialogue.
        /// </summary>
        public Dialogue(byte[] source, Delegates.OnLineDelegate onLine,
            Delegates.OnChoicesDelegate onChoices, Delegates.OutputLogDelegate logger,
            Library.Severity severity = Library.Severity.Error)
        {
            unsafe
            {
                _library = new Library(logger);
                _library.SetDebugSeverity(severity);

                using var memStream = new MemoryStream(source);
                using var reader = new BinaryReader(memStream);
                Externs = ByteCode.GetExterns(reader);

                var linePtr = Marshal.GetFunctionPointerForDelegate(onLine);
                var choicesPtr = Marshal.GetFunctionPointerForDelegate(onChoices);
                fixed (byte* pinned = source)
                {
                    var sourcePtr = (IntPtr)pinned;
                    _vmPtr = _library.CreateVm(sourcePtr, source.Length, linePtr, choicesPtr);
                }
            }

            Dialogues.Add(_vmPtr, this);
        }

        /// <summary>
        /// Releases all resources used by the Dialogue object.
        /// </summary>
        public void Dispose()
        {
            if (_vmPtr == IntPtr.Zero) return;
            Dialogues.Remove(_vmPtr);
            _library.DestroyVm(_vmPtr);
            _library.Dispose();
            _vmPtr = IntPtr.Zero;
        }

        /// <summary>
        /// Compile a ".topi" file into bytes.
        /// Should be saved to a ".topib" file
        /// Will precalculate the required capacity
        /// </summary>
        /// <param name="fullPath">The file absolute path</param>
        /// <param name="logger"></param>
        /// <param name="severity" default="Error">Log severity</param>
        /// <returns>Compiled bytes</returns>
        public static byte[] Compile(string fullPath, Delegates.OutputLogDelegate logger,
            Library.Severity severity = Library.Severity.Error)
        {
            var lib = new Library(logger);
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
        /// <param name="logger"></param>
        /// <param name="severity" default="Error">Log severity</param>
        /// <returns>Compiled bytes</returns>
        public static byte[] Compile(string fullPath, long capacity,
            Delegates.OutputLogDelegate logger, Library.Severity severity = Library.Severity.Error)
        {
            var lib = new Library(logger);
            lib.SetDebugSeverity(severity);
            var output = new byte[capacity];
            var size = lib.Compile(fullPath, fullPath.Length, output, output.Length);
            lib.Dispose();
            return output.Take(size).ToArray();
        }

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
            return System.Text.Encoding.UTF8.GetString(segment.Array!, segment.Offset,
                segment.Count);
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

        public void SetSubscribeCallback(Delegates.Subscriber subscriber) => 
            _library.SetSubscriberCallback(_vmPtr, Marshal.GetFunctionPointerForDelegate(subscriber));
        
        /// <summary>
        /// Subscribe to when a Global variable changes
        /// </summary>
        /// <param name="name">The name of the variable</param>
        public bool Subscribe(string name) => _library.Subscribe(_vmPtr, name, name.Length);

        /// <summary>
        /// Unsubscribe when a Global variable changes
        /// </summary>
        /// <param name="name">The name of the variable</param>
        public bool Unsubscribe(string name) => _library.Unsubscribe(_vmPtr, name, name.Length);

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
        /// <param name="function">The function to set</param>
        public void Set(Delegates.ExternFunctionDelegate function)
        {
            var methodInfo = function.Method;
            var topiAttributes = methodInfo.GetCustomAttributes(typeof(TopiAttribute), false);
            if (topiAttributes.Length == 0)
                throw new InvalidOperationException($"Missing TopiAttribute on function {methodInfo.Name}.");
            if (topiAttributes.Length > 1)
                throw new InvalidOperationException($"Only one instance of TopiAttribute is allowed on function {methodInfo.Name}");
            
            foreach (TopiAttribute topiAttribute in topiAttributes)
            {
                var name = topiAttribute.Name;
                var arity = topiAttribute.Arity;
                _library.SetExternFunc(_vmPtr, name, name.Length,
                    Marshal.GetFunctionPointerForDelegate(function), arity);
            }
        }

        /// <summary>
        /// Set an Extern variable to a nil value
        /// </summary>
        /// <param name="name">The name of the variable</param>
        public void Unset(string name) => _library.SetExternNil(_vmPtr, name, name.Length);
    }
}