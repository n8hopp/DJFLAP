using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DJFLAP
{
	public static class Extensions
	{
		public static void FillTriangle(this Graphics g, Point p, int size)
		{
			g.FillPolygon(Brushes.Aquamarine, new Point[] { p, new Point(p.X - size, p.Y + (int)(size * Math.Sqrt(3))), new Point(p.X + size, p.Y + (int)(size * Math.Sqrt(3))) });
		}
	}
}
