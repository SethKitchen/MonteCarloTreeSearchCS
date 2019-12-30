using MonteCarloTreeSearch;
using System;

namespace Connect4
{
    class Connect4Runner:SampleRunner<Connect4Transition>
    {
        public Connect4Runner() : base(new Connect4IA())
        {
            // Change the thinking depth value > 0
        }

        public static void Main(string[] args)
        {
            SampleRunner<Connect4Transition> runner = new Connect4Runner();
            runner.Run();
        }
    }
}
