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
			_readData = Memory.ReadByteArray(_address, 0x0CF4);
		}

		public string GetName()
		{
			return Memory.ReadString(new IntPtr(BitConverter.ToInt64(_readData, 0xA4)));
		}

		public int WantedLevel
		{
			get
			{
				return BitConverter.ToInt32(_readData, 0x888);
			}
			set
			{
				Memory.Write(_address + 0x888, value);
                Memory.Write(_address + 0x88C, value);
			}
		}

		public int FrameFlags
		{
			get
			{
				return BitConverter.ToInt32(_readData, 0x218);
			}
			set
			{
				Memory.Write(_address + 0x218, value);
			}
		}

		public float SwimSpeed
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0x0170);
			}
			set
			{
				Memory.Write(_address + 0x0170, value);
			}
		}

		public float RunSpeed
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0x0CF0);
			}
			set
			{
				Memory.Write(_address + 0x0CF0, value);
			}
		}


	}
}
