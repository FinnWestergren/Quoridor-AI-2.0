using Server.Game;
using System;
using System.Linq;

namespace Server.Players.Agent
{
    public class MiniMaxAgent : IAgent
    {

        public Guid PlayerId { get; set; }
        public MiniMaxAgent(Guid playerId)
        {
            PlayerId = playerId;
        }

        public IGameAction GetNextAction(IGame game)
        {
            var possibleMoves = game.GetPossibleMoves(PlayerId);
            var maxSoFar = Int32.MinValue;
            IGameAction bestMove = null;
            foreach (var move in possibleMoves)
            {
                var value = GetMoveValue(game, move);
                if(value > maxSoFar)
                {
                    bestMove = move;
                    maxSoFar = value;
                }
            }
            return bestMove;
        }

        private int GetMoveValue(IGame game, IGameAction move)
        {
            game.CommitAction(move.SerializedAction, PlayerId);
            var value = game.GetBoardValue(PlayerId);
            game.UndoAction();
            return value;
        }
    }
}