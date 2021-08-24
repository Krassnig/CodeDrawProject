using CodeDrawNS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace CodeDrawTest
{
	public class CodeDrawEventTest
	{
		public static void MainEvent(string[] args)
		{
			//MouseTest((c, e) => c.MouseClick += e);
			//MouseTest((c, e) => c.MouseMove += e);
			//MouseTest((c, e) => c.MouseDown += e);
			//MouseTest((c, e) => c.MouseUp += e);
			//EnterLeaveTest((c, e) => { c.MouseLeave += e; c.MouseEnter += e; });
			//MouseWheelTest();
			//KeyEventTest((c, e) => c.KeyUp += e);
			//KeyEventTest((c, e) => c.KeyPress += e);
			//KeyEventTest((c, e) => c.KeyDown += e);
			//WindowMoveTest();
		}

		private static void WindowMoveTest()
		{
			CodeDraw cd = new CodeDraw();

			cd.Color = Color.Red;

			int x = cd.FramePositionX + 250;
			int y = cd.FramePositionY + 250;

			cd.DrawSquare(x, y, 100);
			cd.Show();

			cd.FrameMove += (c, a) =>
			{
				int dx = x - c.FramePositionX;
				int dy = y - c.FramePositionY;

				c.Clear();
				c.DrawSquare(dx, dy, 100);
				c.Show();
			};
		}

		private static void KeyEventTest(Action<CodeDraw, CodeDraw.EventHandler<KeyEventArgs>> mapTopEvent)
		{
			CodeDraw cd = new CodeDraw();

			cd.Color = Color.Red;
			string s = "";

			mapTopEvent(cd, (c, a) =>
			{
				c.Clear();
				s += (char)a.KeyCode;
				c.DrawText(300, 300, s);
				c.Show();
			});
		}

		private static void MouseWheelTest()
		{
			CodeDraw cd = new CodeDraw();

			cd.Color = Color.Red;

			int l = 0;

			cd.MouseWheel += (c, a) =>
			{
				l += a.Delta / 5;

				c.Clear();
				c.DrawTriangle(200, 300, 400, 300, 300, 300 + l);
				c.Show();
			};
		}

		private static void EnterLeaveTest(Action<CodeDraw, CodeDraw.EventHandler<EventArgs>> mapToEvent)
		{
			CodeDraw cd = new CodeDraw();

			cd.Color = Color.Red;

			int count = 0;

			mapToEvent(cd, (CodeDraw w, EventArgs a) =>
			{
				w.Clear();
				w.DrawText(300, 300, count++.ToString());
				w.Show();
			});
		}

		private static void MouseTest(Action<CodeDraw, CodeDraw.EventHandler<MouseEventArgs>> mapToEvent)
		{
			CodeDraw cd = new CodeDraw();

			cd.Color = Color.Red;

			mapToEvent(cd, (CodeDraw w, MouseEventArgs a) =>
			{
				w.FillRectangle(a.X - 5, a.Y - 5, 10, 10);
				w.Show();
			});
		}
	}
}
