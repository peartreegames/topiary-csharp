using System;
using System.Runtime.InteropServices;

namespace PeartreeGames.Topiary
{
    public static class Delegates
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OnLineDelegate(IntPtr vmPtr, Line line);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OnChoicesDelegate(IntPtr vmPtr, IntPtr choicePtr, byte length);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OutputLogDelegate(IntPtr msgPtr, Library.Severity severity);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate TopiValue ExternFunctionDelegate(IntPtr argPtr, byte length);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Subscriber(ref TopiValue value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr CreateVmDelegate(byte[] source, int sourceLength,
            IntPtr onLinePtr,
            IntPtr onChoicesPtr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void DestroyVmDelegate(IntPtr vmPtr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void StartDelegate(IntPtr vmPtr, string bough, int boughLength);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U4)]
        public delegate int CompileDelegate(string path, int pathLength, byte[] output,
            int capacity);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U4)]
        public delegate int CalculateCompileSizeDelegate(string path, int pathLength);
        

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void RunDelegate(IntPtr vmPt);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SelectContinueDelegate(IntPtr vmPtr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool CanContinueDelegate(IntPtr vmPtr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool IsWaitingDelegate(IntPtr vmPtr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SelectChoiceDelegate(IntPtr vmPtr, int index);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool TryGetValueDelegate(IntPtr vmPtr, string name, int nameLength,
            out TopiValue value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool DestroyValueDelegate(ref TopiValue value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool SubscribeDelegate(IntPtr vmPtr, string name, int nameLength,
            IntPtr callbackPtr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool UnsubscribeDelegate(IntPtr vmPtr, string name, int nameLength,
            IntPtr callbackPtr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SetExternNumberDelegate(IntPtr vmPtr, string name, int nameLength,
            float value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SetExternStringDelegate(IntPtr vmPtr, string name, int nameLength,
            string value,
            int valueLength);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SetExternBoolDelegate(IntPtr vmPtr, string name, int nameLength,
            bool value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SetExternNilDelegate(IntPtr vmPtr, string name, int nameLength);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SetExternFuncDelegate(IntPtr vmPtr, string name, int nameLength,
            IntPtr funcPtr,
            byte arity);
        
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U4)]
        public delegate int CalculateStateSizeDelegate(IntPtr vmPtr);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int SaveStateDelegate(IntPtr vmPtr, byte[] output, int capacity);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void LoadStateDelegate(IntPtr vmPtr, string json, int jsonLength);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SetDebugLogDelegate(IntPtr logPtr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SetDebugSeverityDelegate(Library.Severity severity);
    }
}