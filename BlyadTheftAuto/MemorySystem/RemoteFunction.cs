using BlyadTheftAuto.MemorySystem.Native.Enums;
using BlyadTheftAuto.MemorySystem.Native.Library;
using System;
using System.Diagnostics;

namespace BlyadTheftAuto.MemorySystem
{
    internal class RemoteFunction
    {
        private const uint INFINITE = 0xFFFFFFFF;

        public Process Process { get; private set; }
        public IntPtr Address { get; set; }

        public RemoteFunction(Process process)
        {
            Process = process;
        }

        public RemoteFunction(Process process, IntPtr address)
        {
            Process = process;
            Address = address;
        }

        private void InvokeStub(IntPtr fn)
        {
            IntPtr hResult = Kernel32.CreateRemoteThread(Process.Handle, IntPtr.Zero, 0, fn, IntPtr.Zero, CreationFlags.Immediately, IntPtr.Zero);

            if (hResult != IntPtr.Zero)
                Kernel32.WaitForSingleObject(hResult, INFINITE);
        }

        public bool Execute(IntPtr param = default(IntPtr))
        {
            bool ret = false;

            IntPtr hResult = Kernel32.CreateRemoteThread(Process.Handle, IntPtr.Zero, 0, Address, param, CreationFlags.Immediately, IntPtr.Zero);

            if (hResult != IntPtr.Zero)
                ret = Kernel32.WaitForSingleObject(hResult, INFINITE) == 0;

            return ret;
        }

        public bool Execute<T>(ProcessMemory mem, T param) where T : struct
        {
            bool ret = false;
            using (RemoteAllocation ralloc = RemoteAllocation.CreateNew<T>(mem, param))
            {
                ret = Execute(ralloc.Address);
            }
            return ret;
        }

        public static bool Execute(Process process, IntPtr address, IntPtr param = default(IntPtr))
        {
            RemoteFunction fn = new RemoteFunction(process, address);
            return fn.Execute(param);
        }

        public static bool Execute<T>(ProcessMemory memory, IntPtr address, T param) where T : struct
        {
            RemoteFunction fn = new RemoteFunction(memory.Process, address);
            return fn.Execute(memory, param);
        }
    }
}
