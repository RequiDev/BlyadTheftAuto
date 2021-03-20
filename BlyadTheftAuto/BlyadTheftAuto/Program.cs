using System;
using BlyadTheftAuto.Extensions;
using BlyadTheftAuto.FeatureSystem;
using BlyadTheftAuto.GrandTheftAuto.Models;
using BlyadTheftAuto.GrandTheftAuto.Models.Backup;
using BlyadTheftAuto.MemorySystem;
using BlyadTheftAuto.Structs;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Console = BlyadTheftAuto.ConsoleSystem.Console;
using ConsoleColor = BlyadTheftAuto.ConsoleSystem.ConsoleColor;

namespace BlyadTheftAuto
{
	internal class Program
	{
		public static World World;
		public static List<TeleportPosition> TeleportPresets = new List<TeleportPosition>();
		private static IntFeature _teleportPreset;
		private static ProcessMemory Memory => BlyadTheftAuto.Memory;
		private static readonly List<IFeature> Features = new List<IFeature>();
		private static readonly List<string> Whitelist = new List<string>() { "Teleport", "Teleport High", "Stop Boost", "Save Position", "Set Preset"};
		private static readonly Thread KeyThread = new Thread(KeyLoop);
		private static readonly Thread InfoThread = new Thread(InfoLoop);
		//private static System.IntPtr AmmoPtr;
		//private static System.IntPtr ReloadPtr;
		//private static byte[] three_nops = { 0x90, 0x90, 0x90 };
		//private static byte[] ammo_backup = { 0x41, 0x2B, 0xD1 };
		//private static byte[] reload_backup = { 0x41, 0x2B, 0xC9 };

		//private static BackupVehicle _backupVehicle;
		private static BackupVehicleHandling _backupVehicleHandling;
		private static BackupWeapon _backupWeapon;
		private static Weapon _currentWeapon;
		private static VehicleHandling _currentVehicleHandling;

