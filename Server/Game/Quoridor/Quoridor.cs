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
        public Guid PlayerOne { get; set; }
        public Guid PlayerTwo { get; set; }
        public Guid WhosTurn { get; set; }

        public Quoridor(string boardString = null, Guid? whosTurn = null)
        {
            _history = new Stack<QuoridorBoard>();
            PlayerOne = Guid.NewGuid();
            PlayerTwo = Guid.NewGuid();
            WhosTurn = whosTurn ?? PlayerOne;
            boardString ??= QuoridorUtilities.Empty2PlayerBoardString;
            var board = QuoridorUtilities.ParseBoard(boardString);
            _history.Push(board);
            GameId = Guid.NewGuid();
        }

        public void CommitAction(int serializedAction, Guid playerId, bool skipValidation = false)
        {
            if (WhosTurn != playerId)
            {
                throw new Exception("Not Your Turn!");
            }
            if (IsGameOver())
            {
                throw new Exception("Game's already over.");
            }
            _history.Push(QuoridorUtilities.TryCommitActionToBoard(serializedAction, CurrentBoard, GetPlayerById(playerId), skipValidation));
            WhosTurn = GetEnemyId(playerId);
        }

        public void UndoAction()
        {
            _history.Pop();
            WhosTurn = GetEnemyId(WhosTurn);
        }
        public string GameType() => "Quoridor";

        public IEnumerable<IGameAction> GetPossibleMoves(Guid playerId)
        {
            var (moveActions, wallActions) = QuoridorUtilities.GetPossibleMoves(CurrentBoard, GetPlayerById(playerId));
            return moveActions.Union<IGameAction>(wallActions);
        }

        public int GetBoardValue(Guid playerId)
        {
            var player = GetPlayerById(playerId);
            var enemy = GetEnemyById(playerId);
            var enemyDist = PathValidator.GetDistanceForPlayer(CurrentBoard, enemy);
            var playerDist = PathValidator.GetDistanceForPlayer(CurrentBoard, player);
            if (enemyDist == 0) return Int32.MinValue + 1; // cant actually be min or max cuz that fux w minimax lol
            if (playerDist == 0) return Int32.MaxValue - 1;
            return enemyDist - playerDist;
        }

        public Guid GetEnemyId(Guid playerId)
        {
            if (playerId == PlayerOne) return PlayerTwo;
            if (playerId == PlayerTwo) return PlayerOne;
            throw new Exception($"Invalid Player Id {playerId}");
        }

        public bool IsGameOver() => QuoridorUtilities.IsWinCondition(1, CurrentBoard) || QuoridorUtilities.IsWinCondition(2, CurrentBoard);

        public int GetPlayerById(Guid id)
        {
            if (id == PlayerOne) return 1;
            if (id == PlayerTwo) return 2;
            throw new Exception("Invalid Player ID");
        }
        private int GetEnemyById(Guid id)
        {
            if (id == PlayerOne) return 2;
            if (id == PlayerTwo) return 1;
            throw new Exception("Invalid Player ID");
        }
    }
}
