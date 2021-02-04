using System.Windows.Forms;

namespace CodeDrawNS
{
	internal class DoubleBufferedForm : Form
	{
		public DoubleBufferedForm()
		{
			DoubleBuffered = true;
		}
	}
}
