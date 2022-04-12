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
            var board = QuoridorUtilities.ParseBoard(boardString, PlayerOne, PlayerTwo);
            _history.Push(board);
            GameId = Guid.NewGuid();
        }

        public void CommitAction(int serializedAction, Guid playerId)
        {
            if (WhosTurn != playerId)
            {
                throw new Exception("Not Your Turn!");
            }
            if (IsGameOver())
            {
                throw new Exception("Game's already over.");
            }
            var print = QuoridorUtilities.PrintHumanReadableBoard(CurrentBoard);
            try
            {
                _history.Push(QuoridorUtilities.TryCommitActionToBoard(serializedAction, CurrentBoard, playerId));
            }
            catch (Exception e)
            { 
                var moves = GetPossibleMoves(playerId);
                throw e;
            }
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
            var (moveActions, wallActions) = QuoridorUtilities.GetPossibleMoves(CurrentBoard, playerId);
            return moveActions.Union<IGameAction>(wallActions);
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

        public bool IsGameOver() => PathValidator.GetDistanceForPlayer(CurrentBoard, PlayerOne) == 0 || PathValidator.GetDistanceForPlayer(CurrentBoard, PlayerTwo) == 0;
    }
}
