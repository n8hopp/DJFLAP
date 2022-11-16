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
using System.Drawing.Drawing2D;

namespace DJFLAP
{
	public partial class Form1 : Form
	{
		Graphics g;
		int x = -1;
		int y = -1;
		Pen pen;
		FiniteAutomaton fa;
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
				fa = new FiniteAutomaton();

				XElement root = XElement.Load(upload.FileName);
				IEnumerable<XElement> states = root.Descendants().Where(x => x.Name == "state");

				foreach(var i in states)
				{
					int stateId = Int32.Parse(i.Attributes().FirstOrDefault(x => x.Name == "id").Value);

					string stateName = i.Attributes().FirstOrDefault(x => x.Name == "name").Value;

					int stateX = Convert.ToInt32(float.Parse(i.Descendants().FirstOrDefault(x => x.Name == "x").Value));
					int stateY = Convert.ToInt32(float.Parse(i.Descendants().FirstOrDefault(x => x.Name == "y").Value));

					XElement finalEl = i.Descendants().FirstOrDefault(x => x.Name == "final");
					XElement initialEl = i.Descendants().FirstOrDefault(x => x.Name == "initial");
					bool final = false;
					bool initial = false;

					if (finalEl != null) final = true;
					if (initialEl != null) initial = true;

					State state = new State(stateId, stateName, stateX, stateY, final, initial);
					fa.states.Add(state);
				}

				IEnumerable<XElement> transitions = root.Descendants().Where(x => x.Name == "transition");

				foreach(var i in transitions)
				{
					int from = Int32.Parse(i.Descendants().FirstOrDefault(x => x.Name == "from").Value);
					int to = Int32.Parse(i.Descendants().FirstOrDefault(x => x.Name == "to").Value);
					string read = i.Descendants().FirstOrDefault(x => x.Name == "read").Value;
					Transition transition = new Transition(from, to, read);
					fa.transitions.Add(transition);
				}
				drawStates(fa);
				
			}
		}

		private void drawStates(FiniteAutomaton fa)
		{
			g.Clear(Color.White);
			pen.Color = Color.Black;
			foreach(State state in fa.states)
			{
				Rectangle rect = new Rectangle();
				int x = state.getX();
				int y = state.getY();
				rect.X = x;
				rect.Y = y;
				rect.Width = 25;
				rect.Height = 25;
				pen.Width = 25;
				pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
				Brush whiteText = new SolidBrush(Color.White);
				StringFormat stringFormat = new StringFormat();
				stringFormat.Alignment = StringAlignment.Center;
				stringFormat.LineAlignment = StringAlignment.Center;

				g.DrawEllipse(pen, rect);
				g.DrawString(state.getName(), Font, whiteText, rect, stringFormat);

				if(state.getFinal() == true)
				{
					Rectangle biggerRect = new Rectangle();
					biggerRect.X = x;
					biggerRect.Y = y;
					biggerRect.Width = 25;
					biggerRect.Height = 25;
					pen.Width = 2;
					pen.Color = Color.Green;
					g.DrawEllipse(pen, biggerRect);
				}
				if(state.getInitial() == true)
				{
					g.FillPolygon(pen.Brush, new Point[] { new Point(x-10, y+13), new Point(x-30, y), new Point(x-30, y+26)});
				}
			}
			pen.Width = 2;
			pen.Color = Color.Green;
			pen.CustomEndCap = new AdjustableArrowCap(5, 5);
			foreach (Transition t in fa.transitions)
			{
				State from = fa.getState(t.getFrom());
				State to = fa.getState(t.getTo());
				int fromX = from.getX() + 12;
				int fromY = from.getY() + 12;
				int toX = to.getX() + 12;
				int toY = to.getY() + 12;
				g.DrawLine(pen, new Point(fromX, fromY), new Point(toX, toY));
				g.DrawString(t.getRead(), Font, Brushes.Black, new Point((fromX + toX) / 2, (fromY + toY) / 2));
			}
		}
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

		private void testDFAInputToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (fa == null) { return; }

			string promptValue = Prompt.ShowDialog("Please input a string you wish to parse.", "String Input");
			string shrunk = String.Concat(promptValue.Where(c => !Char.IsWhiteSpace(c)));

			State firstState = fa.states.FirstOrDefault(x => x.getInitial() == true);
			if (firstState == null) return;

			int currentState = firstState.getId();
			
			foreach(char c in shrunk)
			{
				IEnumerable<Transition> paths = fa.transitions.Where(x => x.getFrom() == currentState);
				foreach(Transition t in paths)
				{
					if(t.getRead() == c.ToString())
					{
						currentState = t.getTo();
						break;
					}
				}
			}

			if(fa.states.FirstOrDefault(x => x.getId() == currentState).getFinal() == true)
			{
				Prompt.ShowInfo("This string parses with the DFA!", "Success!");
			}
			else
			{
				Prompt.ShowInfo("We could not find a pathway.", "Failure.");
			}


		}
	}
}
