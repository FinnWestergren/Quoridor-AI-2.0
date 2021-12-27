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
            var board = boardString == null ? QuoridorUtilities.Empty2PlayerBoard : QuoridorUtilities.ParseBoard(boardString);
            _history.Push(board);
            PlayerOne = Guid.NewGuid();
            PlayerTwo = Guid.NewGuid();
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
            throw new NotImplementedException();
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
