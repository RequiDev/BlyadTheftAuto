using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlyadTheftAuto.GrandTheftAuto.Models.Backup
{
	internal class BackupVehicleHandling
	{
		/// <summary>
		/// Backup members of VehicleHandling
		/// </summary>
		private IntPtr _address;
		private float _acceleration;
		private float _brakeForce;
		private float _handBrakeForce;
		private float _tractionCurveMin;
		private float _collisionDamage;
		private float _weaponDamage;
		private float _deformationDamage;
		private float _engineDamage;
		private float _upShift;
		private float _suspensionForce;

		public IntPtr Address => _address;
		public float Acceleration => _acceleration;
		public float BrakeForce => _brakeForce;
		public float HandBrakeForce => _handBrakeForce;
		public float TractionCurveMin => _tractionCurveMin;
		public float CollisionDamage => _collisionDamage;
		public float WeaponDamage => _weaponDamage;
		public float DeformationDamage => _deformationDamage;
		public float EngineDamage => _engineDamage;
		public float UpShift => _upShift;
		public float SuspensionForce => _suspensionForce;

		public BackupVehicleHandling(VehicleHandling copy)
		{
			_address = copy.Address;
			_acceleration = copy.Acceleration;
			_brakeForce = copy.BrakeForce;
			_handBrakeForce = copy.HandBrakeForce;
			_tractionCurveMin = copy.TractionCurveMin;
			_collisionDamage = copy.CollisionDamage;
			_weaponDamage = copy.WeaponDamage;
			_deformationDamage = copy.DeformationDamage;
			_engineDamage = copy.EngineDamage;
			_upShift = copy.UpShift;
			_suspensionForce = copy.SuspensionForce;
		}
	}
}
