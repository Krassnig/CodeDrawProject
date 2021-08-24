using CodeDrawNS;
using System;
using System.Drawing;
using System.Threading;

namespace CodeDrawTest
{
	public static class CodeDrawTest
	{
		private static void Main(string[] Args)
		{
			//CodeDrawEventTest.MainEvent(Args);
			//TriangleTest();
			//FramePositionTest();
			//LineSizeTest();
			//DisposeAndCloseTest();
			//TransparencyTest();
			//SmallWindowTest();
			//SinCosTest();
			//ArcAngleTest();
			//ArcAnimationTest();
			//DrawingTest();
			//FontTest();
			//ImageSaveTest();
			//ImageScaleTest();
			//ImageTest();
			//PolygonTest();
			//BezierTest();
			//ArcTest();
			//AnimationTest2();
			//TwoWindowTest();
			//CornerTest();
			//AnimationTest();
			ProofOfConcept();
			//AutoCloseTest();
		}

		private static void AutoCloseTest()
		{
			CodeDraw cd1 = new CodeDraw();
			CodeDraw cd2 = new CodeDraw();

			cd1.Dispose();
			cd2.Dispose(false);

			for (int i = 0; i < 10; i++)
			{
				Thread.Sleep(1000);
				Console.WriteLine("sleeping");
			}
		}

		private static void TriangleTest()
		{
			CodeDraw cd = new CodeDraw();

			cd.DrawTriangle(100, 100, 200, 300, 50, 220);
			cd.Show();
		}

		private static void FramePositionTest()
		{
			CodeDraw cd = new CodeDraw();
			cd.FramePositionX = 0;
			cd.FramePositionY = 0;

			for (int i = 0; i < 80; i++)
			{
				int pos = i * 10;

				cd.FramePositionX = pos;
				cd.FramePositionY = pos;

				cd.Clear();
				cd.DrawSquare(500 - pos, 500 - pos, 100);
				cd.Show(100);
			}
		}

		private static void LineSizeTest()
		{
			CodeDraw cd = new CodeDraw();
			cd.LineWidth = 5;

			cd.DrawSquare(20, 20, 100);
			cd.Show();
		}

		private static void DisposeAndCloseTest()
		{
			CodeDraw c1 = new CodeDraw();
			CodeDraw c2 = new CodeDraw();
			c1.Dispose();
		}

		private static void TransparencyTest()
		{
			CodeDraw cd = new CodeDraw();

			cd.Color = Color.Blue;
			cd.FillSquare(100, 100, 100);

			cd.Color = Color.FromArgb(77, Color.Red);
			cd.FillSquare(150, 150, 100);

			cd.Show();
		}

		private static void SmallWindowTest()
		{
			CodeDraw cd = new CodeDraw(150, 1);
			cd.Show();
		}

		private static void SinCosTest()
		{
			CodeDraw c = new CodeDraw(600, 600);
			int radius = 100;

			for (double i = 0; true; i += Math.PI / 64)
			{
				c.Clear();

				c.Color = Color.Black;
				c.DrawCircle(300, 300, radius);

				c.Color = Color.Blue;
				int newx = 300 + (int)(radius * Math.Sin(-i));
				int newy = 300 + (int)(radius * Math.Cos(-i));
				c.DrawLine(300, 300, newx, 300);
				c.DrawLine(newx, 300, newx, newy);

				c.Color = Color.Red;
				c.DrawLine(300, 300, newx, newy);

				c.Show(16);
			}
		}

		private static void ArcAngleTest()
		{
			CodeDraw c = new CodeDraw(1000, 1000);

			double tau = Math.PI * 2;
			double steps = tau / (1 << 14);

			for (double i = 0; i < Math.PI / 2; i += steps)
			{
				c.Clear();

				c.FillArc(100, 100, 800, 800, Math.PI / 2, i);
				c.DrawArc(100, 100, 850, 850, Math.PI / 2, i);

				c.Show();
			}

			c.Show();
		}

		private static void ArcAnimationTest()
		{
			CodeDraw c = new CodeDraw();

			double inc = Math.Tau / 16;

			for (double i = 0; i < Math.Tau; i += inc)
			{
				c.FillArc(300, 300, 100, 100, i, inc);
				c.DrawArc(300, 300, 150, 150, i, inc);

				c.Show(200);
			}

			c.Color = Color.Red;
			c.FillArc(300, 300, 50, 50, -Math.Tau / 8, Math.Tau / 8);

			c.Show();
		}

		private static void FontTest()
		{
			CodeDraw w = new CodeDraw();

			w.Font = new Font(new FontFamily("Arial"), 20, FontStyle.Bold);

			w.DrawText(200, 200, "MY BOLD TEXT!");

			w.Show();
		}

