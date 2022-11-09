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
		private Dictionary<int, char> transitions;
		public State()
		{
			isSelected = false;
			name = "";
			id = -1;
		}
	}
}
