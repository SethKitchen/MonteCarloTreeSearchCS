using System;
using System.Collections.Generic;

namespace MonteCarloTreeSearch
{
    /// <summary>
    /// An abstract utility class for testing MCTS implementations.
    /// 
    /// @author antoine vianey
    /// </summary>
    /// @param <T> </param>
    public abstract class SampleRunner<T> where T : ITransition
    {

        public interface Listener<T> where T : ITransition
        {
            void OnMove(MonteCarloTreeSearch<T> mcts, T transition, int turn);
            void OnGameOver(MonteCarloTreeSearch<T> mcts);
            void OnNoPossibleMove(MonteCarloTreeSearch<T> mcts);
        }

        private MonteCarloTreeSearch<T> mcts;
        private Listener<T> listener;

        public SampleRunner(MonteCarloTreeSearch<T> mcts)
        {
            this.mcts = mcts;
        }

        public virtual void SetListener(Listener<T> listener)
        {
            this.listener = listener;
        }

        public virtual void Run()
        {
            T transition;
            int turn = 0;
            while (!mcts.Over)
            {
                ISet<T> transitions = mcts.PossibleTransitions;
                if (transitions.Count > 0)
                {
                    transition = mcts.BestTransition;
                    mcts.DoTransition(transition);
                    if (listener != null)
                    {
                        listener.OnMove(mcts, transition, ++turn);
                    }
                }
                else
                {
                    if (listener != null)
                    {
                        listener.OnNoPossibleMove(mcts);
                    }
                }
            }
            if (listener != null)
            {
                listener.OnGameOver(mcts);
            }
        }

    }

}
