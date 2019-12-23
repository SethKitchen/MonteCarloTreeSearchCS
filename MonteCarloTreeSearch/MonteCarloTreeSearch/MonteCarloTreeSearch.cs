using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarloTreeSearch
{
    public class MonteCarloTreeSearch<T, S> where T : BasicNodeInfo where S : BasicEdgeInfo
    {
        public double DegreeOfExploration { get; set; }
        public MonteCarloNode<T, S> RootNode { get; set; }
        public double EPSILON { get; set; }

        public double ALPHA { get; set; }

        private Dictionary<string, MonteCarloNode<T, S>> Tree;
        private Random r = new Random();

        //Intentionally private -- must supply initial data
        private MonteCarloTreeSearch()
        {

        }

        public int Length { get { return Tree.Count; } }

        public MonteCarloTreeSearch(MonteCarloNode<T, S> InitialRootNode, double InitialDegreeOfExploration)
        {
            RootNode = InitialRootNode;
            DegreeOfExploration = InitialDegreeOfExploration;
            Tree = new Dictionary<string, MonteCarloNode<T, S>>();
            EPSILON = 0.2;
            ALPHA = 0.8;
        }

        public MovementReturnValue<T,S> MoveToLeaf()
        {
            List<MonteCarloEdge<T, S>> breadcrumbs = new List<MonteCarloEdge<T, S>>();
            MonteCarloNode<T,S > currentNode = RootNode;
            bool done = false;
            double value = 0;
            int simulationAction=0;
            MonteCarloEdge<T, S> simulationEdge = null;

            while(!currentNode.IsLeaf)
            {
                double maxQU = -double.MaxValue;
                double epsilon;
                double alpha;
                double[] nu;
                if(currentNode == RootNode)
                {
                    epsilon = EPSILON;
                    nu = new double[currentNode.ConnectedEdges.Count];
                    epsilon = EPSILON;
                    alpha = ALPHA;
                    for (int i=0; i<nu.Length; i++)
                    {
                        nu[i] = ALPHA;
                    }
                    nu = MathNet.Numerics.Distributions.Dirichlet.Sample(r, nu);
                }
                else
                {
                    epsilon = 0;
                    alpha = ALPHA;
                    nu = new double[currentNode.ConnectedEdges.Count];
                    for(int i=0; i<nu.Length; i++)
                    {
                        nu[i] = 0;
                    }
                }

                int Nb = 0;
                for(int i=0; i<currentNode.ConnectedEdges.Count; i++)
                {
                    Nb = Nb + currentNode.ConnectedEdges[i].EdgeData.N;
                }

                for(int idx=0; idx<currentNode.ConnectedEdges.Count; idx++)
                {
                    int action = currentNode.ConnectedEdges[idx].EdgeData.Action;
                    MonteCarloEdge<T, S> edge = currentNode.ConnectedEdges[idx];

                    double U = DegreeOfExploration * ((1 - epsilon) * edge.EdgeData.P + epsilon * nu[idx]) * Math.Sqrt(Nb) / (1 + edge.EdgeData.N);
                    double Q = edge.EdgeData.Q;

                    if(Q+U > maxQU)
                    {
                        maxQU = Q + U;
                        simulationAction = action;
                        simulationEdge = edge;
                    }
                }

                ActionReturnValue arv = currentNode.NodeData.NodeState.TakeAction(simulationAction);
                var newState = arv.NewState;
                value = arv.Value;
                done = arv.Done;
                currentNode = simulationEdge.OutNode;
                breadcrumbs.Add(simulationEdge);
            }

            return new MovementReturnValue<T, S>() { Breadcrumbs = breadcrumbs, Done = done, Value = value, CurrentNode = currentNode };
        }


        public void BackFill(MonteCarloNode<T,S> leaf, double value, ref List<MonteCarloEdge<T,S>> breadcrumbs)
        {
            int currentPlayer = leaf.NodeData.Turn;
            for(int i=0; i<breadcrumbs.Count; i++)
            {
                int playerTurn = breadcrumbs[i].InNode.NodeData.Turn;
                int direction = 0;
                if(playerTurn==currentPlayer)
                {
                    direction = 1;
                }
                else
                {
                    direction = -1;
                }

                breadcrumbs[i].EdgeData.N = breadcrumbs[i].EdgeData.N + 1;
                breadcrumbs[i].EdgeData.W = breadcrumbs[i].EdgeData.W + value * direction;
                breadcrumbs[i].EdgeData.Q = breadcrumbs[i].EdgeData.W / breadcrumbs[i].EdgeData.N;
            }
        }

        public void AddNode(MonteCarloNode<T,S> node)
        {
            Tree[node.NodeData.Id] = node;
        }
    }
}
