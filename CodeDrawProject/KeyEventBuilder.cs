using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace CodeDrawProject
{
	internal class KeyEventBuilder
	{
		public KeyEventBuilder() { }

		private Dictionary<Keys, bool> pressedKeys = new Dictionary<Keys, bool>();
		private SemaphoreSlim pressedKeysLock = new SemaphoreSlim(1);

		public async void KeyDownEventHandler(object? sender, KeyEventArgs args)
		{
			await pressedKeysLock.WaitAsync();

			if (!pressedKeys.GetValueOrDefault(args.KeyCode, false))
			{
				pressedKeys[args.KeyCode] = true;
				KeyDown?.Invoke(sender, args); ;
			}
			KeyPress?.Invoke(sender, args);

			pressedKeysLock.Release();
		}

		public async void KeyUpEventHandler(object? sender, KeyEventArgs args)
		{
			await pressedKeysLock.WaitAsync();

			pressedKeys[args.KeyCode] = false;
			KeyUp?.Invoke(sender, args);

			pressedKeysLock.Release();
		}

		public event KeyEventHandler? KeyDown;
		public event KeyEventHandler? KeyPress;
		public event KeyEventHandler? KeyUp;
	}
}
