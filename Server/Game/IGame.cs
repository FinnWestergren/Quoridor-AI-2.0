using System;
using System.Collections.Generic;

namespace Server.Game
{
    public interface IGame
    {
        Guid GameId { get; set; }
        void CommitAction(int serializedAction, PLAYER_ID player, bool skipValidation = false);
        void UndoAction();
        string GameType();
        IEnumerable<IGameAction> GetPossibleMoves(PLAYER_ID player); 
        int GetBoardValue(PLAYER_ID player);
        bool IsGameOver();
    }
}
