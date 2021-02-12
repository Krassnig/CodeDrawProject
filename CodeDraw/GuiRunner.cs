using System;
using System.Drawing;
using System.Threading;

namespace CodeDrawNS
{
	internal class GuiRunner : IDisposable
	{
		public static CanvasForm CreateGui(Size size)
		{
			using GuiRunner runner = new GuiRunner(size);
			return runner.Start();
		}

		private GuiRunner(Size size)
		{
			this.size = size;
			thread = new Thread(Run);
		}

		private CanvasForm Start()
		{
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			canvasIsSet.WaitOne();
			if (canvas == null) throw new Exception("LiteDraw is in an invalid state, although gui has been instanciated the gui is still null.");
			canvas.WaitForInvocability();
			return canvas;
		}

		private Size size;
		private CanvasForm? canvas;
		private Semaphore canvasIsSet = new Semaphore(0, 100);
		private Thread thread;

		[STAThread]
		private void Run()
		{
			CanvasForm.Configure();

			try
			{
				IncrementCanvasCount();
				canvas = new CanvasForm(size);
				canvasIsSet.Release();

				CanvasForm.Run(canvas);
			}
			finally
			{
				DecrementCanvasCount(canvas == null ? true : canvas.ExitOnLastClose);
			}
		}

		public void Dispose()
		{
			canvasIsSet.Dispose();
		}

		private static Semaphore canvasCountRegion = new Semaphore(1, int.MaxValue);
		private static int canvasCount = 0;

		private static void IncrementCanvasCount()
		{
			canvasCountRegion.WaitOne();
			canvasCount++;
			canvasCountRegion.Release();
		}

		private static void DecrementCanvasCount(bool closeIfCountZero)
		{
			canvasCountRegion.WaitOne();
			canvasCount--;
			if (canvasCount == 0 && closeIfCountZero) Environment.Exit(0);
			canvasCountRegion.Release();
		}
	}
}
