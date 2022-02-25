using System;
using System.Collections.Generic;

namespace Server.Game
{
    public interface IGame
    {
        Guid PlayerOne { get; }
        Guid PlayerTwo { get; }
        Guid GameId { get; set; }
        void CommitAction(int serializedAction, Guid playerId);
        void UndoAction();
        string GameType();
        IEnumerable<IGameAction> GetPossibleMoves(Guid playerId); 
        int GetBoardValue(Guid playerId);
        Guid GetEnemyId(Guid playerId);
    }

}
