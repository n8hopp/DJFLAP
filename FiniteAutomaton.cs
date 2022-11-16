using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJFLAP
{
	class FiniteAutomaton
	{
		public List<State> states;
		public List<Transition> transitions;
		public FiniteAutomaton()
		{
			states = new List<State>();
			transitions = new List<Transition>();
		}

		public State getState(int id)
		{
			return this.states.FirstOrDefault(x => x.getId() == id);
		}

		public List<Transition> getTransitions(int id)
		{
			return this.transitions.Where(x => x.getFrom() == id).ToList();
		}
	}
}
