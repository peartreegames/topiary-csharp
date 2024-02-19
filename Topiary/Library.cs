using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace PeartreeGames.Topiary
{
    public class Library : IDisposable
    {
        public static int Count => _count;
        public static Action<string, Severity> OnDebugLogMessage = delegate { };
        public static Library Global => _global ??= new Library();
        public static bool IsUnityRuntime { get; set; }
        public static IAllocator Allocator { get; set; } = ManagedAllocator.Global;

        private static SafeHandle _safeHandle = null!;
        private static GCHandle _logHandle;
        private static int _count;
        private static readonly object Lock = new object();

        private static Library? _global;

#if OS_MAC
        public static readonly ILoader Loader = new MacLoader(true);
#elif OS_WINDOWS
        public static readonly ILoader Loader = new WindowsLoader(true);
#endif
        public enum Severity : byte
        {
            Debug,
            Info,
            Warn,
            Error
        }

        public Library()
        {
            lock (Lock)
            {
                _safeHandle = Loader.Load();
                Interlocked.Increment(ref _count);
                SetDebugLog = CreateDelegate<SetDebugLogDelegate>("setDebugLog");
                SetDebugSeverity = CreateDelegate<SetDebugSeverityDelegate>("setDebugSeverity");
                OutputLogDelegate l = Log;
                if (IsUnityRuntime) _logHandle = GCHandle.Alloc(l, GCHandleType.Pinned);

                var logPtr = Marshal.GetFunctionPointerForDelegate(l);
                SetDebugLog(logPtr);

                OnDebugLogMessage($"[Topiary] Creating Library_{_count:000}", Severity.Debug);

                CreateVm = CreateDelegate<CreateVmDelegate>("createVm");
                DestroyVm = CreateDelegate<DestroyVmDelegate>("destroyVm");
                Start = CreateDelegate<StartDelegate>("start");
                Compile = CreateDelegate<CompileDelegate>("compile");
                Run = CreateDelegate<RunDelegate>("run");
                SelectContinue = CreateDelegate<SelectContinueDelegate>("selectContinue");
                CanContinue = CreateDelegate<CanContinueDelegate>("canContinue");
                IsWaiting = CreateDelegate<IsWaitingDelegate>("isWaiting");
                SelectChoice = CreateDelegate<SelectChoiceDelegate>("selectChoice");
                TryGetValue = CreateDelegate<TryGetValueDelegate>("tryGetValue");
                DestroyValue = CreateDelegate<DestroyValueDelegate>("destroyValue");
                Subscribe = CreateDelegate<SubscribeDelegate>("subscribe");
                Unsubscribe = CreateDelegate<UnsubscribeDelegate>("unsubscribe");
                SetExternNumber = CreateDelegate<SetExternNumberDelegate>("setExternNumber");
                SetExternString = CreateDelegate<SetExternStringDelegate>("setExternString");
                SetExternBool = CreateDelegate<SetExternBoolDelegate>("setExternBool");
                SetExternNil = CreateDelegate<SetExternNilDelegate>("setExternNil");
                SetExternFunc = CreateDelegate<SetExternFuncDelegate>("setExternFunc");
            }
        }

        private static T CreateDelegate<T>(string name) where T : Delegate
        {
            var procAddr = Loader.GetProc(name);
            if (procAddr == IntPtr.Zero)
                throw new MissingMethodException($"Could not find {name} proc");
            return (T) Marshal.GetDelegateForFunctionPointer(procAddr, typeof(T));
        }

        private static void Log(IntPtr strPtr, Severity severity) =>
            OnDebugLogMessage($"[Topiary] {PtrToUtf8String(strPtr)}", severity);

        public readonly CreateVmDelegate CreateVm;
        public readonly DestroyVmDelegate DestroyVm;
        public readonly StartDelegate Start;
        public readonly CompileDelegate Compile;
        public readonly RunDelegate Run;
        public readonly SelectContinueDelegate SelectContinue;
        public readonly CanContinueDelegate CanContinue;
        public readonly IsWaitingDelegate IsWaiting;
        public readonly SelectChoiceDelegate SelectChoice;
        public readonly TryGetValueDelegate TryGetValue;
        public readonly DestroyValueDelegate DestroyValue;
        public readonly SubscribeDelegate Subscribe;
        public readonly UnsubscribeDelegate Unsubscribe;
        public readonly SetExternNumberDelegate SetExternNumber;
        public readonly SetExternStringDelegate SetExternString;
        public readonly SetExternBoolDelegate SetExternBool;
        public readonly SetExternNilDelegate SetExternNil;
        public readonly SetExternFuncDelegate SetExternFunc;
        public readonly SetDebugLogDelegate SetDebugLog;
        public readonly SetDebugSeverityDelegate SetDebugSeverity;

        public void Dispose()
        {
            lock (Lock)
            {
                OnDebugLogMessage($"[Topiary] Disposing Library_{_count:000}", Severity.Debug);
                Interlocked.Decrement(ref _count);
                if (_count > 0) return;
                if (_safeHandle.IsClosed) return;
                OnDebugLogMessage($"[Topiary] Closing Library handle", Severity.Debug);
                _safeHandle.Dispose();
                if (_logHandle.IsAllocated) _logHandle.Free();
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OnDialogueDelegate(IntPtr vmPtr, Dialogue dialogue);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OnChoicesDelegate(IntPtr vmPtr, IntPtr choicePtr, byte length);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void OutputLogDelegate(IntPtr msgPtr, Severity severity);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate TopiValue ExternFunctionDelegate(IntPtr argPtr, byte length);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Subscriber(ref TopiValue value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr CreateVmDelegate(byte[] source, int sourceLength,
            IntPtr onDialoguePtr,
            IntPtr onChoicesPtr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void DestroyVmDelegate(IntPtr vmPtr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void StartDelegate(IntPtr vmPtr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CompileDelegate(string path, int pathLength, byte[] output,
            int capacity);

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
        public delegate void SetDebugLogDelegate(IntPtr logPtr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SetDebugSeverityDelegate(Severity severity);

        // Since we're targeting .net471 for unity we need to create our own ptr<>utf8?
        public static string PtrToUtf8String(IntPtr pointer)
        {
            var byteList = new List<byte>(64);
            byte readByte;
            var offset = 0;
            do
            {
                readByte = Marshal.ReadByte(pointer, offset);
                if (readByte != 0)
                {
                    byteList.Add(readByte);
                }

                offset++;
            } while (readByte != 0);

            var byteArray = byteList.ToArray();
            var result = System.Text.Encoding.UTF8.GetString(byteArray);
            return result;
        }
    }
}