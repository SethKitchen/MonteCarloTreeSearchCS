using MonteCarloTreeSearch;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
    public class TicTacToeTransition : ITransition
    {

        /// <summary>
        /// The player owning the move </summary>
        private int player;

        /// <summary>
        /// x coordinate of the move </summary>
        private int x;
        /// <summary>
        /// y coordinate of the move </summary>
        private int y;

        private TicTacToeTransition()
        {
        }

        public TicTacToeTransition(int x, int y, int player)
        {
            this.x = x;
            this.y = y;
            this.player = player;
        }

        public virtual int X
        {
            get
            {
                return x;
            }
            set
            {
                this.x = value;
            }
        }


        public virtual int Y
        {
            get
            {
                return y;
            }
            set
            {
                this.y = value;
            }
        }


        public virtual int Player
        {
            get
            {
                return player;
            }
            set
            {
                this.player = value;
            }
        }


        public override int GetHashCode()
        {
            return (player >> 6) | (x >> 3) | y;
        }

        public override bool Equals(object o)
        {
            return o is TicTacToeTransition && ((TicTacToeTransition)o).player == player && ((TicTacToeTransition)o).x == x && ((TicTacToeTransition)o).y == y;
        }

        public override string ToString()
        {
            return (player == TicTacToeIA.PLAYER_O ? "O" : "X") + " (" + x + ";" + y + ")";
        }

    }

}
