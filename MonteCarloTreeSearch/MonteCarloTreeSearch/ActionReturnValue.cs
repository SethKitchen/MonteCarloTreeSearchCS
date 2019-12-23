using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarloTreeSearch
{
    public class ActionReturnValue
    {
        public IState NewState { get; set; }
        public double Value { get; set; }
        public bool Done { get; set; }
    }
}
