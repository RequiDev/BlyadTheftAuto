using BlyadTheftAuto.FeatureSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BlyadTheftAuto
{
	internal class Settings
	{
		private const string File = "./teleports.ini";

		public static List<TeleportPosition> GetTeleportPresets()
		{
			List<TeleportPosition> ret = new List<TeleportPosition>();
			var sections = GetSectionNames();
			foreach(var section in sections)
			{
				var x = ParseFloat(ReadValue(section, "X"));
				var y = ParseFloat(ReadValue(section, "Y"));
				var z = ParseFloat(ReadValue(section, "Z"));
				ret.Add(new TeleportPosition(section, new Structs.Vector3D(x, y, z)));
			}
			return ret;
		}

		public static void SaveTeleportPresets()
		{
			foreach(var telPos in Program.TeleportPresets)
			{
				WriteValue(telPos.Name, "X", telPos.Positon.X.ToString());
				WriteValue(telPos.Name, "Y", telPos.Positon.Y.ToString());
				WriteValue(telPos.Name, "Z", telPos.Positon.Z.ToString());
			}
		}


		#region ReadWrite
		private static void WriteValue(string section, string key, string value)
		{
			WritePrivateProfileString(section, key, value, File);
		}

		private static string ReadValue(string section, string key)
		{
			var temp = new StringBuilder(255);
			GetPrivateProfileString(section, key, "", temp, 255, File);

			return temp.ToString();
		}

		private static string[] GetSectionNames()
		{
			//    Sets the maxsize buffer to 500, if the more
			//    is required then doubles the size each time.
			for (int maxsize = 500; true; maxsize *= 2)
			{
				//    Obtains the information in bytes and stores
				//    them in the maxsize buffer (Bytes array)
				byte[] bytes = new byte[maxsize];
				int size = GetPrivateProfileString(0, "", "", bytes, maxsize, File);

				// Check the information obtained is not bigger
				// than the allocated maxsize buffer - 2 bytes.
				// if it is, then skip over the next section
				// so that the maxsize buffer can be doubled.
				if (size < maxsize - 2)
				{
					// Converts the bytes value into an ASCII char. This is one long string.
					string Selected = Encoding.ASCII.GetString(bytes, 0,
											   size - (size > 0 ? 1 : 0));
					// Splits the Long string into an array based on the "\0"
					// or null (Newline) value and returns the value(s) in an array
					return Selected.Split(new char[] { '\0' });
				}
			}
		}
		#endregion
		#region Parsing
		private static bool ParseBoolean(string input, bool defaultVal = false)
		{
			if (string.IsNullOrEmpty(input))
				return defaultVal;

			if (!bool.TryParse(input, out bool output))
				return defaultVal;

			return output;
		}

		private static int ParseInteger(string input, int defaultVal = 0)
		{
			if (string.IsNullOrEmpty(input))
				return defaultVal;

			if (!int.TryParse(input, out int output))
				return defaultVal;

			return output;
		}

		private static float ParseFloat(string input, float defaultVal = 0.0f)
		{
			if (string.IsNullOrEmpty(input))
				return defaultVal;

			if (!float.TryParse(input, out float output))
				return defaultVal;

			return output;
		}
		#endregion
		#region Native
		[DllImport("kernel32")]
		static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
		[DllImport("kernel32")]
		static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);        // Third Method
		[DllImport("kernel32")]
		static extern int GetPrivateProfileString(int Section, string Key, string Value, [MarshalAs(UnmanagedType.LPArray)] byte[] Result, int Size, string FileName);
		#endregion
	}
}
