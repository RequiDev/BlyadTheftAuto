using BlyadTheftAuto.GrandTheftAuto.Models.Backup;
using BlyadTheftAuto.MemorySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlyadTheftAuto.GrandTheftAuto.Models
{
	internal class VehicleHandling
	{
		private static ProcessMemory Memory => BlyadTheftAuto.Memory;
		private byte[] _readData;
		private IntPtr _address;

		public VehicleHandling(IntPtr address)
		{
			_address = address;
			Update();
		}

		public IntPtr Address => _address;

		public void Update()
		{
			_readData = Memory.ReadByteArray(_address, 0x100);
		}

		public void Restore(BackupVehicleHandling backup)
		{
			Acceleration = backup.Acceleration;
			BrakeForce = backup.BrakeForce;
			TractionCurveMin = backup.TractionCurveMin;
			CollisionDamage = backup.CollisionDamage;
			WeaponDamage = backup.WeaponDamage;
			EngineDamage = backup.EngineDamage;
			DeformationDamage = backup.DeformationDamage;
			UpShift = backup.UpShift;
			HandBrakeForce = backup.HandBrakeForce;
			SuspensionForce = backup.SuspensionForce;
		}

		public float Acceleration
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0x4C);
			}
			set
			{
				Memory.Write(_address + 0x4C, value);
			}
		}

		public float BrakeForce
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0x6C);
			}
			set
			{
				Memory.Write(_address + 0x6C, value);
			}
		}

		public float HandBrakeForce
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0x7C);
			}
			set
			{
				Memory.Write(_address + 0x7C, value);
			}
		}

		public float TractionCurveMin
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0x90);
			}
			set
			{
				Memory.Write(_address + 0x90, value);
			}
		}
		
		public float CollisionDamage
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0xF0);
			}
			set
			{
				Memory.Write(_address + 0xF0, value);
			}
		}

		public float WeaponDamage
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0xF4);
			}
			set
			{
				Memory.Write(_address + 0xF4, value);
			}
		}

		public float DeformationDamage
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0xF8);
			}
			set
			{
				Memory.Write(_address + 0xF8, value);
			}
		}

		public float EngineDamage
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0xFC);
			}
			set
			{
				Memory.Write(_address + 0xFC, value);
			}
		}

		public float UpShift
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0x58);
			}
			set
			{
				Memory.Write(_address + 0x58, value);
			}
		}

		public float SuspensionForce
		{
			get
			{
				return BitConverter.ToSingle(_readData, 0xBC);
			}
			set
			{
				Memory.Write(_address + 0xBC, value);
			}
		}
	}
}
