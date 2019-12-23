using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarloTreeSearch
{
    /// <summary>
    /// For use in a MonteCarloEdge. Add onto this with your unique data.
    /// </summary>
    public class BasicEdgeInfo
    {
        /// <summary>
        /// What is done to get from InNode to OutNode - ie an index in an All Available Actions array.
        /// </summary>
        public int Action { get; set; }

        /// <summary>
        /// Visit Count
        /// </summary>
        public int N { get; set; }

        /// <summary>
        /// Action Value - How "good" this path is according to learning
        /// </summary>
        public double Q { get; set; }

        /// <summary>
        /// Used for Calculating Q Value.
        /// </summary>
        public double W { get; set; }

        /// <summary>
        /// Prior Probability - Policy (usually returned by neural net)
        /// </summary>
        public double P { get; set; }
    }
}
