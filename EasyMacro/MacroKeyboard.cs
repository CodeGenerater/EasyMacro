using System;
using System.Threading;
using System.Windows.Input;
using System.Collections.Generic;

namespace CodeGenerater.Utility.EasyMacro
{
	public class MacroKeyboard
	{
		#region Constructor
		MacroKeyboard()
		{
			bScanDict = new Dictionary<int, int>();
			string[] array = Properties.Resources.HardwareCode.Split(new string[1]
			{
			"\r\n"
			}, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split('\t');
				bScanDict.Add(Convert.ToInt32(array2[0], 16), Convert.ToInt32(array2[1], 16));
			}
		}
		#endregion

		#region Field
		static MacroKeyboard Instance;

		Dictionary<int, int> bScanDict;
		#endregion

		#region Property
		public static MacroKeyboard Default
		{
			get
			{
				if (Instance == null)
					Instance = new MacroKeyboard();
				return Instance;
			}
		}
		#endregion

		#region Method
		public MacroKeyboard Down(Key Key)
		{
			int num = KeyInterop.VirtualKeyFromKey(Key);
			StaticFunction.keybd_event((byte)num, (byte)(bScanDict.ContainsKey(num) ? bScanDict[num] : 0), 0, IntPtr.Zero);

			return this;
		}

		public MacroKeyboard Up(Key Key)
		{
			int num = KeyInterop.VirtualKeyFromKey(Key);
			StaticFunction.keybd_event((byte)num, (byte)(bScanDict.ContainsKey(num) ? bScanDict[num] : 0), 2, IntPtr.Zero);

			return this;
		}

		public MacroKeyboard Delay(int Delay)
		{
			Thread.Sleep(Delay);

			return this;
		}

		public MKeyState GetKeyState(Key Key)
		{
			return StaticFunction.GetAsyncKeyState(KeyInterop.VirtualKeyFromKey(Key)) == 0 ? MKeyState.U : MKeyState.D;
		}
		#endregion
	}
}