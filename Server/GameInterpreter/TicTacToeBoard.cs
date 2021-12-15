using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.GameInterpreter
{
    public class TicTacToeBoard : IGameBoard
    {
        private string _boardString;

        public TicTacToeBoard(string boardString)
        {
            _boardString = boardString;
        }

        private Cell[,] Board { 
            get 
            {
                var output = new Cell[3, 3];
                int index = 0;
                foreach (var c in _boardString) 
                {
                    var occupiedBy = c switch
                    {
                        'X' => OccupiedBy.X,
                        'O' => OccupiedBy.O,
                        _ => OccupiedBy.None
                    };
                    var col = index % 3;
                    var row = (int)index/3;
                    output[col,row] = new Cell
                    {
                        Row = row,
                        Col = col,
                        OccupiedBy = occupiedBy
                    };
                    index++;
                }
                return output;
            } 
        }

        public IEnumerable<string> BoardString() 
        {
            foreach (var cell in Board) 
            {
                yield return cell.ToString();
            }
        }
        public IEnumerable<IGameAction> GetPossibleMoves(string playerId)
        {
            foreach (var cell in Board)
            {
                if (cell.OccupiedBy == OccupiedBy.None)
                {
                    yield return new TicTacToeAction { Cell = cell };
                }
            }
        }
    }

    class Cell
    {
        public int Row { get; set; } // 0 1 2
        public int Col { get; set; } // 0 1 2

        public OccupiedBy OccupiedBy { get; set; }
        public override string ToString()
        {
            return $"row {Row}, col {Col}: {OccupiedBy}";
        }
    }

    class TicTacToeAction : IGameAction
    {
        public Cell Cell { get; set; }
        public int SerializedAction() => Cell.Col + Cell.Row * 3; 
        public int ValueOf() => 0;
    }

    enum OccupiedBy
    {
        None = 0,
        X = 1,
        O = 2
    }
}
