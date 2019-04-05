using BlyadTheftAuto.MemorySystem;
using System;
using System.Diagnostics;
using System.Linq;

namespace BlyadTheftAuto
{
    internal class Utils
    {
        private static ProcessMemory Memory => BlyadTheftAuto.Memory;

        public static bool IsModuleLoaded(Process p, string moduleName)
        {
            var q = from m in p.Modules.OfType<ProcessModule>()
                    select m;
            return q.Any(pm => pm.ModuleName == moduleName && (int)pm.BaseAddress != 0);
        }

        public static bool IsKeyDown(System.Windows.Forms.Keys key)
        {
            return IsKeyDown((int)key);
        }

        public static bool IsKeyDown(int key)
        {
            return (NativeMethods.GetAsyncKeyState(key) & 0x8000) != 0;
        }
        
        public static string ByteSizeToString(long size)
        {
            string[] strArrays = new string[] { "B", "KB", "MB", "GB", "TB" };
            int num = 0;
            while (size > 1024)
            {
                size = size / 1024;
                num++;
            }
            string str = string.Format("{0} {1}", size.ToString(), strArrays[num]);
            return str;
        }
		
        /*public static bool IsPointInRadius(Vector2D point, Vector2D center, float radius)
        {
            return Math.Sqrt(((center.X - point.X) * (center.X - point.X)) + ((center.Y - point.Y) * (center.Y - point.Y))) < radius;
        }

        public static float DistanceToPoint(Vector2D point, Vector2D otherPoint)
        {
            float ydist = (otherPoint.Y - point.Y);
            float xdist = (otherPoint.X - point.X);
            float hypotenuse = Convert.ToSingle(Math.Sqrt(Math.Pow(ydist, 2) + Math.Pow(xdist, 2)));
            return hypotenuse;
        }*/
    }
}
