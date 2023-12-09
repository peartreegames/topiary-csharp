using System;
using System.Runtime.InteropServices;

namespace Topiary
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Choice
    {
        [MarshalAs(UnmanagedType.LPUTF8Str)] public readonly string Content;
        [MarshalAs(UnmanagedType.U4)] public readonly int Count;
        [MarshalAs(UnmanagedType.U4)] internal readonly int Ip;

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