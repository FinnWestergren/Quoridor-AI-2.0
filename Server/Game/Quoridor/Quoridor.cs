using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            throw new NotImplementedException();
        }

        public void UndoAction()
        {
            throw new NotImplementedException();
        }

        public void Print()
        {
            throw new NotImplementedException();
        }

        public string GameType()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGameAction> GetPossibleMoves(Guid playerId)
        {
            return QuoridorUtilities.GetPossibleMoves(CurrentBoard, playerId);
        }

        public int GetBoardValue(Guid playerId)
        {
            throw new NotImplementedException();
        }

        public Guid GetEnemyId(Guid playerId)
        {
            throw new NotImplementedException();
        }
    }
}
