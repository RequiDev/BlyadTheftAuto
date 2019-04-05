using BlyadTheftAuto.MemorySystem.Native.Enums;
using System;

namespace BlyadTheftAuto.MemorySystem.Native.Structs
{
    internal struct MemoryBasicInformation
    {
        public IntPtr BaseAddress;
        public IntPtr AllocationBase;
        public MemoryProtection AllocationProtect;
        public IntPtr RegionSize;
        public UIntPtr State;
        public MemoryProtection Protect;
        public UIntPtr Type;
    }
}
