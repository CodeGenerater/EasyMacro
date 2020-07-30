using System;
using System.Runtime.InteropServices;

namespace CodeGenerater.Utility.EasyMacro
{
	internal class StaticFunction
	{
		[DllImport("user32.dll")]
		public static extern bool GetCursorPos(out MPoint Point);

		[DllImport("user32.dll")]
		public static extern bool SetCursorPos(int X, int Y);

		[DllImport("user32.dll")]
		public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, IntPtr dwExtraInfo);

		[DllImport("user32.dll")]
		public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, IntPtr dwExtraInfo);

		[DllImport("user32.dll")]
		public static extern short GetAsyncKeyState(int vKey);
	}
}