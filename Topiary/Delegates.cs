using System;
using System.Runtime.InteropServices;

namespace PeartreeGames.Topiary
{
    /// <summary>
    /// The Delegates class provides delegates for various functions used in the PeartreeGames.Topiary namespace.
    /// </summary>
    /// <remarks>
    /// These delegates are used for callback functions, function pointers, and event handlers within the Topiary library.
    /// </remarks>
    public static class Delegates
    {
        /// <summary>
        /// Represents a delegate used for handling an event when a line is encountered during dialogue execution.
        /// </summary>
        /// <param name="vmPtr">The pointer to the virtual machine.</param>
        /// <param name="line">The line structure representing the encountered line.</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OnLineDelegate(IntPtr vmPtr, Line line);

        /// <summary>
        /// Represents a delegate that handles the event when choices are presented to the user.
        /// </summary>
        /// <param name="vmPtr">A pointer to the virtual machine instance.</param>
        /// <param name="choicePtr">A pointer to the choices array.</param>
        /// <param name="length">The length of the choices array.</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OnChoicesDelegate(IntPtr vmPtr, IntPtr choicePtr, byte length);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OutputLogDelegate(IntPtr msgPtr, Library.Severity severity);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate TopiValue ExternFunctionDelegate(IntPtr argPtr, byte length);

        /// <summary>
        /// Provides methods to subscribe and unsubscribe to dialogue events.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Subscriber(ref TopiValue value);

        /// <summary>
        /// Delegate for creating a virtual machine.
        /// </summary>
        /// <param name="source">The source code as a byte array.</param>
        /// <param name="sourceLength">The length of the source code.</param>
        /// <param name="onLinePtr">Pointer to the callback function for handling line output.</param>
        /// <param name="onChoicesPtr">Pointer to the callback function for handling choice selection.</param>
        /// <returns>A pointer to the created virtual machine.</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr CreateVmDelegate(byte[] source, int sourceLength,
            IntPtr onLinePtr,
            IntPtr onChoicesPtr);

        /// <summary>
        /// Delegate representing the DestroyVm method.
        /// </summary>
        /// <param name="vmPtr">Pointer to the virtual machine.</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void DestroyVmDelegate(IntPtr vmPtr);

        /// <summary>
        /// Delegate for the Start method in the PeartreeGames.Topiary library.
        /// </summary>
        /// <param name="vmPtr">A pointer to the virtual machine instance.</param>
        /// <param name="bough">The bough parameter for the Start method.</param>
        /// <param name="boughLength">The length of the bough parameter.</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void StartDelegate(IntPtr vmPtr, string bough, int boughLength);

        /// <summary>
        /// Represents a delegate for compiling a specified path into a byte array output.
        /// </summary>
        /// <param name="path">The path to compile.</param>
        /// <param name="pathLength">The length of the path.</param>
        /// <param name="output">The byte array output where the compiled result will be stored.</param>
        /// <param name="capacity">The capacity of the output byte array.</param>
        /// <returns>An integer value indicating the result of the compilation.</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U4)]
        public delegate int CompileDelegate(string path, int pathLength, byte[] output,
            int capacity);

        /// <summary>
        /// Delegate for calculating the compile size.
        /// </summary>
        /// <param name="path">The path of the file to calculate the compile size.</param>
        /// <param name="pathLength">The length of the path string.</param>
        /// <returns>The calculated compile size.</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U4)]
        public delegate int CalculateCompileSizeDelegate(string path, int pathLength);


        /// <summary>
        /// Represents a delegate that is used to run the library.
        /// </summary>
        /// <param name="vmPtr">The pointer to the VM.</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void RunDelegate(IntPtr vmPt);

        /// <summary>
        /// Represents a delegate used to select the continue option.
        /// </summary>
        /// <param name="vmPtr">The pointer to the virtual machine.</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SelectContinueDelegate(IntPtr vmPtr);

        /// <summary>
        /// Represents a delegate used to determine if a library can continue.
        /// </summary>
        /// <param name="vmPtr">The pointer to the virtual machine.</param>
        /// <returns><c>true</c> if the library can continue; otherwise, <c>false</c>.</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool CanContinueDelegate(IntPtr vmPtr);

        /// .
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool IsWaitingDelegate(IntPtr vmPtr);

        /// <summary>
        /// Delegate for selecting a choice in the game.
        /// </summary>
        /// <param name="vmPtr">A pointer to the virtual machine.</param>
        /// <param name="index">The index of the choice to select.</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SelectChoiceDelegate(IntPtr vmPtr, int index);

