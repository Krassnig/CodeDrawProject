using CodeDrawProject;
using System;
using System.Drawing;

namespace CodeDrawTest
{
	static class AnimationTest
	{
		public static void Main(string[] args)
		{
			// Always commit all function commmented out!

			//ClockTest();
			//SinCosTest();
			//GranularAngleTest();
			//ArcOriginTest();
			//TextAnimationTest();
			//AnimationTest2();
		}

		private static void ClockTest()
		{
			CodeDraw cd = new CodeDraw();

			for (double sec = -Math.Tau / 4; true; sec += Math.Tau / 60)
			{
				cd.Clear();
				cd.DrawLine(300, 300, Math.Cos(sec) * 100 + 300, Math.Sin(sec) * 100 + 300);

				double min = sec / 60 - Math.Tau / 4;
				cd.DrawLine(300, 300, Math.Cos(min) * 70 + 300, Math.Sin(min) * 70 + 300);

				for (double j = 0; j < Math.Tau; j += Math.Tau / 12)
				{
					cd.FillCircle(Math.Cos(j) * 100 + 300, Math.Sin(j) * 100 + 300, 4);
				}

				cd.Show(1000);
			}
		}

		private static void SinCosTest()
		{
			CodeDraw cd = new CodeDraw();
			int radius = 100;

			for (double i = 0; true; i += Math.Tau / 128)
			{
				cd.Clear();

				cd.Color = Color.Black;
				cd.DrawCircle(300, 300, radius);

				cd.Color = Color.Blue;
				int newx = 300 + (int)(radius * Math.Sin(-i));
				int newy = 300 + (int)(radius * Math.Cos(-i));
				cd.DrawLine(300, 300, newx, 300);
				cd.DrawLine(newx, 300, newx, newy);

				cd.Color = Color.Red;
				cd.DrawLine(300, 300, newx, newy);

				cd.Show(16);
			}
		}

		private static void GranularAngleTest()
		{
			CodeDraw cd = new CodeDraw(1000, 1000);

			double steps = Math.Tau / (1 << 14);

			for (double i = 0; i < Math.Tau / 4; i += steps)
			{
				cd.Clear();

				cd.FillPie(100, 100, 800, 800, Math.Tau / 4, i);
				cd.DrawArc(100, 100, 850, 850, Math.Tau / 4, i);

				cd.Show();
			}
		}

		private static void ArcOriginTest()
		{
			CodeDraw cd = new CodeDraw();

			double inc = Math.Tau / 16;

			for (double i = 0; i < Math.Tau; i += inc)
			{
				cd.FillPie(300, 300, 100, 100, i, inc);
				cd.DrawArc(300, 300, 150, 150, i, inc);

				cd.Show(200);
			}

			cd.Color = Color.Red;
			cd.FillPie(300, 300, 50, 50, -Math.Tau / 8, Math.Tau / 8);

			cd.Show();
		}

		private static void TextAnimationTest()
		{
			CodeDraw cd = new CodeDraw();

			int steps = 5;
			int end = 80;
			int offset = 100;
			int pause = 10;

			while (true)
			{
				for (int i = 0; i < end; i++)
				{
					cd.Clear();
					cd.DrawText(offset + i * steps, offset, "I'm animated!");
					cd.Show(pause);
				}

				for (int i = 0; i < end; i++)
				{
					cd.Clear();
					cd.DrawText(offset + steps * end - i * steps, offset, "I'm animated!");
					cd.Show(pause);
				}
			}
		}

		private static void AnimationTest2()
		{
			CodeDraw cd = new CodeDraw();

			for (int i = 0; i < 30; i++)
			{
				cd.Clear();

				cd.Color = Color.Black;
				cd.DrawPoint(99, 399);
				cd.DrawText(100, 400, "Hello World!");
				cd.FillRectangle(100 + i * 10, 100 + i, 100, 100);
				cd.Color = Color.Orange;
				cd.FillEllipse(20, 40, 20, 40);
				cd.Show(30);
			}
		}
	}
}
