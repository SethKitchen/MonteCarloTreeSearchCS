using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarloTreeSearch
{
    public class MonteCarloNode<T, S> where T : BasicNodeInfo where S : BasicEdgeInfo
    {
        public T NodeData { get; set; }

        public List<MonteCarloEdge<T, S>> ConnectedEdges { get; set; }

        public MonteCarloNode(T InitialNodeData)
        {
            NodeData = InitialNodeData;
            ConnectedEdges = new List<MonteCarloEdge<T, S>>();
        }

        // Intentionally private -- must supply initial data.
        private MonteCarloNode()
        {

        }

        public bool IsLeaf
        {
            get { return ConnectedEdges.Count <= 0; }
        }
    }
}
