using Server.Game;
using System;
using System.Linq;

namespace Server.Players.Agent
{
    public class MiniMaxAgent : IAgent
    {
        private const int DEFAULT_SEARCH_DEPTH = 8;
        private const int DEFAULT_BETA = Int32.MaxValue;
        private const int DEFAULT_ALPHA = Int32.MinValue;
        public Guid PlayerId { get; set; }
        private readonly int _maxSearchDepth;
        private readonly bool _isABPruningEnabled;

        public int NodeCount { get; set; } = 0;
        public MiniMaxAgent(Guid playerId, int maxSearchDepth = DEFAULT_SEARCH_DEPTH, bool isABPruningEnabled = true)
        {
            PlayerId = playerId;
            _maxSearchDepth = maxSearchDepth;
            _isABPruningEnabled = isABPruningEnabled;
        }

        public IGameAction GetNextAction(IGame game) 
        {
            NodeCount = 0;
            var (_, action) = MaxMove(game);
            return action;
        }

        private (int value, IGameAction action) MaxMove(IGame game, int depth = 0, int beta = DEFAULT_BETA)
        {
            NodeCount++;
            var possibleMoves = game.GetPossibleMoves(PlayerId);
            if (!possibleMoves.Any() || depth > _maxSearchDepth || game.IsGameOver())
            {
                if (game.IsGameOver())
                {

                }
                return (game.GetBoardValue(PlayerId), null);
            }
            var maxSoFar = Int32.MinValue;
            IGameAction bestMove = null;
            var alpha = DEFAULT_ALPHA;
            foreach (var move in possibleMoves)
            {
                game.CommitAction(move.SerializedAction, PlayerId, true);
                var value = MinMove(game, depth + 1, alpha);
                game.UndoAction();
                if (value > maxSoFar)
                {
                    bestMove = move;
                    maxSoFar = value;
                }
                if (_isABPruningEnabled)
                {
                    if (value >= beta) return (maxSoFar, bestMove); // prune
                    if (value > alpha) alpha = value;
                }
            }
            return (maxSoFar, bestMove);
        }

        private int MinMove(IGame game, int depth, int alpha = DEFAULT_ALPHA)
        {
            NodeCount++;
            var enemyId = game.GetEnemyId(PlayerId);
            var possibleMoves = game.GetPossibleMoves(enemyId);
            if (!possibleMoves.Any() || depth > _maxSearchDepth || game.IsGameOver())
            {
                return game.GetBoardValue(PlayerId);
            }
            var minSoFar = Int32.MaxValue;
            var beta = DEFAULT_BETA;
            foreach (var move in possibleMoves)
            {
                game.CommitAction(move.SerializedAction, enemyId, true);
                var value = MaxMove(game, depth + 1, beta).value;
                game.UndoAction();
                if (value < minSoFar)
                {
                    minSoFar = value;
                }
                if (_isABPruningEnabled)
                {
                    if (value <= alpha) return minSoFar; // prune
                    if (value < beta) beta = value;
                }
            }
            return minSoFar;
        }
    }
}