using BlyadTheftAuto.MemorySystem.Native.Enums;
using BlyadTheftAuto.MemorySystem.Native.Library;
using System;
using System.Diagnostics;

namespace BlyadTheftAuto.MemorySystem
{
    internal class RemoteAllocation : IDisposable
    {
        private bool isAllocated = false;

        public Process Process { get; private set; }
        public IntPtr Address { get; private set; }
        public int Size { get; private set; }

        public RemoteAllocation(Process process)
        {
            Process = process;
        }

        public RemoteAllocation(Process process, int size)
        {
            Process = process;

            Size = size;

            Allocate(Size);
        }

        ~RemoteAllocation()
        {
            Dispose(false);
        }

        public bool Allocate(int size)
        {
            if (isAllocated)
                return false;

            Address = Kernel32.VirtualAllocEx(Process.Handle, IntPtr.Zero, Size, AllocationType.Commit | AllocationType.Reserve, MemoryProtection.ExecuteReadWrite);

            isAllocated = true;
            return true;
        }

        public bool Free()
        {
            if (!isAllocated)
                return false;

            bool result = Kernel32.VirtualFreeEx(Process.Handle, Address, 0, FreeType.Release);

            isAllocated = !result;

            return result;
        }

        public static RemoteAllocation CreateNew<T>(ProcessMemory memory, T data) where T : struct
        {
            RemoteAllocation ralloc = new RemoteAllocation(memory.Process);

            if (!ralloc.Allocate(TypeCache<T>.Size))
                return null;

            memory.Write(ralloc.Address, data);

            return ralloc;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                Free();

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
