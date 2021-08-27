using CodeDrawProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDrawTest
{
	static class AnimationTest
	{
		public static void Main(string[] args)
		{
			ClockTest();
			GranularAngleTest();
		}

		private static void ClockTest()
		{
			CodeDraw cd = new CodeDraw();

			for (double sec = -Math.Tau / 4; true; sec += Math.Tau / 60)
			{
				cd.Clear();
				//cd.DrawLine(300, 300, Math.Cos(sec) * 100 + 300, Math.Sin(sec) * 100 + 300);

				double min = sec / 60 - Math.Tau / 4;

				for (double j = 0; j < Math.Tau; j += Math.Tau / 12)
				{
					//cd.FillCircle(Math.Cos(j) * 100 + 300, Math.Sin(j) * 100 + 300, 4);
				}

				cd.Show(1000);
			}
		}

		private static void GranularAngleTest()
		{
			CodeDraw cd = new CodeDraw(1000, 1000);

			double steps = Math.Tau / (1 << 14);

			for (double i = 0; i < Math.Tau / 4; i += steps)
			{
				cd.Clear();

				cd.FillArc(100, 100, 800, 800, Math.Tau / 4, i);
				cd.DrawArc(100, 100, 850, 850, Math.Tau / 4, i);

				cd.Show();
			}
		}
	}
}
