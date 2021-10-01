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
		public Action<MouseEventArgs>? MouseEnter { get; init; }
		public Action<MouseEventArgs>? MouseLeave { get; init; }

		public Action<KeyEventArgs>? KeyPress { get; init; }
		public Action<KeyEventArgs>? KeyDown { get; init; }
		public Action<KeyEventArgs>? KeyUp { get; init; }

		public Action<EventArgs>? WindowMove { get; init; }

		private KeyEventBuilder keyDownBuilder = new KeyEventBuilder();
		private EnterLeaveEventBuilder mouseEnterLeaveBuilder = new EnterLeaveEventBuilder();

		public void SubscribeEvents(Form form)
		{
			form.MouseClick += (s, a) => MouseClick?.Invoke(a);
			form.MouseMove += (s, a) => MouseMove?.Invoke(a);
			form.MouseDown += (s, a) => MouseDown?.Invoke(a);
			form.MouseUp += (s, a) => MouseUp?.Invoke(a);
			form.MouseWheel += (s, a) => MouseWheel?.Invoke(a);
			form.Move += (s, a) => WindowMove?.Invoke(a);

			form.KeyDown += keyDownBuilder.KeyDownEventHandler;
			form.KeyUp += keyDownBuilder.KeyUpEventHandler;
			keyDownBuilder.KeyDown += (s, a) => KeyDown?.Invoke(a);
			keyDownBuilder.KeyPress += (s, a) => KeyPress?.Invoke(a);
			keyDownBuilder.KeyUp += (s, a) => KeyUp?.Invoke(a);

			form.MouseEnter += mouseEnterLeaveBuilder.MouseEnterEventHandler;
			form.MouseLeave += mouseEnterLeaveBuilder.MouseLeaveEventHandler;
			form.MouseMove += mouseEnterLeaveBuilder.MouseMoveEventHandler;
			mouseEnterLeaveBuilder.MouseEnter += (s, a) => MouseEnter?.Invoke(a);
			mouseEnterLeaveBuilder.MouseLeave += (s, a) => MouseLeave?.Invoke(a);
		}
	}
}
