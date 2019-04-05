using BlyadTheftAuto.GrandTheftAuto.Models.Backup;
using BlyadTheftAuto.MemorySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlyadTheftAuto.GrandTheftAuto.Models
{
	internal class Weapon
	{
		private static ProcessMemory Memory => BlyadTheftAuto.Memory;
		private byte[] _readData;
		private IntPtr _address;

		public Weapon(IntPtr address)
		{
			this._address = address;
			Update();
		}

		public void Update()
		{
			_readData = Memory.ReadByteArray(_address, 0x2DC);
		}

		public void Restore(BackupWeapon backup)
		{
			Damage = backup.Damage;
			BulletBatch = backup.BulletBatch;
			ReloadTime = backup.ReloadTime;
			Spread = backup.Spread;
			Range = backup.Range;
			Recoil = backup.Recoil;
			BatchSpread = backup.BatchSpread;
			SpinUp = backup.SpinUp;
			Spin = backup.Spin;
			MuzzleVelocity = backup.MuzzleVelocity;
		}

		public int NameHash
		{
			get
			{
				return BitConverter.ToInt32(_readData, 0x10);
			}
		}

		public float Damage
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0xB0);
			}
			set
			{
				Memory.Write(_address + 0xB0, value);
			}
		}

		public int BulletBatch
		{
			get
			{
				return BitConverter.ToInt32(_readData, 0x118);
			}
			set
			{
				Memory.Write(_address + 0x118, value);
			}
		}

		public float ReloadTime
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0x12C);
			}
			set
			{
				Memory.Write(_address + 0x12C, value);
			}
		}

		public float Spread
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0x70);
			}
			set
			{
				Memory.Write(_address + 0x70, value);
			}
		}

		public float BatchSpread
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0x11C);
			}
			set
			{
				Memory.Write(_address + 0x11C, value);
			}
		}

		public float Range
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0x28C);
			}
			set
			{
				Memory.Write(_address + 0x28C, value);
			}
		}

		public float Recoil
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0x2D8);
			}
			set
			{
				Memory.Write(_address + 0x2D8, value);
			}
		}

		public float SpinUp
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0x13C);
			}
			set
			{
				Memory.Write(_address + 0x13C, value);
			}
		}

		public float Spin
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0x140);
			}
			set
			{
				Memory.Write(_address + 0x140, value);
			}
		}

		public float MuzzleVelocity
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0x114);
			}
			set
			{
				Memory.Write(_address + 0x114, value);
			}
		}
	}
}
