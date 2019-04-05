using BlyadTheftAuto.FeatureSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlyadTheftAuto.Extensions
{
	internal static class Extensions
	{
		public static DateTime GetLinkerTime(this Assembly assembly, TimeZoneInfo target = null)
		{
			var filePath = assembly.Location;
			const int c_PeHeaderOffset = 60;
			const int c_LinkerTimestampOffset = 8;

			var buffer = new byte[2048];

			using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
				stream.Read(buffer, 0, 2048);

			var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
			var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

			var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

			var tz = target ?? TimeZoneInfo.Local;
			var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

			return localTime;
		}

		public static IntPtr Add(this IntPtr first, IntPtr second)
		{
			if (IntPtr.Size == 4)
				return new IntPtr(first.ToInt32() + second.ToInt32());
			return new IntPtr(first.ToInt64() + second.ToInt64());
		}

		public static IntPtr Subtract(this IntPtr first, IntPtr second)
		{
			if (IntPtr.Size == 4)
				return new IntPtr(first.ToInt32() - second.ToInt32());
			return new IntPtr(first.ToInt64() - second.ToInt64());
		}

		public static T ByName<T>(this List<IFeature> list, string name)
		{
			return ((T)list.Find(f => f.Name == name));
		}
	}
}
