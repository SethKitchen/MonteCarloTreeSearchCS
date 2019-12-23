using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarloTreeSearch
{
    public class Agent<T1,T2> where T1:BasicNodeInfo where T2:BasicEdgeInfo
    {
        private int NumSimulation { get; set; }
        private double DegreeExploration { get; set; }
        private MonteCarloTreeSearch<T1,T2> MCTS { get; set; }
        public Agent(int numSimulations, double degreeExploration, MonteCarloTreeSearch<T1,T2> mcts)
        {
            NumSimulation = numSimulations;
            DegreeExploration = degreeExploration;
            MCTS = mcts;
        }

        public void Simulate()
        {
            var returnValue = MCTS.MoveToLeaf(); 
            //Evaluate Leaf

            //
            var value = returnValue.Value;
            var breadCrumbs = returnValue.Breadcrumbs;
            MCTS.BackFill(returnValue.CurrentNode,value, ref breadCrumbs);
        }
    }
}
