using BlyadTheftAuto.MemorySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlyadTheftAuto.GrandTheftAuto.Models
{
	internal class VehicleColors
	{
		private static ProcessMemory Memory => BlyadTheftAuto.Memory;
		private byte[] _readData;
		private IntPtr _address;

		public VehicleColors(IntPtr address)
		{
			_address = address;
			Update();
		}

		public IntPtr Address => _address;

		public void Update()
		{
			_readData = Memory.ReadByteArray(_address, 0xAC);
		}


		public byte PrimaryBlue
		{
			get
			{
				return _readData[0xA4];
			}
			set
			{
				Memory.Write(_address + 0xA4, value);
			}
		}

		public byte PrimaryGreen
		{
			get
			{
				return _readData[0xA5];
			}
			set
			{
				Memory.Write(_address + 0xA5, value);
			}
		}

		public byte PrimaryRed
		{
			get
			{
				return _readData[0xA6];
			}
			set
			{
				Memory.Write(_address + 0xA6, value);
			}
		}

		public byte PrimaryAlpha
		{
			get
			{
				return _readData[0xA7];
			}
			set
			{
				Memory.Write(_address + 0xA7, value);
			}
		}

		public byte SecondaryBlue
		{
			get
			{
				return _readData[0xA8];
			}
			set
			{
				Memory.Write(_address + 0xA8, value);
			}
		}

		public byte SecondaryGreen
		{
			get
			{
				return _readData[0xA9];
			}
			set
			{
				Memory.Write(_address + 0xA9, value);
			}
		}

		public byte SecondaryRed
		{
			get
			{
				return _readData[0xAA];
			}
			set
			{
				Memory.Write(_address + 0xAA, value);
			}
		}

		public byte SecondaryAlpha
		{
			get
			{
				return _readData[0xAB];
			}
			set
			{
				Memory.Write(_address + 0xAB, value);
			}
		}
	}
}
