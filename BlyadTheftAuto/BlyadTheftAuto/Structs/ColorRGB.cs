using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlyadTheftAuto.Structs
{
	public struct ColorRGB
	{
		public byte R;
		public byte G;
		public byte B;
		public ColorRGB(Color value)
		{
			this.R = value.R;
			this.G = value.G;
			this.B = value.B;
		}
		public static implicit operator Color(ColorRGB rgb)
		{
			Color c = Color.FromArgb(rgb.R, rgb.G, rgb.B);
			return c;
		}
		public static explicit operator ColorRGB(Color c)
		{
			return new ColorRGB(c);
		}
	}
}
