using System;
using System.Drawing;
using System.Windows.Forms;

namespace CodeDrawProject
{
	public class CodeDraw : IDisposable
	{
		public CodeDraw() : this(600, 600) { }

		public CodeDraw(int canvasWidth, int canvasHeight)
		{
			if (canvasWidth < 150) throw new ArgumentOutOfRangeException("The width of the canvas has to be at least 150px.");
			if (canvasHeight < 1) throw new ArgumentOutOfRangeException("The height of the canvas has to be positive");

			EventInvokeCollection events = CreateEventInvokeCollection();
			g = new CodeDrawGraphics(canvasWidth, canvasHeight);
			form = CanvasForm.Create(new Size(Width, Height), events);
			
			Title = "CodeDraw";
			Show();

			KeyDown += (cd, args) =>
			{
				if (args.Control && args.KeyCode == Keys.C)
				{
					form.CopyToClipboard();
				}
			};
		}

		private CanvasForm form;
		private CodeDrawGraphics g;

		public TextFormat TextFormat { get; set; } = new TextFormat();

		public int Width => g.Width;
		public int Height => g.Height;

		public int FramePositionX
		{
			get => form.FramePosition.X;
			set => form.FramePosition = new Point(value, form.FramePosition.Y);
		}
		public int FramePositionY
		{
			get => form.FramePosition.Y;
			set => form.FramePosition = new Point(form.FramePosition.X, value);
		}

		public string Title
		{
			get => form.Title;
			set => form.Title = value;
		}

		public Color Color
		{
			get => g.Color;
			set => g.Color = value;
		}

		public double LineWidth
		{
			get => g.LineWidth;
			set
			{
				if (value <= 0) throw CreateArgumentNotNegative(nameof(LineWidth));
				g.LineWidth = value;
			}
		}

		public bool IsAntialiased
		{
			get => g.IsAntialiased;
			set => g.IsAntialiased = value;
		}

		public event EventHandler<MouseEventArgs>? MouseClick;
		public event EventHandler<MouseEventArgs>? MouseMove;
		public event EventHandler<MouseEventArgs>? MouseDown;
		public event EventHandler<MouseEventArgs>? MouseUp;
		public event EventHandler<MouseEventArgs>? MouseEnter;
		public event EventHandler<MouseEventArgs>? MouseLeave;
		public event EventHandler<MouseEventArgs>? MouseWheel;
		public event EventHandler<KeyEventArgs>? KeyPress;
		public event EventHandler<KeyEventArgs>? KeyUp;
		public event EventHandler<KeyEventArgs>? KeyDown;
		public event EventHandler<EventArgs>? WindowMove;

		protected virtual void OnMouseClick(MouseEventArgs args) => MouseClick?.Invoke(this, args);
		protected virtual void OnMouseMove(MouseEventArgs args) => MouseMove?.Invoke(this, args);
		protected virtual void OnMouseDown(MouseEventArgs args) => MouseDown?.Invoke(this, args);
		protected virtual void OnMouseUp(MouseEventArgs args) => MouseUp?.Invoke(this, args);
		protected virtual void OnMouseEnter(MouseEventArgs args) => MouseEnter?.Invoke(this, args);
		protected virtual void OnMouseLeave(MouseEventArgs args) => MouseLeave?.Invoke(this, args);
		protected virtual void OnMouseWheel(MouseEventArgs args) => MouseWheel?.Invoke(this, args);
		protected virtual void OnKeyPress(KeyEventArgs args) => KeyPress?.Invoke(this, args);
		protected virtual void OnKeyUp(KeyEventArgs args) => KeyUp?.Invoke(this, args);
		protected virtual void OnKeyDown(KeyEventArgs args) => KeyDown?.Invoke(this, args);
		protected virtual void OnWindowMove(EventArgs args) => WindowMove?.Invoke(this, args);

		private EventInvokeCollection CreateEventInvokeCollection()
		{
			return new EventInvokeCollection()
			{
				MouseClick = OnMouseClick,
				MouseMove = OnMouseMove,
				MouseDown = OnMouseDown,
				MouseUp = OnMouseUp,
				MouseEnter = OnMouseEnter,
				MouseLeave = OnMouseLeave,
				MouseWheel = OnMouseWheel,
				KeyPress = OnKeyPress,
				KeyUp = OnKeyUp,
				KeyDown = OnKeyDown,
				WindowMove = OnWindowMove
			};
		}

		public void DrawText(double x, double y, string text)
		{
			if (text == null) throw CreateArgumentNull(nameof(text));

			g.DrawText(x, y, text, TextFormat);
		}

		public void DrawPixel(double x, double y)
		{
			g.DrawPixel(x, y);
		}

		public void DrawPoint(double x, double y)
		{
			g.DrawPoint(x, y);
		}

