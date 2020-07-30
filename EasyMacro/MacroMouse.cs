using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CodeGenerater.Utility.EasyMacro
{
	public class MacroMouse : IDisposable
	{
		#region Enum
		private enum MouseEvent
		{
			MOUSEEVENTF_ABSOLUTE = 0x8000,
			MOUSEEVENTF_LEFTDOWN = 0x0002,
			MOUSEEVENTF_LEFTUP = 0x0004,
			MOUSEEVENTF_MIDDLEDOWN = 0x0020,
			MOUSEEVENTF_MIDDLEUP = 0x0040,
			MOUSEEVENTF_MOVE = 0x0001,
			MOUSEEVENTF_RIGHTDOWN = 0x0008,
			MOUSEEVENTF_RIGHTUP = 0x0010,
			MOUSEEVENTF_XDOWN = 0x0080,
			MOUSEEVENTF_XUP = 0x0100,
			MOUSEEVENTF_WHEEL = 0x0800,
			MOUSEEVENTF_HWHEEL = 0x01000,
		}
		#endregion

		#region Constructor
		private MacroMouse()
		{
			KeyHolder = new KeyHolder<MMouseButton>(b => Down(b));
		}
		#endregion

		#region Field
		static MacroMouse Instance;

		KeyHolder<MMouseButton> KeyHolder;
		#endregion

		#region Property
		public static MacroMouse Default
		{
			get
			{
				if (Instance == null)
					Instance = new MacroMouse();
				return Instance;
			}
		}

		public MPoint Position
		{
			set
			{
				if (!StaticFunction.SetCursorPos(value.X, value.Y))
					throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			get
			{
				if (StaticFunction.GetCursorPos(out MPoint Point))
					return Point;

				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
		}

		public MKeyState Left
		{
			set
			{
				StaticFunction.mouse_event((value == MKeyState.Up) ? (int)MouseEvent.MOUSEEVENTF_LEFTUP : (int)MouseEvent.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, IntPtr.Zero);
			}
			get
			{
				if (StaticFunction.GetAsyncKeyState((int)MMouseButton.L) != 0)
					return MKeyState.D;
				else
					return MKeyState.U;
			}
		}

		public MKeyState Right
		{
			set
			{
				StaticFunction.mouse_event((value == MKeyState.Up) ? (int)MouseEvent.MOUSEEVENTF_RIGHTUP : (int)MouseEvent.MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, IntPtr.Zero);
			}
			get
			{
				if (StaticFunction.GetAsyncKeyState((int)MMouseButton.R) != 0)
					return MKeyState.D;
				else
					return MKeyState.U;
			}
		}

		public MKeyState Middle
		{
			set
			{
				StaticFunction.mouse_event((value == MKeyState.Up) ? (int)MouseEvent.MOUSEEVENTF_MIDDLEUP : (int)MouseEvent.MOUSEEVENTF_MIDDLEDOWN, 0, 0, 0, IntPtr.Zero);
			}
			get
			{
				if (StaticFunction.GetAsyncKeyState((int)MMouseButton.M) != 0)
					return MKeyState.D;
				else
					return MKeyState.U;
			}
		}

		public MKeyState X1
		{
			set
			{
				StaticFunction.mouse_event((value == MKeyState.Up) ? (int)MouseEvent.MOUSEEVENTF_XUP : (int)MouseEvent.MOUSEEVENTF_XDOWN, 0, 0, 1, IntPtr.Zero);
			}
			get
			{
				if (StaticFunction.GetAsyncKeyState((int)MMouseButton.X1) != 0)
					return MKeyState.D;
				else
					return MKeyState.U;
			}
		}

		public MKeyState X2
		{
			set
			{
				StaticFunction.mouse_event((value == MKeyState.Up) ? (int)MouseEvent.MOUSEEVENTF_XUP : (int)MouseEvent.MOUSEEVENTF_XDOWN, 0, 0, 2, IntPtr.Zero);
			}
			get
			{
				if (StaticFunction.GetAsyncKeyState((int)MMouseButton.X2) != 0)
					return MKeyState.D;
				else
					return MKeyState.U;
			}
		}
		#endregion

		#region Method
		public MacroMouse Down(MMouseButton Button)
		{
			switch (Button)
			{
				case MMouseButton.Left:
					StaticFunction.mouse_event((int)MouseEvent.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, IntPtr.Zero);
					break;
				case MMouseButton.Right:
					StaticFunction.mouse_event((int)MouseEvent.MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, IntPtr.Zero);
					break;
				case MMouseButton.Middle:
					StaticFunction.mouse_event((int)MouseEvent.MOUSEEVENTF_MIDDLEDOWN, 0, 0, 0, IntPtr.Zero);
					break;
				case MMouseButton.X1:
					StaticFunction.mouse_event((int)MouseEvent.MOUSEEVENTF_XDOWN, 0, 0, 1, IntPtr.Zero);
					break;
				case MMouseButton.X2:
					StaticFunction.mouse_event((int)MouseEvent.MOUSEEVENTF_XDOWN, 0, 0, 2, IntPtr.Zero);
					break;
				default:
					throw new ArgumentOutOfRangeException("Button");
			}

			return this;
		}

		public MacroMouse KeepDown(MMouseButton Button, TimeSpan Time)
		{
			Down(Button);

			KeyHolder.Regist(Button, Time);

			return this;
		}

		public MacroMouse Up(MMouseButton Button)
		{
			if (KeyHolder.IsRegist(Button))
				KeyHolder.Unregist(Button);

			switch (Button)
			{
				case MMouseButton.Left:
					StaticFunction.mouse_event((int)MouseEvent.MOUSEEVENTF_LEFTUP, 0, 0, 0, IntPtr.Zero);
					break;
				case MMouseButton.Right:
					StaticFunction.mouse_event((int)MouseEvent.MOUSEEVENTF_RIGHTUP, 0, 0, 0, IntPtr.Zero);
					break;
				case MMouseButton.Middle:
					StaticFunction.mouse_event((int)MouseEvent.MOUSEEVENTF_MIDDLEUP, 0, 0, 0, IntPtr.Zero);
					break;
				case MMouseButton.X1:
					StaticFunction.mouse_event((int)MouseEvent.MOUSEEVENTF_XUP, 0, 0, 1, IntPtr.Zero);
					break;
				case MMouseButton.X2:
					StaticFunction.mouse_event((int)MouseEvent.MOUSEEVENTF_XUP, 0, 0, 2, IntPtr.Zero);
					break;
				default:
					throw new ArgumentOutOfRangeException("Button");
			}

			return this;
		}

		public MacroMouse Click(MMouseButton Button)
		{
			Down(Button).Up(Button);

			return this;
		}

		public MacroMouse Click(MMouseButton Button, int Delay)
		{
			Down(Button).Delay(Delay).Up(Button);

			return this;
		}

		public MPoint GetPos()
		{
			return Position;
		}

		public MacroMouse SetPos(MPoint Point)
		{
			Position = Point;

			return this;
		}

		public MacroMouse SetPos(int X, int Y)
		{
			Position = new MPoint(X, Y);

			return this;
		}

		public MacroMouse Move(MPoint Point)
		{
			StaticFunction.mouse_event((int)MouseEvent.MOUSEEVENTF_MOVE, Point.X, Point.Y, 0, IntPtr.Zero);

			return this;
		}

		public MacroMouse Move(int X, int Y)
		{
			StaticFunction.mouse_event((int)MouseEvent.MOUSEEVENTF_MOVE, X, Y, 0, IntPtr.Zero);

			return this;
		}
		public MacroMouse Wheel(WheelDirection Direction, double Count)
		{
			if (Count <= 0)
				throw new ArgumentOutOfRangeException("Count");

			MouseEvent E;

			if (Direction == WheelDirection.Up || Direction == WheelDirection.Down)
				E = MouseEvent.MOUSEEVENTF_WHEEL;
			else
				E = MouseEvent.MOUSEEVENTF_HWHEEL;

			int dwData = (int)(120.0 * Count);

			if (Direction == WheelDirection.Left || Direction == WheelDirection.Down)
				dwData = -dwData;

			StaticFunction.mouse_event((int)E, 0, 0, dwData, IntPtr.Zero);

			return this;
		}

		public void Dispose()
		{
			KeyHolder.Dispose();
		}
		#endregion
	}
}