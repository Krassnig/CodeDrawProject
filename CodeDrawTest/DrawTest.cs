using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CodeDrawProject;

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
			ImageSaveTest();
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
	}
}
