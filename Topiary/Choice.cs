using System;
using System.Runtime.InteropServices;

namespace Topiary
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Choice
    {
        [MarshalAs(UnmanagedType.LPStr)] public readonly string Content;
        [MarshalAs(UnmanagedType.U1)] public readonly byte Count;
        [MarshalAs(UnmanagedType.U4)] public readonly int Ip;

        public readonly struct Wrapper
        {
            private readonly Library.OnChoicesDelegate _onChoices;
            public Wrapper(Library.OnChoicesDelegate onChoices) => _onChoices = onChoices;

            public void OnChoices(IntPtr vmPtr, IntPtr choicePtr, byte count)
            {
                var choices = new Choice[count];
                var ptr = choicePtr;
                for (var i = 0; i < count; i++)
                {
                    choices[i] = Marshal.PtrToStructure<Choice>(ptr);
                    ptr = IntPtr.Add(ptr, Marshal.SizeOf<Choice>());
                }

                _onChoices.Invoke(vmPtr, choices);
            }
        }
    }
}