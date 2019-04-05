using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlyadTheftAuto.GrandTheftAuto.Models.Backup
{
	internal class BackupWeapon
	{
		/// <summary>
		/// Backup members of Weapon
		/// </summary>
		private int _nameHash;
		private float _damage;
		private int _bulletBatch;
		private float _reloadTime;
		private float _spread;
		private float _batchSpread;
		private float _range;
		private float _recoil;
		private float _spinUp;
		private float _spin;
		private float _muzzleVelocity;

		public float Recoil => _recoil;
		public float Range => _range;
		public float Spread => _spread;
		public float BatchSpread => _batchSpread;
		public float ReloadTime => _reloadTime;
		public int BulletBatch => _bulletBatch;
		public float Damage => _damage;
		public int NameHash => _nameHash;
		public float SpinUp => _spinUp;
		public float Spin => _spin;
		public float MuzzleVelocity => _muzzleVelocity;

		public BackupWeapon(Weapon copy)
		{
			_nameHash = copy.NameHash;
			_damage = copy.Damage;
			_bulletBatch = copy.BulletBatch;
			_reloadTime = copy.ReloadTime;
			_recoil = copy.Recoil;
			_batchSpread = copy.BatchSpread;
			_spread = copy.Spread;
			_range = copy.Range;
			_reloadTime = copy.Recoil;
			_spinUp = copy.SpinUp;
			_spin = copy.Spin;
			_muzzleVelocity = copy.MuzzleVelocity;
		}
	}
}
