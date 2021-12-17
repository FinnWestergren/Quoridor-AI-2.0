using System;

namespace Server.Game.TicTacToe
{
    public class TicTacToeAction : IGameAction
    {
        public Cell Cell { get; set; }
        public Guid CommittedBy { get; set; }
        public TicTacToeAction(Cell cell, Guid committedBy)
        {
            Cell = cell;
            CommittedBy = committedBy;
        }
        public int SerializedAction() => Cell.Col + Cell.Row * 3;

    }
}
