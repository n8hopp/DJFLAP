using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJFLAP
{
	class State
	{
		public bool isSelected;
		private bool isFinal;
		private bool isInitial;
		private string name;
		private int id;
		private int x, y;
		public Rectangle boundingBox;
		public State()
		{
			isSelected = false;
			isFinal = false;
			isInitial = false;
			name = "";
			id = -1;
			x = y = 0;
			boundingBox = new Rectangle();
		}

		public State(int id)
		{
			this.id = id;
			isSelected = false;
			isFinal = false;
			isInitial = false;
			name = "";
		}

		public State(int id, string name, int x, int y, bool final, bool initial)
		{
			this.name = name;
			this.id = id;
			this.x = x;
			this.y = y;
			this.isInitial = initial;
			this.isFinal = final;
			isSelected = false;
		}

		public void Draw()
		{

		}
		public int getId()
		{
			return this.id;
		}

		public int getX()
		{
			return this.x;
		}

		public int getY()
		{
			return this.y;
		}

		public bool getFinal()
		{
			return this.isFinal;
		}

		public bool getInitial()
		{
			return this.isInitial;
		}
		public string getName()
		{
			return this.name;
		}

		public bool toggleSelect()
		{
			isSelected = !isSelected;
			return isSelected;
		}

		public bool setCoordinates(int x, int y)
		{
			this.x = x;
			this.y = y;
			return true;
		}

		public bool setName(string name)
		{
			this.name = name;
			return true;
		}

		public bool setId(int id)
		{
			this.id = id;
			return true;
		}
	}
}
