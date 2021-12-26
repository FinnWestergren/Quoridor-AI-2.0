using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Game.Quoridor
{
    public class Quoridor : IGame
    {
        private readonly Stack<QuoridorBoard> _history;
        private Dictionary<int, Guid> _playerIds;
        public Guid GameId { get; set; }
        public QuoridorBoard CurrentBoard => _history.Peek();
        public Guid PlayerOne => _playerIds[0];
        public Guid PlayerTwo => _playerIds[1];

        public Quoridor(bool is4Player = false)
        {
            _history.Push(QuoridorUtilities.Empty2PlayerBoard);

        }
        public Quoridor(string boardString)
        {
            _history.Push(QuoridorUtilities.ParseBoard(boardString));
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
