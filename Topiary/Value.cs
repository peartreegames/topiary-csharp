using System;
using System.Collections.Generic;
using System.Linq;
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
        [FieldOffset(0)] public TopiSet setValue;
        [FieldOffset(0)] public TopiMap mapValue;
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
            Tag.Nil => null,
            Tag.Bool => data.boolValue != 0,
            Tag.Number => data.numberValue,
            Tag.String => Marshal.PtrToStringAnsi(data.stringValue),
            Tag.List => data.listValue.Value,
            Tag.Set => data.setValue.Value,
            Tag.Map => data.mapValue.Value,
            _ => null
        };

        public override string ToString() => tag switch
        {
            Tag.Nil => "Nil",
            Tag.List => $"[{string.Join(",", data.listValue.Value)}]",
            Tag.Set => $"{{{string.Join(",", data.setValue.Value)}}}",
            Tag.Map => $"{{{string.Join(", ", data.mapValue.Value.Select(kvp => kvp.Key + ": " + kvp.Value))} }}",
            _ => Value.ToString(),
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
                    value[i] = Marshal.PtrToStructure<TopiValue>(listPtr + offset);
                    offset += Marshal.SizeOf<TopiValue>();
                }
                return value;
            }
        }
    }
    

    [StructLayout(LayoutKind.Sequential)]
    public struct TopiSet
    {
        public IntPtr ptr;
        [MarshalAs(UnmanagedType.U2)]
        public short count;

        public HashSet<TopiValue> Value
        {
            get
            {
                var value = new HashSet<TopiValue>();
                var offset = 0;
                for (var i = 0; i < count; i++)
                {
                    value.Add(Marshal.PtrToStructure<TopiValue>(ptr + offset));
                    offset += Marshal.SizeOf<TopiValue>();
                }
                return value;
            }
        }
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct TopiMap
    {
        public IntPtr ptr;
        [MarshalAs(UnmanagedType.U2)]
        public short count;

        public Dictionary<TopiValue, TopiValue> Value
        {
            get
            {
                var value = new Dictionary<TopiValue, TopiValue>();
                var offset = 0;
                var size = Marshal.SizeOf<TopiValue>();
                for (var i = 0; i < count; i++)
                {
                    var key = Marshal.PtrToStructure<TopiValue>(ptr + offset);
                    offset += size; 
                    value.Add(key, Marshal.PtrToStructure<TopiValue>(ptr + offset));
                    offset += size;
                }
                return value;
            }
        }
    }
    
    
}