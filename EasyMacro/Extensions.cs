using System;
using System.Threading;

namespace CodeGenerater.Utility.EasyMacro
{
	public static class Extensions
	{
		public static MacroMouse Delay(this MacroMouse Mouse, int Delay)
		{
			Thread.Sleep(Delay);

			return Mouse;
		}

		public static MacroKeyboard Delay(this MacroKeyboard Keyboard, int Delay)
		{
			Thread.Sleep(Delay);

			return Keyboard;
		}
	}
}