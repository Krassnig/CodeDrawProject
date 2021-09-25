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
			canvas = new Bitmap(size.Width, size.Height);
			form = new DoubleBufferedForm();
			events.SubscribeEvents(form);

			form.SuspendLayout();

			form.AutoScaleDimensions = new SizeF(7F, 15F);
			form.AutoScaleMode = AutoScaleMode.Font;
			form.ClientSize = size;
			form.Icon = Properties.Resources.CodeDrawIcon;
			form.MaximizeBox = false;
			form.FormBorderStyle = FormBorderStyle.FixedSingle;

			form.ResumeLayout(false);
		}

		private Form form;
		private Bitmap canvas;
		private BufferedGraphicsContext context = new BufferedGraphicsContext();

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

		public BufferedGraphics CreateBufferedGraphics()
		{
			return context.Allocate(form.CreateGraphics(), form.DisplayRectangle);
		}

		public void Render(BufferedGraphics buffer)
		{
			CopyToCanvas(buffer);
			InvokeSync(() => CopyToForm(buffer));
		}

		public void Render(BufferedGraphics buffer, int waitMilliseconds)
		{
			CopyToCanvas(buffer);

			using SemaphoreSlim s = new SemaphoreSlim(0);
			InvokeAsync(() =>
			{
				CopyToForm(buffer);
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
			Clipboard.SetImage(canvas);
		}

		private void CopyToCanvas(BufferedGraphics buffer)
		{
			using Graphics g1 = Graphics.FromImage(canvas);
			buffer.Render(g1);
		}

		private void CopyToForm(BufferedGraphics buffer)
		{
			using Graphics g2 = form.CreateGraphics();
			buffer.Render(g2);
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
			canvas.Dispose();
			form.Dispose();
			context.Dispose();
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
