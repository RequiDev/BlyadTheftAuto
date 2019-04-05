using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlyadTheftAuto.GrandTheftAuto.Models.Backup
{
	internal class BackupVehicle
	{
		/// <summary>
		/// Backup Members of Vehicle. Not being used currently.
		/// </summary>
		private float _gravity;
		private int _alarmLength;

		public float Gravity => _gravity;
		public int AlarmLength => _alarmLength;

		public BackupVehicle(Vehicle copy)
		{
			_gravity = copy.Gravity;
			_alarmLength = copy.AlarmLength;
		}
	}
}
