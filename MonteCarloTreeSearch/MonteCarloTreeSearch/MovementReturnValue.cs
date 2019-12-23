using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarloTreeSearch
{
    public class MovementReturnValue<T, S> where T : BasicNodeInfo where S : BasicEdgeInfo
    {
        public MonteCarloNode<T,S> CurrentNode { get; set; }
        public double Value { get; set; }
        public bool Done { get; set; }
        public List<MonteCarloEdge<T,S>> Breadcrumbs { get; set; }
    }
}
