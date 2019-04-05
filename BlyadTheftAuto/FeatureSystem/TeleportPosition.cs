using BlyadTheftAuto.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlyadTheftAuto.FeatureSystem
{
	internal class TeleportPosition
	{
		public TeleportPosition(string name, Vector3D position)
		{
			_name = name;
			_positon = position;
		}

		private string _name;
		private Vector3D _positon;

		public string Name => _name;
		public Vector3D Positon => _positon;
	}
}
