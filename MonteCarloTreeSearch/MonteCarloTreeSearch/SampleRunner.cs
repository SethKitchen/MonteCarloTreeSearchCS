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

        public interface IListener<T1> where T1 : ITransition
        {
            void OnMove(MonteCarloTreeSearch<T1> mcts, T1 transition, int turn);
            void OnGameOver(MonteCarloTreeSearch<T1> mcts);
            void OnNoPossibleMove(MonteCarloTreeSearch<T1> mcts);
        }

        private readonly MonteCarloTreeSearch<T> mcts;
        private IListener<T> listener;

        public SampleRunner(MonteCarloTreeSearch<T> mcts)
        {
            this.mcts = mcts;
        }

        public virtual void SetListener(IListener<T> listener)
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
