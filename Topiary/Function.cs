using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PeartreeGames.Topiary
{
    public class Function : IDisposable
    {
        private readonly Delegate _delegate;
        private GCHandle _handle;

        public Function(Delegate del)
        {
            _delegate = del;
            if (Library.IsUnityRuntime) _handle = GCHandle.Alloc(_delegate, GCHandleType.Pinned);
        }

        public void Dispose()
        {
            if (_handle.IsAllocated) _handle.Free();
        }

        public delegate TopiValue FuncDel();

        public delegate TopiValue FuncDel1(TopiValue value1);

        public delegate TopiValue FuncDel2(TopiValue value1, TopiValue value2);

        public delegate TopiValue FuncDel3(TopiValue value1, TopiValue value2, TopiValue value3);

        public delegate TopiValue FuncDel4(TopiValue value1, TopiValue value2, TopiValue value3,
            TopiValue value4);

        public delegate void ActionDel();

        public delegate void ActionDel1(TopiValue value1);

        public delegate void ActionDel2(TopiValue value1, TopiValue value2);

        public delegate void ActionDel3(TopiValue value1, TopiValue value2, TopiValue value3);

        public delegate void ActionDel4(TopiValue value1, TopiValue value2, TopiValue value3,
            TopiValue value4);


        public override string ToString() => $"Function {_delegate.Method.Name}";

        public TopiValue Call(IntPtr argPtr, byte count)
        {
            var args = CreateArgs(argPtr, count);
            switch (_delegate)
            {
                case ActionDel a:
                    a();
                    return default;
                case ActionDel1 a1:
                    a1(args[0]);
                    return default;
                case ActionDel2 a2:
                    a2(args[0], args[1]);
                    return default;
                case ActionDel3 a3:
                    a3(args[0], args[1], args[2]);
                    return default;
                case ActionDel4 a4:
                    a4(args[0], args[1], args[2], args[3]);
                    return default;
                case FuncDel a:
                    return a();
                case FuncDel1 a1:
                    return a1(args[0]);
                case FuncDel2 a2:
                    return a2(args[0], args[1]);
                case FuncDel3 a3:
                    return a3(args[0], args[1], args[2]);
                case FuncDel4 a4:
                    return a4(args[0], args[1], args[2], args[3]);
                default:
                    throw new Exception($"Unsupported Delegate type {_delegate}");
            }
        }

        private static TopiValue[] CreateArgs(IntPtr argPtr, byte count)
        {
            var args = new TopiValue[count];
            var ptr = argPtr;
            for (var i = 0; i < count; i++)
            {
                args[i] = TopiValue.FromPtr(ptr);
                ptr = IntPtr.Add(ptr, Marshal.SizeOf<TopiValue>());
            }

            return args;
        }

        public static Function Create(MethodInfo method)
        {
            var parameters = method.GetParameters();
            var hasNoReturn = method.ReturnType == typeof(void);
            var delegateType = parameters.Length switch
            {
                0 => hasNoReturn ? typeof(ActionDel) : typeof(FuncDel),
                1 => hasNoReturn ? typeof(ActionDel1) : typeof(FuncDel1),
                2 => hasNoReturn ? typeof(ActionDel2) : typeof(FuncDel2),
                3 => hasNoReturn ? typeof(ActionDel3) : typeof(FuncDel3),
                4 => hasNoReturn ? typeof(ActionDel4) : typeof(FuncDel4),
                _ => throw new NotSupportedException("Unsupported number of parameters")
            };
            dynamic del = method.CreateDelegate(delegateType);
            return new Function(del);
        }
    }
}