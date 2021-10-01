using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CodeDrawProject
{
	internal class CodeDrawGraphics : IDisposable
	{
		public CodeDrawGraphics(int width, int height)
		{
			buffer = new Bitmap(width, height);
			g = Graphics.FromImage(buffer);
			Clear();
		}

		private Bitmap buffer;
		private Graphics g;
		private SolidBrush brush = new SolidBrush(Color.Black);
		private Pen pen = new Pen(Color.Black, 1);

		private Color color = Color.Black;
		private double lineWidth = 1;

		public Corner Corner { get; set; } = Corner.Sharp;
		public bool IsAntialiased { get; set; } = true;

		public int Width => buffer.Width;
		public int Height => buffer.Height;

		public Color Color
		{
			get => color;
			set { color = value; UpdateBrushes(); }
		}

		public double LineWidth
		{
			get => lineWidth;
			set { lineWidth = value; UpdateBrushes(); }
		}

		private DrawMode Mode
		{
			set
			{
				if (IsAntialiased)
				{
					g.SmoothingMode = value switch
					{
						DrawMode.Line => SmoothingMode.AntiAlias,
						DrawMode.Fill => SmoothingMode.HighSpeed,
						DrawMode.Image => SmoothingMode.AntiAlias,
						DrawMode.Text => SmoothingMode.AntiAlias,
						_ => throw new Exception("DrawMode could not be mapped to Smoothing mode. Invalid or unknown DrawMode.")
					};
				}
				else
				{
					g.SmoothingMode = SmoothingMode.HighSpeed;
				}
			}
		}

		private void UpdateBrushes()
		{
			brush.Dispose();
			brush = new SolidBrush(color);
			pen.Dispose();
			pen = new Pen(color, (float)lineWidth);
		}

		public void DrawText(double x, double y, string text, TextFormat textFormat)
		{
			Mode = DrawMode.Text;
			g.DrawString(text, textFormat.Font, brush, (float)x, (float)y);
		}

		public void DrawPixel(double x, double y)
		{
			buffer.SetPixel((int)x, (int)y, Color);
		}

		public void DrawPoint(double x, double y)
		{
			FillCircle(x, y, lineWidth);
		}

		public void DrawLine(double startX, double startY, double endX, double endY)
		{
			Mode = DrawMode.Line;
			g.DrawLine(pen, (float)startX, (float)startY, (float)endX, (float)endY);
		}

		public void DrawCurve(double startX, double startY, double controlX, double controlY, double endX, double endY)
		{
			DrawBezier(
				startX, startY,
				controlX * 2 / 3 + startX / 3, controlY * 2 / 3 + startY / 3,
				controlX * 2 / 3 + endX / 3, controlY * 2 / 3 + endY / 3,
				endX, endY
			);
		}

		public void DrawBezier(double startX, double startY, double control1X, double control1Y, double control2X, double control2Y, double endX, double endY)
		{
			Mode = DrawMode.Line;
			g.DrawBezier(pen, MapToPoint(startX, startY), MapToPoint(control1X, control1Y), MapToPoint(control2X, control2Y), MapToPoint(endX, endY));
		}

		public void DrawArc(double x, double y, double radius, double startRadians, double sweepRadians)
		{
			DrawArc(x, y, radius, radius, startRadians, sweepRadians);
		}

		public void DrawArc(double x, double y, double horizontalRadius, double verticalRadius, double startRadians, double sweepRadians)
		{
			Mode = DrawMode.Line;
			g.DrawArc(
				pen,
				(float)(x - horizontalRadius),
				(float)(y - verticalRadius),
				(float)(2 * horizontalRadius),
				(float)(2 * verticalRadius),
				TransformStart(startRadians),
				TransformSweep(sweepRadians)
			);
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
			Mode = DrawMode.Line;
			g.DrawRectangle(pen, (float)x, (float)y, (float)width, (float)height);
		}

		public void FillRectangle(double x, double y, double width, double height)
		{
			Mode = DrawMode.Fill;
			g.FillRectangle(brush, (float)x, (float)y, (float)width, (float)height);
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
			Mode = DrawMode.Line;
			g.DrawEllipse(
				pen,
				(float)(x - horizontalRadius),
				(float)(y - verticalRadius),
				(float)(2 * horizontalRadius),
				(float)(2 * verticalRadius)
			);
		}

		public void FillEllipse(double x, double y, double horizontalRadius, double verticalRadius)
		{
			Mode = DrawMode.Fill;
			g.FillEllipse(
				brush,
				(float)(x - horizontalRadius),
				(float)(y - verticalRadius),
				(float)(2 * horizontalRadius),
				(float)(2 * verticalRadius)
			);
		}

		public void DrawPie(double x, double y, double radius, double startRadians, double sweepRadians)
		{
			DrawPie(x, y, radius, radius, startRadians, sweepRadians);
		}

		public void DrawPie(double x, double y, double horizontalRadius, double verticalRadius, double startRadians, double sweepRadians)
		{
			Mode = DrawMode.Line;
			g.DrawPie(
				pen,
				(float)(x - horizontalRadius),
				(float)(y - verticalRadius),
				(float)(2 * horizontalRadius),
				(float)(2 * verticalRadius),
				TransformStart(startRadians),
				TransformSweep(sweepRadians)
			);
		}

		public void FillPie(double x, double y, double radius, double startRadians, double sweepRadians)
		{
			FillPie(x, y, radius, radius, startRadians, sweepRadians);
		}

		public void FillPie(double x, double y, double horizontalRadius, double verticalRadius, double startRadians, double sweepRadians)
		{
			Mode = DrawMode.Fill;
			g.FillPie(
				brush,
				(float)(x - horizontalRadius),
				(float)(y - verticalRadius),
				(float)(2 * horizontalRadius),
				(float)(2 * verticalRadius),
				TransformStart(startRadians),
				TransformSweep(sweepRadians)
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
			Mode = DrawMode.Line;
			g.DrawPolygon(pen, MapToPoints(points));
		}

		public void FillPolygon(params (double, double)[] points)
		{
			Mode = DrawMode.Fill;
			g.FillPolygon(brush, MapToPoints(points));
		}

		public void DrawImage(double x, double y, Image image)
		{
			Mode = DrawMode.Image;
			g.DrawImage(image, (float)x, (float)y);
		}

		public void DrawImage(double x, double y, double width, double height, Image image)
		{
			Mode = DrawMode.Image;
			g.DrawImage(image, (float)x, (float)y, (float)width, (float)height);
		}

		public void CopyTo(CodeDrawGraphics graphics)
		{
			CopyTo(graphics.g);
		}

		public void CopyTo(Graphics graphics)
		{
			graphics.DrawImage(buffer, 0, 0, Width, Height);
		}

		public Bitmap CopyAsImage()
		{
			Bitmap bitmap = new Bitmap(Width, Height);
			using Graphics graphics = Graphics.FromImage(bitmap);
			graphics.DrawImage(buffer, 0, 0);
			return bitmap;
		}

		public void Clear()
		{
			Clear(Color.White);
		}

		public void Clear(Color color)
		{
			Color tmpColor = Color;
			bool tmpAntiAliasing = IsAntialiased;

			IsAntialiased = false;
			Color = color;
			FillRectangle(0, 0, Width, Height);

			IsAntialiased = tmpAntiAliasing;
			Color = tmpColor;
		}

		public void Dispose()
		{
			buffer.Dispose();
			g.Dispose();
			brush.Dispose();
			pen.Dispose();
		}

		private static float TransformStart(double startRadians)
		{
			return (float)ToDegrees(startRadians - Math.PI / 2);
		}

		private static float TransformSweep(double sweepRadians)
		{
			return (float)ToDegrees(sweepRadians);
		}

		private static double ToDegrees(double Radians)
		{
			return Radians * (180 / Math.PI);
		}

		private static PointF[] MapToPoints(params double[] points)
		{
			PointF[] result = new PointF[points.Length / 2];

			for (int i = 0; i < points.Length / 2; i += 2)
			{
				result[i] = MapToPoint(points[i], points[i + 1]);
			}

			return result;
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

		private static PointF MapToPoint(double x, double y)
		{
			return new PointF((float)x, (float)y);
		}

		private static PointF MapToPoint((double, double) tuple)
		{
			return MapToPoint(tuple.Item1, tuple.Item2);
		}

		private enum DrawMode
		{
			Line,
			Fill,
			Image,
			Text
		}
	}
}
