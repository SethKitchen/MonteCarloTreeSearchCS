using System;
using System.Collections.Generic;

namespace MonteCarloTreeSearch
{
    public class MonteCarloEdge<T, S> where T:BasicNodeInfo where S:BasicEdgeInfo
    {
        public MonteCarloNode<T,S> InNode { get; set; }
        public MonteCarloNode<T,S> OutNode { get; set; }
        public S EdgeData { get; set; }

        //Intentionally Private -- Must supply initial data
        private MonteCarloEdge()
        {

        }

        public MonteCarloEdge(MonteCarloNode<T,S> InitialInNode, MonteCarloNode<T,S> InitialOutNode, S InitialEdgeData)
        {
            InNode = InitialInNode;
            OutNode = InitialOutNode;
            EdgeData = InitialEdgeData;
        }

    }
}
