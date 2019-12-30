using MonteCarloTreeSearch;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
    public class TicTacToeRunner : SampleRunner<TicTacToeTransition>
    {

        public TicTacToeRunner() : base(new TicTacToeIA())
        {
            // Change the thinking depth value > 0
        }

        public static void Main()
        {
            SampleRunner<TicTacToeTransition> runner = new TicTacToeRunner();
            runner.Run();
        }

    }
}