        /// <summary>
        /// Delegate for the TryGetValue function.
        /// </summary>
        /// <param name="vmPtr">The pointer to the virtual machine.</param>
        /// <param name="name">The name of the value.</param>
        /// <param name="nameLength">The length of the name.</param>
        /// <param name="value">The output parameter to store the retrieved value.</param>
        /// <returns>True if the value was successfully retrieved, false otherwise.</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool TryGetValueDelegate(IntPtr vmPtr, string name, int nameLength,
            out TopiValue value);

        /// DestroyValueDelegate
        /// Delegate that represents a function to destroy a Topiary value.
        /// /
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool DestroyValueDelegate(ref TopiValue value);

        /// <summary>
        /// Represents a delegate used to subscribe to events in the Topiary library.
        /// </summary>
        /// <param name="vmPtr">The pointer to the VM.</param>
        /// <param name="name">The name of the event to subscribe to.</param>
        /// <param name="nameLength">The length of the event name.</param>
        /// <param name="callbackPtr">The pointer to the callback function.</param>
        /// <returns>True if the subscription is successful, otherwise false.</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool SubscribeDelegate(IntPtr vmPtr, string name, int nameLength,
            IntPtr callbackPtr);

        /// <summary>
        /// Represents a delegate used for unregistering a callback function from the library.
        /// </summary>
        /// <param name="vmPtr">A pointer to the virtual machine instance.</param>
        /// <param name="name">The name of the callback function.</param>
        /// <param name="nameLength">The length of the callback function name.</param>
        /// <param name="callbackPtr">A pointer to the callback function.</param>
        /// <returns>true if the callback function was successfully unregistered; otherwise, false.</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool UnsubscribeDelegate(IntPtr vmPtr, string name, int nameLength,
            IntPtr callbackPtr);

        /// <summary>
        /// Delegate for the function that sets an external number in the library.
        /// </summary>
        /// <param name="vmPtr">
        /// Pointer to the virtual machine instance.
        /// </param>
        /// <param name="name">
        /// The name of the external number.
        /// </param>
        /// <param name="nameLength">
        /// The length of the name string.
        /// </param>
        /// <param name="value">
        /// The value to set for the external number.
        /// </param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SetExternNumberDelegate(IntPtr vmPtr, string name, int nameLength,
            float value);

        /// <summary>
        /// Delegate for setting an external string in the VM.
        /// </summary>
        /// <param name="vmPtr">Pointer to the VM.</param>
        /// <param name="name">The name of the external string.</param>
        /// <param name="nameLength">The length of the name.</param>
        /// <param name="value">The value of the external string.</param>
        /// <param name="valueLength">The length of the value.</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SetExternStringDelegate(IntPtr vmPtr, string name, int nameLength,
            string value,
            int valueLength);

        /// <summary>
        /// Delegate for setting an extern boolean value.
        /// </summary>
        /// <param name="vmPtr">The pointer to the virtual machine.</param>
        /// <param name="name">The name of the boolean value.</param>
        /// <param name="nameLength">The length of the name.</param>
        /// <param name="value">The new value for the boolean.</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SetExternBoolDelegate(IntPtr vmPtr, string name, int nameLength,
            bool value);

        /// <summary>
        /// Represents a delegate used to set an extern value to nil.
        /// </summary>
        /// <param name="vmPtr">A pointer to the virtual machine.</param>
        /// <param name="name">The name of the extern value to set to nil.</param>
        /// <param name="nameLength">The length of the extern value name.</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SetExternNilDelegate(IntPtr vmPtr, string name, int nameLength);

        /// <summary>
        /// Represents a delegate that can be used to set an external function in the Library class.
        /// </summary>
        /// <param name="vmPtr">The pointer to the virtual machine.</param>
        /// <param name="name">The name of the function.</param>
        /// <param name="nameLength">The length of the name string.</param>
        /// <param name="funcPtr">The pointer to the function implementation.</param>
        /// <param name="arity">The number of arguments the function takes.</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SetExternFuncDelegate(IntPtr vmPtr, string name, int nameLength,
            IntPtr funcPtr,
            byte arity);


        /// <summary>
        /// Represents a delegate used to calculate the size of the state.
        /// </summary>
        /// <param name="vmPtr">A pointer to the virtual machine.</param>
        /// <returns>The size of the state.</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U4)]
        public delegate int CalculateStateSizeDelegate(IntPtr vmPtr);

        /// <summary>
        /// Represents a delegate for saving the state of a virtual machine.
        /// </summary>
        /// <param name="vmPtr">A pointer to the virtual machine.</param>
        /// <param name="output">The output buffer to store the saved state.</param>
        /// <param name="capacity">The capacity of the output buffer.</param>
        /// <returns>The number of bytes written to the output buffer.</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int SaveStateDelegate(IntPtr vmPtr, byte[] output, int capacity);

        /// <summary>
        /// Delegate used to load the state of the library.
        /// </summary>
        /// <param name="vmPtr">Pointer to the virtual machine.</param>
        /// <param name="json">The JSON string representing the state data.</param>
        /// <param name="jsonLength">The length of the JSON string.</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void LoadStateDelegate(IntPtr vmPtr, string json, int jsonLength);

        /// <summary>
        /// Represents a delegate for setting the debug log.
        /// </summary>
        /// <param name="logPtr">The pointer to the debug log function.</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SetDebugLogDelegate(IntPtr logPtr);

        /// SetDebugSeverityDelegate is a delegate type used to set the debug severity level.
        /// It is defined in the PeartreeGames.Topiary.Delegates class.
        /// /
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SetDebugSeverityDelegate(Library.Severity severity);
    }
}