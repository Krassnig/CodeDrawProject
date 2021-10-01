using System;
using System.Threading;
using System.Windows.Forms;

namespace CodeDrawProject
{
	internal class EnterLeaveEventBuilder
	{
		public EnterLeaveEventBuilder() { }

		private MouseEventArgs? lastMouseMoveArgs = null;
		private SemaphoreSlim builderLock = new SemaphoreSlim(1);
		private bool triggerMouseEnter = false;

		public async void MouseMoveEventHandler(object? sender, MouseEventArgs args)
		{
			await builderLock.WaitAsync();

			if (triggerMouseEnter)
			{
				triggerMouseEnter = false;
				MouseEnter?.Invoke(sender, args);
			}

			lastMouseMoveArgs = args;

			builderLock.Release();
		}

		public async void MouseEnterEventHandler(object? sender, EventArgs args)
		{
			await builderLock.WaitAsync();

			triggerMouseEnter = true;

			builderLock.Release();
		}

		public async void MouseLeaveEventHandler(object? sender, EventArgs args)
		{
			await builderLock.WaitAsync();

			if (lastMouseMoveArgs is not null)
			{
				MouseLeave?.Invoke(sender, lastMouseMoveArgs);
			}

			builderLock.Release();
		}

		public event MouseEventHandler? MouseEnter;
		public event MouseEventHandler? MouseLeave;
	}
}
