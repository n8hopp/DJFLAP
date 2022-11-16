using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJFLAP
{
	class Transition
	{
		int from;
		int to;
		string read;

		public Transition()
		{
			from = 0;
			to = 0;
			read = "";
		}
		
		public Transition(int from, int to, string read)
		{
			this.from = from;
			this.to = to;
			this.read = read;
		}

		public int getFrom()
		{
			return this.from;
		}

		public int getTo()
		{
			return this.to;
		}
		public string getRead()
		{
			return this.read;
		}
	}
}
