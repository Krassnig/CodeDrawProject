using CodeDrawProject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeDrawTest
{
	static class EventTest
	{
		public static void Main(string[] args)
		{
			// Always commit all function commmented out!

			UnsubscribeTest();
		}



		private static int unsubscribeProgress = 0;
		private static CodeDrawProject.EventHandler<MouseEventArgs>? mouse;
		private static CodeDrawProject.EventHandler<KeyEventArgs>? key;

		private static void UnsubscribeTest()
		{
			CodeDraw cd = new CodeDraw();

			mouse = new CodeDrawProject.EventHandler<MouseEventArgs>(Unsubscribe_Mouse);
			key = new CodeDrawProject.EventHandler<KeyEventArgs>(Unsubscribe_Key);

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

		private static void Unsubscribe_Key(CodeDraw cd, KeyEventArgs args)
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
