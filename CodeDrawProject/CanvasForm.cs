using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace CodeDrawProject
{
	internal class CanvasForm : IDisposable
	{
		public CanvasForm(Size size, EventInvokeCollection events)
		{
			displayBuffer = new CodeDrawGraphics(size.Width, size.Height);
			form = new DoubleBufferedForm(size);
			formGraphics = form.CreateGraphics();
			events.SubscribeEvents(form);
		}

		private Form form;
		private Graphics formGraphics;
		private CodeDrawGraphics displayBuffer;

		public bool ExitOnLastClose { get; set; } = true;

		public string Title
		{
			get => form.Text;
			set => InvokeAsync(() => form.Text = value);
		}

		public Point FramePosition
		{
			get => form.DesktopLocation;
			set => InvokeAsync(() => form.DesktopLocation = value);
		}

		public void Render(CodeDrawGraphics buffer)
		{
			CopyToDisplayBuffer(buffer);
			InvokeSync(() => CopyDisplayBufferToForm());
		}

		public void Render(CodeDrawGraphics buffer, int waitMilliseconds)
		{
			CopyToDisplayBuffer(buffer);

			using SemaphoreSlim s = new SemaphoreSlim(0);
			InvokeAsync(() =>
			{
				CopyDisplayBufferToForm();
				s.Release();
			});
			Thread.Sleep(waitMilliseconds);
			s.Wait();
		}

		public void WaitForInvocability()
		{
			while (!form.IsHandleCreated) Thread.Sleep(10);
			Thread.Sleep(10);
			InvokeSync(() => { });
		}

		public void CopyToClipboard()
		{
			Clipboard.SetImage(displayBuffer.CopyAsImage());
		}

		private void CopyToDisplayBuffer(CodeDrawGraphics codeDrawBuffer)
		{
			codeDrawBuffer.CopyTo(displayBuffer);
		}

		private void CopyDisplayBufferToForm()
		{
			displayBuffer.CopyTo(formGraphics);
		}

		private void InvokeAsync(Action action)
		{
			try
			{
				if (form.InvokeRequired)
				{
					form.Invoke(action);
				}
				else
				{
					action();
				}
			}
			catch (ObjectDisposedException) { }
		}

		private void InvokeSync(Action action)
		{
			using SemaphoreSlim s = new SemaphoreSlim(0);

			InvokeAsync(() =>
			{
				action();
				s.Release();
			});

			s.Wait();
		}

		public void CloseAndDispose(bool exitOnLastClose)
		{
			ExitOnLastClose = exitOnLastClose;
			InvokeSync(() => form.Close());
			Dispose();
		}

		public void Dispose()
		{
			displayBuffer.Dispose();
			form.Dispose();
			formGraphics.Dispose();
		}

		public static CanvasForm Create(Size size, EventInvokeCollection events)
		{
			return GuiRunner.CreateGui(size, events);
		}

		public static void Configure()
		{
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
		}

		public static void Run(CanvasForm form)
		{
			Application.Run(form.form);
		}
	}
}
