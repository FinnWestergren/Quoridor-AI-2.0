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

        public static TicTacToeGameViewModel FromGame(TicTacToe game)
        {

            IEnumerable<Cell> board()
            { 
                foreach(var c in game.CurrentBoard)
                {
                    yield return c;
                }
            }

            return new TicTacToeGameViewModel
            {
                CurrentBoard = board(),
                PlayerOne = game.PlayerOne,
                PlayerTwo = game.PlayerTwo
            };
        }
    }
}
