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
                SetDebugLog = CreateDelegate<Delegates.SetDebugLogDelegate>("setDebugLog");
                SetDebugSeverity = CreateDelegate<Delegates.SetDebugSeverityDelegate>("setDebugSeverity");
                _logDelegate = Log;

                var logPtr = Marshal.GetFunctionPointerForDelegate(_logDelegate);
                SetDebugLog(logPtr);

                OnDebugLogMessage($"[Topiary] Creating Library_{_count:000}", Severity.Debug);

                CreateVm = CreateDelegate<Delegates.CreateVmDelegate>("createVm");
                DestroyVm = CreateDelegate<Delegates.DestroyVmDelegate>("destroyVm");
                Start = CreateDelegate<Delegates.StartDelegate>("start");
                Compile = CreateDelegate<Delegates.CompileDelegate>("compile");
                Run = CreateDelegate<Delegates.RunDelegate>("run");
                SelectContinue = CreateDelegate<Delegates.SelectContinueDelegate>("selectContinue");
                CanContinue = CreateDelegate<Delegates.CanContinueDelegate>("canContinue");
                IsWaiting = CreateDelegate<Delegates.IsWaitingDelegate>("isWaiting");
                SelectChoice = CreateDelegate<Delegates.SelectChoiceDelegate>("selectChoice");
                TryGetValue = CreateDelegate<Delegates.TryGetValueDelegate>("tryGetValue");
                DestroyValue = CreateDelegate<Delegates.DestroyValueDelegate>("destroyValue");
                Subscribe = CreateDelegate<Delegates.SubscribeDelegate>("subscribe");
                Unsubscribe = CreateDelegate<Delegates.UnsubscribeDelegate>("unsubscribe");
                SetExternNumber = CreateDelegate<Delegates.SetExternNumberDelegate>("setExternNumber");
                SetExternString = CreateDelegate<Delegates.SetExternStringDelegate>("setExternString");
                SetExternBool = CreateDelegate<Delegates.SetExternBoolDelegate>("setExternBool");
                SetExternNil = CreateDelegate<Delegates.SetExternNilDelegate>("setExternNil");
                SetExternFunc = CreateDelegate<Delegates.SetExternFuncDelegate>("setExternFunc");
            }
        }

        private static T CreateDelegate<T>(string name) where T : Delegate
        {
            var procAddr = Loader.GetProc(name);
            if (procAddr == IntPtr.Zero)
                throw new MissingMethodException($"Could not find {name} proc");
            return (T) Marshal.GetDelegateForFunctionPointer(procAddr, typeof(T));
        }

        private readonly Delegates.OutputLogDelegate _logDelegate;

        private static void Log(IntPtr strPtr, Severity severity) =>
            OnDebugLogMessage($"[Topiary] {PtrToUtf8String(strPtr)}", severity);

        public readonly Delegates.CreateVmDelegate CreateVm;
        public readonly Delegates.DestroyVmDelegate DestroyVm;
        public readonly Delegates.StartDelegate Start;
        public readonly Delegates.CompileDelegate Compile;
        public readonly Delegates.RunDelegate Run;
        public readonly Delegates.SelectContinueDelegate SelectContinue;
        public readonly Delegates.CanContinueDelegate CanContinue;
        public readonly Delegates.IsWaitingDelegate IsWaiting;
        public readonly Delegates.SelectChoiceDelegate SelectChoice;
        public readonly Delegates.TryGetValueDelegate TryGetValue;
        public readonly Delegates.DestroyValueDelegate DestroyValue;
        public readonly Delegates.SubscribeDelegate Subscribe;
        public readonly Delegates.UnsubscribeDelegate Unsubscribe;
        public readonly Delegates.SetExternNumberDelegate SetExternNumber;
        public readonly Delegates.SetExternStringDelegate SetExternString;
        public readonly Delegates.SetExternBoolDelegate SetExternBool;
        public readonly Delegates.SetExternNilDelegate SetExternNil;
        public readonly Delegates.SetExternFuncDelegate SetExternFunc;
        public readonly Delegates.SetDebugLogDelegate SetDebugLog;
        public readonly Delegates.SetDebugSeverityDelegate SetDebugSeverity;

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