using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Game.TicTacToe
{
    public class TicTacToe : IGame
    {
    }

    public class Cell
    {
        public Cell() { }
        public Cell(int row, int col, PlayerMarker occupiedBy)
        {
            Row = row;
            Col = col;
            OccupiedBy = occupiedBy;
        }
        public int Row { get; set; } // 0 1 2
        public int Col { get; set; } // 0 1 2

        public PlayerMarker OccupiedBy { get; set; }
        public override string ToString()
        {
            return $"row {Row}, col {Col}: {OccupiedBy}";
        }
        public bool IsOccupied => OccupiedBy != PlayerMarker.None;
    }

    public class TicTacToeAction : IGameAction<TicTacToe>
    {
        public int Row { get; set; } // 0 1 2
        public int Col { get; set; } // 0 1 2
        public PlayerMarker CommittedBy { get; set; }
        public TicTacToeAction(Cell cell, PlayerMarker committedBy)
        {
            Col = cell.Col;
            Row = cell.Row;
            CommittedBy = committedBy;
        }
        public int SerializedAction() => Col + Row * 3 + ((int)CommittedBy) * 9 ;
    }

    public enum PlayerMarker
    {
        X = 0,
        O = 1,
        None = 2
    }
}