		internal static void Main(string[] args)
        {
            Console.SetWindowSize(Console.WindowWidth, 35);
			Console.Title = $"BlyadTheftAuto@{System.Environment.UserName}";
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteWatermark();
            Console.CursorVisible = false;

			Features.Add(new BoolFeature("Master Toggle", Keys.MButton));
			Features.Add(new BoolFeature("God Mode", Keys.NumPad0));
			Features.Add(new BoolFeature("Super Bullets", Keys.NumPad1)); //damage, bullet damage, bullet amount, range, no spinup, muzzle velocity
			Features.Add(new BoolFeature("Never Wanted", Keys.NumPad2));
			Features.Add(new BoolFeature("Car God Mode", Keys.NumPad3));
			Features.Add(new BoolFeature("Rank Boost", Keys.NumPad4));

			Features.Add(new EventFeature("Teleport", Teleport, Keys.NumPad5));

			Features.Add(new BoolFeature("Anti NPC", Keys.NumPad6));
			Features.Add(new BoolFeature("No Spread", Keys.NumPad7));
			Features.Add(new BoolFeature("No Recoil", Keys.NumPad8));
			Features.Add(new BoolFeature("No Reload", Keys.NumPad9));

			Features.Add(new BoolFeature("Super Jump", Keys.F5));
			Features.Add(new BoolFeature("Explosive Melee", Keys.F6));
			Features.Add(new BoolFeature("Explosive Ammo", Keys.F7));
			Features.Add(new BoolFeature("Fire Ammo", Keys.F8));

			Features.Add(new EventFeature("Teleport High", Teleport2, Keys.F9));

			Features.Add(new BoolFeature("Rainbow Vehicle", Keys.F11));
			Features.Add(new EventFeature("Suicide", Suicide, Keys.F12));
			Features.Add(new EventFeature("Stop Boost", StopBoost, Keys.E));

			Features.Add(new IntFeature("Acceleration", Keys.Up, Keys.Down, 1, 1, 10));
			Features.Add(new IntFeature("Brake Force", Keys.Right, Keys.Left, 1, 1, 10));
			Features.Add(new IntFeature("Traction Curve", Keys.Add, Keys.Subtract, 1, 1, 5));
			Features.Add(new IntFeature("Suspension Force", Keys.Multiply, Keys.Divide, 1, 0, 2));
			Features.Add(new IntFeature("Shift Rate", Keys.Home, Keys.End, 1, 1, 25));
			Features.Add(new IntFeature("Run/Swim Speed", Keys.Insert, Keys.Delete, 1, 1, 5));
			Features.Add(new IntFeature("Wanted Level", Keys.F3, Keys.F4, 1, 0, 5, ChangeWantedLevel));
			Features.Add(new EventFeature("Save Position", SavePosition, Keys.XButton1));
			Features.Add(new EventFeature("Set Preset", SetPreset, Keys.XButton2));

			TeleportPresets = Settings.GetTeleportPresets();
			_teleportPreset = new IntFeature("Teleport Preset", Keys.OemPeriod, Keys.Oemcomma, 1, 0, TeleportPresets.Count - 1);

			while (BlyadTheftAuto.Memory is null)
			{
				Thread.Sleep(100);
				Process process;
				try
				{
					process = Process.GetProcessesByName(BlyadTheftAuto.ProcessName).FirstOrDefault();
					if (process == null) continue;
				}
				catch
				{
					continue;
				}

				BlyadTheftAuto.Memory = new ProcessMemory(process);
				BlyadTheftAuto.Game = new PatternScan(BlyadTheftAuto.Memory, "GTA5.exe");
			}
			
			Console.WriteLine("\n  Offsets:");
            Console.WriteOffset("World", SignatureManager.GetWorld().Subtract(BlyadTheftAuto.Memory.MainModule.BaseAddress));
			//Console.WriteOffset("Ammo", AmmoPtr = new System.IntPtr(0x0F71C38));
			//Console.WriteOffset("Reload", ReloadPtr = new System.IntPtr(0x0F71C7D));

			World = new World(SignatureManager.GetWorld());

			KeyThread.Start();
			InfoThread.Start();

			Console.WriteNotification("\n  ");

			double rainbow = 0;

			while (Memory.IsProcessRunning)
			{
				rainbow += 0.01;
				if (rainbow >= 1) rainbow = 0;

				var localPlayer = World.GetLocalPlayer();
				var info = localPlayer.GetPlayerInfo();
				var weapon = localPlayer.GetWeapon();
				var vehicle = localPlayer.GetVehicle();
				var vehicleColors = vehicle.GetColors();
				var vehicleHandling = vehicle.GetHandling();

				if (Features.ByName<BoolFeature>("Rainbow Vehicle").Value)
				{
					var clr = GetRainbow(rainbow);
					vehicleColors.PrimaryRed = clr.R;
					vehicleColors.PrimaryGreen = clr.G;
					vehicleColors.PrimaryBlue = clr.B;
					vehicleColors.SecondaryRed = clr.R;
					vehicleColors.SecondaryGreen = clr.G;
					vehicleColors.SecondaryBlue = clr.B;
				}

				if (_currentWeapon == null)
					_currentWeapon = weapon;
				if (_backupWeapon == null)
					_backupWeapon = new BackupWeapon(_currentWeapon);
				if (_currentVehicleHandling == null)
					_currentVehicleHandling = vehicleHandling;
				if (_backupVehicleHandling == null)
					_backupVehicleHandling = new BackupVehicleHandling(_currentVehicleHandling);

				if (_backupWeapon.NameHash != weapon.NameHash)
				{
					_currentWeapon.Restore(_backupWeapon);
					_currentWeapon = weapon;
					_backupWeapon = new BackupWeapon(_currentWeapon);
				}

				if (_backupVehicleHandling.Address != vehicleHandling.Address)
				{
					_currentVehicleHandling.Restore(_backupVehicleHandling);
					_currentVehicleHandling = vehicleHandling;
					_backupVehicleHandling = new BackupVehicleHandling(_currentVehicleHandling);
				}

				localPlayer.GodMode = Features.ByName<BoolFeature>("God Mode").Value;
				localPlayer.CanBeRagdolled = !Features.ByName<BoolFeature>("God Mode").Value;
				localPlayer.HasSeatBelt = Features.ByName<BoolFeature>("God Mode").Value;
				if (Features.ByName<BoolFeature>("Never Wanted").Value) info.WantedLevel = 0;

				var carGodMode = Features.ByName<BoolFeature>("Car God Mode").Value;
				vehicle.GodMode = carGodMode;
				vehicleHandling.CollisionDamage = carGodMode ? 0.0f : 1;
				vehicleHandling.EngineDamage = carGodMode ? 0.0f : 1;
				vehicleHandling.WeaponDamage = carGodMode ? 0.0f : 1;
				vehicleHandling.DeformationDamage = carGodMode ? 0.0f : 1;
				if (carGodMode)
					vehicle.BulletproofTires = true;
                vehicle.BoostAmount = Int32.MaxValue;

                info.RunSpeed = Features.ByName<IntFeature>("Run/Swim Speed").Value;
				info.SwimSpeed = Features.ByName<IntFeature>("Run/Swim Speed").Value;

				vehicleHandling.Acceleration = Features.ByName<IntFeature>("Acceleration").Value * _backupVehicleHandling.Acceleration;
				vehicleHandling.BrakeForce = Features.ByName<IntFeature>("Brake Force").Value * _backupVehicleHandling.BrakeForce;
				vehicleHandling.HandBrakeForce = Features.ByName<IntFeature>("Brake Force").Value * _backupVehicleHandling.HandBrakeForce;
				vehicleHandling.TractionCurveMin = Features.ByName<IntFeature>("Traction Curve").Value * _backupVehicleHandling.TractionCurveMin;
				vehicleHandling.SuspensionForce = Features.ByName<IntFeature>("Suspension Force").Value * _backupVehicleHandling.SuspensionForce;

				var frameFlags = info.FrameFlags;
				if (Features.ByName<BoolFeature>("Super Jump").Value)
					frameFlags |= 1 << 14;
				if (Features.ByName<BoolFeature>("Explosive Melee").Value)
					frameFlags |= 1 << 13;
				if (Features.ByName<BoolFeature>("Fire Ammo").Value)
					frameFlags |= 1 << 12;
				if (Features.ByName<BoolFeature>("Explosive Ammo").Value)
					frameFlags |= 1 << 11;

				info.FrameFlags = frameFlags;

				if (Features.ByName<BoolFeature>("Rank Boost").Value && !Features.ByName<BoolFeature>("Never Wanted").Value)
				{
					if (info.WantedLevel >= 5)
						info.WantedLevel = 0;
					else
						info.WantedLevel = 5;
					Thread.Sleep(10);
				}

				if (Features.ByName<BoolFeature>("Anti NPC").Value)
				{
					var attackers = localPlayer.GetAttackers();
					foreach (var ped in attackers)
					{
						ped.Health = 0;
						localPlayer.Health = localPlayer.MaxHealth;
					}
				}

				vehicleHandling.UpShift = _backupVehicleHandling.UpShift * Features.ByName<IntFeature>("Shift Rate").Value;

				//weapon.ReloadTime =  _backupWeapon.ReloadTime * (Features.ByName<BoolFeature>("Fast Reload").Value ? 10 : 1);

                var superBullets = Features.ByName<BoolFeature>("Super Bullets").Value;
				weapon.Damage = _backupWeapon.Damage * (superBullets ? 10 : 1);
				weapon.BulletBatch = _backupWeapon.BulletBatch * (superBullets ? 25 : 1);
				weapon.MuzzleVelocity = _backupWeapon.MuzzleVelocity * (superBullets ? 10 : 1);
				weapon.Range = _backupWeapon.Range * (superBullets ? 10 : 1);
				weapon.SpinUp = superBullets ? 0 : _backupWeapon.SpinUp;
				weapon.Spin = superBullets ? 0 : _backupWeapon.Spin;

                if (Features.ByName<BoolFeature>("No Reload").Value)
                    _currentWeapon.PrimaryAmmoCount.AmmoCount = 9999;

				//if (Features.ByName<BoolFeature>("Infinite Ammo").Value)
				//	Memory.WriteByteArray(AmmoPtr, three_nops);
				//else
				//	Memory.WriteByteArray(AmmoPtr, ammo_backup);

				Thread.Sleep(10);
			}
		}

