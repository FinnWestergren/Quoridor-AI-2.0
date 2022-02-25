using System;
using System.Collections.Generic;

namespace Server.Game.Quoridor
{
    public class Quoridor : IGame
    {
        private readonly Stack<QuoridorBoard> _history;
        public Guid GameId { get; set; }
        public QuoridorBoard CurrentBoard => _history.Peek();
        public Guid PlayerOne { get; set; }
        public Guid PlayerTwo { get; set; }

        public Quoridor(string boardString = null)
        {
            _history = new Stack<QuoridorBoard>();
            PlayerOne = Guid.NewGuid();
            PlayerTwo = Guid.NewGuid();
            boardString ??= QuoridorUtilities.Empty2PlayerBoardString;
            var board = QuoridorUtilities.ParseBoard(boardString, PlayerOne, PlayerTwo);
            _history.Push(board);
            GameId = Guid.NewGuid();
        }

        public void CommitAction(int serializedAction, Guid playerId)
        {
            _history.Push(QuoridorUtilities.TryCommitActionToBoard(serializedAction, CurrentBoard, playerId));
        }

        public void UndoAction() => _history.Pop();

        public string GameType() => "Quoridor";

        public IEnumerable<IGameAction> GetPossibleMoves(Guid playerId)
        {
            return QuoridorUtilities.GetPossibleMoves(CurrentBoard, playerId);
        }

        public int GetBoardValue(Guid playerId)
        {
            return PathValidator.GetDistanceForPlayer(CurrentBoard, GetEnemyId(playerId)) - PathValidator.GetDistanceForPlayer(CurrentBoard, playerId);
        }

        public Guid GetEnemyId(Guid playerId)
        {
            if (playerId == PlayerOne) return PlayerTwo;
            if (playerId == PlayerTwo) return PlayerOne;
            throw new Exception($"Invalid Player Id {playerId}");
        }
    }
}
