using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarloTreeSearch
{
    public class TickTackToeField : IState
    {
        int[,] board;
        TickTackToe Game;
        public int Turn { get; set; }

        public TickTackToeField(ref TickTackToe game, int playerTurn)
        {
            Game = game;
            board = game.Field;
            Turn = playerTurn;
        }

        public override ActionReturnValue TakeAction(int SimulationAction)
        {
            ActionReturnValue toReturn = new ActionReturnValue();
            int x = SimulationAction % Game.Field.GetLength(0);
            int y = SimulationAction / Game.Field.GetLength(1);
            Game.MakeMove(ref board, Turn, x, y);
            if(Game.CheckWin(ref board)==Turn)
            {
                toReturn.Done = true;
                toReturn.NewState = new TickTackToeField(ref Game, -Turn);
                toReturn.Value = 1;
            }
            else if(Game.CheckWin(ref board)==-Turn)
            {
                toReturn.Done = true;
                toReturn.NewState = new TickTackToeField(ref Game, -Turn);
                toReturn.Value = -1;
            }
            else if(!Game.IsPlayable(ref board))
            {
                toReturn.Done = true;
                toReturn.NewState = new TickTackToeField(ref Game, -Turn);
                toReturn.Value = 0;
            }
            else
            {
                toReturn.Done = false;
                toReturn.NewState = new TickTackToeField(ref Game, -Turn);
                toReturn.Value = 0;
            }
            return toReturn;
        }
    }

    public class TickTackToe
    {
        public static int TTT_EMPTY = 0;
        public static int TTT_CROSS = 1;
        public static int TTT_CIRCLE = -1;
        public static int TTT_FIELDSIZE = 3;

        public int[,] Field { get; set; }

        public TickTackToe(ref int[,] __field)
        {
            Field = new int[TTT_FIELDSIZE, TTT_FIELDSIZE];
            for (int i = 0; i < TTT_FIELDSIZE; i++)
                for (int j = 0; j < TTT_FIELDSIZE; j++)
                    Field[i, j] = TTT_EMPTY;
            __field = Field;
        }

        void Print(ref int[,] __field)
        {
            for (int i = 0; i < TTT_FIELDSIZE; i++)
            {
                for (int j = 0; j < TTT_FIELDSIZE; j++)
                {
                    if (j != 0)
                        Console.Write("|");

                    Console.Write(" " + __field[i,j] +" ");
                }
                if (i != TTT_FIELDSIZE - 1)
                {
                    Console.WriteLine();
                    Console.WriteLine("-----------");
                }
                else
                {
                    Console.WriteLine();
                }
            }
        }

        public bool MakeMove(ref int[,] __field, int __who, int __wherex, int __wherey)
        {
            if (__wherex < 0 || __wherex >= TTT_FIELDSIZE)
                return false;
            if (__wherey < 0 || __wherey >= TTT_FIELDSIZE)
                return false;

            if (__field[__wherey,__wherex] != TTT_EMPTY)
                return false;

            __field[__wherey,__wherex] = __who;
            return true;
        }

        public int CheckWin(ref int[,] __field )
        {
            /// vertical
            for (int i = 0; i < TTT_FIELDSIZE; i++)
            {
                if (__field[0,i] == __field[1,i] && __field[1,i] == __field[2,i] && __field[1,i] != TTT_EMPTY)
                    return __field[1,i];
            }

            /// horizontal
            for (int i = 0; i < TTT_FIELDSIZE; i++)
            {
                if (__field[i,0] == __field[i,1] && __field[i,1] == __field[i,2] && __field[i,1] != TTT_EMPTY)
                    return __field[i,1];
            }

            /// diagonal 
            if (__field[0,0] == __field[1,1] && __field[1,1] == __field[2,2] && __field[1,1] != TTT_EMPTY)
                return __field[1,1];

            if (__field[0,2] == __field[1,1] && __field[1,1] == __field[2,0] && __field[1,1] != TTT_EMPTY)
                return __field[1,1];

            /// no one wins
            return TTT_EMPTY;
        }

        public bool IsPlayable(ref int[,] __field )
        {
            if (CheckWin(ref __field)==1) return false;

            for (int i = 0; i < TTT_FIELDSIZE; i++)
                for (int j = 0; j < TTT_FIELDSIZE; j++)
                    if (__field[i,j] == TTT_EMPTY)
                        return true;

            return false;
        }
    }
}
