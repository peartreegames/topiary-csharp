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
        private TopiValueData _data;

        public static TopiValue FromPtr(IntPtr ptr) => Marshal.PtrToStructure<TopiValue>(ptr);

        public TopiValue(bool b)
        {
            tag = Tag.Bool;
            _data = new TopiValueData
            {
                boolValue = (byte) (b ? 1 : 0)
            };
        }

        public TopiValue(int i)
        {
            tag = Tag.Number;
            _data = new TopiValueData
            {
                numberValue = i
            };
        }

        public TopiValue(float i)
        {
            tag = Tag.Number;
            _data = new TopiValueData
            {
                numberValue = i
            };
        }

        public TopiValue(string s)
        {
            tag = Tag.String;
            _data = new TopiValueData
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
            ? _data.boolValue == 1
            : throw new InvalidOperationException($"Value {tag} cannot be used as bool");

        public int Int => tag == Tag.Number
            ? Convert.ToInt32(_data.numberValue)
            : throw new InvalidOperationException($"Value {tag} cannot be used as int");

        public float Float => tag == Tag.Number
            ? _data.numberValue
            : throw new InvalidOperationException($"Value {tag} cannot be used as float");

        public string String => tag == Tag.String
            ? Library.PtrToUtf8String(_data.stringValue)
            : throw new InvalidOperationException($"Value {tag} cannot be used as string");

        public TopiValue[] List => tag == Tag.List
            ? _data.listValue.List
            : throw new InvalidOperationException($"Value {tag} cannot be used as list");

        public HashSet<TopiValue> Set => tag == Tag.Set
            ? _data.listValue.Set
            : throw new InvalidOperationException($"Value {tag} cannot be used as set");

        public Dictionary<TopiValue, TopiValue> Map => tag == Tag.Map
            ? _data.listValue.Map
            : throw new InvalidOperationException($"Value {tag} cannot be used as set");

        // Will create boxing, better to use the above is value type is known
        public object? Value => tag switch
        {
            Tag.Bool => _data.boolValue == 1,
            Tag.Number => _data.numberValue,
            Tag.String => Library.PtrToUtf8String(_data.stringValue),
            Tag.List => _data.listValue.List,
            Tag.Set => _data.listValue.Set,
            Tag.Map => _data.listValue.Map,
            _ => null
        };

        public T As<T>() => (T) Convert.ChangeType(Value, typeof(T));

        public override string ToString() =>
            tag switch
            {
                Tag.Bool => _data.boolValue == 1 ? "True" : "False",
                Tag.Number => _data.numberValue.ToString(CultureInfo.CurrentCulture),
                Tag.String => Library.PtrToUtf8String(_data.stringValue),
                Tag.List => $"List{{{string.Join(", ", _data.listValue.List)}}}",
                Tag.Set => $"Set{{{string.Join(", ", _data.listValue.Set)}}}",
                Tag.Map => $"Map{{{string.Join(", ", _data.listValue.Map)}}}",
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
                    return _data.boolValue == other._data.boolValue;
                case Tag.Number:
                    return Math.Abs(_data.numberValue - other._data.numberValue) < 0.0001f;
                case Tag.String:
                    return _data.stringValue == other._data.stringValue;
                case Tag.List:
                case Tag.Set:
                case Tag.Map:
                    return _data.listValue.Equals(other._data.listValue);
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
                    Tag.Bool => _data.boolValue.GetHashCode(),
                    Tag.Number => _data.numberValue.GetHashCode(),
                    Tag.String => _data.stringValue.GetHashCode(),
                    Tag.List => _data.listValue.GetHashCode(),
                    Tag.Set => _data.listValue.GetHashCode(),
                    Tag.Map => _data.listValue.GetHashCode(),
                    _ => -1
                };
            }
        }

        public static bool operator ==(TopiValue left, TopiValue right) => left.Equals(right);
        public static bool operator !=(TopiValue left, TopiValue right) => !(left == right);
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct TopiValueData
    {
        [FieldOffset(0)] [MarshalAs(UnmanagedType.I1)]
        public byte boolValue;

        [FieldOffset(0)] [MarshalAs(UnmanagedType.R4)]
        public float numberValue;

        [FieldOffset(0)] public IntPtr stringValue;

        [FieldOffset(0)] public TopiList listValue;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct TopiList
    {
        public IntPtr listPtr;
        [MarshalAs(UnmanagedType.U2)] public short count;

        internal TopiValue[] List
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

        internal HashSet<TopiValue> Set
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

        internal Dictionary<TopiValue, TopiValue> Map
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