﻿using System;

namespace Server.Game.TicTacToe
{
    public class TicTacToeAction : IGameAction
    {
        public TicTacToeCell Cell { get; set; }
        public int CommittedBy { get; set; }
        public TicTacToeAction(TicTacToeCell cell, int committedBy)
        {
            Cell = cell;
            CommittedBy = committedBy;
        }
        public int SerializedAction => Cell.SerializedCell;
        public bool IsValidated { get; set; } = false;
    }
}
