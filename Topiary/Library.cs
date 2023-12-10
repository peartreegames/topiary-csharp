using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Topiary
{
    public class Library : IDisposable
    {
        private static IntPtr _libPtr;
        private static int _count;
        private static readonly object Lock = new object();
#if OS_MAC
        private static readonly ILoader Loader = new MacLoader(); 
#elif OS_WINDOWS
        private static readonly ILoader Loader = new WindowsLoader(); 
#endif

        public Library()
        {
            if (!File.Exists(Loader.DllPath)) throw new Exception($"DllPath not found at {Loader.DllPath}");
            lock (Lock)
            {
                _libPtr = Loader.Load();
                if (_libPtr == IntPtr.Zero) throw new System.ComponentModel.Win32Exception();
                Interlocked.Increment(ref _count);
            }

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

        private static T CreateDelegate<T>(string name) where T : Delegate
        {
            var procAddr = Loader.GetProc(_libPtr, name);
            if (procAddr == IntPtr.Zero) throw new Exception($"Could not find {name} proc");
            return (T)Marshal.GetDelegateForFunctionPointer(procAddr, typeof(T));
        }

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

        public void Dispose()
        {
            lock (Lock)
            {
                Interlocked.Decrement(ref _count);
                if (_count > 0) return;
                Loader.Free(_libPtr);
                _libPtr = IntPtr.Zero;
            }
        }

        public delegate void OnDialogueDelegate(IntPtr vmPtr, Dialogue dialogue);

        public delegate void OnChoicesDelegate(IntPtr vmPtr, IntPtr choicePtr, byte length);

        public delegate TopiValue ExternFunctionDelegate(IntPtr argPtr, byte length);

        public delegate void Subscriber(ref TopiValue value);

        public delegate IntPtr CreateVmDelegate(byte[] source, int sourceLength, IntPtr onDialoguePtr,
            IntPtr onChoicesPtr);

        public delegate void DestroyVmDelegate(IntPtr vmPtr);

        public delegate void StartDelegate(IntPtr vmPtr);

        public delegate void CompileDelegate(byte[] source, int sourceLength, byte[] output, int capacity);

        public delegate void RunDelegate(IntPtr vmPtr, out int errLine, StringBuilder errMsg, int capacity);

        public delegate void SelectContinueDelegate(IntPtr vmPtr);

        public delegate bool CanContinueDelegate(IntPtr vmPtr);

        public delegate bool IsWaitingDelegate(IntPtr vmPtr);

        public delegate void SelectChoiceDelegate(IntPtr vmPtr, int index);

        public delegate bool TryGetValueDelegate(IntPtr vmPtr, string name, int nameLength, out TopiValue value);

        public delegate bool DestroyValueDelegate(ref TopiValue value);

        public delegate void SubscribeDelegate(IntPtr vmPtr, string name, int nameLength, IntPtr callbackPtr);

        public delegate void UnsubscribeDelegate(IntPtr vmPtr, string name, int nameLength, IntPtr callbackPtr);

        public delegate void SetExternNumberDelegate(IntPtr vmPtr, string name, int nameLength, float value);

        public delegate void SetExternStringDelegate(IntPtr vmPtr, string name, int nameLength, string value,
            int valueLength);

        public delegate void SetExternBoolDelegate(IntPtr vmPtr, string name, int nameLength, bool value);

        public delegate void SetExternNilDelegate(IntPtr vmPtr, string name, int nameLength);

        public delegate void SetExternFuncDelegate(IntPtr vmPtr, string name, int nameLength, IntPtr funcPtr,
            byte arity);
    }
}