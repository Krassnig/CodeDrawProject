using System.Collections.Generic;
using System.Windows.Forms;

namespace CodeDrawProject
{
	internal class KeyDownDictionary
	{
		public KeyDownDictionary() { }

		private Dictionary<Keys, bool> keyPress = new Dictionary<Keys, bool>();

		public void KeyDownEventHandler(object? sender, KeyEventArgs args)
		{
			if (!keyPress.GetValueOrDefault(args.KeyData, false))
			{
				keyPress[args.KeyData] = true;
				KeyDown?.Invoke(sender, args);
			}
		}

		public void KeyUpEventHandler(object? sender, KeyEventArgs args)
		{
			keyPress[args.KeyData] = false;
		}

		public event KeyEventHandler? KeyDown;
	}
}
