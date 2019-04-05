using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlyadTheftAuto.FeatureSystem
{
	internal interface IFeature
	{
		string Name { get; }
		Keys Key { get; }
		Keys SecondaryKey { get; }

		void OnKey();
		void OnSecondaryKey();
		string ToString();
	}
}
