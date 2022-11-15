using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJFLAP
{
	class State
	{
		private bool isSelected;
		private string name;
		private int id;
		private int x, y;
		public State()
		{
			isSelected = false;
			name = "";
			id = -1;
			x = y = 0;
		}

		public State(int id)
		{
			this.id = id;
			isSelected = false;
			name = "";
		}

		public State(int id, string name, int x, int y)
		{
			this.name = name;
			this.id = id;
			this.x = x;
			this.y = y;
			isSelected = false;
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
