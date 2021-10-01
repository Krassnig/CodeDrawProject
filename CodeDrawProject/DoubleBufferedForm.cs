using System.Drawing;
using System.Windows.Forms;

namespace CodeDrawProject
{
	internal class DoubleBufferedForm : Form
	{
		public DoubleBufferedForm(Size size)
		{
			SuspendLayout();

			DoubleBuffered = true;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = size;
			Icon = Properties.Resources.CodeDrawIcon;
			MaximizeBox = false;
			FormBorderStyle = FormBorderStyle.FixedSingle;

			ResumeLayout(false);
		}
	}
}
