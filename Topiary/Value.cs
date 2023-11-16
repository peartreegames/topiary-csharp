using System;
using System.Collections.Generic;
using System.Globalization;
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
            List,
            Set,
            Map,
        }

        public object? Value => tag switch
        {
            Tag.Bool => data.boolValue == 1,
            Tag.Number => data.numberValue,
            Tag.String => Marshal.PtrToStringAnsi(data.stringValue),
            Tag.List => data.listValue.List,
            Tag.Set => data.listValue.Set,
            Tag.Map => data.listValue.Map,
            _ => null 
        };

        public override string ToString() =>
            tag switch
            {
                Tag.Bool => data.boolValue == 1 ? "True" : "False",
                Tag.Number => data.numberValue.ToString(CultureInfo.CurrentCulture),
                Tag.String => Marshal.PtrToStringAnsi(data.stringValue),
                Tag.List => $"[{string.Join(", ", data.listValue.List)}]",
                Tag.Set => $"{{{string.Join(", ", data.listValue.Set)}}}",
                Tag.Map => $"{{{string.Join(", ", data.listValue.Map)}}}",
                _ => $"{tag}: null"
            } ?? throw new InvalidOperationException();
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TopiList
    {
        public IntPtr listPtr;
        [MarshalAs(UnmanagedType.U2)] public short count;

        public TopiValue[] List 
        {
            get
            {
                var value = new TopiValue[count];
                var ptr = listPtr;
                for (var i = 0; i < count; i++)
                {
                    value[i] = Marshal.PtrToStructure<TopiValue>(ptr);
                    ptr = IntPtr.Add(ptr, Marshal.SizeOf<TopiValue>());
                }
                return value;
            }
        }
        
        public HashSet<TopiValue> Set
        {
            get
            {
                var set = new HashSet<TopiValue>(count);
                var ptr = listPtr;
                for (var i = 0; i < count; i++)
                {
                    set.Add(Marshal.PtrToStructure<TopiValue>(ptr));
                    ptr = IntPtr.Add(ptr, Marshal.SizeOf<TopiValue>());
                }
                return set;
            }
        }
        
        public Dictionary<TopiValue, TopiValue> Map
        {
            get
            {
                var map = new Dictionary<TopiValue, TopiValue>(count);
                var ptr = listPtr;
                for (var i = 0; i < count; i++)
                {
                    var key = Marshal.PtrToStructure<TopiValue>(ptr);
                    ptr = IntPtr.Add(ptr, Marshal.SizeOf<TopiValue>());
                    var value = Marshal.PtrToStructure<TopiValue>(ptr);
                    ptr = IntPtr.Add(ptr, Marshal.SizeOf<TopiValue>());
                    map.Add(key, value);
                }
                return map;
            }
        }
        
        
    }
}