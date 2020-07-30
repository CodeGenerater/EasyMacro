using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace CodeGenerater.Utility.EasyMacro
{
	internal class KeyHolder<TKeyType> : IDisposable where TKeyType : Enum
	{
		#region Constructor
		public KeyHolder(Action<TKeyType> MacroAction)
		{
			this.MacroAction = MacroAction;

			thread = new Thread(ThreadProc);
			thread.Start();
		}
		#endregion

		#region Finalizer
		~KeyHolder()
		{
			Dispose();
		}
		#endregion

		#region Field
		Action<TKeyType> MacroAction;

		Thread thread;

		bool Continue = true;

		Dictionary<TKeyType, DateTime> KeyDict = new Dictionary<TKeyType, DateTime>();

		Dictionary<TKeyType, DateTime> LoopDict = new Dictionary<TKeyType, DateTime>();
		#endregion

		#region Property
		public int PressInterval
		{
			set;
			get;
		}
		#endregion

		#region Method
		public void Regist(TKeyType Key, TimeSpan Time)
		{
			DateTime Target = DateTime.Now + Time;

			if (!KeyDict.ContainsKey(Key))
			{
				KeyDict.Add(Key, Target);
				LoopDict.Add(Key, DateTime.Now + TimeSpan.FromMilliseconds(PressInterval));
			}
			else
				KeyDict[Key] = Target;
		}

		public bool IsRegist(TKeyType Key)
		{
			return KeyDict.ContainsKey(Key);
		}

		public void Unregist(TKeyType Key)
		{
			if (KeyDict.ContainsKey(Key))
			{
				KeyDict.Remove(Key);
				LoopDict.Remove(Key);
			}
		}

		public void Dispose()
		{
			Continue = false;
		}
		#endregion

		#region Helper
		void ThreadProc()
		{
			while (Continue)
			{
				if (KeyDict.Count == 0)
				{
					Thread.Sleep(1);
					continue;
				}

				DateTime Now = DateTime.Now;

				foreach (var each in LoopDict.ToArray())
				{
					if (KeyDict[each.Key] <= Now)
					{
						LoopDict.Remove(each.Key);
						KeyDict.Remove(each.Key);
					}

					if (each.Value <= Now)
					{
						MacroAction(each.Key);
						LoopDict[each.Key] = Now + TimeSpan.FromMilliseconds(PressInterval);
					}
				}
			}
		}
		#endregion
	}
}