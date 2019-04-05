using BlyadTheftAuto.MemorySystem.Enums;
using System;
using System.Diagnostics;
using System.Globalization;

namespace BlyadTheftAuto.MemorySystem
{
    internal class PatternScan
    {
        private IntPtr baseAddress;
        private int size;

        private byte[] dump;

        private ProcessMemory memory;

        private ProcessModule module;

        public ProcessModule Module { get { return module; } }
        public IntPtr BaseAddress { get { return baseAddress; } }
        public int MemorySize { get { return size; } }

        public PatternScan(Process process, string moduleName)
        {
			process.Refresh();
            memory = new ProcessMemory(process);

            module = memory.ModuleFromName(moduleName);

            baseAddress = module.BaseAddress;
            size = module.ModuleMemorySize;

            dump = memory.ReadByteArray(baseAddress, size);
        }

        public PatternScan(ProcessMemory processMemory, string moduleName)
        {
            memory = processMemory;

            module = memory.ModuleFromName(moduleName);

            baseAddress = module.BaseAddress;
			size = module.ModuleMemorySize;

			dump = memory.ReadByteArray(baseAddress, size);
        }

		public PatternScan(Process process, ProcessModule module)
		{
			memory = new ProcessMemory(process);

			baseAddress = module.BaseAddress;
			size = module.ModuleMemorySize;

			dump = memory.ReadByteArray(baseAddress, size);
		}

        public IntPtr Find(string mask, int patternOffset, int addressOffset, ScanMethod method)
        {
            return Find(MaskToPattern(mask), patternOffset, addressOffset, method);
        }

        public IntPtr Find(byte[] pattern, int patternOffset, int addressOffset, ScanMethod method)
        {
            int length = pattern.Length;
            int i = 0;
            int k = 0;

            int loopDist = dump.Length - length;

            while (i < loopDist)
            {
                if (pattern[k] == 0x00 || dump[i] == pattern[k])
                {
                    k++;
                    if (k == length)
                        break;
                }
                else
                {
                    i -= k;
                    k = 0;
                }
                i++;
            }

            int address = i - k + 1;

            if (address < 1)
                return IntPtr.Zero;

            switch (method)
            {
                case ScanMethod.None:
                    return (IntPtr)(address + patternOffset + addressOffset);
                case ScanMethod.Add:
                    return baseAddress + address + patternOffset + addressOffset;
                case ScanMethod.Subtract:
                    return baseAddress - address + patternOffset + addressOffset;
                case ScanMethod.Read:
                    return memory.Read<IntPtr>(baseAddress + address + patternOffset) + addressOffset;
                case ScanMethod.ReadAndSubtract:
                    return (IntPtr)((memory.Read<IntPtr>(baseAddress + address + patternOffset) + addressOffset).ToInt64() - baseAddress.ToInt64());
            }

            return (IntPtr)address;
        }

        private byte[] MaskToPattern(string mask)
        {
            string[] numbers = mask.StartsWith(@"\x") ? mask.Split('\\', 'x') : mask.Split(' ');

            byte[] pattern = new byte[numbers.Length];

            for (int i = 0; i < pattern.Length; i++)
            {
                if (numbers[i].StartsWith("?"))
                {
                    pattern[i] = 0x00;
                }
                else
                {
                    pattern[i] = byte.Parse(numbers[i], NumberStyles.HexNumber);
                }
            }

            return pattern;
        }
    }
}
