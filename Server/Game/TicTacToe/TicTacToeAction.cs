using System;

namespace Server.Game.TicTacToe
{
    public class TicTacToeAction : IGameAction
    {
        public TicTacToeCell Cell { get; set; }
        public PLAYER_ID CommittedBy { get; set; }
        public TicTacToeAction(TicTacToeCell cell, PLAYER_ID committedBy)
        {
            Cell = cell;
            CommittedBy = committedBy;
        }
        public int SerializedAction => Cell.SerializedCell;
        public bool IsValidated { get; set; } = false;
    }
}
