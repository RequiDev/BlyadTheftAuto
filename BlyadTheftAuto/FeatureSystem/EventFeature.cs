using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlyadTheftAuto.FeatureSystem
{
	internal class EventFeature : IFeature
	{
		private string _name;
		private Keys _key;
		private Keys _secondaryKey = Keys.None;
		private string _status;
		private Action _function;

		public string Name => _name;
		public Keys Key => _key;
		public Keys SecondaryKey => _secondaryKey;


		public EventFeature(string name, Action function, Keys key, Keys secondaryKey = Keys.None)
		{
			_name = name;
			_key = key;
			_secondaryKey = secondaryKey;
			_function = function;
			_status = "Idle";
		}

		public void OnKey()
		{
			_status = "Running";
			_function();
			_status = "Idle";
		}

		public void OnSecondaryKey()
		{
			_status = "Running";
			_function();
			_status = "Idle";
		}

		public override string ToString()
		{
			return _status;
		}
	}
}
