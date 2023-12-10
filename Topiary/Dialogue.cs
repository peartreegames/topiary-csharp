using System;
using System.Runtime.InteropServices;
using IntPtr = System.IntPtr;

namespace Topiary
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Dialogue
    {
        [MarshalAs(UnmanagedType.LPUTF8Str)] public readonly string Content;
        [MarshalAs(UnmanagedType.LPUTF8Str)] public readonly string Speaker;

        private readonly IntPtr _tagsPtr;
        private readonly byte _tagsLen;

        public string[] Tags
        {
            get
            {
                if (_tagsLen == 0) return Array.Empty<string>();
                var offset = 0;
                var result = new string[_tagsLen];
                for (var i = 0; i < _tagsLen; i++)
                {
                    var ptr = Marshal.ReadIntPtr(_tagsPtr, offset);
                    result[i] = Marshal.PtrToStringUTF8(ptr) ?? string.Empty;
                    offset += IntPtr.Size;
                }

                return result;
            }
        }
    }
}