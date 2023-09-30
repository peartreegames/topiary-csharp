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
            String
        }
        
        public object? Value => tag switch
        {
            Tag.Nil => null,
            Tag.Bool => data.boolValue != 0,
            Tag.Number => data.numberValue,
            Tag.String => Marshal.PtrToStringAnsi(data.stringValue),
            _ => null
        };
    }
}