using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace PeartreeGames.Topiary
{
    /// <summary>
    /// Topiary Value container
    /// Data is overlapped in memory so ensure correct value is used
    /// or check tag if unknown
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TopiValue : IDisposable, IEquatable<TopiValue>
    {
        [MarshalAs(UnmanagedType.U1)] public Tag tag;
        public TopiValueData data;

        public static TopiValue FromPtr(IntPtr ptr) => Marshal.PtrToStructure<TopiValue>(ptr);

        public TopiValue(bool b)
        {
            tag = Tag.Bool;
            data = new TopiValueData
            {
                boolValue = (byte) (b ? 1 : 0)
            };
        }

        public TopiValue(int i)
        {
            tag = Tag.Number;
            data = new TopiValueData
            {
                numberValue = i
            };
        }

        public TopiValue(float i)
        {
            tag = Tag.Number;
            data = new TopiValueData
            {
                numberValue = i
            };
        }

        public TopiValue(string s)
        {
            tag = Tag.String;
            data = new TopiValueData
            {
                stringValue = Marshal.StringToHGlobalAnsi(s)
            };
        }

        public enum Tag : byte
        {
            Nil,
            Bool,
            Number,
            String,
            List,
            Set,
            Map
        }


        public bool Bool => tag == Tag.Bool
            ? data.boolValue == 1
            : throw new InvalidOperationException($"Value {tag} cannot be used as bool");

        public int Int => tag == Tag.Number
            ? Convert.ToInt32(data.numberValue)
            : throw new InvalidOperationException($"Value {tag} cannot be used as int");

        public float Float => tag == Tag.Number
            ? data.numberValue
            : throw new InvalidOperationException($"Value {tag} cannot be used as float");

        public string String => tag == Tag.String
            ? Library.PtrToUtf8String(data.stringValue)
            : throw new InvalidOperationException($"Value {tag} cannot be used as string");

        public TopiValue[] List => tag == Tag.List
            ? data.listValue.List
            : throw new InvalidOperationException($"Value {tag} cannot be used as list");

        public HashSet<TopiValue> Set => tag == Tag.Set
            ? data.listValue.Set
            : throw new InvalidOperationException($"Value {tag} cannot be used as set");

        public Dictionary<TopiValue, TopiValue> Map => tag == Tag.Map
            ? data.listValue.Map
            : throw new InvalidOperationException($"Value {tag} cannot be used as set");

        // Will create boxing, better to use the above is value type is known
        public object? Value => tag switch
        {
            Tag.Bool => data.boolValue == 1,
            Tag.Number => data.numberValue,
            Tag.String => Library.PtrToUtf8String(data.stringValue),
            Tag.List => data.listValue.List,
            Tag.Set => data.listValue.Set,
            Tag.Map => data.listValue.Map,
            _ => null
        };

        public T As<T>() => (T) Convert.ChangeType(Value, typeof(T));

        public override string ToString() =>
            tag switch
            {
                Tag.Bool => data.boolValue == 1 ? "True" : "False",
                Tag.Number => data.numberValue.ToString(CultureInfo.CurrentCulture),
                Tag.String => Library.PtrToUtf8String(data.stringValue),
                Tag.List => $"[{string.Join(", ", data.listValue.List)}]",
                Tag.Set => $"{{{string.Join(", ", data.listValue.Set)}}}",
                Tag.Map => $"{{{string.Join(", ", data.listValue.Map)}}}",
                _ => $"{tag}: null"
            } ?? throw new InvalidOperationException();

        public void Dispose()
        {
            Library.Global.DestroyValue(ref this);
        }

        public bool Equals(TopiValue other)
        {
            if (tag != other.tag) return false;
            switch (tag)
            {
                case Tag.Bool:
                    return data.boolValue == other.data.boolValue;
                case Tag.Number:
                    return Math.Abs(data.numberValue - other.data.numberValue) < 0.0001f;
                case Tag.String:
                    return data.stringValue == other.data.stringValue;
                case Tag.List:
                case Tag.Set:
                case Tag.Map:
                    return data.listValue.Equals(other.data.listValue);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override bool Equals(object? obj) => obj is TopiValue other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) tag * 397) ^ tag switch
                {
                    Tag.Nil => 0,
                    Tag.Bool => data.boolValue.GetHashCode(),
                    Tag.Number => data.numberValue.GetHashCode(),
                    Tag.String => data.stringValue.GetHashCode(),
                    Tag.List => data.listValue.GetHashCode(),
                    Tag.Set => data.listValue.GetHashCode(),
                    Tag.Map => data.listValue.GetHashCode(),
                    _ => -1
                };
            }
        }

        public static bool operator ==(TopiValue left, TopiValue right) => left.Equals(right);
        public static bool operator !=(TopiValue left, TopiValue right) => !(left == right);
    }

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
                    value[i] = TopiValue.FromPtr(ptr);
                    ptr = IntPtr.Add(ptr, Marshal.SizeOf<TopiValue>());
                }

                return value;
            }
        }

        public HashSet<TopiValue> Set
        {
            get
            {
                var set = new HashSet<TopiValue>();
                var ptr = listPtr;
                for (var i = 0; i < count; i++)
                {
                    set.Add(TopiValue.FromPtr(ptr));
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
                    var key = TopiValue.FromPtr(ptr);
                    ptr = IntPtr.Add(ptr, Marshal.SizeOf<TopiValue>());
                    var value = TopiValue.FromPtr(ptr);
                    ptr = IntPtr.Add(ptr, Marshal.SizeOf<TopiValue>());
                    map.Add(key, value);
                }

                return map;
            }
        }
    }
}