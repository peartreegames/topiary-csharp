using System;
using System.Runtime.InteropServices;
using IntPtr = System.IntPtr;

namespace PeartreeGames.Topiary
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Dialogue
    {
        private readonly IntPtr _contentPtr;
        [MarshalAs(UnmanagedType.U4)] private readonly int _contentLen;
        private readonly IntPtr _speakerPtr;
        [MarshalAs(UnmanagedType.U4)] private readonly int _speakerLen;
        private readonly IntPtr _tagsPtr;
        private readonly byte _tagsLen;

        [NonSerialized] private string _content;
        [NonSerialized] private string _speaker;
        [NonSerialized] private string[] _tags;

        public string Content => _content ??= Library.PtrToUtf8String(_contentPtr, _contentLen);
        public string Speaker => _speaker ??= Library.PtrToUtf8String(_speakerPtr, _speakerLen);

        public string[] Tags
        {
            get
            {
                if (_tags != null) return _tags;
                if (_tagsLen == 0) return Array.Empty<string>();
                var offset = 0;
                _tags = new string[_tagsLen];
                for (var i = 0; i < _tagsLen; i++)
                {
                    var ptr = Marshal.ReadIntPtr(_tagsPtr, offset);
                    _tags[i] = Library.PtrToUtf8String(ptr);
                    offset += IntPtr.Size;
                }
                return _tags;
            }
        }
    }
}