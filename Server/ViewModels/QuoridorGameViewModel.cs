using Newtonsoft.Json;
using Server.Game.Quoridor;
using Server.Utilities;
using System;
using System.Collections.Generic;

namespace Server.ViewModels
{
    public class QuoridorGameViewModel
    {
        public IEnumerable<IEnumerable<WallOrientation>> Walls { get; set; }
        public string PlayerWallCounts { get; set; }
        public string PlayerPositions { get; set; }
        public Guid PlayerOne { get; set; }
        public Guid PlayerTwo { get; set; }
        public Guid WhosTurn { get; set; }
        public Guid GameId { get; set; }
        public Guid? Winner { get; set; }
        public bool IsTie = false;

        public static QuoridorGameViewModel FromGame(Quoridor game)
        {
            Guid? winner = null;
            var value = game.GetBoardValue(game.PlayerOne);
            if (value > 0) winner = game.PlayerOne;
            if (value < 0) winner = game.PlayerTwo;


            return new QuoridorGameViewModel
            {
                Walls = EnumerableUtilities<WallOrientation>.ToSquareEnumerable(game.CurrentBoard.Walls),
                PlayerWallCounts = JsonConvert.SerializeObject(game.CurrentBoard.PlayerWallCounts),
                PlayerPositions = JsonConvert.SerializeObject(game.CurrentBoard.PlayerPositions),
                PlayerOne = game.PlayerOne,
                PlayerTwo = game.PlayerTwo,
                GameId = game.GameId,
                Winner = winner,
                WhosTurn = game.WhosTurn,

            };
        }
    }
}
