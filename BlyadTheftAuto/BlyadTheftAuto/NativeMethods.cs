using BlyadTheftAuto.Structs;
using System;
using System.Runtime.InteropServices;

namespace BlyadTheftAuto
{
    internal class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr hwnd, out RECT lpRect);
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(int key);
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hwnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
    }
}
