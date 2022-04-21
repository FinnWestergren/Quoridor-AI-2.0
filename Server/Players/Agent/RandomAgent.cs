using Server.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Players.Agent
{
    public class RandomAgent : IAgent
    {
        public RandomAgent(PLAYER_ID pId)
        {
            PlayerId = pId;
        }
        public PLAYER_ID PlayerId { get; set; }

        public IGameAction GetNextAction(IGame game)
        {
            var moves = game.GetPossibleMoves(PlayerId);
            var rand = new Random();
            var selection = rand.Next(moves.Count());
            return moves.ElementAt(selection);
        }
    }
}