		private static void SetPreset()
		{
			World.GetLocalPlayer().Position = TeleportPresets[_teleportPreset.Value].Positon;
		}

		private static void SavePosition()
		{
			var pos = World.GetLocalPlayer().Position;
			var name = $"Preset{TeleportPresets.Count}";
			var telPos = new TeleportPosition(name, pos);
			TeleportPresets.Add(telPos);
			Settings.SaveTeleportPresets();
		}

		private static void StopBoost()
		{
			for(int i = 0; i < 20; i++)
				Memory.Write<byte>(World.GetLocalPlayer().GetVehicle().Address + 0x318, 0);
		}

		public static Color GetRainbow(double progress)
		{
			return Hsl2Rgb(progress, 0.5, 0.5);
		}

		public static ColorRGB Hsl2Rgb(double h, double sl, double l)
		{
			double v;
			double r, g, b;

			r = l;   // default to gray
			g = l;
			b = l;
			v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);
			if (v > 0)
			{
				var m = l + l - v;
				var sv = (v - m) / v;
				h *= 6.0;
				var sextant = (int)h;
				var fract = h - sextant;
				var vsf = v * sv * fract;
				var mid1 = m + vsf;
				var mid2 = v - vsf;
				switch (sextant)
				{
					case 0:
						r = v;
						g = mid1;
						b = m;
						break;
					case 1:
						r = mid2;
						g = v;
						b = m;
						break;
					case 2:
						r = m;
						g = v;
						b = mid1;
						break;
					case 3:
						r = m;
						g = mid2;
						b = v;
						break;
					case 4:
						r = mid1;
						g = m;
						b = v;
						break;
					case 5:
						r = v;
						g = m;
						b = mid2;
						break;
				}
			}
			ColorRGB rgb;
			rgb.R = System.Convert.ToByte(r * 255.0f);
			rgb.G = System.Convert.ToByte(g * 255.0f);
			rgb.B = System.Convert.ToByte(b * 255.0f);
			return rgb;
		}

