using System;
using System.Collections.Generic;

namespace Server.Game
{
    public interface IGame
    {
        Guid PlayerOne { get; set; }
        Guid PlayerTwo { get; set; }
        Guid GameId { get; set; }
        void CommitAction(int serializedAction, Guid playerId);
        void UndoAction();
        void Print();
        string GameType();
        IEnumerable<IGameAction> GetPossibleMoves(Guid playerId);
        int GetBoardValue(Guid playerId);
    }

}
