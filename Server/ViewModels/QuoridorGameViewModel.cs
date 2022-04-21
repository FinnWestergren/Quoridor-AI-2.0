using Newtonsoft.Json;
using Server.Game;
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
        public PLAYER_ID WhosTurn { get; set; }
        public Guid GameId { get; set; }
        public PLAYER_ID? Winner { get; set; }
        public bool IsTie = false;

        public static QuoridorGameViewModel FromGame(Quoridor game)
        {
            PLAYER_ID? winner = null;
            if (game.IsGameOver())
            {
                var value = game.GetBoardValue(PLAYER_ID.PLAYER_ONE);
                if (value > 0) winner = PLAYER_ID.PLAYER_ONE;
                if (value < 0) winner = PLAYER_ID.PLAYER_TWO;
            }


            return new QuoridorGameViewModel
            {
                Walls = EnumerableUtilities.ToSquareEnumerable(game.CurrentBoard.Walls),
                PlayerWallCounts = JsonConvert.SerializeObject(game.CurrentBoard.PlayerWallCounts),
                PlayerPositions = JsonConvert.SerializeObject(game.CurrentBoard.PlayerPositions),
                GameId = game.GameId,
                Winner = winner,
                WhosTurn = game.WhosTurn
            };
        }
    }
}
