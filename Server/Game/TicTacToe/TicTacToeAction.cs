using System;

namespace Server.Game.TicTacToe
{
    public class TicTacToeAction : IGameAction
    {
        public TicTacToeCell Cell { get; set; }
        public Guid CommittedBy { get; set; }
        public TicTacToeAction(TicTacToeCell cell, Guid committedBy)
        {
            Cell = cell;
            CommittedBy = committedBy;
        }
        public int SerializedAction => Cell.SerializedCell;

    }
}
