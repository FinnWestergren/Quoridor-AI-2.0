using Server.Game;
using System;
using System.Linq;

namespace Server.Players.Agent
{
    public class MiniMaxAgent : IAgent
    {
        private const int DEFAULT_SEARCH_DEPTH = 8;
        public Guid PlayerId { get; set; }
        private readonly int _maxSearchDepth;
        public MiniMaxAgent(Guid playerId, int maxSearchDepth = DEFAULT_SEARCH_DEPTH)
        {
            PlayerId = playerId;
            _maxSearchDepth = maxSearchDepth;
        }

        public IGameAction GetNextAction(IGame game) => MaxMove(game).action;

        private (int value, IGameAction action) MaxMove(IGame game, int depth = 0)
        {
            var possibleMoves = game.GetPossibleMoves(PlayerId);
            if (!possibleMoves.Any() || depth > _maxSearchDepth)
            {
                return (game.GetBoardValue(PlayerId), null);
            }
            var maxSoFar = Int32.MinValue;
            IGameAction bestMove = null;
            foreach (var move in possibleMoves)
            {
                game.CommitAction(move.SerializedAction, PlayerId);
                var value = MinMove(game, depth + 1).value;
                game.UndoAction();
                if (value > maxSoFar)
                {
                    bestMove = move;
                    maxSoFar = value;
                }
            }
            return (maxSoFar, bestMove);
        }

        private (int value, IGameAction action) MinMove(IGame game, int depth)
        {
            var possibleMoves = game.GetPossibleMovesForEnemy(PlayerId);
            if (!possibleMoves.Any() || depth > _maxSearchDepth)
            {
                return (game.GetBoardValue(PlayerId), null);
            }
            var minSoFar = Int32.MaxValue;
            IGameAction bestMove = null;
            foreach (var move in possibleMoves)
            {
                game.CommitAction(move.SerializedAction, PlayerId);
                var value = MaxMove(game, depth + 1).value;
                game.UndoAction();
                if (value < minSoFar)
                {
                    bestMove = move;
                    minSoFar = value;
                }
            }
            return (minSoFar, bestMove);
        }
    }
}