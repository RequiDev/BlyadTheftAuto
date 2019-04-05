using BlyadTheftAuto.MemorySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlyadTheftAuto
{
	internal static class BlyadTheftAuto
	{
		public static string GameName => "Grand Theft Auto V";
		public static string ProcessName => "GTA5";
		public static bool IsAttached { get; set; }
		public static int Width { get; set; }
		public static int Height { get; set; }
		public static ProcessMemory Memory { get; set; }
		public static PatternScan Game { get; set; }
	}
}
