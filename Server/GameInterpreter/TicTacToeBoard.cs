using System;
using System.Collections.Generic;

namespace Server.GameInterpreter
{
    public class TicTacToeBoard : IGameBoard<TicTacToe>
    {
        private string _boardString;
        private Cell[,] _board = null;
        private readonly int _playerOffset = 9;
        private readonly int _dimension = 3;
        private readonly int _maxPlayers = 2;

        public TicTacToeBoard(string boardString)
        {
            _boardString = boardString;
        }

        private Cell[,] Board {
            get
            {
                if (_board == null) 
                {
                    _board = ParseBoard(_boardString);
                }
                return _board;
            }
            set => _board = value;
        }

        static Cell[,] ParseBoard(string boardString) 
        {
            var output = new Cell[3, 3];
            int index = 0;
            foreach (var c in boardString)
            {
                var occupiedBy = c switch
                {
                    'X' => OccupiedBy.X,
                    'O' => OccupiedBy.O,
                    _ => OccupiedBy.None
                };
                var col = index % 3;
                var row = index / 3;
                output[row, col] = new Cell
                {
                    Row = row,
                    Col = col,
                    OccupiedBy = occupiedBy
                };
                index++;
            }
            return output;
        }

        private TicTacToeAction DeserializeAction(int serializedAction) 
        {
            var maxValue = _maxPlayers * _playerOffset;
            if (serializedAction >= maxValue || serializedAction < 0)
            {
                throw new InvalidOperationException($"Action Invalid for a {_dimension}X{_dimension} tic tac toe board with {_maxPlayers} players. Must be a non-negative number less than {maxValue}");
            }
            var occupiedBy = serializedAction / _playerOffset + 1;
            var row = (serializedAction % _playerOffset) / _dimension;
            var col = serializedAction % _dimension;
            var cell = new Cell(row, col, (OccupiedBy)occupiedBy);
            return new TicTacToeAction(cell);
        }

        public IEnumerable<string> BoardString() 
        {
            foreach (var cell in Board) 
            {
                yield return cell.ToString();
            }
        }
        public IEnumerable<IGameAction<TicTacToe>> GetPossibleMoves(Guid? playerId)
        {
            foreach (var cell in Board)
            {
                var action = new TicTacToeAction(cell);
                if(IsValidAction(action))
                {
                    yield return action;
                }
            }
        }

        public GameType GetGameType() => GameType.TicTacToe;

        public void CommitAction(IGameAction<TicTacToe> action)
        {
            var cell = (Cell)action;
            Board[cell.Row, cell.Col] = cell;
        }
        public void CommitAction(int serializedAction)
        {
            var cell = (Cell)DeserializeAction(serializedAction);
            var temp = Board;
            temp[cell.Row, cell.Col] = cell;
            Board = temp;
        }

        private bool IsValidAction(TicTacToeAction action) 
        {
            if(!(action.OccupiedBy == OccupiedBy.X || action.OccupiedBy == OccupiedBy.O))
            {
                return false;
            }

            return !Board[action.Row, action.Col].IsOccupied;
        }
    }

    class Cell
    {
        public Cell() { }
        public Cell(int row, int col, OccupiedBy occupiedBy)
        {
            Row = row;
            Col = col;
            OccupiedBy = occupiedBy;
        }
        public int Row { get; set; } // 0 1 2
        public int Col { get; set; } // 0 1 2

        public OccupiedBy OccupiedBy { get; set; }
        public override string ToString()
        {
            return $"row {Row}, col {Col}: {OccupiedBy}";
        }
        public bool IsOccupied => OccupiedBy != OccupiedBy.None;
    }

    class TicTacToeAction : Cell, IGameAction<TicTacToe>
    {
        public TicTacToeAction(Cell cell) : base(cell.Row, cell.Col, cell.OccupiedBy) { }
        public int SerializedAction() => Col + Row * 3 + ((int)OccupiedBy) * 9;
    }

    enum OccupiedBy
    {
        None = 0,
        X = 1,
        O = 2
    }
}
