using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PeartreeGames.Topiary
{
    /// <summary>
    /// Represents a function that can be called dynamically.
    /// </summary>
    public class Function : IDisposable
    {
        private readonly Delegate _delegate;

        private readonly Delegates.ExternFunctionDelegate _callDel;

        private GCHandle _handle;
        private GCHandle _callHandle;

        public Function(Delegate del)
        {
            _delegate = del;
            _callDel = Call;
            if (Library.IsUnityRuntime)
            {
                _handle = GCHandle.Alloc(_delegate, GCHandleType.Pinned);
                _callHandle = GCHandle.Alloc(_callDel, GCHandleType.Pinned); 
            }
        }

        /// <summary>
        /// Returns the function pointer for the GetCallIntPtr method.
        /// </summary>
        /// <returns>The function pointer for the GetCallIntPtr method.</returns>
        public IntPtr GetCallIntPtr() => Marshal.GetFunctionPointerForDelegate(_callDel);

        /// <summary>
        /// Disposes of the resources used by the Function instance.
        /// </summary>
        public void Dispose()
        {
            if (_handle.IsAllocated) _handle.Free();
            if (_callHandle.IsAllocated) _callHandle.Free();
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

        /// <summary>
        /// Converts the Function object to its string representation.
        /// </summary>
        /// <returns>
        /// A string that represents the current Function object.
        /// </returns>
        public override string ToString() => $"Function {_delegate.Method.Name}";

        /// <summary>
        /// Executes the delegate stored in the Function object.
        /// </summary>
        /// <param name="argPtr">A pointer to the arguments for the delegate.</param>
        /// <param name="count">The number of arguments.</param>
        /// <returns>The result of executing the delegate.</returns>
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

        /// <summary>
        /// Creates an array of TopiValue objects from the given IntPtr and count.
        /// </summary>
        /// <param name="argPtr">The IntPtr pointing to the start of the memory block containing the TopiValues.</param>
        /// <param name="count">The number of TopiValues to create.</param>
        /// <returns>An array of TopiValue objects.</returns>
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

        /// <summary>
        /// Creates a <see cref="Function"/> object based on the given <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="method">The <see cref="MethodInfo"/> representing the method.</param>
        /// <returns>A new instance of <see cref="Function"/> created from the method.</returns>
        /// <exception cref="NotSupportedException">Thrown when the number of parameters is not supported.</exception>
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