		private static void InfoLoop()
		{
			while (Memory.IsProcessRunning)
			{
				var enabled = Features.ByName<BoolFeature>("Master Toggle").Value;
				var count = 0;
				foreach (var feature in Features)
				{
					if (!enabled && feature.Name != "Master Toggle" && !Whitelist.Contains(feature.Name)) Console.NotificationColor = ConsoleColor.Gray;
					else Console.NotificationColor = ConsoleColor.Yellow;

					var key = feature.Key.ToString();
					if (feature.SecondaryKey != Keys.None) key += "/" + feature.SecondaryKey;
					Console.WriteNotification($"  [{key}] {feature.Name}: {feature.ToString()}   ");
					count++;
				}

				Console.NotificationColor = ConsoleColor.Yellow;
				var val = _teleportPreset.Value;
				Console.WriteNotification($"\n  [{_teleportPreset.Key}/{_teleportPreset.SecondaryKey}] {_teleportPreset.Name}: {TeleportPresets[val].Name} ({val})                ");
				count+=2;

				Console.SetCursorPosition(0, Console.CursorTop - count);
				Thread.Sleep(25);
			}
		}

		private static void Suicide()
		{
			World.GetLocalPlayer().Health = 0;
		}

		private static void Teleport()
		{
			var localPlayer = World.GetLocalPlayer();
			var vehicle = localPlayer.GetVehicle();
			var waypoint = localPlayer.GetWaypoint();
			var teleportPos = new Vector3D(waypoint, -225.0f);
			if (localPlayer.IsInVehicle)
				vehicle.Position = teleportPos;
			else
				localPlayer.Position = teleportPos;
		}

		private static void Teleport2()
		{
			var localPlayer = World.GetLocalPlayer();
			var vehicle = localPlayer.GetVehicle();
			var waypoint = localPlayer.GetWaypoint();
			var teleportPos = new Vector3D(waypoint, 225.0f);
			if (localPlayer.IsInVehicle)
				vehicle.Position = teleportPos;
			else
				localPlayer.Position = teleportPos;
		}

		private static void ChangeWantedLevel()
		{
			var localPlayer = World.GetLocalPlayer();
			var info = localPlayer.GetPlayerInfo();
			info.WantedLevel = Features.ByName<IntFeature>("Wanted Level").Value;
		}

		private static void KeyLoop()
		{
			while(Memory.IsProcessRunning)
			{
				var enabled = Features.ByName<BoolFeature>("Master Toggle").Value;
				foreach (var feature in Features)
				{
					if (!enabled && feature.Name != "Master Toggle" && !Whitelist.Contains(feature.Name)) continue;
					if (Utils.IsKeyDown(feature.Key))
					{
						feature.OnKey();
						Thread.Sleep(150);
					}
					else if (Utils.IsKeyDown(feature.SecondaryKey))
					{
						feature.OnSecondaryKey();
						Thread.Sleep(150);
					}
				}

				if (_teleportPreset != null)
				{
					var prevVal = _teleportPreset.Value;
					_teleportPreset.Value = prevVal;

					if (Utils.IsKeyDown(_teleportPreset.Key))
					{
						_teleportPreset.OnKey();
						Thread.Sleep(150);
					}
					else if (Utils.IsKeyDown(_teleportPreset.SecondaryKey))
					{
						_teleportPreset.OnSecondaryKey();
						Thread.Sleep(150);
					}
				}

				Thread.Sleep(25);
			}
		}
	}
}
