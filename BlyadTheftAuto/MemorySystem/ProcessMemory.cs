using BlyadTheftAuto.MemorySystem.Native.Library;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BlyadTheftAuto.MemorySystem
{
    internal class ProcessMemory
    {
        #region Fields
        private Process process;

        private int pid;
        #endregion

        #region Properties
        public Process Process
        {
            get { return process; }
        }

        public int ProcessId
        {
            get { return pid; }
        }

        public bool IsProcessRunning
        {
            get { return !process.HasExited; }
        }

        public IntPtr MainWindowHandle
        {
            get { return process.MainWindowHandle; }
        }

        public ProcessModule MainModule
        {
            get { return process.MainModule; }
        }

        public ProcessModuleCollection ProcessModules
        {
            get { return process.Modules; }
        }
        #endregion

        public ProcessMemory(string name)
        {
            this.process = ProcessByName(name);
            this.pid = process.Id;
        }

        public ProcessMemory(Process process)
        {
            this.process = process;
            this.pid = process.Id;
        }

        public ProcessModule this[string name]
        {
            get
            {
                return Process.Modules.Cast<ProcessModule>().FirstOrDefault(item => item.ModuleName == name);
            }
        }

        #region Read/Write Memory
        public byte[] ReadByteArray(IntPtr address, int length)
        {
            byte[] buffer = new byte[length];

            Kernel32.ReadProcessMemory(Process.Handle, address, buffer, length, IntPtr.Zero);

            return buffer;
        }

        public void WriteByteArray(IntPtr address, byte[] data)
        {
            Kernel32.WriteProcessMemory(Process.Handle, address, data, data.Length, IntPtr.Zero);
        }

        public T Read<T>(IntPtr address) where T : struct
        {
            return BytesTo<T>(ReadByteArray(address, TypeCache<T>.Size));
        }

        public unsafe T[] ReadArray<T>(IntPtr address, int length) where T : new()
        {
            int size = TypeCache<T>.Size * length;
            byte[] data = ReadByteArray(address, size);
            T[] array = new T[length];

            fixed (byte* b = data)
            {
                for (int i = 0; i < length; i++)
                {
                    array[i] = Marshal.PtrToStructure<T>((IntPtr)(b + i * TypeCache<T>.Size));
                }
            }
            return array;
        }

        public void Write<T>(IntPtr address, T data) where T : struct
        {
            WriteByteArray(address, ConvertToBytes<T>(data));
        }

        public unsafe void WriteArray<T>(IntPtr address, T[] array)
        {
            byte[] buffer = new byte[TypeCache<T>.Size * array.Length];

            fixed (byte* b = buffer)
                for (int i = 0; i < array.Length; i++)
                    Marshal.StructureToPtr(array[i], (IntPtr)(b + i * TypeCache<T>.Size), true);

            WriteByteArray(address, buffer);
        }

        public T ReadPointer<T>(IntPtr baseAddress, params int[] offsets) where T : struct
        {
            return Read<T>(DereferencePointer(baseAddress, offsets));
        }

        internal void Write(object p, float value)
        {
            throw new NotImplementedException();
        }

        public void WritePointer<T>(IntPtr baseAddress, T data, params int[] offsets) where T : struct
        {
            Write<T>(DereferencePointer(baseAddress, offsets), data);
        }

        public string ReadString(IntPtr address, bool unicode = false)
        {
            var encoding = unicode ? Encoding.UTF8 : Encoding.Default;
            var numArray = ReadByteArray(address, 255);
            var str = encoding.GetString(numArray);

            if (str.Contains('\0'))
                str = str.Substring(0, str.IndexOf('\0'));
            return str;
        }

        public void WriteString(IntPtr address, string str, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.Default;

            WriteByteArray(address, encoding.GetBytes(str));
        }

        public IntPtr DereferencePointer(IntPtr baseAddress, params int[] offsets)
        {
            for (int i = 0; i < offsets.Length - 1; i++)
                baseAddress = Read<IntPtr>(baseAddress + offsets[i]);
            return baseAddress + offsets[offsets.Length - 1];
        }

        public IntPtr GetVirtualFunction(IntPtr classPointer, int index)
        {
            var table = Read<IntPtr>(classPointer);
            return Read<IntPtr>(table + (index * TypeCache<IntPtr>.Size));
        }
        #endregion

        #region Advanced
        public RemoteAllocation Allocate(int size)
        {
            return new RemoteAllocation(process, size);
        }
        public RemoteAllocation Allocate<T>() where T : struct
        {
            return new RemoteAllocation(process, TypeCache<T>.Size);
        }
        public RemoteAllocation Allocate<T>(T data) where T : struct
        {
            return RemoteAllocation.CreateNew<T>(this, data);
        }

        public bool Execute(IntPtr fn, IntPtr param = default(IntPtr))
        {
            RemoteFunction remoteFn = new RemoteFunction(process, fn);
            return remoteFn.Execute(param);
        }
        public bool Execute<T>(IntPtr fn, T param) where T : struct
        {
            RemoteFunction remoteFn = new RemoteFunction(process, fn);
            return remoteFn.Execute<T>(this, param);
        }
        #endregion

        #region Process Modules
        public bool ModuleExists(string name)
        {
            return Process.Modules.Cast<ProcessModule>().Any(module => module.ModuleName == name);
        }

        public ProcessModule ModuleFromName(string name)
        {
            return Process.Modules.Cast<ProcessModule>().FirstOrDefault(item => item.ModuleName == name);
        }

        public IntPtr ModuleBaseAddress(string name)
        {
            ProcessModule module = ModuleFromName(name);
            if (module == null)
                return IntPtr.Zero;
            return module.BaseAddress;
        }

        public int ModuleMemorySize(string name)
        {
            ProcessModule module = ModuleFromName(name);
            if (module == null)
                return 0;
            return module.ModuleMemorySize;
        }
        #endregion

        #region Static methods
        public static bool ProcessExists(string name)
        {
            return Process.GetProcessesByName(name).FirstOrDefault() == null ? false : true;
        }

        public static Process ProcessByName(string name)
        {
            return Process.GetProcessesByName(name).FirstOrDefault();
        }

        public static int SizeOf<T>() where T : struct
        {
            return TypeCache<T>.Size;
        }

        public static Type TypeOf<T>() where T : struct
        {
            return TypeCache<T>.Type;
        }

        public static unsafe byte[] ConvertToBytes<T>(T obj) where T : struct
        {
            byte[] buffer = new byte[TypeCache<T>.Size];
            fixed (byte* b = buffer)
            {
                Marshal.StructureToPtr<T>(obj, (IntPtr)b, true);
            }
            return buffer;
        }

        public static unsafe T BytesTo<T>(byte[] data) where T : struct
        {
            fixed (byte* b = data)
            {
                return Marshal.PtrToStructure<T>((IntPtr)b);
            }
        }

        public static unsafe T BytesTo<T>(byte[] data, int index) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] tmp = new byte[size];
            Array.Copy(data, index, tmp, 0, size);
            return BytesTo<T>(tmp);
        }
        #endregion
    }
}
