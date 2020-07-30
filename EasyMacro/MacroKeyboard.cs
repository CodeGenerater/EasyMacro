using System;
using System.Threading;
using System.Windows.Input;
using System.Collections.Generic;

namespace CodeGenerater.Utility.EasyMacro
{
	public class MacroKeyboard : IDisposable
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

			KeyHolder = new KeyHolder<Key>(k => Down(k));
		}
		#endregion

		#region Field
		static MacroKeyboard Instance;

		Dictionary<int, int> bScanDict;

		KeyHolder<Key> KeyHolder;
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

		public MacroKeyboard KeepDown(Key Key, TimeSpan Time)
		{
			Down(Key);

			KeyHolder.Regist(Key, Time);

			return this;
		}

		public MacroKeyboard Up(Key Key)
		{
			if (KeyHolder.IsRegist(Key))
				KeyHolder.Unregist(Key);

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

		public void Dispose()
		{
			KeyHolder.Dispose();
		}
		#endregion
	}
}