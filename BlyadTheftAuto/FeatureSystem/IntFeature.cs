using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlyadTheftAuto.FeatureSystem
{
	internal class IntFeature : IFeature
	{
		private string _name;
		private Keys _key;
		private Keys _secondaryKey = Keys.None;
		private int _amount;
		private int _change;
		private int _min;
		private int _max;
		private Action _function;

		public string Name => _name;
		public Keys Key => _key;
		public Keys SecondaryKey => _secondaryKey;
		public int Min => _min;
		public int Max => _max;
		public int Value
		{
			get => _amount;
			set => _amount = value;
		}


		public IntFeature(string name, Keys key, Keys secondaryKey, int change, int min, int max, Action function = null)
		{
			_name = name;
			_key = key;
			_secondaryKey = secondaryKey;
			_change = change;
			_min = min;
			_max = max;
			_amount = _min;
			_function = function;
		}

		public void OnKey()
		{
			_amount += _change;
			if (_amount > _max) _amount = _max;
			_function?.Invoke();
		}
		public void OnSecondaryKey()
		{
			_amount -= _change;
			if (_amount < _min) _amount = _min;
			_function?.Invoke();
		}

		public override string ToString()
		{
			return _amount.ToString();
		}
	}
}
