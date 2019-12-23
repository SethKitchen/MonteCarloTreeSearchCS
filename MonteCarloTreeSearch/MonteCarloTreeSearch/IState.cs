using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarloTreeSearch
{
    public abstract class IState
    {
        public abstract ActionReturnValue TakeAction(int SimulationAction);
    }
}
