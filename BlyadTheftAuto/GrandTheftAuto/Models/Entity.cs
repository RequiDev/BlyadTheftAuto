using BlyadTheftAuto.MemorySystem;
using BlyadTheftAuto.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlyadTheftAuto.GrandTheftAuto.Models
{
	internal class Entity
	{
		protected static ProcessMemory Memory => BlyadTheftAuto.Memory;
		protected byte[] readData;
		protected IntPtr address;

		public Entity(IntPtr address)
		{
			this.address = address;
			Update();
		}

		public IntPtr Address => address;

		public void Update()
		{
			readData = Memory.ReadByteArray(address, 0x14F4);
		}

		public bool GodMode
		{
			get
			{
				return readData[0x189] == 1;
			}
			set
			{
				Memory.Write(address + 0x189, (char)(value ? 1 : 0));
			}
		}

		public Vector3D Position
		{
			get
			{
				byte[] vecData = new byte[12];
				Buffer.BlockCopy(readData, 0x90, vecData, 0, 12);
				return ProcessMemory.BytesTo<Vector3D>(vecData);
			}
			set
			{
				var navigation = new IntPtr(BitConverter.ToInt64(readData, 0x30));
				Memory.Write(navigation + 0x50, value);
				Memory.Write(address + 0x90, value);
			}
		}
	}
}
