using System;
using System.Windows.Forms;

namespace CodeDrawProject
{
	internal class EventInvokeCollection
	{
		public EventInvokeCollection() { }

		public Action<MouseEventArgs>? MouseClick { get; init; }
		public Action<MouseEventArgs>? MouseMove { get; init; }
		public Action<MouseEventArgs>? MouseDown { get; init; }
		public Action<MouseEventArgs>? MouseUp { get; init; }
		public Action<MouseEventArgs>? MouseWheel { get; init; }
		public Action<EventArgs>? MouseEnter { get; init; }
		public Action<EventArgs>? MouseLeave { get; init; }

		public Action<KeyPressEventArgs>? KeyPress { get; init; }
		public Action<KeyEventArgs>? KeyDown { get; init; }
		public Action<KeyEventArgs>? KeyUp { get; init; }

		public Action<EventArgs>? WindowMove { get; init; }

		private KeyDownDictionary keyDownDictionary = new KeyDownDictionary();

		public void SubscribeEvents(Form form)
		{
			form.MouseClick += (s, a) => MouseClick?.Invoke(a);
			form.MouseMove += (s, a) => MouseMove?.Invoke(a);
			form.MouseDown += (s, a) => MouseDown?.Invoke(a);
			form.MouseUp += (s, a) => MouseUp?.Invoke(a);
			form.MouseWheel += (s, a) => MouseWheel?.Invoke(a);
			form.MouseEnter += (s, a) => MouseEnter?.Invoke(a);
			form.MouseLeave += (s, a) => MouseLeave?.Invoke(a);
			form.KeyPress += (s, a) => KeyPress?.Invoke(a);
			form.KeyDown += keyDownDictionary.KeyDownEventHandler;
			form.KeyUp += keyDownDictionary.KeyUpEventHandler;
			keyDownDictionary.KeyDown += (s, a) => KeyDown?.Invoke(a);
			form.KeyUp += (s, a) => KeyUp?.Invoke(a);
			form.Move += (s, a) => WindowMove?.Invoke(a);
		}
	}
}
