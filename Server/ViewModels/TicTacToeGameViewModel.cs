using Server.Game;
using Server.Game.TicTacToe;
using Server.Utilities;
using System;
using System.Collections.Generic;

namespace Server.ViewModels
{
    public class TicTacToeGameViewModel
    {
        public IEnumerable<TicTacToeCell> CurrentBoard { get; set; }
        public Guid GameId { get; set; }
        public PLAYER_ID? Winner { get; set; }
        public bool IsTie { get; set; } = false;

        public static TicTacToeGameViewModel FromGame(TicTacToe game)
        {

            PLAYER_ID? winner = null;
            var value = game.GetBoardValue(PLAYER_ID.PLAYER_ONE);
            if (value > 0) winner = PLAYER_ID.PLAYER_ONE;
            if (value < 0) winner = PLAYER_ID.PLAYER_TWO;


            return new TicTacToeGameViewModel
            {
                CurrentBoard = EnumerableUtilities.From2DArray(game.CurrentBoard),
                GameId = game.GameId,
                Winner = winner,
                IsTie = game.IsTie()
            };
        }
    }
}
