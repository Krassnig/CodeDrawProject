using CodeDrawProject;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CodeDrawTest
{
	static class EventTest
	{
		public static void Main(string[] args)
		{
			// Always commit all function commmented out!

			//EventSleepTest();
			//CurveTest();
			//MouseTest((cd, handler) => cd.MouseClick += handler);
			//MouseTest((cd, handler) => cd.MouseMove += handler);
			//MouseTest((cd, handler) => cd.MouseDown += handler);
			//MouseTest((cd, handler) => cd.MouseUp += handler);
			//MouseTest((cd, handler) => cd.MouseLeave += handler);
			//MouseTest((cd, handler) => cd.MouseEnter += handler);
			//MouseWheelTest();
			//KeyPressTest((cd, handler) => cd.KeyPress += handler);
			//KeyTest((cd, handler) => cd.KeyDown += handler);
			//KeyTest((cd, handler) => cd.KeyUp += handler);
			//WindowMoveTest();
			//UnsubscribeTest();
		}

		private static void EventSleepTest()
		{
			CodeDraw cd = new CodeDraw();

			cd.DrawText(50, 50, "Click here, then I will turn blue, then green!");
			cd.Color = Color.Red;
			cd.FillSquare(100, 100, 100);
			cd.Show();

			cd.MouseDown += (cd, args) => {
				cd.Clear();
				cd.Color = Color.Blue;
				cd.DrawText(50, 50, "I'm blue da ba dee!");
				cd.FillSquare(100, 100, 100);
				cd.Show(3000);

				cd.Clear();
				cd.Color = Color.Green;
				cd.DrawText(50, 50, "Now I'm green.");
				cd.FillSquare(100, 100, 100);
				cd.Show();
			};
		}

		private static void CurveTest()
		{
			CodeDraw cd = new CodeDraw();

			cd.MouseMove += (cd, args) =>
			{
				cd.Clear();
				cd.DrawBezier((200, 200), (args.X, args.Y), (args.X, args.Y), (400, 400));
				cd.Show();
			};
		}

		private static void MouseWheelTest()
		{
			CodeDraw cd = new CodeDraw();
			cd.Color = Color.Red;
			cd.MouseWheel += (cd, args) =>
			{
				cd.Clear();
				cd.DrawTriangle(200, 300, 400, 300, 300, 300 + args.Delta);
				cd.Show();
			};
		}

		private static void MouseTest(Action<CodeDraw, CodeDrawProject.EventHandler<MouseEventArgs>> s)
		{
			CodeDraw cd = new CodeDraw();

			cd.Color = Color.Red;
			s(cd, (cd, args) =>
			{
				cd.FillSquare(args.X - 5, args.Y - 5, 10);
				cd.Show();
			});
		}

		private static void KeyPressTest(Action<CodeDraw, CodeDrawProject.EventHandler<KeyPressEventArgs>> s)
		{
			CodeDraw cd = new CodeDraw();

			cd.Color = Color.Red;
			string text = "";
			cd.Show();

			s(cd, (cd, args) =>
			{
				cd.Clear();
				text += args.KeyChar;
				cd.DrawText(100, 100, text);
				cd.Show();
			});
		}

		private static void KeyTest(Action<CodeDraw, CodeDrawProject.EventHandler<KeyEventArgs>> s)
		{
			CodeDraw cd = new CodeDraw();

			cd.Color = Color.Red;
			string text = "";
			cd.Show();

			s(cd, (cd, args) =>
			{
				cd.Clear();
				text += (char)args.KeyData;
				cd.DrawText(100, 100, text);
				cd.Show();
			});
		}

		private static void WindowMoveTest()
		{
			CodeDraw cd = new CodeDraw();

			cd.Color = Color.Red;
			cd.DrawSquare(200, 200, 100);
			cd.Show();

			int x = cd.FramePositionX;
			int y = cd.FramePositionY;

			cd.WindowMove += (c, a) =>
			{
				int dx = x - c.FramePositionX;
				int dy = y - c.FramePositionY;

				c.Clear();
				c.DrawSquare(dx + 200, dy + 200, 100);
				c.Show();
			};
		}

		private static int unsubscribeProgress = 0;
		private static CodeDrawProject.EventHandler<MouseEventArgs>? mouse;
		private static CodeDrawProject.EventHandler<KeyPressEventArgs>? key;

		private static void UnsubscribeTest()
		{
			CodeDraw cd = new CodeDraw();

			mouse = new CodeDrawProject.EventHandler<MouseEventArgs>(Unsubscribe_Mouse);
			key = new CodeDrawProject.EventHandler<KeyPressEventArgs>(Unsubscribe_Key);

			Unsubscribe_Mouse(cd, null);
		}

		private static void Unsubscribe_Mouse(CodeDraw cd, MouseEventArgs args)
		{
			cd.Clear();
			cd.Color = Color.Blue;
			cd.DrawText(200, 100, "Press a key on your keyboard.");
			cd.DrawTriangle(200, 200, 400, 200, 300, 400);
			cd.FillRectangle(10, 10, 40, unsubscribeProgress++ * 5);
			cd.Show();

			cd.MouseClick -= mouse;
			cd.KeyPress += key;
		}

		private static void Unsubscribe_Key(CodeDraw cd, KeyPressEventArgs args)
		{
			cd.Clear();
			cd.Color = Color.Red;
			cd.DrawText(200, 100, "Press a button on your mouse.");
			cd.DrawSquare(200, 200, 200);
			cd.FillRectangle(10, 10, 40, unsubscribeProgress++ * 5);
			cd.Show();

			cd.KeyPress -= key;
			cd.MouseClick += mouse;
		}
	}
}
