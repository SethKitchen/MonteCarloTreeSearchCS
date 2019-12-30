# MonteCarloTreeSearchCS

Adapted from https://github.com/avianey/mcts4j - a purely JAVA MCTS.

This does not contain an agent - just MCTS with some basic examples. The samples just initialize a game using the correct architecture for MCTS.

To use this:

1) Create a new Project add add reference to MonteCarloTreeSearch CS Project.

2) Add a Transition class to your project that extends ITransition. A Transition or Action tells the tree where it can go from the current State.  For example see (TicTacToe->TicTacToeTransition or Connect4->Connect4Transition).

3) Add an IA class to your project that extends UCT<[YOUR]Transition>. This is where game logic goes. How to make an action, change turns, determine if the game is won, etc. For example see (TicTacToe->TicTacToeIA.cs or Connect4->Connect4Transition).

4) Create a Runner to play the game (this eventually would be an agent). We include SampleRunner in the MonteCarloTreeSearch CS Project, and the TicTacToe/Connect4 runners are only extending that. Just initializes.

