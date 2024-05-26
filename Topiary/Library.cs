using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace PeartreeGames.Topiary
{
    /// <summary>
    /// Represents a library of functions and utilities for working with dialogue systems.
    /// </summary>
    public class Library : IDisposable
    {
        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public static int Count => _count;
        
        /// <summary>
        /// Represents a global instance of the <see cref="Library"/> class.
        /// </summary>
        public static Library Global => _global ??= new Library(Log);
        private static SafeHandle _safeHandle = null!;
        private static int _count;
        private static readonly object Lock = new object();

        private static Library? _global;

#if OS_MAC
        /// <summary>
        /// Represents a loader for the PeartreeGames.Topiary library.
        /// </summary>
        public static readonly ILoader Loader = new MacLoader(true);
#elif OS_WINDOWS
        /// <summary>
        /// Represents a loader for the PeartreeGames.Topiary library.
        /// </summary>
        public static readonly ILoader Loader = new WindowsLoader(true);
#endif
        /// <summary>
        /// Represents the severity level of a log message.
        /// </summary>
        public enum Severity : byte
        {
            Debug,
            Info,
            Warn,
            Error
        }


        /// <summary>
        /// Represents a library that provides functionality for working with the Topiary dialogue system.
        /// </summary>
        public Library(Delegates.OutputLogDelegate logger)
        {
            lock (Lock)
            {
                _safeHandle = Loader.Load();
                Interlocked.Increment(ref _count);
                SetDebugLog = CreateDelegate<Delegates.SetDebugLogDelegate>("setDebugLog");
                SetDebugSeverity =
                    CreateDelegate<Delegates.SetDebugSeverityDelegate>("setDebugSeverity");
                _logDelegate = logger;
                SendMessage($"Creating Library_{_count:000}", Severity.Debug);
                var logPtr = Marshal.GetFunctionPointerForDelegate(_logDelegate);
                SetDebugLog(logPtr);

                CreateVm = CreateDelegate<Delegates.CreateVmDelegate>("createVm");
                DestroyVm = CreateDelegate<Delegates.DestroyVmDelegate>("destroyVm");
                Start = CreateDelegate<Delegates.StartDelegate>("start");
                CalculateCompileSize =
                    CreateDelegate<Delegates.CalculateCompileSizeDelegate>("calculateCompileSize");
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
                SetExternNumber =
                    CreateDelegate<Delegates.SetExternNumberDelegate>("setExternNumber");
                SetExternString =
                    CreateDelegate<Delegates.SetExternStringDelegate>("setExternString");
                SetExternBool = CreateDelegate<Delegates.SetExternBoolDelegate>("setExternBool");
                SetExternNil = CreateDelegate<Delegates.SetExternNilDelegate>("setExternNil");
                SetExternFunc = CreateDelegate<Delegates.SetExternFuncDelegate>("setExternFunc");
                CalculateStateSize =
                    CreateDelegate<Delegates.CalculateStateSizeDelegate>("calculateStateSize");
                SaveState = CreateDelegate<Delegates.SaveStateDelegate>("saveState");
                LoadState = CreateDelegate<Delegates.LoadStateDelegate>("loadState");
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

        private void SendMessage(string message, Severity severity)
        {
            var msg = $"[Topiary] {message}\0";
            var msgBytes = Encoding.UTF8.GetBytes(msg);
            var handle = GCHandle.Alloc(msgBytes, GCHandleType.Pinned);
            _logDelegate(handle.AddrOfPinnedObject(), severity);
            handle.Free();
        }

        public static void Log(IntPtr strPtr, Severity severity)
        {
            var msg = PtrToUtf8String(strPtr);
            Console.Write($"{severity}: ");
             switch (severity)
            {
                case Severity.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(msg);
                    break;
                case Severity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(msg);
                    break;
                case Severity.Warn:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(msg);
                    break;
                case Severity.Error:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(msg);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(severity), severity, null);
            }

            Console.ResetColor();
        }

        public readonly Delegates.CreateVmDelegate CreateVm;
        public readonly Delegates.DestroyVmDelegate DestroyVm;
        public readonly Delegates.StartDelegate Start;
        public readonly Delegates.CalculateCompileSizeDelegate CalculateCompileSize;
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
        public readonly Delegates.CalculateStateSizeDelegate CalculateStateSize;
        public readonly Delegates.SaveStateDelegate SaveState;
        public readonly Delegates.LoadStateDelegate LoadState;
        
        public readonly Delegates.SetDebugLogDelegate SetDebugLog;
        public readonly Delegates.SetDebugSeverityDelegate SetDebugSeverity;

        /// <summary>
        /// Releases all resources used by the object.
        /// </summary>
        public void Dispose()
        {
            lock (Lock)
            {
                SendMessage($"Disposing Library_{_count:000}", Severity.Debug);
                Interlocked.Decrement(ref _count);
                if (_count > 0) return;
                if (_safeHandle.IsClosed) return;
                SendMessage("Closing Library handle", Severity.Debug);
                _safeHandle.Dispose();
            }
        }


        /// <summary>
        /// Converts a pointer to a null-terminated UTF8-encoded string into a C# string.
        /// </summary>
        /// <param name="pointer">The pointer to the null-terminated UTF8-encoded string.</param>
        /// <param name="count">Optional: The number of bytes to read from the pointer. Use null to read until null termination.</param>
        /// <returns>The converted C# string.</returns>
        /// <remarks> Since we're targeting .net471 for unity we need to create our own ptr to utf8 it seems </remarks>
        public static string PtrToUtf8String(IntPtr pointer, int? count = null)
        {
            if (count == 0) return string.Empty;
            if (count > 0)
            {
                var len = count.Value;
                var bytes = new byte[len];
                Marshal.Copy(pointer, bytes, 0, len);
                return Encoding.UTF8.GetString(bytes).TrimEnd('\u0000');
            }
            
            var byteList = new List<byte>(64);
            byte readByte;
            var offset = 0;
            do
            {
                readByte = Marshal.ReadByte(pointer, offset);
                if (readByte != 0) byteList.Add(readByte);
                offset++;
            } while (readByte != 0);

            return Encoding.UTF8.GetString( byteList.ToArray());
        }
    }
}