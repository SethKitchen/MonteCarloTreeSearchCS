using MonteCarloTreeSearch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4
{
    class Connect4IA : UCT<Connect4Transition>
    {

        internal const int FREE = 0;
        internal const int PLAYER_R= 1;  // Red
        internal const int PLAYER_B = 2; // Black

        private const int GRID_SIZE = 7;

        /// <summary>
        /// The grid </summary>
        private readonly int[][] grid;

        private int currentPlayer;
        private int turn = 0;

        public Connect4IA() : base()
        {
            //JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
            //ORIGINAL LINE: this.grid = new int[GRID_SIZE][GRID_SIZE];
            this.grid = RectangularArrays.RectangularIntArray(GRID_SIZE, GRID_SIZE);
            NewGame();
        }

        public virtual void NewGame()
        {
            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    grid[i][j] = FREE;
                }
            }
            // X start to play
            currentPlayer = PLAYER_R;
            turn = 0;
        }

        public override bool Over
        {
            get
            {
                return HasWon(PLAYER_R) || HasWon(PLAYER_B) || turn == 49;
            }
        }

        private bool HasWon(int playerP)
        {
            for (int r = 0; r < GRID_SIZE; r++)
            { // iterate rows, bottom to top
                for (int c = 0; c < GRID_SIZE; c++)
                { // iterate columns, left to right
                    int player = grid[r][c];
                    if (player == FREE)
                        continue; // don't check empty slots

                    if (c + 3 < GRID_SIZE &&
                        player == grid[r][c + 1] && // look right
                        player == grid[r][c + 2] &&
                        player == grid[r][c + 3] && player==playerP)
                        return true;
                    if (r + 3 < GRID_SIZE)
                    {
                        if (player == grid[r + 1][c] && // look up
                            player == grid[r + 2][c] &&
                            player == grid[r + 3][c] && player==playerP)
                            return true;
                        if (c + 3 < GRID_SIZE &&
                            player == grid[r + 1][c + 1] && // look up & right
                            player == grid[r + 2][c + 2] &&
                            player == grid[r + 3][c + 3] && player==playerP)
                            return true;
                        if (c - 3 >= 0 &&
                            player == grid[r + 1][c - 1] && // look up & left
                            player == grid[r + 2][c - 2] &&
                            player == grid[r + 3][c - 3] && player==playerP)
                            return true;
                    }
                }
            }
            return false; // no winner found
        }

        protected override void MakeTransition(Connect4Transition transition)
        {
            if (grid[transition.X][transition.Y] != FREE)
            {
                throw new Exception();
            }
            grid[transition.X][transition.Y] = currentPlayer;
            turn++;
            Next();
        }

        protected override void UnmakeTransition(Connect4Transition transition)
        {
            if (grid[transition.X][transition.Y] == FREE)
            {
                throw new Exception();
            }
            grid[transition.X][transition.Y] = FREE;
            turn--;
            Previous();
        }

        public override ISet<Connect4Transition> PossibleTransitions
        {
            get
            {
                ISet<Connect4Transition> moves = new HashSet<Connect4Transition>();
                for (int i = 0; i < GRID_SIZE; i++)
                {
                    for (int j = 0; j < GRID_SIZE; j++)
                    {
                        if (grid[i][j] == FREE && (i==GRID_SIZE || grid[i+1][j]!=FREE))
                        {
                            moves.Add(new Connect4Transition(i, j, currentPlayer));
                        }
                    }
                }
                return moves;
            }
        }

        /// <summary>
        /// Changes Turn from Player 1 to 2 or Player 2 to 1
        /// </summary>
        private void Next()
        {
            currentPlayer = 3 - currentPlayer;
        }

        /// <summary>
        /// Changes Turn from Player 1 to 2 or Player 2 to 1
        /// </summary>
        private void Previous()
        {
            Next();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(grid[0][0] == FREE ? " " : (grid[0][0] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[0][1] == FREE ? " " : (grid[0][1] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[0][2] == FREE ? " " : (grid[0][2] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[0][3] == FREE ? " " : (grid[0][3] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[0][4] == FREE ? " " : (grid[0][4] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[0][5] == FREE ? " " : (grid[0][5] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[0][6] == FREE ? " " : (grid[0][6] == PLAYER_R ? "R" : "B"));
            sb.Append(Environment.NewLine);
            sb.Append(grid[1][0] == FREE ? " " : (grid[1][0] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[1][1] == FREE ? " " : (grid[1][1] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[1][2] == FREE ? " " : (grid[1][2] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[1][3] == FREE ? " " : (grid[1][3] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[1][4] == FREE ? " " : (grid[1][4] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[1][5] == FREE ? " " : (grid[1][5] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[1][6] == FREE ? " " : (grid[1][6] == PLAYER_R ? "R" : "B"));
            sb.Append(Environment.NewLine);
            sb.Append(grid[2][0] == FREE ? " " : (grid[2][0] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[2][1] == FREE ? " " : (grid[2][1] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[2][2] == FREE ? " " : (grid[2][2] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[2][3] == FREE ? " " : (grid[2][3] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[2][4] == FREE ? " " : (grid[2][4] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[2][5] == FREE ? " " : (grid[2][5] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[2][6] == FREE ? " " : (grid[2][6] == PLAYER_R ? "R" : "B"));
            sb.Append(Environment.NewLine);
            sb.Append(grid[3][0] == FREE ? " " : (grid[3][0] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[3][1] == FREE ? " " : (grid[3][1] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[3][2] == FREE ? " " : (grid[3][2] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[3][3] == FREE ? " " : (grid[3][3] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[3][4] == FREE ? " " : (grid[3][4] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[3][5] == FREE ? " " : (grid[3][5] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[3][6] == FREE ? " " : (grid[3][6] == PLAYER_R ? "R" : "B"));
            sb.Append(Environment.NewLine);
            sb.Append(grid[4][0] == FREE ? " " : (grid[4][0] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[4][1] == FREE ? " " : (grid[4][1] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[4][2] == FREE ? " " : (grid[4][2] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[4][3] == FREE ? " " : (grid[4][3] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[4][4] == FREE ? " " : (grid[4][4] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[4][5] == FREE ? " " : (grid[4][5] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[4][6] == FREE ? " " : (grid[4][6] == PLAYER_R ? "R" : "B"));
            sb.Append(Environment.NewLine);
            sb.Append(grid[5][0] == FREE ? " " : (grid[5][0] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[5][1] == FREE ? " " : (grid[5][1] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[5][2] == FREE ? " " : (grid[5][2] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[5][3] == FREE ? " " : (grid[5][3] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[5][4] == FREE ? " " : (grid[5][4] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[5][5] == FREE ? " " : (grid[5][5] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[5][6] == FREE ? " " : (grid[5][6] == PLAYER_R ? "R" : "B"));
            sb.Append(Environment.NewLine);
            sb.Append(grid[6][0] == FREE ? " " : (grid[6][0] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[6][1] == FREE ? " " : (grid[6][1] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[6][2] == FREE ? " " : (grid[6][2] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[6][3] == FREE ? " " : (grid[6][3] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[6][4] == FREE ? " " : (grid[6][4] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[6][5] == FREE ? " " : (grid[6][5] == PLAYER_R ? "R" : "B"));
            sb.Append(grid[6][6] == FREE ? " " : (grid[6][6] == PLAYER_R ? "R" : "B"));
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }

        public override Connect4Transition SimulationTransition()
        {
            return ExpansionTransition();
        }

        public override Connect4Transition ExpansionTransition()
        {
            ISet<Connect4Transition> possibleTransitions = PossibleTransitions;
            if (possibleTransitions.Count == 0)
            {
                return null;
            }
            IList<Connect4Transition> transitions = new List<Connect4Transition>(possibleTransitions);
            return transitions[(int)Math.Floor(GlobalRandom.NextDouble * possibleTransitions.Count)];
        }

        public override int Winner
        {
            get
            {
                // TODO : handle draw with null return ?
                if (HasWon(PLAYER_R))
                {
                    return PLAYER_R;
                }
                else
                {
                    return PLAYER_B;
                }
            }
        }

        public override int CurrentPlayer
        {
            get
            {
                return currentPlayer;
            }
        }

    }
}

//Helper class added by Java to C# Converter:

//---------------------------------------------------------------------------------------------------------
//	Copyright © 2007 - 2019 Tangible Software Solutions, Inc.
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class is used to replace calls to the static java.lang.Math.random method.
//---------------------------------------------------------------------------------------------------------
internal static class GlobalRandom
{
    private static System.Random randomInstance = null;

    public static double NextDouble
    {
        get
        {
            if (randomInstance == null)
                randomInstance = new System.Random();

            return randomInstance.NextDouble();
        }
    }
}

//Helper class added by Java to C# Converter:

//----------------------------------------------------------------------------------------
//	Copyright © 2007 - 2019 Tangible Software Solutions, Inc.
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class includes methods to convert Java rectangular arrays (jagged arrays
//	with inner arrays of the same length).
//----------------------------------------------------------------------------------------
internal static class RectangularArrays
{
    public static int[][] RectangularIntArray(int size1, int size2)
    {
        int[][] newArray = new int[size1][];
        for (int array1 = 0; array1 < size1; array1++)
        {
            newArray[array1] = new int[size2];
        }

        return newArray;
    }
}
