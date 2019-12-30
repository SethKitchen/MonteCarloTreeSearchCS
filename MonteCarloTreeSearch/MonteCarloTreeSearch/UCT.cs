using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarloTreeSearch
{
    public abstract class UCT<T> : MonteCarloTreeSearch<T> where T : ITransition
    {

        private static readonly double C = Math.Sqrt(2);

        // TODO if node is leaf pick random transition
        //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public T selectTransition(Node<T> node, final int player)
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
        public override T SelectTransition(Node<T> node, int player)
        {
            double v = double.NegativeInfinity;
            T best = default(T);
            foreach (T transition in PossibleTransitions)
            {
                Node<T> n = node.GetChild(transition);
                if (n == null)
                {
                    // unexplored path
                    return transition;
                }
                if (!n.Terminal)
                {
                    // child already explored and non terminal
                    long simulations = n.Simulations;
                    long wins = n.WinsF(player);
                    // w/n + C * Math.sqrt(ln(n(p)) / n)
                    // TODO : add a random hint to avoid ex-aequo
                    double value = (simulations == 0 ? 0 : wins / simulations + C * Math.Sqrt(Math.Log(node.Simulations) / simulations));
                    if (value > v)
                    {
                        v = value;
                        best = transition;
                    }
                }
            }
            return best;
        }
    }
}