		private static void ImageSaveTest()
		{
			CodeDraw w = new CodeDraw();

			w.Color = Color.BlueViolet;

			w.DrawArc(200, 200, 50, 50, 0, Math.PI / 2);
			w.FillArc(200, 400, 50, 50, 0, Math.PI * 3 / 2);

			w.DrawArc(400, 200, 50, 50, 0, Math.PI / 2);
			w.FillArc(400, 400, 50, 50, 0, Math.PI * 3 / 2);

			w.Color = Color.Orange;
			w.DrawRectangle(150, 150, 100, 100);

			w.Color = Color.Red;
			w.FillCircle(200, 200, 10);
			w.AsImage().Save("C:\\Users\\Dewernh\\Desktop\\saved.png");

			w.Show();
		}

		private static void ImageScaleTest()
		{
			CodeDraw w = new CodeDraw();

			w.DrawImage(100, 100, 200, 200, Image.FromFile("C:\\Users\\Dewernh\\Desktop\\test.jpg"));

			w.Show();
		}

		private static void ImageTest()
		{
			CodeDraw w = new CodeDraw();

			w.DrawImage(10, 10, Image.FromFile("C:\\Users\\Dewernh\\Desktop\\test.jpg"));

			w.Show();
		}

		private static void PolygonTest()
		{
			CodeDraw w = new CodeDraw();
			w.FillPolygon((10, 40), (200, 200), (100, 30));
			w.Show();
		}

		private static void BezierTest()
		{
			CodeDraw w = new CodeDraw();
			w.DrawBezier((100, 100), (300, 200), (200, 300), (400, 400));
			w.Show();
		}

		private static void ArcTest()
		{
			CodeDraw w = new CodeDraw();

			w.Color = Color.BlueViolet;

			w.DrawArc(200, 200, 50, 50, 0, Math.PI / 2);
			w.FillArc(200, 400, 50, 50, 0, Math.PI * 3 / 2);

			w.DrawArc(400, 200, 50, 50, 0, Math.PI / 2);
			w.FillArc(400, 400, 50, 50, 0, Math.PI * 3 / 2);

			w.Color = Color.Orange;
			w.DrawRectangle(150, 150, 100, 100);

			w.Color = Color.Red;
			w.FillCircle(200, 200, 10);

			w.Show();
		}

		private static void AnimationTest2()
		{
			CodeDraw w = new CodeDraw();

			int steps = 5;
			int end = 80;
			int offset = 100;
			int pause = 10;

			while (true)
			{
				for (int i = 0; i < end; i++)
				{
					w.Clear();
					w.DrawText(offset + i * steps, offset, "I'm animated!");
					w.Show(pause);
				}

				for (int i = 0; i < end; i++)
				{
					w.Clear();
					w.DrawText(offset + steps * end - i * steps, offset, "I'm animated!");
					w.Show(pause);
				}
			}
		}

		private static void TwoWindowTest()
		{
			CodeDraw w1 = new CodeDraw();
			CodeDraw w2 = new CodeDraw();

			w1.DrawCircle(100, 100, 50);
			w2.DrawCircle(400, 200, 100);

			w1.Show();
			w2.Show();
		}

		private static void CornerTest()
		{
			CodeDraw draw = new CodeDraw();

			int size = 1;

			draw.Color = Color.Red;
			draw.FillRectangle(0, 0, size, size);
			draw.FillRectangle(0, draw.Height - size, size, size);
			draw.FillRectangle(draw.Width - size, 0, size, size);
			draw.FillRectangle(draw.Width - size, draw.Height - size, size, size);
			draw.Show();
		}

		private static void AnimationTest()
		{
			CodeDraw draw = new CodeDraw();

			for (int i = 0; i < 30; i++)
			{
				draw.Clear();

				draw.Color = Color.Black;
				draw.DrawPoint(99, 399);
				draw.DrawText(100, 400, "Hello World!");
				draw.FillRectangle(100 + i * 10, 100 + i, 100, 100);
				draw.Color = Color.Orange;
				draw.FillEllipse(20, 40, 20, 40);
				draw.Show(30);
			}
		}

		private static void ProofOfConcept()
		{
			CodeDraw l = new CodeDraw();

			l.Color = Color.Red;
			l.FillRectangle(20, 20, 100, 100);

			l.Title = "Hello World";

			l.Color = Color.Blue;
			l.FillCircle(50, 50, 50);

			l.Color = Color.LightBlue;
			l.LineWidth = 5;
			l.DrawRectangle(30, 30, 200, 200);

			l.Show();
		}
	}
}
