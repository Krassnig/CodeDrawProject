using CodeDrawProject;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;

namespace CodeDrawTest
{
	static class DrawTest
	{
		public static void Main(string[] args)
		{
			// Always commit all function commmented out!

			//AutoCloseTest();
			//FramePositionTest();
			//DisposeCloseTest();
			//SmallWindowTest();
			//ImageSaveTest();
			//ImageScaleTest();
			//ImageTest();
			//TwoWindowTest();
			//CornerTest();
			//ProofOfConcept();
		}

		private static void AutoCloseTest()
		{
			CodeDraw cd1 = new CodeDraw();
			CodeDraw cd2 = new CodeDraw();

			cd1.Dispose();
			cd2.Dispose(false);

			Thread.Sleep(5000);
		}

		private static void FramePositionTest()
		{
			CodeDraw cd = new CodeDraw();

			cd.FramePositionX = 0;
			cd.FramePositionY = 0;

			for (int i = 0; i < 60; i++)
			{
				int pos = i * 10;

				cd.FramePositionX = pos;
				cd.FramePositionY = pos;

				cd.Clear();
				cd.DrawSquare(500 - pos, 500 - pos, 100);
				cd.Show(100);
			}
		}

		private static void DisposeCloseTest()
		{
			CodeDraw cd1 = new CodeDraw();
			cd1.DrawText(300, 300, "I should close.");
			cd1.Show();

			CodeDraw cd2 = new CodeDraw();
			cd2.DrawText(300, 300, "I should stay open.");
			cd2.Show();

			cd1.Dispose();
		}

		private static void SmallWindowTest()
		{
			CodeDraw cd = new CodeDraw(150, 1);

			for (int i = 0; i < 150; i++)
			{
				cd.DrawPixel(i, 0);
				cd.Show(50);
			}
		}

		private static void ImageSaveTest()
		{
			CodeDraw cd = new CodeDraw();

			cd.DrawText(100, 100, "There should be a out.png in your debug folder.");
			cd.DrawText(0, 120, GetExecutingFolder() ?? "Cannot determine folder");
			cd.Color = Color.BlueViolet;

			cd.DrawArc(200, 200, 50, 50, 0, Math.PI / 2);
			cd.FillArc(200, 400, 50, 50, 0, Math.PI * 3 / 2);

			cd.DrawArc(400, 200, 50, 50, 0, Math.PI / 2);
			cd.FillArc(400, 400, 50, 50, 0, Math.PI * 3 / 2);

			cd.Color = Color.Orange;
			cd.DrawRectangle(150, 150, 100, 100);

			cd.Color = Color.Red;
			cd.FillCircle(200, 200, 10);

			cd.AsImage().Save("saved.png");
			
			cd.Show();
		}

		private static string? GetExecutingFolder()
		{
			return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		}

		private static void ImageScaleTest()
		{
			CodeDraw w = new CodeDraw();

			w.DrawImage(100, 100, 200, 200, Properties.Resources.CuteChick);

			w.Show();
		}

		private static void ImageTest()
		{
			CodeDraw w = new CodeDraw(820, 620);

			w.DrawImage(10, 10, Properties.Resources.CuteChick);

			w.Show();
		}

		private static void TwoWindowTest()
		{
			CodeDraw cd1 = new CodeDraw();
			CodeDraw cd2 = new CodeDraw();

			cd1.DrawCircle(100, 100, 50);
			cd2.DrawCircle(400, 200, 100);

			cd1.Show();
			cd2.Show();
		}

		private static void CornerTest()
		{
			CodeDraw cd = new CodeDraw();

			int size = 1;

			cd.Color = Color.Red;
			cd.FillRectangle(0, 0, size, size);
			cd.FillRectangle(0, cd.Height - size, size, size);
			cd.FillRectangle(cd.Width - size, 0, size, size);
			cd.FillRectangle(cd.Width - size, cd.Height - size, size, size);
			cd.Show();
		}

		private static void ProofOfConcept()
		{
			CodeDraw cd = new CodeDraw();

			cd.Color = Color.Red;
			cd.FillRectangle(20, 20, 100, 100);

			cd.Title = "Hello World";

			cd.Color = Color.Blue;
			cd.FillCircle(50, 50, 50);

			cd.Color = Color.LightBlue;
			cd.LineWidth = 5;
			cd.DrawRectangle(30, 30, 200, 200);

			cd.Show();
		}
	}
}
