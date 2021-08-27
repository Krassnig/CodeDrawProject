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

			Title = "CodeDraw";
			Color = Color.Black;
			LineWidth = 1;

			Clear();
			Show();
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

		private SolidBrush brush = new SolidBrush(Color.Black);
		private Pen pen = new Pen(Color.Black, 1);
		private void UpdateBrushes()
		{
			brush.Dispose();
			brush = new SolidBrush(color);
			pen.Dispose();
			pen = new Pen(color, (float)lineWidth);
		}

		public event EventHandler<MouseEventArgs> MouseClick
		{
			add => form.MouseClick += MapEvent(value);
			remove => form.MouseClick -= MapEvent(value);
		}
		public event EventHandler<MouseEventArgs> MouseMove
		{
			add => form.MouseMove += MapEvent(value);
			remove => form.MouseMove -= MapEvent(value);
		}
		public event EventHandler<MouseEventArgs> MouseDown
		{
			add => form.MouseDown += MapEvent(value);
			remove => form.MouseDown -= MapEvent(value);
		}
		public event EventHandler<MouseEventArgs> MouseUp
		{
			add => form.MouseUp += MapEvent(value);
			remove => form.MouseUp -= MapEvent(value);
		}
		public event EventHandler<EventArgs> MouseEnter
		{
			add => form.MouseEnter += MapEvent(value);
			remove => form.MouseEnter -= MapEvent(value);
		}
		public event EventHandler<EventArgs> MouseLeave
		{
			add => form.MouseLeave += MapEvent(value);
			remove => form.MouseLeave -= MapEvent(value);
		}
		public event EventHandler<MouseEventArgs> MouseWheel
		{
			add => form.MouseWheel += MapEvent(value);
			remove => form.MouseWheel -= MapEvent(value);
		}

		public event EventHandler<KeyEventArgs> KeyPress
		{
			add => form.KeyPress += MapEvent(value);
			remove => form.KeyPress -= MapEvent(value);
		}
		public event EventHandler<KeyEventArgs> KeyUp
		{
			add => form.KeyUp += MapEvent(value);
			remove => form.KeyUp -= MapEvent(value);
		}
		public event EventHandler<KeyEventArgs> KeyDown
		{
			add => form.KeyDown += MapEvent(value);
			remove => form.KeyDown += MapEvent(value);
		}

		public event EventHandler<EventArgs> FrameMove
		{
			add => form.FrameMove += MapEvent(value);
			remove => form.FrameMove -= MapEvent(value);
		}

		private EventHandler MapEvent(EventHandler<EventArgs> anyEvent)
		{
			return (sender, args) => anyEvent(this, args);
		}
		private MouseEventHandler MapEvent(EventHandler<MouseEventArgs> mouseEvent)
		{
			return (sender, args) => mouseEvent(this, args);
		}
		private KeyEventHandler MapEvent(EventHandler<KeyEventArgs> keyEvent)
		{
			return (sender, args) => keyEvent(this, args);
		}

		public void DrawPoint(int x, int y)
		{
			FillSquare(x, y, 1);
		}

		public void DrawLine(int startx, int starty, int endx, int endy)
		{
			G.DrawLine(pen, startx, starty, endx, endy);
		}

		public void DrawBezier((int, int) start, (int, int) control1, (int, int) control2, (int, int) end)
		{
			G.DrawBezier(pen, MapToPoint(start), MapToPoint(control1), MapToPoint(control2), MapToPoint(end));
		}

		public void DrawSquare(int x, int y, int sideLength)
		{
			DrawRectangle(x, y, sideLength, sideLength);
		}

		public void FillSquare(int x, int y, int sideLength)
		{
			FillRectangle(x, y, sideLength, sideLength);
		}

		public void DrawRectangle(int x, int y, int width, int height)
		{
			G.DrawRectangle(pen, x, y, width, height);
		}

		public void FillRectangle(int x, int y, int width, int height)
		{
			G.FillRectangle(brush, x, y, width, height);
		}

		public void DrawCircle(int x, int y, int radius)
		{
			DrawEllipse(x, y, radius, radius);
		}

		public void FillCircle(int x, int y, int radius)
		{
			FillEllipse(x, y, radius, radius);
		}

		public void DrawEllipse(int x, int y, int horizontalRadius, int verticalRadius)
		{
			G.DrawEllipse(pen, x - horizontalRadius, y - verticalRadius, 2 * horizontalRadius, 2 * verticalRadius);
		}

		public void FillEllipse(int x, int y, int horizontalRadius, int verticalRadius)
		{
			G.FillEllipse(brush, x - horizontalRadius, y - verticalRadius, 2 * horizontalRadius, 2 * verticalRadius);
		}

		public void DrawArc(int x, int y, int horizontalRadius, int verticalRadius, double startRadians, double sweepRadians)
		{
			G.DrawArc(pen, new Rectangle(x - horizontalRadius, y - verticalRadius, 2 * horizontalRadius, 2 * verticalRadius), transformStart(startRadians), transformSweep(sweepRadians));
		}

		public void FillArc(int x, int y, int horizontalRadius, int verticalRadius, double startRadians, double sweepRadians)
		{
			G.FillPie(brush, new Rectangle(x - horizontalRadius, y - verticalRadius, 2 * horizontalRadius, 2 * verticalRadius), transformStart(startRadians), transformSweep(sweepRadians));
		}

		public void DrawTriangle(int x1, int y1, int x2, int y2, int x3, int y3)
		{
			DrawPolygon((x1, y1), (x2, y2), (x3, y3));
		}

		public void FillTriangle(int x1, int y1, int x2, int y2, int x3, int y3)
		{
			FillPolygon((x1, y1), (x2, y2), (x3, y3));
		}

		public void DrawPolygon(params (int, int)[] points)
		{
			if (points.Length < 2) throw new ArgumentException("There have to be at least two points to draw a polygon.");

			G.DrawPolygon(pen, MapToPoints(points));
		}

		public void FillPolygon(params (int, int)[] points)
		{
			if (points.Length < 2) throw new ArgumentException("There have to be at least two points to draw a polygon.");

			G.FillPolygon(brush, MapToPoints(points));
		}

		public void DrawImage(int x, int y, Image image)
		{
			if (image == null) throw CreateArgumentNull(nameof(image));

			G.DrawImage(image, x, y);
		}

		public void DrawImage(int x, int y, int width, int height, Image image)
		{
			if (width < 0) throw CreateArgumentNotNegative(nameof(width));
			if (height < 0) throw CreateArgumentNotNegative(nameof(height));
			if (image == null) throw CreateArgumentNull(nameof(image));

			G.DrawImage(image, x, y, width, height);
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
			Color = color;
			FillRectangle(0, 0, Width, Height);
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

		private static Point[] MapToPoints((int, int)[] tuples)
		{
			Point[] result = new Point[tuples.Length];

			for (int i = 0; i < result.Length; i++)
			{
				result[i] = MapToPoint(tuples[i]);
			}

			return result;
		}

		private static Point MapToPoint((int, int) tuple)
		{
			return new Point(tuple.Item1, tuple.Item2);
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
