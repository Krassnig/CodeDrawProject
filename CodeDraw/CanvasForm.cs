using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace CodeDrawNS
{
	internal class CanvasForm : IDisposable
	{
		public CanvasForm(Size size)
		{
			this.canvas = new Bitmap(size.Width, size.Height);
			this.form = new DoubleBufferedForm();

			form.SuspendLayout();

			form.AutoScaleDimensions = new SizeF(7F, 15F);
			form.AutoScaleMode = AutoScaleMode.Font;
			form.ClientSize = size;
			form.Name = "LiteDrawForm";
			form.Icon = Properties.Resources.CodeDrawIcon;
			form.MaximizeBox = false;
			form.FormBorderStyle = FormBorderStyle.FixedSingle;
			form.KeyDown += OnCtrlCPasteDisplayImageToClipboard;

			form.ResumeLayout(false);

			form.KeyDown += keyDown.KeyPressEventHandler;
			form.KeyUp += keyDown.KeyUpEventHandler;
		}

		private Form form;
		private Bitmap canvas;
		private BufferedGraphicsContext context = new BufferedGraphicsContext();
		private KeyDownDictionary keyDown = new KeyDownDictionary();

		public string Title
		{
			get => form.Text;
			set => InvokeAsync(() => form.Text = value);
		}

		public Point DesktopLocation
		{
			get => form.DesktopLocation;
			set => InvokeAsync(() => form.DesktopLocation = value);
		}

		public bool ExitOnLastClose { get; set; } = true;

		public event MouseEventHandler MouseClick
		{
			add => form.MouseClick += value;
			remove => form.MouseClick -= value;
		}
		public event MouseEventHandler MouseMove
		{
			add => form.MouseMove += value;
			remove => form.MouseMove -= value;
		}
		public event MouseEventHandler MouseDown
		{
			add => form.MouseDown += value;
			remove => form.MouseDown -= value;
		}
		public event MouseEventHandler MouseUp
		{
			add => form.MouseUp += value;
			remove => form.MouseUp -= value;
		}
		public event MouseEventHandler MouseWheel
		{
			add => form.MouseWheel += value;
			remove => form.MouseWheel -= value;
		}
		public event EventHandler MouseEnter
		{
			add => form.MouseEnter += value;
			remove => form.MouseLeave -= value;
		}
		public event EventHandler MouseLeave
		{
			add => form.MouseLeave += value;
			remove => form.MouseLeave -= value;
		}

		public event KeyEventHandler KeyDown
		{
			add => keyDown.KeyDown += value;
			remove => keyDown.KeyDown -= value;
		}
		public event KeyEventHandler KeyPress
		{
			add => form.KeyDown += value;
			remove => form.KeyDown -= value;
		}
		public event KeyEventHandler KeyUp
		{
			add => form.KeyUp += value;
			remove => form.KeyUp -= value;
		}

		public event EventHandler WindowMove
		{
			add => form.Move += value;
			remove => form.Move -= value;
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

			using Semaphore s = new Semaphore(0, 100);
			InvokeAsync(() =>
			{
				CopyToForm(buffer);
				s.Release();
			});
			Thread.Sleep(waitMilliseconds);
			s.WaitOne();
		}

		public void WaitForInvocability()
		{
			while (!form.IsHandleCreated) Thread.Sleep(10);
			InvokeSync(() => { });
		}

		private void OnCtrlCPasteDisplayImageToClipboard(object? sender, KeyEventArgs args)
		{
			if (args.Control && args.KeyCode == Keys.C)
			{
				Clipboard.SetImage(canvas);
			}
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
			using Semaphore s = new Semaphore(0, 100);

			InvokeAsync(() =>
			{
				action();
				s.Release();
			});

			s.WaitOne();
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

		public static CanvasForm Create(Size size)
		{
			return GuiRunner.CreateGui(size);
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
