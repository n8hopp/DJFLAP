using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DJFLAP
{
	public partial class Form1 : Form
	{
		Graphics g;
		int x = -1;
		int y = -1;
		Pen pen;

		public Form1()
		{
			InitializeComponent();
			g = panel1.CreateGraphics();
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			pen = new Pen(Color.Black, 5);
			pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
		}

		private void testDFAToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void toolStripDropDownButton1_Click(object sender, EventArgs e)
		{

		}

		private void panel1_Click(object sender, EventArgs e)
		{
			
		}

		private void panel1_MouseClick(object sender, MouseEventArgs e)
		{
			x = e.X; //Starting x and Y
			y = e.Y;

			Rectangle rect = new Rectangle();
			rect.X = x;
			rect.Y = y;
			rect.Width = 5;
			rect.Height = 5;
			pen.Width = 5;
			g.DrawEllipse(pen, rect);
		}
	}
}
