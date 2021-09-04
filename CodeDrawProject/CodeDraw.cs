using System;
using System.Drawing;
using System.Drawing.Drawing2D;
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

			Width = canvasWidth;
			Height = canvasHeight;

			form = CanvasForm.Create(new Size(Width, Height));
			buffer = form.CreateBufferedGraphics();
			UpdateBrushes();
			BindEvents();

			Title = "CodeDraw";
			Color = Color.Black;
			LineWidth = 1;
			IsAntialiased = true;

			Clear();
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
		private BufferedGraphics buffer;
		private Graphics G => buffer.Graphics;

		public int Width { get; private set; }
		public int Height { get; private set; }

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

		public Font Font { get; set; } = new Font(new FontFamily("Arial"), 12);

		private Color color = Color.Black;
		public Color Color
		{
			get => color;
			set { color = value; UpdateBrushes(); }
		}

		private double lineWidth = 1;
		public double LineWidth
		{
			get => lineWidth;
			set
			{
				if (value < 1) throw new ArgumentException($"{nameof(LineWidth)} must be greater than 1.", nameof(LineWidth));
				lineWidth = value;
				UpdateBrushes();
			}
		}

		public bool IsAntialiased
		{
			get => G.SmoothingMode == SmoothingMode.AntiAlias;
			set => G.SmoothingMode = value ? SmoothingMode.AntiAlias : SmoothingMode.HighSpeed;
		}

		private SolidBrush brush = new SolidBrush(Color.Black);
		private Pen pen = new Pen(Color.Black, 1);
		private void UpdateBrushes()
		{
			brush.Dispose();
			brush = new SolidBrush(color);
			pen.Dispose();
			pen = new Pen(color, (float)lineWidth);
		}

		private void BindEvents()
		{
			mouseClickEvent = new Event<MouseEventArgs>(this);
			form.MouseClick += mouseClickEvent.Invoke;
			mouseMoveEvent = new Event<MouseEventArgs>(this);
			form.MouseMove += mouseMoveEvent.Invoke;
			mouseDownEvent = new Event<MouseEventArgs>(this);
			form.MouseDown += mouseDownEvent.Invoke;
			mouseUpEvent = new Event<MouseEventArgs>(this);
			form.MouseUp += mouseUpEvent.Invoke;
			mouseEnterEvent = new Event<EventArgs>(this);
			form.MouseEnter += mouseEnterEvent.Invoke;
			mouseLeaveEvent = new Event<EventArgs>(this);
			form.MouseLeave += mouseLeaveEvent.Invoke;
			mouseWheelEvent = new Event<MouseEventArgs>(this);
			form.MouseWheel += mouseWheelEvent.Invoke;

			keyPressEvent = new Event<KeyEventArgs>(this);
			form.KeyPress += keyPressEvent.Invoke;
			keyUpEvent = new Event<KeyEventArgs>(this);
			form.KeyUp += keyUpEvent.Invoke;
			keyDownEvent = new Event<KeyEventArgs>(this);
			form.KeyDown += keyDownEvent.Invoke;

			frameMoveEvent = new Event<EventArgs>(this);
			form.FrameMove += frameMoveEvent.Invoke;
		}

		private Event<MouseEventArgs> mouseClickEvent;
		public event EventHandler<MouseEventArgs> MouseClick
		{
			add => mouseClickEvent += value;
			remove => mouseClickEvent -= value;
		}
		private Event<MouseEventArgs> mouseMoveEvent;
		public event EventHandler<MouseEventArgs> MouseMove
		{
			add => mouseMoveEvent += value;
			remove => mouseMoveEvent -= value;
		}
		private Event<MouseEventArgs> mouseDownEvent;
		public event EventHandler<MouseEventArgs> MouseDown
		{
			add => mouseDownEvent += value;
			remove => mouseDownEvent -= value;
		}
		private Event<MouseEventArgs> mouseUpEvent;
		public event EventHandler<MouseEventArgs> MouseUp
		{
			add => mouseUpEvent += value;
			remove => mouseUpEvent -= value;
		}
		private Event<EventArgs> mouseEnterEvent;
		public event EventHandler<EventArgs> MouseEnter
		{
			add => mouseEnterEvent += value;
			remove => mouseEnterEvent -= value;
		}
		private Event<EventArgs> mouseLeaveEvent;
		public event EventHandler<EventArgs> MouseLeave
		{
			add => mouseLeaveEvent += value;
			remove => mouseLeaveEvent -= value;
		}
		private Event<MouseEventArgs> mouseWheelEvent;
		public event EventHandler<MouseEventArgs> MouseWheel
		{
			add => mouseWheelEvent += value;
			remove => mouseWheelEvent -= value;
		}

		private Event<KeyEventArgs> keyPressEvent;
		public event EventHandler<KeyEventArgs> KeyPress
		{
			add => keyPressEvent += value;
			remove => keyPressEvent -= value;
		}
		private Event<KeyEventArgs> keyUpEvent;
		public event EventHandler<KeyEventArgs> KeyUp
		{
			add => keyUpEvent += value;
			remove => keyUpEvent -= value;
		}
		private Event<KeyEventArgs> keyDownEvent;
		public event EventHandler<KeyEventArgs> KeyDown
		{
			add => keyDownEvent += value;
			remove => keyDownEvent -= value;
		}

		private Event<EventArgs> frameMoveEvent;
		public event EventHandler<EventArgs> FrameMove
		{
			add => frameMoveEvent += value;
			remove => frameMoveEvent -= value;
		}

		public void DrawText(double x, double y, string text)
		{
			if (text == null) throw CreateArgumentNull(nameof(text));

			G.DrawString(text, Font, brush, (float)x, (float)y);
		}

		public void DrawPixel(double x, double y)
		{
			FillSquare(x, y, 1);
		}

		public void DrawPoint(double x, double y)
		{
			FillCircle(x, y, lineWidth);
		}

		public void DrawLine(double startx, double starty, double endx, double endy)
		{
			G.DrawLine(pen, (float)startx, (float)starty, (float)endx, (float)endy);
		}

		public void DrawBezier((double, double) start, (double, double) control1, (double, double) control2, (double, double) end)
		{
			G.DrawBezier(pen, MapToPoint(start), MapToPoint(control1), MapToPoint(control2), MapToPoint(end));
		}

		public void DrawSquare(double x, double y, double sideLength)
		{
			DrawRectangle(x, y, sideLength, sideLength);
		}

		public void FillSquare(double x, double y, double sideLength)
		{
			FillRectangle(x, y, sideLength, sideLength);
		}

		public void DrawRectangle(double x, double y, double width, double height)
		{
			G.DrawRectangle(pen, (float)x, (float)y, (float)width, (float)height);
		}

		public void FillRectangle(double x, double y, double width, double height)
		{
			G.FillRectangle(brush, (float)x, (float)y, (float)width, (float)height);
		}

		public void DrawCircle(double x, double y, double radius)
		{
			DrawEllipse(x, y, radius, radius);
		}

		public void FillCircle(double x, double y, double radius)
		{
			FillEllipse(x, y, radius, radius);
		}

		public void DrawEllipse(double x, double y, double horizontalRadius, double verticalRadius)
		{
			G.DrawEllipse(
				pen,
				(float)(x - horizontalRadius),
				(float)(y - verticalRadius),
				(float)(2 * horizontalRadius),
				(float)(2 * verticalRadius)
			);
		}

		public void FillEllipse(double x, double y, double horizontalRadius, double verticalRadius)
		{
			G.FillEllipse(
				brush,
				(float)(x - horizontalRadius),
				(float)(y - verticalRadius),
				(float)(2 * horizontalRadius),
				(float)(2 * verticalRadius)
			);
		}

		public void DrawArc(double x, double y, double horizontalRadius, double verticalRadius, double startRadians, double sweepRadians)
		{
			G.DrawArc(
				pen,
				(float)(x - horizontalRadius),
				(float)(y - verticalRadius),
				(float)(2 * horizontalRadius),
				(float)(2 * verticalRadius),
				transformStart(startRadians),
				transformSweep(sweepRadians)
			);
		}

		public void FillArc(double x, double y, double horizontalRadius, double verticalRadius, double startRadians, double sweepRadians)
		{
			G.FillPie(
				brush,
				(float)(x - horizontalRadius),
				(float)(y - verticalRadius),
				(float)(2 * horizontalRadius),
				(float)(2 * verticalRadius),
				transformStart(startRadians),
				transformSweep(sweepRadians)
			);
		}

		public void DrawTriangle(double x1, double y1, double x2, double y2, double x3, double y3)
		{
			DrawPolygon((x1, y1), (x2, y2), (x3, y3));
		}

		public void FillTriangle(double x1, double y1, double x2, double y2, double x3, double y3)
		{
			FillPolygon((x1, y1), (x2, y2), (x3, y3));
		}

		public void DrawPolygon(params (double, double)[] points)
		{
			if (points.Length < 2) throw new ArgumentException("There have to be at least two points to draw a polygon.");

			G.DrawPolygon(pen, MapToPoints(points));
		}

		public void FillPolygon(params (double, double)[] points)
		{
			if (points.Length < 2) throw new ArgumentException("There have to be at least two points to draw a polygon.");

			G.FillPolygon(brush, MapToPoints(points));
		}

		public void DrawImage(double x, double y, Image image)
		{
			if (image == null) throw CreateArgumentNull(nameof(image));

			G.DrawImage(image, (float)x, (float)y);
		}

		public void DrawImage(double x, double y, double width, double height, Image image)
		{
			if (width < 0) throw CreateArgumentNotNegative(nameof(width));
			if (height < 0) throw CreateArgumentNotNegative(nameof(height));
			if (image == null) throw CreateArgumentNull(nameof(image));

			G.DrawImage(image, (float)x, (float)y, (float)width, (float)height);
		}

		public Image AsImage()
		{
			Bitmap bitmap = new Bitmap(Width, Height);
			using Graphics graphics = Graphics.FromImage(bitmap);
			buffer.Render(graphics);
			return bitmap;
		}

		public void Transform(Matrix matrix)
		{
			G.MultiplyTransform(matrix);
		}

		public void Clear()
		{
			Clear(Color.White);
		}

		public void Clear(Color color)
		{
			Color c = Color;
			bool aa = IsAntialiased;

			IsAntialiased = false;
			Color = color;
			FillRectangle(0, 0, Width, Height);

			IsAntialiased = aa;
			Color = c;
		}

		public void Show()
		{
			form.Render(buffer);
		}

		public void Show(int waitMilliseconds)
		{
			form.Render(buffer, waitMilliseconds);
		}

		public void Dispose()
		{
			Dispose(true);
		}

		public void Dispose(bool exit)
		{
			brush.Dispose();
			pen.Dispose();
			Font.Dispose();
			form.CloseAndDispose(exit);
		}

		private static float transformStart(double startRadians)
		{
			return (float)ToDegrees(startRadians - Math.PI / 2);
		}

		private static float transformSweep(double sweepRadians)
		{
			return (float)ToDegrees(sweepRadians);
		}

		private static double ToDegrees(double Radians)
		{
			return Radians * (180 / Math.PI);
		}

		private static PointF[] MapToPoints((double, double)[] tuples)
		{
			PointF[] result = new PointF[tuples.Length];

			for (int i = 0; i < result.Length; i++)
			{
				result[i] = MapToPoint(tuples[i]);
			}

			return result;
		}

		private static PointF MapToPoint((double, double) tuple)
		{
			return new PointF((float)tuple.Item1, (float)tuple.Item2);
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
