using BlyadTheftAuto.MemorySystem.Native.Enums;
using BlyadTheftAuto.MemorySystem.Native.Structs;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace BlyadTheftAuto.MemorySystem.Native.Library
{
    [SuppressUnmanagedCodeSecurity()]
    internal static class Kernel32
    {
        [DllImport("kernel32.dll", SetLastError = false)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In][Out] byte[] lpBuffer, int dwSize, IntPtr lpNumberOfBytesRead);

		[DllImport("kernel32.dll", SetLastError = false)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = false, ExactSpelling = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
           int dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport("kernel32.dll", SetLastError = false, ExactSpelling = true)]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress,
           int dwSize, FreeType dwFreeType);

        [DllImport("kernel32.dll", SetLastError = false)]
        public static extern IntPtr VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MemoryBasicInformation lpBuffer, uint dwLength);

        [DllImport("kernel32.dll", SetLastError = false)]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess,
           IntPtr lpThreadAttributes, uint dwStackSize, IntPtr
           lpStartAddress, IntPtr lpParameter, CreationFlags dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll", SetLastError = false)]
        public static extern int WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);
    }
}
