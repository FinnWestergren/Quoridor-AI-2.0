using Server.Game.TicTacToe;
using System;
using System.Collections.Generic;

namespace Server.ViewModels
{
    public class TicTacToeGameViewModel
    {
        public IEnumerable<Cell> CurrentBoard { get; set; }
        public Guid PlayerOne { get; set; }
        public Guid PlayerTwo { get; set; }
        public Guid GameId { get; set; }
        public Guid? Winner { get; set; }

        public static TicTacToeGameViewModel FromGame(TicTacToe game)
        {

            IEnumerable<Cell> board()
            { 
                foreach(var c in game.CurrentBoard)
                {
                    yield return c;
                }
            }

            Guid? winner = null;
            var value = game.GetBoardValue(game.PlayerOne);
            if (value > 0) winner = game.PlayerOne;
            if (value < 0) winner = game.PlayerTwo;


            return new TicTacToeGameViewModel
            {
                CurrentBoard = board(),
                PlayerOne = game.PlayerOne,
                PlayerTwo = game.PlayerTwo,
                GameId = game.GameId,
                Winner = winner
            };
        }
    }
}
