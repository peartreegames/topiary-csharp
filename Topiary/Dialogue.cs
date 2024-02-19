using System;
using System.Runtime.InteropServices;
using IntPtr = System.IntPtr;

namespace PeartreeGames.Topiary
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Dialogue
    {
        [MarshalAs(UnmanagedType.LPUTF8Str)] public readonly string Content;
        [MarshalAs(UnmanagedType.U4)] public readonly int ContentLength;
        [MarshalAs(UnmanagedType.LPUTF8Str)] public readonly string Speaker;
        [MarshalAs(UnmanagedType.U4)] public readonly int SpeakerLength;
        

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
                    result[i] = Library.PtrToUtf8String(ptr);
                    offset += IntPtr.Size;
                }

                return result;
            }
        }
    }
}