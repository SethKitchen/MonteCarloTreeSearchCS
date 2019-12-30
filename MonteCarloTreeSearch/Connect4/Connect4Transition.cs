using MonteCarloTreeSearch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4
{
    class Connect4Transition:ITransition
    {
        /// <summary>
        /// The player owning the move </summary>
        public int Player { get; set; }

        /// <summary>
        /// x coordinate of the move </summary>
        public int X { get; set; }

        /// <summary>
        /// y coordinate of the move </summary>
        public int Y { get; set; }


        private Connect4Transition()
        {
        }

        public Connect4Transition(int x, int y, int player)
        {
            this.X = x;
            this.Y = y;
            this.Player = player;
        }


        public override int GetHashCode()
        {
            return (Player >> 6) | (X >> 3) | Y;
        }

        public override bool Equals(object o)
        {
            return o is Connect4Transition && ((Connect4Transition)o).Player == Player && ((Connect4Transition)o).X == X && ((Connect4Transition)o).Y == Y;
        }

        public override string ToString()
        {
            return (Player == Connect4IA.PLAYER_R ? "R" : "B") + " (" + X + ";" + Y + ")";
        }
    }
}
