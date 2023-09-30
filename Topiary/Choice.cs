using System.Runtime.InteropServices;

namespace Topiary
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Choice
    {
        [MarshalAs(UnmanagedType.LPStr)] public readonly string Content;
        [MarshalAs(UnmanagedType.U1)] public readonly byte Count;
        [MarshalAs(UnmanagedType.U4)] public readonly int Ip;
    }
}
