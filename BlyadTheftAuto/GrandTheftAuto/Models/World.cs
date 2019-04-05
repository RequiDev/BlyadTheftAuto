using BlyadTheftAuto.MemorySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlyadTheftAuto.GrandTheftAuto.Models
{
	internal class World
	{
		private static ProcessMemory Memory => BlyadTheftAuto.Memory;
		private IntPtr _address;

		public World(IntPtr address)
		{
			this._address = Memory.Read<IntPtr>(address);
		}

		public IntPtr Address => _address;

		public Ped GetLocalPlayer()
		{
			return new Ped(Memory.Read<IntPtr>(_address + 0x8));
		}
	}
}
