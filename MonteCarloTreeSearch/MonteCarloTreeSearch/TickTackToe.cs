using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarloTreeSearch
{
    public enum TickTackToeMarker
    {
        PlayerX,
        PlayerO,
        Draw,
        None
    }

    public class TickTackToe : IState
    {
        private TickTackToeMarker[,] Board;
        private TickTackToeMarker Turn { get; set; }
        public int MoveCount { get; }
        private int GridDimension { get; set; }

        public TickTackToe(int gridDimension)
        {
            GridDimension = gridDimension;
            Board=new TickTackToeMarker[GridDimension,GridDimension];
            for (int x = 0; x < GridDimension; x++)
            {
                for (int y = 0; y < GridDimension; y++)
                {
                    Board[x, y] = TickTackToeMarker.None;
                }
            }
            Turn = TickTackToeMarker.PlayerX;
            MoveCount = 0;
        }

        public TickTackToe(TickTackToeMarker[,] board, TickTackToeMarker turn, int moveCount, int gridDimension)
        {
            Board = board;
            Turn = turn;
            GridDimension = gridDimension;
        }


        private TickTackToeMarker Move(int x, int y)
        {
            if (Board[x,y] == TickTackToeMarker.None)
            {
                Board[x,y] = Turn;
            }
            
            //check end conditions

            //check col
            for (int i = 0; i < GridDimension; i++)
            {
                if (Board[x,i] != Turn)
                    break;
                if (i == GridDimension - 1)
                {
                    //report win for s
                    return Turn;
                }
            }

            //check row
            for (int i = 0; i < GridDimension; i++)
            {
                if (Board[i,y] != Turn)
                    break;
                if (i == GridDimension - 1)
                {
                    //report win for s
                    return Turn;
                }
            }

            //check diag
            if (x == y)
            {
                //we're on a diagonal
                for (int i = 0; i < GridDimension; i++)
                {
                    if (Board[i,i] != Turn)
                        break;
                    if (i == GridDimension - 1)
                    {
                        //report win for s
                        return Turn;
                    }
                }
            }

            //check anti diag
            if (x + y == GridDimension - 1)
            {
                for (int i = 0; i < GridDimension; i++)
                {
                    if (Board[i,(GridDimension - 1) - i] != Turn)
                        break;
                    if (i == GridDimension - 1)
                    {
                        //report win for s
                        return Turn;
                    }
                }
            }

            //check draw
            if ((MoveCount+1) == (Math.Pow(GridDimension, 2) - 1))
            {
                //report draw
                return TickTackToeMarker.Draw;
            }

            //Report no one has won yet and there's no draw
            return TickTackToeMarker.None;
        }

        public override ActionReturnValue TakeAction(int simulationAction)
        {
            ActionReturnValue toReturn = new ActionReturnValue();
            int x = simulationAction % Board.GetLength(0);
            int y = simulationAction / Board.GetLength(1);
            TickTackToeMarker resultOfMove = Move(x, y);
            var newTurn = TickTackToeMarker.None;
            if (Turn == TickTackToeMarker.PlayerX)
            {
                newTurn = TickTackToeMarker.PlayerO;
            }
            else
            {
                newTurn = TickTackToeMarker.PlayerX;
            }
            if (resultOfMove == TickTackToeMarker.None)
            {
                toReturn.Done = false;
                toReturn.NewState=new TickTackToe(Board,newTurn,MoveCount+1,GridDimension);
                toReturn.Value = 0;
            }
            else if (resultOfMove == Turn)
            {
                toReturn.Done = true;
                toReturn.NewState=new TickTackToe(Board, newTurn, MoveCount+1, GridDimension);
                toReturn.Value = 1;
            }
            else // Draw
            {
                toReturn.Done = true;
                toReturn.NewState=new TickTackToe(Board, newTurn, MoveCount+1, GridDimension);
                toReturn.Value = 0;
            }
            return toReturn;
        }

        public void PrettyPrint()
        {
            for (int y = 0; y < GridDimension; y++)
            {
                for (int x = 0; x < GridDimension; x++)
                {
                    if (Board[x, y] == TickTackToeMarker.None)
                    {
                        Console.Write(" - ");
                    }
                    else if (Board[x, y] == TickTackToeMarker.PlayerO)
                    {
                        Console.Write(" O ");
                    }
                    else if (Board[x, y] == TickTackToeMarker.PlayerX)
                    {
                        Console.Write(" X ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
