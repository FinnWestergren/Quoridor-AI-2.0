using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Game.Quoridor
{
    public class Quoridor : IGame
    {
        private readonly Stack<QuoridorBoard> _history;
        public Guid GameId { get; set; }
        public QuoridorBoard CurrentBoard => _history.Peek();
        public PLAYER_ID WhosTurn { get; set; }

        public Quoridor(string boardString = null, PLAYER_ID? whosTurn = null)
        {
            _history = new Stack<QuoridorBoard>();
            WhosTurn = whosTurn ?? PLAYER_ID.PLAYER_ONE;
            boardString ??= QuoridorUtilities.Empty2PlayerBoardString;
            var board = QuoridorUtilities.ParseBoard(boardString);
            _history.Push(board);
            GameId = Guid.NewGuid();
        }

        public void CommitAction(int serializedAction, PLAYER_ID player, bool skipValidation = false)
        {
            if (WhosTurn != player)
            {
                throw new Exception("Not Your Turn!");
            }
            if (IsGameOver())
            {
                throw new Exception("Game's already over.");
            }
            _history.Push(QuoridorUtilities.TryCommitActionToBoard(serializedAction, CurrentBoard, player, skipValidation));
            ToggleWhosTurn();
        }

        public void UndoAction()
        {
            _history.Pop();
            ToggleWhosTurn();
        }
        public string GameType() => "Quoridor";

        public IEnumerable<IGameAction> GetPossibleMoves(PLAYER_ID player)
        {
            var (moveActions, wallActions) = QuoridorUtilities.GetPossibleMoves(CurrentBoard, player);
            return moveActions.Union<IGameAction>(wallActions);
        }

        public int GetBoardValue(PLAYER_ID player)
        {
            var enemy = player == PLAYER_ID.PLAYER_ONE ? PLAYER_ID.PLAYER_TWO : PLAYER_ID.PLAYER_ONE;
            var enemyDist = PathValidator.GetDistanceForPlayer(CurrentBoard, enemy);
            var playerDist = PathValidator.GetDistanceForPlayer(CurrentBoard, player);
            if (enemyDist == 0) return Int32.MinValue + 1; // cant actually be min or max cuz that fux w minimax lol
            if (playerDist == 0) return Int32.MaxValue - 1;
            return enemyDist - playerDist;
        }

        public bool IsGameOver() => QuoridorUtilities.IsWinCondition(PLAYER_ID.PLAYER_ONE, CurrentBoard) || QuoridorUtilities.IsWinCondition(PLAYER_ID.PLAYER_TWO, CurrentBoard);

        private void ToggleWhosTurn()
        {
            WhosTurn = WhosTurn == PLAYER_ID.PLAYER_ONE ? PLAYER_ID.PLAYER_TWO : PLAYER_ID.PLAYER_ONE;
        }
    }
}
