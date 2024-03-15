using System;
using System.Runtime.InteropServices;

namespace PeartreeGames.Topiary
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Choice
    {
        private readonly IntPtr _contentPtr;
        [MarshalAs(UnmanagedType.U4)] private readonly int _contentLen;
        private readonly IntPtr _tagsPtr;
        private readonly byte _tagsLen;
        [MarshalAs(UnmanagedType.U4)] private readonly int _visitCount;
        [MarshalAs(UnmanagedType.U4)] private readonly int _ip;

        public int VisitCount => _visitCount;
        public int Ip => _ip;
        public string Content => Library.PtrToUtf8String(_contentPtr);

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

        public static Choice[] MarshalPtr(IntPtr choicePtr, byte count)
        {
            var choices = new Choice[count];
            var ptr = choicePtr;
            for (var i = 0; i < count; i++)
            {
                choices[i] = Marshal.PtrToStructure<Choice>(ptr);
                ptr = IntPtr.Add(ptr, Marshal.SizeOf<Choice>());
            }

            return choices;
        }
    }
}