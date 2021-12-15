using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.GameInterpreter
{
    public class TicTacToeBoard : IGameBoard
    {
        private string BoardString { get; set; }

        private Cell[,] Board { 
            get 
            {
                var output = new Cell[3, 3];
                int index = 0;
                foreach (var c in BoardString) 
                {
                    var occupiedBy = c switch
                    {
                        'X' => OccupiedBy.X,
                        'O' => OccupiedBy.O,
                        _ => OccupiedBy.None
                    };
                    var row = index % 3;
                    var col = (int)index/3;
                    output[row, col] = new Cell
                    {
                        Row = row,
                        Col = col,
                        OccupiedBy = occupiedBy
                    };
                }
                return output;
            } 
        }

        public IEnumerable<GameAction> GetPossibleMoves()
        {
            Board.ToString();
            throw new NotImplementedException();
        }
    }

    class Cell
    {
        public int Row { get; set; } // 0 1 2
        public int Col { get; set; } // 0 1 2

        public OccupiedBy OccupiedBy { get; set; }
    }

    enum OccupiedBy
    {
        None = 0,
        X = 1,
        O = 2
    }
}
