using MonteCarloTreeSearch;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
    /// <summary>
    /// Simple TicTacToe IA to showcase the API.
    /// 
    /// @author antoine vianey
    /// </summary>
    public class TicTacToeIA : UCT<TicTacToeTransition>
    {

        internal const int FREE = 0;
        internal const int PLAYER_X = 1; // X
        internal const int PLAYER_O = 2; // O

        private const int GRID_SIZE = 3;

        /// <summary>
        /// The grid </summary>
        private readonly int[][] grid;

        private int currentPlayer;
        private int turn = 0;

        public TicTacToeIA() : base()
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
            currentPlayer = PLAYER_X;
            turn = 0;
        }

        public override bool Over
        {
            get
            {
                return HasWon(PLAYER_O) || HasWon(PLAYER_X) || turn == 9;
            }
        }

        private bool HasWon(int player)
        {
            return (player == grid[0][1] && player == grid[0][2] && player == grid[0][0]) || (player == grid[1][1] && player == grid[1][2] && player == grid[1][0]) || (player == grid[2][1] && player == grid[2][2] && player == grid[2][0]) || (player == grid[1][0] && player == grid[2][0] && player == grid[0][0]) || (player == grid[1][1] && player == grid[2][1] && player == grid[0][1]) || (player == grid[1][2] && player == grid[2][2] && player == grid[0][2]) || (player == grid[1][1] && player == grid[2][2] && player == grid[0][0]) || (player == grid[1][1] && player == grid[2][0] && player == grid[0][2]);
        }

        protected override void MakeTransition(TicTacToeTransition transition)
        {
            if (grid[transition.X][transition.Y] != FREE)
            {
                throw new Exception();
            }
            grid[transition.X][transition.Y] = currentPlayer;
            turn++;
            Next();
        }

        protected override void UnmakeTransition(TicTacToeTransition transition)
        {
            if (grid[transition.X][transition.Y] == FREE)
            {
                throw new Exception();
            }
            grid[transition.X][transition.Y] = FREE;
            turn--;
            Previous();
        }

        public override ISet<TicTacToeTransition> PossibleTransitions
        {
            get
            {
                ISet<TicTacToeTransition> moves = new HashSet<TicTacToeTransition>();
                for (int i = 0; i < GRID_SIZE; i++)
                {
                    for (int j = 0; j < GRID_SIZE; j++)
                    {
                        if (grid[i][j] == FREE)
                        {
                            moves.Add(new TicTacToeTransition(i, j, currentPlayer));
                        }
                    }
                }
                return moves;
            }
        }

        private void Next()
        {
            currentPlayer = 3 - currentPlayer;
        }

        private void Previous()
        {
            Next();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(grid[0][0] == FREE ? " " : (grid[0][0] == PLAYER_O ? "O" : "X"));
            sb.Append(grid[1][0] == FREE ? " " : (grid[1][0] == PLAYER_O ? "O" : "X"));
            sb.Append(grid[2][0] == FREE ? " " : (grid[2][0] == PLAYER_O ? "O" : "X"));
            sb.Append("\n");
            sb.Append(grid[0][1] == FREE ? " " : (grid[0][1] == PLAYER_O ? "O" : "X"));
            sb.Append(grid[1][1] == FREE ? " " : (grid[1][1] == PLAYER_O ? "O" : "X"));
            sb.Append(grid[2][1] == FREE ? " " : (grid[2][1] == PLAYER_O ? "O" : "X"));
            sb.Append("\n");
            sb.Append(grid[0][2] == FREE ? " " : (grid[0][2] == PLAYER_O ? "O" : "X"));
            sb.Append(grid[1][2] == FREE ? " " : (grid[1][2] == PLAYER_O ? "O" : "X"));
            sb.Append(grid[2][2] == FREE ? " " : (grid[2][2] == PLAYER_O ? "O" : "X"));
            sb.Append("\n");
            return sb.ToString();
        }

        public override TicTacToeTransition SimulationTransition()
        {
            return ExpansionTransition();
        }

        public override TicTacToeTransition ExpansionTransition()
        {
            ISet<TicTacToeTransition> possibleTransitions = PossibleTransitions;
            if (possibleTransitions.Count == 0)
            {
                return null;
            }
            IList<TicTacToeTransition> transitions = new List<TicTacToeTransition>(possibleTransitions);
            return transitions[(int)Math.Floor(GlobalRandom.NextDouble * possibleTransitions.Count)];
        }

        public override int Winner
        {
            get
            {
                // TODO : handle draw with null return ?
                if (HasWon(PLAYER_O))
                {
                    return PLAYER_O;
                }
                else
                {
                    return PLAYER_X;
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
