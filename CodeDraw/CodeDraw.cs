using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace CodeDrawNS
{
	/// <summary>
	/// CodeDraw is an easy to use drawing library.
	/// <para>
	/// How to use:
	/// </para>
	/// <example>
	/// <code>
	/// // creates a canvas of size 600x600 pixel<para />
	/// CodeDraw cd = new CodeDraw();<para />
	/// 
	/// // All following drawn objects will be red, until the color is set to a different color.<para />
	/// cd.Color = Color.Red;<para />
	/// 
	/// // draws a red circle at the center of the canvas with a radius of 50 pixel. The circle is not yet displayed!<para />
	/// cd.DrawCircle(300, 300, 50);<para />
	/// 
	/// // Must be called to display everything that has been drawn until now!<para />
	/// cd.Show();<para />
	/// </code>
	/// </example>
	/// There are a few key ideas described by certain keywords used in this library:
	/// 
	/// <para>
	/// canvas - Is the rectangle on the screen that is used for drawing. It's origin
	/// point (0, 0) is at the top left. Everything is drawn front the top left to the bottom right.
	/// Once the size is set via the constructor the size of the canvas remains fixed.
	/// </para>
	/// <para>
	/// frame - Is the frame surrounding the canvas. It is larger than the size given
	/// to the constructor of CodeDraw. It contains the closing and minimize button.
	/// </para>
	/// <para>
	/// Fun Fact: You can copy the currently displayed canvas to your clipboard by pressing Ctrl + C
	/// </para>
	/// </summary>
	public class CodeDraw : IDisposable
	{
		//private static void Main(string[] args) { }

		/// <summary>
		/// Creates a canvas with size 600x600 pixel
		/// </summary>
		public CodeDraw() : this(600, 600) { }

		/// <summary>
		/// Creates a canvas with the specified size. The frame surrounding the canvas will be slightly bigger.
		/// </summary>
		/// <param name="canvasWidth">must be at least 150 pixel</param>
		/// <param name="canvasHeight">must be at least 1 pixel</param>
		public CodeDraw(int canvasWidth, int canvasHeight)
		{
			if (canvasWidth < 150) throw new ArgumentOutOfRangeException("The width of the canvas has to be at least 150px.");
			if (canvasHeight < 1) throw new ArgumentOutOfRangeException("The height of the canvas has to be positive");

			Width = canvasWidth;
			Height = canvasHeight;

			form = CanvasForm.Create(new Size(Width, Height));
			form.Title = "CodeDraw";
			buffer = form.CreateBufferedGraphics();

			UpdateBrushes();
			Reset();
			Show();
		}

		private CanvasForm form;
		private BufferedGraphics buffer;
		private Graphics G => buffer.Graphics;

		/// <summary>
		/// The width of the canvas
		/// </summary>
		public int Width { get; private set; }
		/// <summary>
		/// The height of the canvas
		/// </summary>
		public int Height { get; private set; }

		public int FramePositionX
		{
			get => form.DesktopLocation.X;
			set => form.DesktopLocation = new Point(value, form.DesktopLocation.Y);
		}

		public int FramePositionY
		{
			get => form.DesktopLocation.Y;
			set => form.DesktopLocation = new Point(form.DesktopLocation.X, value);
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

		private double linesize = 1;
		public double LineSize
		{
			get => linesize;
			set
			{
				if (value < 1) throw new ArgumentException($"{nameof(LineSize)} must be greater than 1.", nameof(LineSize));
				linesize = value;
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
			pen = new Pen(color, (float)linesize);
		}

		public delegate void EventHandler<T>(CodeDraw litedraw, T args);

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
			add => form.WindowMove += MapEvent(value);
			remove => form.WindowMove -= MapEvent(value);
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

		/// <summary>
		/// Draws text to the right and below the xy-coordinate. The text will be left aligned.
		/// </summary>
		public void DrawText(int x, int y, string text)
		{
			if (text == null) throw CreateArgumentNull(nameof(text));

			G.DrawString(text, Font, brush, x, y);
		}

		public void DrawText(int x, int y, string text, StringFormat format)
		{
			if (text == null) throw CreateArgumentNull(nameof(text));
			if (format == null) throw CreateArgumentNull(nameof(format));

			G.DrawString(text, Font, brush, new PointF(x, y), format);
		}

		public void DrawPoint(int x, int y)
		{
			FillSquare(x, y, 1);
		}

		public void DrawLine(int startx, int starty, int endx, int endy)
		{
			G.DrawLine(pen, startx, starty, endx, endy);
		}

		/// <summary>
		/// Draws a cubic bezier curve. See: <see href="https://en.wikipedia.org/wiki/B%C3%A9zier_curve">Wikipedia Bezier Curve</see>
		/// </summary>
		public void DrawBezier((int, int) start, (int, int) control1, (int, int) control2, (int, int) end)
		{
			G.DrawBezier(pen, MapToPoint(start), MapToPoint(control1), MapToPoint(control2), MapToPoint(end));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x">The top left corner of the square</param>
		/// <param name="y">The top left corner of the square</param>
		/// <param name="sideLength"></param>
		public void DrawSquare(int x, int y, int sideLength)
		{
			DrawRectangle(x, y, sideLength, sideLength);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x">The top left corner of the square</param>
		/// <param name="y">The top left corner of the square</param>
		/// <param name="sideLength"></param>
		public void FillSquare(int x, int y, int sideLength)
		{
			FillRectangle(x, y, sideLength, sideLength);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x">The top left corner of the rectangle</param>
		/// <param name="y">The top left corner of the rectangle</param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void DrawRectangle(int x, int y, int width, int height)
		{
			G.DrawRectangle(pen, x, y, width, height);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x">The top left corner of the rectangle</param>
		/// <param name="y">The top left corner of the rectangle</param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void FillRectangle(int x, int y, int width, int height)
		{
			G.FillRectangle(brush, x, y, width, height);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x">The center of the circle</param>
		/// <param name="y">The center of the circle</param>
		/// <param name="radius"></param>
		public void DrawCircle(int x, int y, int radius)
		{
			DrawEllipse(x, y, radius, radius);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x">The center of the circle</param>
		/// <param name="y">The center of the circle</param>
		/// <param name="radius"></param>
		public void FillCircle(int x, int y, int radius)
		{
			FillEllipse(x, y, radius, radius);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x">The center of the ellipse</param>
		/// <param name="y">The center of the ellipse</param>
		/// <param name="horizontalRadius"></param>
		/// <param name="verticalRadius"></param>
		public void DrawEllipse(int x, int y, int horizontalRadius, int verticalRadius)
		{
			G.DrawEllipse(pen, x - horizontalRadius, y - verticalRadius, 2 * horizontalRadius, 2 * verticalRadius);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x">The center of the ellipse</param>
		/// <param name="y">The center of the ellipse</param>
		/// <param name="horizontalRadius"></param>
		/// <param name="verticalRadius"></param>
		public void FillEllipse(int x, int y, int horizontalRadius, int verticalRadius)
		{
			G.FillEllipse(brush, x - horizontalRadius, y - verticalRadius, 2 * horizontalRadius, 2 * verticalRadius);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x">The center of the arc</param>
		/// <param name="y">The center of the arc</param>
		/// <param name="horizontalRadius"></param>
		/// <param name="verticalRadius"></param>
		/// <param name="startRadians">The starting angle. A 0 radians angle would be interpreted as starting at 12 o'clock going clock-wise.</param>
		/// <param name="sweepRadians">The length of the arc in radians from the start angle in a clockwise direction.</param>
		public void DrawArc(int x, int y, int horizontalRadius, int verticalRadius, double startRadians, double sweepRadians)
		{
			G.DrawArc(pen, new Rectangle(x - horizontalRadius, y - verticalRadius, 2 * horizontalRadius, 2 * verticalRadius), transformStart(startRadians), transformSweep(sweepRadians));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x">The center of the arc</param>
		/// <param name="y">The center of the arc</param>
		/// <param name="horizontalRadius"></param>
		/// <param name="verticalRadius"></param>
		/// <param name="startRadians">The starting angle. A 0 radians angle would be interpreted as starting at 12 o'clock going clock-wise.</param>
		/// <param name="sweepRadians">The length of the arc in radians from the start angle in a clockwise direction.</param>
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

		/// <summary>
		/// The width and height of the image will be used to draw the image.
		/// Example:
		/// <code>
		/// CodeDraw cd = new CodeDraw();
		/// cd.DrawImage(100, 100, Image.FromFile("C:\\pathToDirectory\\filename.png"));
		/// cd.Show();
		/// </code>
		/// </summary>
		/// <param name="x">The position of the top left corner of the image</param>
		/// <param name="y">The position of the top left corner of the image</param>
		/// <param name="image"></param>
		public void DrawImage(int x, int y, Image image)
		{
			if (image == null) throw CreateArgumentNull(nameof(image));

			G.DrawImage(image, x, y);
		}

		/// <summary>
		/// The width and height of the image will be used to draw the image.
		/// Example:
		/// <code>
		/// CodeDraw cd = new CodeDraw();
		/// cd.DrawImage(100, 100, 200, 200 Image.FromFile("C:\\pathToDirectory\\filename.png"));
		/// cd.Show();
		/// </code>
		/// The size of the image will be 100x100 pixel.
		/// </summary>
		/// <param name="x">The position of the top left corner of the image</param>
		/// <param name="y">The position of the top left corner of the image</param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="image"></param>
		public void DrawImage(int x, int y, int width, int height, Image image)
		{
			if (width < 0) throw CreateArgumentNotNegative(nameof(width));
			if (height < 0) throw CreateArgumentNotNegative(nameof(height));
			if (image == null) throw CreateArgumentNull(nameof(image));

			G.DrawImage(image, x, y, width, height);
		}

		/// <summary>
		/// Creates a copy of the current buffer (not the displayed image) and returns it as an image. An Image can be saved as a file with this code:
		/// <code>
		/// CodeDraw cd = new CodeDraw();
		/// /* some drawing occurs here */
		/// cd.AsImage().Save("C:\\pathToDirectory\\filename.png");
		/// </code>
		/// </summary>
		/// <returns></returns>
		public Image AsImage()
		{
			Bitmap bitmap = new Bitmap(Width, Height);
			using Graphics graphics = Graphics.FromImage(bitmap);
			buffer.Render(graphics);
			return bitmap;
		}

		/// <summary>
		/// Multiplies the world transformation of the matrix with the canvas.
		/// </summary>
		/// <param name="matrix">4x4 System.Drawing.Drawing2D.Matrix that multiplies the world transformation.</param>
		public void Transform(Matrix matrix)
		{
			G.MultiplyTransform(matrix);
		}

		/// <summary>
		/// Colors the whole canvas in white.
		/// </summary>
		public void Reset()
		{
			Reset(Color.White);
		}

		/// <summary>
		/// Colors the whole canvas in the color given as a parameter.
		/// </summary>
		/// <param name="color"></param>
		public void Reset(Color color)
		{
			Color c = Color;
			Color = color;
			FillRectangle(0, 0, Width, Height);
			Color = c;
		}

		/// <summary>
		/// Displays the drawn graphics on the canvas.
		/// </summary>
		public void Show()
		{
			form.Render(buffer);
		}

		/// <summary>
		/// Displays the drawn graphics on the canvas and then waits for the given amount of milliseconds.
		/// The copying of the buffer to the screen also takes a bit of time so the wait time might be
		/// larger than the given amount of milliseconds.
		/// How many milliseconds the program must pause in order to display a certain amount of frames per second:
		/// 30 fps = 33ms,
		/// 60 fps = 16ms,
		/// 120 fps = 8ms
		/// </summary>
		/// <param name="waitMilliseconds">Time it takes this function to return.</param>
		public void Show(int waitMilliseconds)
		{
			form.Render(buffer, waitMilliseconds);
		}

		/// <summary>
		/// Closes the JFrame and disposes all created resources associated with this CodeDraw instance.
		/// </summary>
		public void Dispose()
		{
			brush.Dispose();
			pen.Dispose();
			Font.Dispose();
			form.CloseAndDispose();
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
