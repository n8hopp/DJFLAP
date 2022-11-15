using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace DJFLAP
{
	public partial class Form1 : Form
	{
		Graphics g;
		int x = -1;
		int y = -1;
		Pen pen;

		String fileName;

		

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

        private void loadDFAFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
			OpenFileDialog upload = new OpenFileDialog();
			
			upload.Title = "Select File";
			upload.Filter = "All files(*.*) | *.*";
			string filePath = upload.FileName;
			this.fileName = Path.GetFileName(filePath);

			if (upload.ShowDialog() == DialogResult.OK)
			{
				FiniteAutomaton fa = new FiniteAutomaton();

				XElement root = XElement.Load(upload.FileName);
				IEnumerable<XElement> states = root.Elements().Where(x => x.Name == "state");

				foreach (XmlNode node in nodes)
				{
					int stateId = Int32.Parse(node.Attributes["id"].Value);
					string stateName = node.Attributes["name"].Value;
					int stateX = Int32.Parse(node.SelectSingleNode("X").InnerText);
					int stateY = Int32.Parse(node.SelectSingleNode("Y").InnerText);
					State state = new State(stateId, stateName, stateX, stateY);
					fa.states.Add(state);
				}

				XmlNodeList transitions = doc.DocumentElement.SelectNodes("structure/automaton/transition");

				foreach (XmlNode node in nodes)
				{
					int from = Int32.Parse(node.Attributes["from"].Value);
					int to = Int32.Parse(node.Attributes["to"].Value);
					fa.transitions.Add(from, to);
				}

				drawStates(fa);
				
				}
		}

		private void drawStates(FiniteAutomaton fa)
		{
			foreach(State state in fa.states)
			{
				Rectangle rect = new Rectangle();
				rect.X = state.getX();
				rect.Y = state.getY();
				rect.Width = 50;
				rect.Height = 50;
				pen.Width = 50;
				g.DrawEllipse(pen, rect);
			}
			for(int i = 0; i < fa.transitions.Count; i++)
			{

			}
		}
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
