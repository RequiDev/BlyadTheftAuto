using System;

namespace BlyadTheftAuto.MemorySystem.Native.Enums
{
    [Flags]
    internal enum CreationFlags : uint
    {
        Immediately = 0,
        Suspended = 0x4,
        StackSizeParamIsAReservation = 0x10000
    }
}
