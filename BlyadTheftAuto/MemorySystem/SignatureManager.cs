﻿using System;
using BlyadTheftAuto.Extensions;
using System.Diagnostics;
using System.Text;
using BlyadTheftAuto.ConsoleSystem;

namespace BlyadTheftAuto.MemorySystem
{
    internal static class SignatureManager
    {
        private static ProcessMemory Memory => BlyadTheftAuto.Memory;

        public static System.IntPtr GetWorld()
		{
			//return new IntPtr(0x24AECE0).Add(BlyadTheftAuto.Memory.MainModule.BaseAddress);
			var address = BlyadTheftAuto.Game.Find("48 8B 05 ? ? ? ? 48 8B 48 08 48 85 C9 74 52 8B 81", 0, 0, Enums.ScanMethod.Add);
			//Console.WriteOffset("", address);
			return address + Memory.Read<int>(address + 0x3) + 0x7;
		}
	}
}
