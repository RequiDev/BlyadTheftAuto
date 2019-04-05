using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlyadTheftAuto.FeatureSystem
{
	internal class BoolFeature : IFeature
	{
		private string _name;
		private Keys _key;
		private Keys _secondaryKey = Keys.None;
		private bool _status;
		private Action _function;

		public string Name => _name;
		public Keys Key => _key;
		public Keys SecondaryKey => _secondaryKey;
		public bool Value => _status;

		public BoolFeature(string name, Keys key, Action function = null)
		{
			_name = name;
			_key = key;
			_function = function;
		}

		public void OnKey()
		{
			_status = !_status;
			_function?.Invoke();
		}

		public void OnSecondaryKey()
		{
			return;
		}

		public override string ToString()
		{
			return _status.ToString();
		}
	}
}
