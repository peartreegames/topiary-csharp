using System;
using System.Runtime.InteropServices;

namespace Topiary
{
    [StructLayout(LayoutKind.Explicit)]
    public struct TopiValueData
    {
        [FieldOffset(0)] [MarshalAs(UnmanagedType.I1)]
        public byte boolValue;

        [FieldOffset(0)] [MarshalAs(UnmanagedType.R4)]
        public float numberValue;

        [FieldOffset(0)] public IntPtr stringValue;

        [FieldOffset(0)] public TopiList listValue;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TopiValue
    {
        [MarshalAs(UnmanagedType.U1)] public Tag tag;
        public TopiValueData data;

        public enum Tag : byte
        {
            Nil,
            Bool,
            Number,
            String,
            List
        }
        
        public object? Value => tag switch
        {
            Tag.Nil => null,
            Tag.Bool => data.boolValue != 0,
            Tag.Number => data.numberValue,
            Tag.String => Marshal.PtrToStringAnsi(data.stringValue),
            Tag.List => data.listValue.Value,
            _ => null
        };
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TopiList
    {
        public IntPtr listPtr;
        [MarshalAs(UnmanagedType.U2)]
        public short count;

        public TopiValue[] Value
        {
            get
            {
                var value = new TopiValue[count];
                var offset = 0;
                for (var i = 0; i < count; i++)
                {
                    var ptr = Marshal.ReadIntPtr(listPtr, offset);
                    value[i] = Marshal.PtrToStructure<TopiValue>(ptr);
                    offset += Marshal.SizeOf<TopiValue>();
                }
                return value;
            }
        }
    }
}