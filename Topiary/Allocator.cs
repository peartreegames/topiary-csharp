using System;
using System.Runtime.InteropServices;

namespace PeartreeGames.Topiary
{
    // Currently unused
    public interface IAllocator
    {
        IntPtr Allocate(ulong size);
        void Deallocate(IntPtr ptr);
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr AllocateDelegate(ulong size);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DeallocateDelegate(IntPtr ptr);
    
    public struct ManagedAllocator : IAllocator
    {
        public static readonly ManagedAllocator Global = new ManagedAllocator();
        
        public IntPtr Allocate(ulong size)
        {
            var mem = new byte[size];
            var handle = GCHandle.Alloc(mem, GCHandleType.Pinned);
            return handle.AddrOfPinnedObject();
        }

        public void Deallocate(IntPtr ptr)
        {
            var handle = GCHandle.FromIntPtr(ptr);
            handle.Free();
        }
    }
    
}