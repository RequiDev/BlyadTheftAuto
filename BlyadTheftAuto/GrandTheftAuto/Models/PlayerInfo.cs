using BlyadTheftAuto.Extensions;
using BlyadTheftAuto.MemorySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlyadTheftAuto.GrandTheftAuto.Models
{
	internal class PlayerInfo
	{
		private static ProcessMemory Memory => BlyadTheftAuto.Memory;
		private byte[] _readData;
		private readonly IntPtr _address;

		public PlayerInfo(IntPtr address)
		{
			this._address = address;
			Update();
		}
		public IntPtr Address => _address;

		public void Update()
		{
			_readData = Memory.ReadByteArray(_address, 0xC08);
		}

		public string GetName()
		{
			return Memory.ReadString(new IntPtr(BitConverter.ToInt64(_readData, 0x7C)));
		}

		public int WantedLevel
		{
			get
			{
				return BitConverter.ToInt32(_readData, 0x818);
			}
			set
			{
				Memory.Write(_address + 0x818, value);
			}
		}

		public int FrameFlags
		{
			get
			{
				return BitConverter.ToInt32(_readData, 0x1F8);
			}
			set
			{
				Memory.Write(_address + 0x1F8, value);
			}
		}

		public float SwimSpeed
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0x0148);
			}
			set
			{
				Memory.Write(_address + 0x0148, value);
			}
		}

		public float RunSpeed
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0x14C);
			}
			set
			{
				Memory.Write(_address + 0x14C, value);
			}
		}


	}
}
