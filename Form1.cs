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
using System.Text.RegularExpressions;
using System.Reflection;

namespace DJFLAP
{
	public partial class Form1 : Form
	{
		Graphics g;
		int x = -1;
		int y = -1;
		Pen pen;
		FiniteAutomaton fa;
		Rectangle selectRect;
		IEnumerable<Rectangle> stateRectList;
		// When the form initializes, we define g (our graphics in our panel) and our pen.
		public Form1()
		{
			InitializeComponent();
			pen = new Pen(Color.Black, 5);
			pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
			fa = new FiniteAutomaton();
			this.DoubleBuffered = true;
			typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
			| BindingFlags.Instance | BindingFlags.NonPublic, null,
			panel1, new object[] { true });
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			     
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
			


		}

		/* When click the "Load DFA From File", read as XML, assigning all relevant fields to a Finite Automaton
		 *	object. */
		private void loadDFAFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
			OpenFileDialog upload = new OpenFileDialog();
			
			upload.Title = "Select File";
			upload.Filter = "All files(*.*) | *.*"; // Just defining any filetype, since xml can come in many formats
			string filePath = upload.FileName;

			if (upload.ShowDialog() == DialogResult.OK) // This opens the dialog
			{
				fa = new FiniteAutomaton(); // custom obj with a list of states & a list of transitions

				XElement root = XElement.Load(upload.FileName);

				// select xml elements with name 'state'
				IEnumerable<XElement> states = root.Descendants().Where(x => x.Name == "state"); 

				// for each state
				foreach(var i in states)
				{
					// find the attribute with the name 'id' in the state, set int stateId to its value
					int stateId = Int32.Parse(i.Attributes().FirstOrDefault(x => x.Name == "id").Value);

					// find the attribute with the name 'name', set stateName to its value
					string stateName = i.Attributes().FirstOrDefault(x => x.Name == "name").Value;

					// find the elements underneath state with the names 'x' and 'y'
					int stateX = Convert.ToInt32(float.Parse(i.Descendants().FirstOrDefault(x => x.Name == "x").Value));
					int stateY = Convert.ToInt32(float.Parse(i.Descendants().FirstOrDefault(x => x.Name == "y").Value));

					// if a state is final/initial, it has another element underneath it, named respectively
					XElement finalEl = i.Descendants().FirstOrDefault(x => x.Name == "final");
					XElement initialEl = i.Descendants().FirstOrDefault(x => x.Name == "initial");
					bool final = false;
					bool initial = false;

					// if they exist, they're true.
					if (finalEl != null) final = true;
					if (initialEl != null) initial = true;

					// create a new state with the found attributes & values
					State state = new State(stateId, stateName, stateX, stateY, final, initial);
					fa.states.Add(state);
				}

				// find the list of transitions
				IEnumerable<XElement> transitions = root.Descendants().Where(x => x.Name == "transition");

				// iterate through each transition
				foreach(var i in transitions)
				{
					// find the elements nested underneath transition with names 'from' and 'to'
					int from = Int32.Parse(i.Descendants().FirstOrDefault(x => x.Name == "from").Value);
					int to = Int32.Parse(i.Descendants().FirstOrDefault(x => x.Name == "to").Value);

					// find string that it reads
					string read = i.Descendants().FirstOrDefault(x => x.Name == "read").Value;

					// only accept file if 'read' is single character (multi characters are hard to parse)
					if (!Regex.IsMatch(read, "^.$"))
					{
						Prompt.ShowInfo($"The transition from state id '{from}' to the state id '{to}' has a read that is " +
							$"not 1 character ({read}). This is incompatible with DJFLAP.", "Invalid Format");

						// wipe any data we've read by reinstantiating fa
						fa = new FiniteAutomaton();
						return;
					}

					// create a new transition object, add it to our fa
					Transition transition = new Transition(from, to, read);
					fa.transitions.Add(transition);
				}
				panel1.Invalidate();
			}
		}

		// draw or redraw states & arrows
		private void drawStates(FiniteAutomaton fa)
		{
			// clear canvas so that existing drawings get wiped, set color back to default black
			g.Clear(Color.White);
			pen.Color = Color.Black;
			stateRectList = Enumerable.Empty<Rectangle>();

			// for each state
			foreach(State state in fa.states)
			{
				pen.Color = Color.Black;
				Rectangle rect = new Rectangle();
				int x = state.getX();
				int y = state.getY(); // origin of ellipse = state x,y

				// denote rectangle boundaries of ellipse
				rect.X = x;
				rect.Y = y;
				rect.Width = 25;
				rect.Height = 25;

				if (rect.Contains(selectRect))
				{
					pen.Color = Color.Yellow;
					state.isSelected = true;
				}
				else state.isSelected = false;

				stateRectList.Append(rect); 
				pen.Width = 25;
				pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;

				// define center alignment for text
				StringFormat stringFormat = new StringFormat();
				stringFormat.Alignment = StringAlignment.Center;
				stringFormat.LineAlignment = StringAlignment.Center;

				g.DrawEllipse(pen, rect);
				g.DrawString(state.getName(), Font, Brushes.White, rect, stringFormat);

				// if state is the last, draw another circle (green) on the interior to denote this
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

				// if state is initial, draw an arrow on the left side of it
				if(state.getInitial() == true)
				{
					g.FillPolygon(pen.Brush, new Point[] { new Point(x-10, y+13), new Point(x-30, y), new Point(x-30, y+26)});
				}
			}

			// start drawing arrows
			pen.Width = 2;
			pen.Color = Color.Green;
			pen.CustomEndCap = new AdjustableArrowCap(5, 5);

			// draw an arrow for every transition
			foreach (Transition t in fa.transitions)
			{
				State from = fa.getState(t.getFrom());
				State to = fa.getState(t.getTo());

				int fromX = from.getX();
				int fromY = from.getY();
				int toX = to.getX();
				int toY = to.getY();
				if (from == to)
				{
					fromX += 24;
					toX += 12;
					toY -= 48;

					g.DrawLine(pen, new Point(fromX, fromY), new Point(toX, toY));

					fromX -= 24;

					g.DrawLine(pen, new Point(toX, toY), new Point(fromX, fromY));
					g.DrawString(t.getRead(), Font, Brushes.Black, new Point(toX, toY-12));
				}
				else
				{
					fromX += 12;
					fromY += 12;
					toX += 12;
					toY += 12;
					g.DrawLine(pen, new Point(fromX, fromY), new Point(toX, toY));
					g.DrawString(t.getRead(), Font, Brushes.Black, new Point((fromX + toX) / 2, (fromY + toY) / 2));
				}
			}
		}

		private void drawText()
		{
			foreach (State state in fa.states)
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

				g.DrawString(state.getName(), Font, whiteText, rect, stringFormat);
			}
		}
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
			SaveFileDialog download = new SaveFileDialog();
			download.Filter = "JFF File(*.jff) | *.jff";
			download.Title = "Save a DFA";
			download.ShowDialog();

			if (download.FileName != "")
			{
				FileStream fs = (FileStream)download.OpenFile();
				XmlWriterSettings settings = new XmlWriterSettings();
				settings.Indent = true;
				XmlWriter writer = XmlWriter.Create(fs, settings);

				writer.WriteStartElement("structure");

				writer.WriteStartElement("type");
				writer.WriteString("fa");
				writer.WriteEndElement();

				writer.WriteStartElement("automaton");

				foreach(var state in fa.states)
				{
					writer.WriteStartElement("state");
					writer.WriteAttributeString("id", state.getId().ToString());
					writer.WriteAttributeString("name", state.getName());

					writer.WriteElementString("x", state.getX().ToString());
					writer.WriteElementString("y", state.getY().ToString());

					if(state.getFinal())
					{
						writer.WriteElementString("final", null);
					}
					if(state.getInitial())
					{
						writer.WriteElementString("initial", null);
					}

					writer.WriteEndElement();
				}
				foreach(var transition in fa.transitions)
				{
					writer.WriteStartElement("transition");

					writer.WriteElementString("from", transition.getFrom().ToString());
					writer.WriteElementString("to", transition.getTo().ToString());
					writer.WriteElementString("read", transition.getRead());

					writer.WriteEndElement();
				}

				writer.WriteEndDocument();

				writer.Flush();
				writer.Close();
			}

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
				Rectangle biggerRect = new Rectangle();
				biggerRect.X = fa.getState(currentState).getX();
				biggerRect.Y = fa.getState(currentState).getY();
				biggerRect.Width = 25;
				biggerRect.Height = 25;
				pen.Width = 25;
				pen.Color = Color.Green;
				g.DrawEllipse(pen, biggerRect);
				drawText();
				bool pathFound = false;

				IEnumerable<Transition> paths = fa.transitions.Where(x => x.getFrom() == currentState);
				foreach(Transition t in paths)
				{
					Prompt.ShowInfo($"Reading char {c}, checking transition {t.getFrom()}->{t.getTo()}", "Parsing String");
					if(t.getRead() == c.ToString())
					{
						Prompt.ShowInfo($"Transition success, new state is {t.getTo()}", "Transition Success");
						currentState = t.getTo();
						pathFound = true;
						break;
					}
				}
				if(pathFound == false)
				{
					break;
				}

				drawStates(fa);
			}

			if(fa.states.FirstOrDefault(x => x.getId() == currentState).getFinal() == true)
			{
				Prompt.ShowInfo("This string parses with the DFA!", "Success!");
			}
			else
			{
				Prompt.ShowInfo("We could not find a pathway.", "Failure.");
			}

			drawStates(fa);
		}

		private void panel1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				selectRect = new Rectangle(e.X, e.Y, 0, 0);
				panel1.Invalidate();
			}
		}

		private void panel1_MouseMove(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left)
			{
				selectRect = new Rectangle(e.X, e.Y, 0, 0);

				State selected = fa.states.FirstOrDefault(x => x.isSelected == true);

				if (selected != null)
				{
					selected.setCoordinates(e.X, e.Y);
				}

				panel1.Invalidate();
			}
		}

		private void panel1_Paint(object sender, PaintEventArgs e)
		{
			g = e.Graphics;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			drawStates(fa);
			//Generates the shape       
		}
	}
}
