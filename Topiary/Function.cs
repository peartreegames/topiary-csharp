using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Topiary
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Function
    {
        [FieldOffset(0)] private Func<TopiValue> _func;
        [FieldOffset(0)] private Func<TopiValue, TopiValue> _func1;
        [FieldOffset(0)] private Func<TopiValue, TopiValue, TopiValue> _func2;
        [FieldOffset(0)] private Func<TopiValue, TopiValue, TopiValue, TopiValue> _func3;
        [FieldOffset(0)] private Func<TopiValue, TopiValue, TopiValue, TopiValue, TopiValue> _func4;

        [FieldOffset(0)] private Action _action;
        [FieldOffset(0)] private Action<TopiValue> _action1;
        [FieldOffset(0)] private Action<TopiValue, TopiValue> _action2;
        [FieldOffset(0)] private Action<TopiValue, TopiValue, TopiValue> _action3;
        [FieldOffset(0)] private Action<TopiValue, TopiValue, TopiValue, TopiValue> _action4;

        [FieldOffset(32)] private bool _hasReturn;

        public static Function Create(Func<TopiValue> func) =>
            new Function {_func = func, _hasReturn = true};

        public static Function Create(Func<TopiValue, TopiValue> func) =>
            new Function {_func1 = func, _hasReturn = true};

        public static Function Create(Func<TopiValue, TopiValue, TopiValue> func) =>
            new Function {_func2 = func, _hasReturn = true};

        public static Function Create(Func<TopiValue, TopiValue, TopiValue, TopiValue> func) =>
            new Function {_func3 = func, _hasReturn = true};

        public static Function Create(Func<TopiValue, TopiValue, TopiValue, TopiValue, TopiValue> func) =>
            new Function {_func4 = func, _hasReturn = true};

        public static Function Create(Action action) => new Function {_action = action};

        public static Function Create(Action<TopiValue> action) =>
            new Function {_action1 = action};

        public static Function Create(Action<TopiValue, TopiValue> action) =>
            new Function {_action2 = action};

        public static Function Create(Action<TopiValue, TopiValue, TopiValue> action) =>
            new Function {_action3 = action};

        public static Function Create(Action<TopiValue, TopiValue, TopiValue, TopiValue> action) =>
            new Function {_action4 = action};

        public TopiValue Call(IntPtr argPtr, byte count)
        {
            var args = CreateArgs(argPtr, count);
            if (_hasReturn) return FuncCall(args);
            ActionCall(args);
            return default;
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

        private TopiValue FuncCall(IReadOnlyList<TopiValue> args)
        {
            var count = args.Count;
            return count switch
            {
                0 => _func(),
                1 => _func1(args[0]),
                2 => _func2(args[0], args[1]),
                3 => _func3(args[0], args[1], args[2]),
                4 => _func4(args[0], args[1], args[2], args[3]),
                _ => throw new ArgumentOutOfRangeException(nameof(count), "The parameter count is unsupported")
            };
        }

        private void ActionCall(IReadOnlyList<TopiValue> args)
        {
            var count = args.Count;
            switch (count)
            {
                case 0:
                    _action();
                    break;
                case 1:
                    _action1(args[0]);
                    break;
                case 2:
                    _action2(args[0], args[1]);
                    break;
                case 3:
                    _action3(args[0], args[1], args[2]);
                    break;
                case 4:
                    _action4(args[0], args[1], args[2], args[3]);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(count), "The parameter count is unsupported");
            }
        }

        public static Library.ExternFunctionDelegate CreateFromMethodInfo(MethodInfo method)
        {
            var parameters = method.GetParameters();
            var hasReturn = method.ReturnType == typeof(void);
            var delegateType = parameters.Length switch
            {
                0 => hasReturn ? typeof(Action) : typeof(Func<TopiValue>),
                1 => hasReturn
                    ? typeof(Action<TopiValue>)
                    : typeof(Func<TopiValue, TopiValue>),
                2 => hasReturn
                    ? typeof(Action<TopiValue, TopiValue>)
                    : typeof(Func<TopiValue, TopiValue, TopiValue>),
                3 => hasReturn
                    ? typeof(Action<TopiValue, TopiValue, TopiValue>)
                    : typeof(Func<TopiValue, TopiValue, TopiValue, TopiValue>),
                4 => hasReturn
                    ? typeof(Action<TopiValue, TopiValue, TopiValue, TopiValue>)
                    : typeof(Func<TopiValue, TopiValue, TopiValue, TopiValue, TopiValue>),
                _ => throw new NotSupportedException("Unsupported number of parameters")
            };

            var myDelegate = Delegate.CreateDelegate(delegateType, method);

            return ((Function) Create((dynamic) myDelegate)).Call;
        }
    }
}