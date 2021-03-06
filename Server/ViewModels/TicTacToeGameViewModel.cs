using Server.Game.TicTacToe;
using Server.Utilities;
using System;
using System.Collections.Generic;

namespace Server.ViewModels
{
    public class TicTacToeGameViewModel
    {
        public IEnumerable<TicTacToeCell> CurrentBoard { get; set; }
        public Guid PlayerOne { get; set; }
        public Guid PlayerTwo { get; set; }
        public Guid GameId { get; set; }
        public Guid? Winner { get; set; }

        public static TicTacToeGameViewModel FromGame(TicTacToe game)
        {

            Guid? winner = null;
            var value = game.GetBoardValue(game.PlayerOne);
            if (value > 0) winner = game.PlayerOne;
            if (value < 0) winner = game.PlayerTwo;


            return new TicTacToeGameViewModel
            {
                CurrentBoard = EnumerableUtilities<TicTacToeCell>.From2DArray(game.CurrentBoard),
                PlayerOne = game.PlayerOne,
                PlayerTwo = game.PlayerTwo,
                GameId = game.GameId,
                Winner = winner
            };
        }
    }
}