		public void DrawLine(double startX, double startY, double endX, double endY)
		{
			g.DrawLine(startX, startY, endX, endY);
		}

		public void DrawCurve(double startX, double startY, double controlX, double controlY, double endX, double endY)
		{
			g.DrawCurve(startX, startY, controlX, controlY, endX, endY);
		}

		public void DrawBezier(double startX, double startY, double control1X, double control1Y, double control2X, double control2Y, double endX, double endY)
		{
			g.DrawBezier(startX, startY, control1X, control1Y, control2X, control2Y, endX, endY);
		}

		public void DrawArc(double x, double y, double horizontalRadius, double verticalRadius, double startRadians, double sweepRadians)
		{
			g.DrawArc(x, y, horizontalRadius, verticalRadius, startRadians, sweepRadians);
		}

		public void DrawSquare(double x, double y, double sideLength)
		{
			g.DrawSquare(x, y, sideLength);
		}

		public void FillSquare(double x, double y, double sideLength)
		{
			g.FillSquare(x, y, sideLength);
		}

		public void DrawRectangle(double x, double y, double width, double height)
		{
			g.DrawRectangle(x, y, width, height);
		}

		public void FillRectangle(double x, double y, double width, double height)
		{
			g.FillRectangle(x, y, width, height);
		}

		public void DrawCircle(double x, double y, double radius)
		{
			g.DrawCircle(x, y, radius);
		}

		public void FillCircle(double x, double y, double radius)
		{
			g.FillCircle(x, y, radius);
		}

		public void DrawEllipse(double x, double y, double horizontalRadius, double verticalRadius)
		{
			g.DrawEllipse(x, y, horizontalRadius, verticalRadius);
		}

		public void FillEllipse(double x, double y, double horizontalRadius, double verticalRadius)
		{
			g.FillEllipse(x, y, horizontalRadius, verticalRadius);
		}

		public void DrawPie(double x, double y, double radius, double startRadians, double sweepRadians)
		{
			g.DrawPie(x, y, radius, startRadians, sweepRadians);
		}

		public void DrawPie(double x, double y, double horizontalRadius, double verticalRadius, double startRadians, double sweepRadians)
		{
			g.DrawPie(x, y, horizontalRadius, verticalRadius, startRadians, sweepRadians);
		}

		public void FillPie(double x, double y, double radius, double startRadians, double sweepRadians)
		{
			g.FillPie(x, y, radius, startRadians, sweepRadians);
		}

		public void FillPie(double x, double y, double horizontalRadius, double verticalRadius, double startRadians, double sweepRadians)
		{
			g.FillPie(x, y, horizontalRadius, verticalRadius, startRadians, sweepRadians);
		}

		public void DrawTriangle(double x1, double y1, double x2, double y2, double x3, double y3)
		{
			g.DrawTriangle(x1, y1, x2, y2, x3, y3);
		}

		public void FillTriangle(double x1, double y1, double x2, double y2, double x3, double y3)
		{
			g.FillTriangle(x1, y1, x2, y2, x3, y3);
		}

		public void DrawPolygon(params (double, double)[] points)
		{
			if (points.Length < 2) throw new ArgumentException("There have to be at least two points to draw a polygon.");

			g.DrawPolygon(points);
		}

		public void FillPolygon(params (double, double)[] points)
		{
			if (points.Length < 2) throw new ArgumentException("There have to be at least two points to draw a polygon.");

			g.FillPolygon(points);
		}

		public void DrawImage(double x, double y, Image image)
		{
			if (image == null) throw CreateArgumentNull(nameof(image));

			g.DrawImage(x, y, image);
		}

		public void DrawImage(double x, double y, double width, double height, Image image)
		{
			if (width < 0) throw CreateArgumentNotNegative(nameof(width));
			if (height < 0) throw CreateArgumentNotNegative(nameof(height));
			if (image == null) throw CreateArgumentNull(nameof(image));

			g.DrawImage(x, y, width, height, image);
		}

		public Bitmap SaveCanvas()
		{
			return g.CopyAsImage();
		}

		public void Clear()
		{
			g.Clear();
		}

		public void Clear(Color color)
		{
			g.Clear(color);
		}

		public void Show()
		{
			form.Render(g);
		}

		public void Show(int waitMilliseconds)
		{
			form.Render(g, waitMilliseconds);
		}

		public void Dispose()
		{
			Dispose(true);
		}

		public void Dispose(bool exit)
		{
			g.Dispose();
			form.CloseAndDispose(exit);
		}

		private static NullReferenceException CreateArgumentNull(string argumentName)
		{
			return new NullReferenceException($"The parameter {argumentName} cannot be null.");
		}

		private static ArgumentOutOfRangeException CreateArgumentNotNegative(string argumentName)
		{
			return new ArgumentOutOfRangeException($"Argument {argumentName} cannot be negative.");
		}
	}
}
